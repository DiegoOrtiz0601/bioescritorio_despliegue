using BiomentricoHolding.Data;
using BiomentricoHolding.Services;
using BiomentricoHolding.Utils;
using DPFP;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using EmpleadoModel = BiomentricoHolding.Data.Empleado;

namespace BiomentricoHolding.Views.Empleado
{
    public partial class RegistrarEmpleado : UserControl
    {
        private DPFP.Template _huellaCapturada = null;
        private bool esModificacion = false;
        private int idEmpleadoActual = 0;

        public RegistrarEmpleado()
        {
            InitializeComponent();
            Logger.Agregar("🔄 Abriendo formulario de Registro de Empleado");
            CargarEmpresas();
            CargarTiposEmpleado();
            CargarJefes();
        }

        private void CargarEmpresas()
        {
            using var context = AppSettings.GetContextUno();
            cbEmpresa.ItemsSource = context.Empresas.OrderBy(e => e.Nombre).ToList();
            cbEmpresa.DisplayMemberPath = "Nombre";
            cbEmpresa.SelectedValuePath = "IdEmpresa";
        }

        private void CargarTiposEmpleado()
        {
            using var context = AppSettings.GetContextUno();
            cbTipoEmpleado.ItemsSource = context.TiposEmpleados.Where(t => t.Estado).OrderBy(t => t.Nombre).ToList();
            cbTipoEmpleado.DisplayMemberPath = "Nombre";
            cbTipoEmpleado.SelectedValuePath = "Id";
        }

        private void CargarJefes()
        {
            using var context = AppSettings.GetContextUno();
            
            // Primero, obtener información sobre los roles disponibles
            var roles = context.Roles.ToList();
            Logger.Agregar($"🔍 Roles disponibles: {string.Join(", ", roles.Select(r => $"{r.IdRol}:{r.Rol}"))}");
            
            // Cargar usuarios activos con sus roles
            var usuarios = context.Usuarios
                .Where(u => u.Estado == true)
                .Include(u => u.IdRolUsuarioNavigation)
                .OrderBy(u => u.NombreUsuario)
                .ToList();
            
            // Filtrar solo usuarios con IdRolUsuario = 3 y Estado = true
            var jefes = usuarios.Where(u => u.IdRolUsuario == 3 && u.Estado == true)
                               .ToList();
            
            Logger.Agregar($"👨‍💼 Usuarios con rol 3 (jefes) encontrados: {jefes.Count}");
            
            cbJefe.ItemsSource = jefes;
            cbJefe.DisplayMemberPath = "Nombre";
            cbJefe.SelectedValuePath = "IdUsuario";
            
            Logger.Agregar($"👨‍💼 Jefes cargados: {jefes.Count} jefes disponibles");
        }

        private void cbEmpresa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbSede.ItemsSource = null;
            cbArea.ItemsSource = null;

            if (cbEmpresa.SelectedValue is int idEmpresa)
            {
                Logger.Agregar($"🏢 Empresa seleccionada: ID {idEmpresa}");
                using var context = AppSettings.GetContextUno();
                
                // Obtener sedes a través de la tabla intermedia SedeCiudadEmpresaArea
                var sedes = context.SedeCiudadEmpresaAreas
                    .Where(sca => sca.IdEmpresa == idEmpresa)
                    .Select(sca => sca.IdSedeNavigation)
                    .Where(s => s.Estado == true || s.Estado == null) // Incluir null como válido
                    .Distinct()
                    .OrderBy(s => s.Nombre)
                    .ToList();
                
                cbSede.ItemsSource = sedes;
                cbSede.DisplayMemberPath = "Nombre";
                cbSede.SelectedValuePath = "IdSede";
                Logger.Agregar($"📍 Sedes cargadas: {sedes.Count} sedes para empresa {idEmpresa}");
            }
        }

        private void cbSede_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbArea.ItemsSource = null;

            if (cbSede.SelectedValue is int idSede && cbEmpresa.SelectedValue is int idEmpresa)
            {
                Logger.Agregar($"📍 Sede seleccionada: ID {idSede}, Empresa: ID {idEmpresa}");
                using var context = AppSettings.GetContextUno();
                
                // Obtener áreas a través de la tabla intermedia SedeCiudadEmpresaArea
                var areas = context.SedeCiudadEmpresaAreas
                    .Where(sca => sca.IdSede == idSede && sca.IdEmpresa == idEmpresa)
                    .Select(sca => sca.IdAreaNavigation)
                    .Where(a => a.Estado == true || a.Estado == null) // Incluir null como válido
                    .OrderBy(a => a.Nombre)
                    .ToList();
                
                cbArea.ItemsSource = areas;
                cbArea.DisplayMemberPath = "Nombre";
                cbArea.SelectedValuePath = "IdArea";
                Logger.Agregar($"📍 Áreas cargadas: {areas.Count} áreas para sede {idSede} y empresa {idEmpresa}");
            }
        }

        private void txtCedula_LostFocus(object sender, RoutedEventArgs e)
        {
            string cedulaTexto = txtCedula.Text.Trim();
            var icon = (TextBlock)this.FindName("iconCedulaCheck");

            if (string.IsNullOrWhiteSpace(cedulaTexto))
            {
                icon.Visibility = Visibility.Collapsed;
                return;
            }

            if (!string.IsNullOrWhiteSpace(cedulaTexto))
            {
                if (CedulaExiste(cedulaTexto))
                {
                    icon.Visibility = Visibility.Collapsed;
                    MostrarModalUsuarioYaExiste(cedulaTexto);
                }
                else
                {
                    icon.Visibility = Visibility.Visible;
                }
            }
            else
            {
                new MensajeWindow("⚠ La cédula ingresada no es válida.", false, "Entendido", "").ShowDialog();
                icon.Visibility = Visibility.Collapsed;
            }
        }

        private bool CedulaExiste(string cedula)
        {
            using var context = AppSettings.GetContextUno();
            return context.Empleados.Any(e => e.Documento == cedula);
        }

        private void MostrarModalUsuarioYaExiste(string cedulaTexto)
        {
            var modal = new EmpleadoYaExisteDialog(cedulaTexto);
            bool? result = modal.ShowDialog();

            if (result == true && modal.Modificar)
            {
                Logger.Agregar($"✏️ Usuario con cédula {cedulaTexto} desea modificar datos.");
                {
                    using var context = AppSettings.GetContextUno();
                    var empleado = context.Empleados.FirstOrDefault(e => e.Documento == cedulaTexto);
                    if (empleado != null)
                    {
                        esModificacion = true;
                        idEmpleadoActual = empleado.IdEmpleado;

                        txtTituloFormulario.Text = "✏️ Actualización de Empleado";
                        btnRegistrar.Content = "💾 Actualizar";

                        txtNombres.Text = empleado.Nombres;
                        txtApellidos.Text = empleado.Apellidos;
                        txtCedula.Text = empleado.Documento.ToString();
                        // Cargar los datos de empresa, sede y área desde los campos del empleado
                        cbEmpresa.SelectedValue = empleado.IdEmpresa;
                        cbEmpresa_SelectionChanged(null, null);
                        cbSede.SelectedValue = empleado.IdSede;
                        cbSede_SelectionChanged(null, null);
                        cbArea.SelectedValue = empleado.IdArea;
                        Logger.Agregar($"📄 Datos de empresa/sede/área cargados para empleado {empleado.Documento}");
                        cbTipoEmpleado.SelectedValue = empleado.IdTipoEmpleado;
                        cbJefe.SelectedValue = empleado.IdJefe;

                        if (empleado.Huella != null)
                        {
                            _huellaCapturada = new DPFP.Template(new MemoryStream(empleado.Huella));
                            Logger.Agregar("🧬 Huella digital cargada desde base de datos");
                        }

                        Logger.Agregar($"📄 Datos cargados para modificar empleado ID: {empleado.IdEmpleado}");
                    }
                }
            }
        }
        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Logger.Agregar($"📥 [{SesionSistema.NombreUsuario}] Botón Registrar presionado");

            if (string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellidos.Text) ||
                string.IsNullOrWhiteSpace(txtCedula.Text) ||
                cbEmpresa.SelectedItem == null ||
                cbSede.SelectedItem == null ||
                cbArea.SelectedItem == null ||
                cbTipoEmpleado.SelectedItem == null ||
                cbJefe.SelectedItem == null)
            {
                new MensajeWindow("⚠️ Todos los campos son obligatorios.", false, "Entendido", "").ShowDialog();
                return;
            }

            if (_huellaCapturada == null)
            {
                new MensajeWindow("🛑 Debes capturar la huella antes de continuar.", false, "Entendido", "").ShowDialog();
                return;
            }

            try
            {
                string cedula = txtCedula.Text.Trim();
                byte[] huellaBytes = HuellaHelper.ConvertirTemplateABytes(_huellaCapturada);

                using var context = AppSettings.GetContextUno();
                EmpleadoModel empleado;

                if (esModificacion && idEmpleadoActual > 0)
                {
                    empleado = context.Empleados.FirstOrDefault(e => e.IdEmpleado == idEmpleadoActual);
                    if (empleado == null)
                    {
                        new MensajeWindow("❌ No se encontró el empleado para modificar.", false, "Cerrar", "").ShowDialog();
                        return;
                    }

                    empleado.Nombres = txtNombres.Text.Trim();
                    empleado.Apellidos = txtApellidos.Text.Trim();
                    empleado.Documento = cedula;
                    empleado.IdTipoEmpleado = (int)cbTipoEmpleado.SelectedValue;
                    empleado.Huella = huellaBytes;
                    empleado.FechaIngreso = DateTime.Now;
                    empleado.Estado = true;
                    // Actualizar los campos requeridos por la base de datos
                    empleado.IdEmpresa = cbEmpresa.SelectedValue is int idEmp ? idEmp : 1;
                    empleado.IdSede = cbSede.SelectedValue is int idSed ? idSed : 1;
                    empleado.IdArea = cbArea.SelectedValue is int idAr ? idAr : 1;
                    empleado.IdEnrolador = ObtenerIdEnroladorValido(context);
                    empleado.IdJefe = cbJefe.SelectedValue is int idJef ? idJef : (int?)null;

                    // Actualizar la relación en la tabla intermedia SedeCiudadEmpresaArea
                    if (cbEmpresa.SelectedValue is int idEmpresa && 
                        cbSede.SelectedValue is int idSede && 
                        cbArea.SelectedValue is int idArea)
                    {
                        CrearRelacionSedeCiudadEmpresaArea(context, idEmpresa, idSede, idArea, empleado.Documento);
                    }

                    Logger.Agregar($"✏️ [{SesionSistema.NombreUsuario}] Modificando empleado ID {empleado.IdEmpleado}");
                }
                else
                {
                    empleado = new EmpleadoModel
                    {
                        Documento = cedula,
                        Nombres = txtNombres.Text.Trim(),
                        Apellidos = txtApellidos.Text.Trim(),
                        IdTipoEmpleado = (int)cbTipoEmpleado.SelectedValue,
                        Huella = huellaBytes,
                        Estado = true,
                        FechaIngreso = DateTime.Now,
                        // Asignar valores a los campos requeridos por la base de datos
                        IdEmpresa = cbEmpresa.SelectedValue is int idEmp ? idEmp : 1,
                        IdSede = cbSede.SelectedValue is int idSed ? idSed : 1,
                        IdArea = cbArea.SelectedValue is int idAr ? idAr : 1,
                        IdEnrolador = ObtenerIdEnroladorValido(context),
                        IdJefe = cbJefe.SelectedValue is int idJef ? idJef : (int?)null
                    };

                    context.Empleados.Add(empleado);
                    context.SaveChanges();
                    idEmpleadoActual = empleado.IdEmpleado;

                    // Guardar la relación en la tabla intermedia SedeCiudadEmpresaArea
                    if (cbEmpresa.SelectedValue is int idEmpresa && 
                        cbSede.SelectedValue is int idSede && 
                        cbArea.SelectedValue is int idArea)
                    {
                        CrearRelacionSedeCiudadEmpresaArea(context, idEmpresa, idSede, idArea, empleado.Documento);
                    }

                    Logger.Agregar($"🆕 [{SesionSistema.NombreUsuario}] Nuevo empleado registrado: {empleado.Nombres} {empleado.Apellidos} ({empleado.Documento})");
                }

                // Guardar el empleado ANTES de la lógica de horarios
                context.SaveChanges();

                int idEmpleado = empleado.IdEmpleado;
                int idTipoEmpleado = (int)cbTipoEmpleado.SelectedValue;
                string nombreTipoEmpleado = context.TiposEmpleados.FirstOrDefault(t => t.Id == idTipoEmpleado)?.Nombre ?? "";
                bool esRotativo = nombreTipoEmpleado.Trim().ToLower() == "enrolado rotativo";

                                    var asignacionExistente = context.AsignacionHorarios.FirstOrDefault(a => a.Documento == empleado.Documento && a.Estado);
                bool usarHorarioEspecifico = false;

                if (asignacionExistente != null)
                {
                    string mensaje = esRotativo
                        ? "Ya existe un horario activo. ¿Deseas reemplazarlo por un horario específico o mantener el actual?"
                        : "Ya existe un horario activo. ¿Deseas reemplazarlo por un horario genérico o mantener el actual?";

                    var decision = new MensajeWindow(mensaje, true, "Reemplazar", "Mantener");
                    bool? resultado = decision.ShowDialog();

                    if (resultado == true && decision.Resultado)
                    {
                        asignacionExistente.Estado = false;
                        usarHorarioEspecifico = esRotativo;
                        Logger.Agregar($"🔁 [{SesionSistema.NombreUsuario}] Reemplazando horario activo para empleado ID {idEmpleado}");
                    }
                    else if (resultado == true && !decision.Resultado)
                    {
                        Logger.Agregar($"✅ [{SesionSistema.NombreUsuario}] Se mantuvo el horario actual para empleado ID {idEmpleado}");
                        // No hacer return aquí, continuar al final para finalizar el guardado
                    }
                    else
                    {
                        // Usuario cerró la ventana (X) - no hacer nada
                        Logger.Agregar($"❌ [{SesionSistema.NombreUsuario}] Canceló la operación de horario para empleado ID {idEmpleado}");
                        return;
                    }
                }
                else
                {
                    string mensaje = esRotativo
                        ? "¿Deseas crear un horario específico para este colaborador?"
                        : "¿Deseas crear un horario genérico para este colaborador?";

                    var decision = new MensajeWindow(mensaje, true, "Crear", "Cancelar");
                    bool? resultado = decision.ShowDialog();
                    
                    if (resultado == true && decision.Resultado)
                    {
                        usarHorarioEspecifico = esRotativo;
                    }
                    else
                    {
                        // Usuario canceló o cerró la ventana
                        Logger.Agregar($"🚫 [{SesionSistema.NombreUsuario}] Canceló la creación de horario para empleado ID {idEmpleado}");
                        return;
                    }
                }

                if (usarHorarioEspecifico)
                {
                    var ventana = new AsignarHorarioWindow();
                    if (ventana.ShowDialog() == true)
                    {
                        var asignacion = new AsignacionHorario
                        {
                            Documento = empleado.Documento,
                            FechaInicio = DateOnly.FromDateTime(DateTime.Today),
                            FechaFin = DateOnly.FromDateTime(new DateTime(DateTime.Today.Year, 12, 31)),
                            FechaCreacion = DateTime.Now,
                            CreadoPor = SesionSistema.IdUsuarioActual,
                            Estado = true,
                            TipoHorario = 2
                        };

                        context.AsignacionHorarios.Add(asignacion);
                        context.SaveChanges();

                        foreach (var dia in ventana.HorariosAsignados)
                        {
                            context.DetalleHorarios.Add(new DetalleHorario
                            {
                                IdAsignacion = asignacion.Id,
                                DiaSemana = dia.Key,
                                HoraInicio = TimeOnly.FromTimeSpan(dia.Value.Inicio),
                                HoraFin = TimeOnly.FromTimeSpan(dia.Value.Fin)
                            });
                        }

                        context.SaveChanges();
                        Logger.Agregar($"📅 [{SesionSistema.NombreUsuario}] Horario específico asignado para empleado ID {idEmpleado}");
                    }
                    else
                    {
                        Logger.Agregar($"⛔ [{SesionSistema.NombreUsuario}] Canceló ventana de horario específico");
                        return;
                    }
                }
                else
                {
                    var asignacion = new AsignacionHorario
                    {
                        Documento = empleado.Documento,
                        FechaInicio = DateOnly.FromDateTime(DateTime.Today),
                        FechaFin = DateOnly.FromDateTime(new DateTime(DateTime.Today.Year, 12, 31)),
                        FechaCreacion = DateTime.Now,
                        CreadoPor = SesionSistema.IdUsuarioActual,
                        Estado = true,
                        TipoHorario = 1
                    };

                    context.AsignacionHorarios.Add(asignacion);
                    context.SaveChanges();

                    for (int dia = 1; dia <= 7; dia++)
                    {
                        context.DetalleHorarios.Add(new DetalleHorario
                        {
                            IdAsignacion = asignacion.Id,
                            DiaSemana = dia,
                            HoraInicio = ConfiguracionSistema.ObtenerHoraInicio(dia),
                            HoraFin = ConfiguracionSistema.ObtenerHoraFin(dia)
                        });
                    }

                    context.SaveChanges();
                    Logger.Agregar($"🕒 [{SesionSistema.NombreUsuario}] Horario genérico configurable asignado para empleado ID {idEmpleado}");
                }

                // Finalizar el guardado del empleado
                FinalizarGuardado(esModificacion);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "Sin detalle interno.";
                Logger.Agregar($"❌ [{SesionSistema.NombreUsuario}] Error inesperado: {ex.Message} | Interno: {inner}");
                new MensajeWindow($"❌ Ocurrió un error:\n{ex.Message}\nDetalles: {inner}", false, "Cerrar", "").ShowDialog();
            }
        }

        private void FinalizarGuardado(bool fueModificacion)
        {
            string mensajeFinal = fueModificacion ? "Empleado actualizado correctamente." : "Empleado registrado correctamente.";
            new MensajeWindow($"✅ {mensajeFinal}", false, "Aceptar", "").ShowDialog();

            if (!fueModificacion)
            {
                var confirmacion = new MensajeWindow("¿Deseas agregar otro empleado?", true, "Sí", "No");
                if (confirmacion.ShowDialog() == true)
                {
                    LimpiarFormulario();
                }
                else
                {
                    if (Application.Current.MainWindow.FindName("MainContent") is ContentControl contenedor)
                        contenedor.Content = null;
                }
            }

            esModificacion = false;
            idEmpleadoActual = 0;
            LimpiarFormulario();
        }

        private void BtnCapturarHuella_Click(object sender, RoutedEventArgs e)
        {
            Logger.Agregar("📸 Iniciando captura de huella para registro de empleado");
            try
            {
                new CapturaHuellaService().DetenerCaptura();
            }
            catch (Exception ex)
            {
                Logger.Agregar("⚠ No se pudo detener correctamente una captura previa: " + ex.Message);
            }

            var ventanaCaptura = new CapturarHuellaWindow();
            if (ventanaCaptura.ShowDialog() == true)
            {
                var template = ventanaCaptura.ResultadoTemplate;
                var imagenHuella = ventanaCaptura.UltimaHuellaCapturada;

                if (template != null && imagenHuella != null)
                {
                    _huellaCapturada = template;
                    imgHuella.Source = imagenHuella;
                    imgHuella.Visibility = Visibility.Visible;
                    imgHuellaBorder.Visibility = Visibility.Visible;
                    
                    Logger.Agregar("✅ Huella capturada correctamente");
                    new MensajeWindow("✅ Huella capturada exitosamente", false, "Perfecto", "").ShowDialog();
                }
            }
            else
            {
                Logger.Agregar("❌ Captura de huella cancelada por el usuario");
                new MensajeWindow("❌ La captura fue cancelada.", false, "Cerrar", "").ShowDialog();
            }
        }



        private void LimpiarFormulario()
        {
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtCedula.Text = "";
            cbEmpresa.SelectedIndex = -1;
            cbSede.ItemsSource = null;
            cbArea.ItemsSource = null;
            cbTipoEmpleado.SelectedIndex = -1;
            cbJefe.SelectedIndex = -1;
            _huellaCapturada = null;
            iconCedulaCheck.Visibility = Visibility.Collapsed;
            Logger.Agregar("🧹 Formulario de empleado limpiado");
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Logger.Agregar("↩️ Usuario volvió a la pantalla principal desde RegistroEmpleado");
            if (Application.Current.MainWindow is MainWindow ventanaPrincipal &&
                ventanaPrincipal.FindName("MainContent") is ContentControl contenedor)
            {
                contenedor.Content = null;
                ventanaPrincipal.MostrarGifBienvenida();
            }
        }

        private BitmapImage ConvertirBitmapAImageSource(Bitmap bitmap)
        {
            using MemoryStream memory = new();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        // 👉 Permite solo letras
        private void SoloTexto_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsLetter);
        }

        // 👉 Convierte el texto a mayúsculas automáticamente
        private void ConvertirAMayusculas_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                int cursorPos = textBox.SelectionStart;
                string textoOriginal = textBox.Text;
                string textoMayuscula = textoOriginal.ToUpper();

                if (textoOriginal != textoMayuscula)
                {
                    textBox.Text = textoMayuscula;
                    textBox.SelectionStart = cursorPos;
                }
            }
        }

        private void CrearRelacionSedeCiudadEmpresaArea(BiometricoDbContext context, int idEmpresa, int idSede, int idArea, string documentoEmpleado)
        {
            // Buscar la relación existente o crear una nueva
            var relacionExistente = context.SedeCiudadEmpresaAreas
                .FirstOrDefault(sca => sca.IdEmpresa == idEmpresa && 
                                       sca.IdSede == idSede && 
                                       sca.IdArea == idArea);
            
            if (relacionExistente == null)
            {
                // Obtener el IdCiudad desde una relación existente o usar valor por defecto
                var relacionExistenteConCiudad = context.SedeCiudadEmpresaAreas
                    .Where(sca => sca.IdSede == idSede)
                    .FirstOrDefault();
                int idCiudad = relacionExistenteConCiudad?.IdCiudad ?? 1; // Usar IdCiudad de relación existente o 1 por defecto
                
                // Crear nueva relación si no existe
                var nuevaRelacion = new SedeCiudadEmpresaArea
                {
                    IdEmpresa = idEmpresa,
                    IdSede = idSede,
                    IdArea = idArea,
                    IdCiudad = idCiudad
                };
                context.SedeCiudadEmpresaAreas.Add(nuevaRelacion);
                context.SaveChanges();
                Logger.Agregar($"🔗 [{SesionSistema.NombreUsuario}] Nueva relación SedeCiudadEmpresaArea creada para empleado {documentoEmpleado}");
            }
        }

        private int ObtenerIdEnroladorValido(BiometricoDbContext context)
        {
            // Primero intentar usar el usuario actual de la sesión
            if (SesionSistema.IdUsuarioActual > 0)
            {
                var usuarioActual = context.Usuarios.FirstOrDefault(u => u.IdUsuario == SesionSistema.IdUsuarioActual);
                if (usuarioActual != null)
                {
                    return SesionSistema.IdUsuarioActual;
                }
            }

            // Si no hay usuario actual válido, buscar cualquier usuario activo
            var usuarioValido = context.Usuarios.FirstOrDefault(u => u.Estado == true);
            if (usuarioValido != null)
            {
                Logger.Agregar($"⚠️ Usando usuario alternativo como enrolador: {usuarioValido.NombreUsuario}");
                return usuarioValido.IdUsuario;
            }

            // Si no hay usuarios activos, usar el primer usuario disponible
            var primerUsuario = context.Usuarios.FirstOrDefault();
            if (primerUsuario != null)
            {
                Logger.Agregar($"⚠️ Usando primer usuario disponible como enrolador: {primerUsuario.NombreUsuario}");
                return primerUsuario.IdUsuario;
            }

            // Último recurso: usar ID 1 (asumiendo que existe)
            Logger.Agregar("⚠️ No se encontraron usuarios válidos, usando ID 1 como enrolador");
            return 1;
        }
    }
}

