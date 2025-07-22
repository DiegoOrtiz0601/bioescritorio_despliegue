using BiomentricoHolding.Data;
using BiomentricoHolding.Utils;
using BiomentricoHolding.Views;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace BiomentricoHolding.Views.Configuracion
{
    public partial class ConfiguracionControl : UserControl
    {
        private readonly BiometricoDbContext _context = AppSettings.GetContextUno();

        public ConfiguracionControl()
        {
            InitializeComponent();
            ConfiguracionSistema.CargarConfiguracion();
            CargarEmpresasDesdeBD();
            
            // Cargar configuración actual después de que se hayan cargado las empresas
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MostrarConfiguracionActual();
            }));
            
            MostrarLogEnPantalla();
            Logger.LogActualizado += MostrarLogEnPantalla;
        }

        private void CargarEmpresasDesdeBD()
        {
            try
            {
                var empresas = _context.Empresas.OrderBy(e => e.Nombre).ToList();
                cmbEmpresas.ItemsSource = empresas;
                cmbEmpresas.DisplayMemberPath = "Nombre";
                cmbEmpresas.SelectedValuePath = "IdEmpresa";
                
                // Configurar evento de cambio de empresa
                cmbEmpresas.SelectionChanged += CmbEmpresas_SelectionChanged;
                
                Logger.Agregar("✅ Empresas cargadas correctamente");
            }
            catch (Exception ex)
            {
                Logger.Agregar("❌ Error al cargar las empresas: " + ex.Message);
                MostrarLogEnPantalla();
                MostrarToast("❌ Error al cargar las empresas desde la base de datos");
            }
        }

        private void CmbEmpresas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEmpresas.SelectedValue is int idEmpresa)
            {
                try
                {
                    // Obtener sedes a través de la tabla intermedia SedeCiudadEmpresaArea
                    var sedes = _context.SedeCiudadEmpresaAreas
                        .Where(sca => sca.IdEmpresa == idEmpresa)
                        .Select(sca => sca.IdSedeNavigation)
                        .Where(s => s.Estado == true || s.Estado == null) // Incluir null como válido
                        .Distinct()
                        .OrderBy(s => s.Nombre)
                        .ToList();

                    cmbSedes.ItemsSource = sedes;
                    cmbSedes.DisplayMemberPath = "Nombre";
                    cmbSedes.SelectedValuePath = "IdSede";
                    
                    // Solo limpiar selección si no estamos cargando la configuración inicial
                    if (!ConfiguracionSistema.EstaConfigurado || ConfiguracionSistema.IdEmpresaActual != idEmpresa)
                    {
                        cmbSedes.SelectedIndex = -1;
                    }
                    
                    Logger.Agregar($"✅ Sedes cargadas para empresa ID {idEmpresa}");
                }
                catch (Exception ex)
                {
                    Logger.Agregar($"❌ Error al cargar sedes para empresa {idEmpresa}: {ex.Message}");
                    MostrarToast("❌ Error al cargar las sedes");
                }
            }
        }

        private void MostrarConfiguracionActual()
        {
            if (ConfiguracionSistema.EstaConfigurado)
            {
                try
                {
                    // Seleccionar empresa actual
                    Logger.Agregar($"🔍 Intentando seleccionar empresa: {ConfiguracionSistema.IdEmpresaActual} - {ConfiguracionSistema.NombreEmpresaActual}");
                    cmbEmpresas.SelectedValue = ConfiguracionSistema.IdEmpresaActual;
                    
                    // Cargar sedes de la empresa actual
                    if (ConfiguracionSistema.IdEmpresaActual.HasValue)
                    {
                        // Obtener sedes a través de la tabla intermedia SedeCiudadEmpresaArea
                        var sedes = _context.SedeCiudadEmpresaAreas
                            .Where(sca => sca.IdEmpresa == ConfiguracionSistema.IdEmpresaActual.Value)
                            .Select(sca => sca.IdSedeNavigation)
                            .Where(s => s.Estado == true || s.Estado == null) // Incluir null como válido
                            .Distinct()
                            .OrderBy(s => s.Nombre)
                            .ToList();

                        cmbSedes.ItemsSource = sedes;
                        cmbSedes.DisplayMemberPath = "Nombre";
                        cmbSedes.SelectedValuePath = "IdSede";
                        Logger.Agregar($"🔍 Sedes cargadas para configuración: {sedes.Count} sedes");
                        
                        // Seleccionar sede actual después de un pequeño delay para asegurar que las sedes se cargaron
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                Logger.Agregar($"🔍 Intentando seleccionar sede: {ConfiguracionSistema.IdSedeActual} - {ConfiguracionSistema.NombreSedeActual}");
                                cmbSedes.SelectedValue = ConfiguracionSistema.IdSedeActual;
                                Logger.Agregar($"✅ Sede seleccionada: {ConfiguracionSistema.NombreSedeActual}");
                            }
                            catch (Exception ex)
                            {
                                Logger.Agregar($"⚠️ No se pudo seleccionar la sede automáticamente: {ex.Message}");
                            }
                        }));
                    }
                    
                    Logger.Agregar($"✅ Configuración actual cargada: {ConfiguracionSistema.NombreEmpresaActual} - {ConfiguracionSistema.NombreSedeActual}");
                }
                catch (Exception ex)
                {
                    Logger.Agregar($"❌ Error al cargar configuración actual: {ex.Message}");
                }
            }
        }

        private void BtnGuardarConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            // VALIDACIÓN
            if (cmbEmpresas.SelectedItem is not Empresa empresa)
            {
                MostrarToast("⚠️ Por favor selecciona una empresa antes de guardar.");
                return;
            }

            if (cmbSedes.SelectedItem is not Sede sede)
            {
                MostrarToast("⚠️ Por favor selecciona una sede antes de guardar.");
                return;
            }

            // GUARDAR CONFIGURACIÓN
            ConfiguracionSistema.EstablecerConfiguracion(
                empresa.IdEmpresa,
                empresa.Nombre,
                sede.IdSede,
                sede.Nombre
            );

            Logger.Agregar($"✅ Configuración guardada: {empresa.Nombre} - {sede.Nombre}");
            MostrarLogEnPantalla();

            MostrarToast($"✅ Configuración guardada correctamente: {empresa.Nombre} - {sede.Nombre}");
        }

        private void BtnConfigurarHorario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Agregar("🕒 Abriendo configuración de horario por defecto...");
                MostrarLogEnPantalla();

                var ventanaHorario = new ConfiguracionHorarioWindow();
                var resultado = ventanaHorario.ShowDialog();

                if (resultado == true)
                {
                    Logger.Agregar("✅ Configuración de horario actualizada correctamente");
                    MostrarToast("✅ Configuración de horario guardada correctamente");
                }
                else
                {
                    Logger.Agregar("❌ Configuración de horario cancelada por el usuario");
                }

                MostrarLogEnPantalla();
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al abrir configuración de horario: {ex.Message}");
                MostrarToast("❌ Error al abrir configuración de horario");
                MostrarLogEnPantalla();
            }
        }

        private void BtnDescargarLog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "log_sistema.txt",
                Filter = "Archivo de texto (*.txt)|*.txt"
            };

            if (dlg.ShowDialog() == true)
            {
                Logger.GuardarEnArchivo(dlg.FileName);
                MostrarToast("📥 Log del sistema descargado correctamente");
            }
        }

        private void MostrarLogEnPantalla()
        {
            try
            {
                // Verificar que estamos en el hilo correcto
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.BeginInvoke(new Action(MostrarLogEnPantalla));
                    return;
                }

                txtLog.Text = Logger.ObtenerContenido();
                txtLog.ScrollToEnd();
            }
            catch (Exception ex)
            {
                // Silenciar errores de actualización de UI
                System.Diagnostics.Debug.WriteLine("Error al actualizar log en pantalla: " + ex.Message);
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Logger.LogActualizado -= MostrarLogEnPantalla;

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MostrarGifBienvenida();
            }
        }

        private async void MostrarToast(string mensaje)
        {
            try
            {
                // Verificar que estamos en el hilo correcto
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.BeginInvoke(new Action(() => MostrarToast(mensaje)));
                    return;
                }

                ToastMessage.Text = mensaje;
                
                // Determinar el tipo de mensaje basado en el emoji
                if (mensaje.StartsWith("✅"))
                {
                    ToastBorder.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#059669")); // Verde
                    ToastIcon.Text = "✅";
                }
                else if (mensaje.StartsWith("⚠️"))
                {
                    ToastBorder.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#D97706")); // Naranja
                    ToastIcon.Text = "⚠️";
                }
                else if (mensaje.StartsWith("❌"))
                {
                    ToastBorder.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC2626")); // Rojo
                    ToastIcon.Text = "❌";
                }
                else if (mensaje.StartsWith("📥") || mensaje.StartsWith("🧹"))
                {
                    ToastBorder.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#3B82F6")); // Azul
                    ToastIcon.Text = "ℹ️";
                }
                else
                {
                    ToastBorder.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#059669")); // Verde por defecto
                    ToastIcon.Text = "ℹ️";
                }

                ToastContainer.Visibility = Visibility.Visible;

                // Ocultar después de 3 segundos
                await Task.Delay(3000);
                ToastContainer.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al mostrar toast: " + ex.Message);
            }
        }

        private void BtnLimpiarLog_Click(object sender, RoutedEventArgs e)
        {
            Logger.Limpiar();
            MostrarLogEnPantalla();
            MostrarToast("🧹 Log del sistema limpiado correctamente");
        }
    }
}
