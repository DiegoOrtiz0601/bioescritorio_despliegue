using BiomentricoHolding.Data.DataBaseRegistro_Test;
using BiomentricoHolding.Services;
using BiomentricoHolding.Utils;
using DPFP;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Media;
using EmpleadoModel = BiomentricoHolding.Data.DataBaseRegistro_Test.Empleado;

namespace BiomentricoHolding.Views.Empleado
{
    public partial class CapturaEntradaSalidaWindow : Window
    {
        private DispatcherTimer _timer;
        private readonly CapturaHuellaService _capturaService = new();
        private bool _procesandoVerificacion = false;

        public CapturaEntradaSalidaWindow()
        {
            InitializeComponent();
            Logger.Agregar("📡 Iniciando módulo de verificación de huella.");

            IniciarReloj();
            ConfigurarEventosHuella();
        }

        private void IniciarReloj()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) =>
            {
                var cultura = new CultureInfo("es-CO");
                txtReloj.Text = DateTime.Now.ToString("HH:mm:ss", cultura);
                txtFecha.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy", cultura).ToUpper();
            };
            _timer.Start();
        }

        private void ConfigurarEventosHuella()
        {
            _capturaService.Modo = ModoCaptura.Verificacion;
            _capturaService.Mensaje += MostrarMensaje;
            _capturaService.MuestraProcesada += ProcesarHuellaVerificacion;
            _capturaService.MuestraProcesadaImagen += MostrarImagenHuella;
            _capturaService.CalidadMuestraEvaluada += OnCalidadMuestraEvaluada;
            
            // VERIFICACIÓN PREVENTIVA ANTES DE INICIAR
            if (_capturaService.VerificarLector())
            {
                _capturaService.IniciarCaptura();
            }
            else
            {
                Logger.Agregar($"❌ No se pudo iniciar captura. Estado del lector: {_capturaService.EstadoLector}");
                MostrarMensaje("❌ Lector no disponible. Verifique la conexión.");
            }
        }

        private void OnCalidadMuestraEvaluada(CalidadMuestra calidad)
        {
            Dispatcher.Invoke(() =>
            {
                switch (calidad)
                {
                    case CalidadMuestra.Excelente:
                        MostrarMensaje("🌟 Calidad de huella excelente - Procesando...");
                        break;
                    case CalidadMuestra.Buena:
                        MostrarMensaje("👍 Calidad de huella buena - Continuando...");
                        break;
                    case CalidadMuestra.Aceptable:
                        MostrarMensaje("✅ Calidad de huella aceptable - Verificando...");
                        break;
                    case CalidadMuestra.Insuficiente:
                        MostrarMensaje("⚠️ Calidad insuficiente - Ajuste el dedo");
                        break;
                    case CalidadMuestra.Invalida:
                        MostrarMensaje("❌ Huella inválida - Intente nuevamente");
                        break;
                }
            });
        }

        private async void BtnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            await ReiniciarCaptura();
        }

        private void LimpiarFormulario()
        {
            txtEstadoHuella.Text = "👆 Por favor coloque su dedo en el lector";
            lblNombreEmpleado.Text = "👤 Nombre: ---";
            lblDocumento.Text = "🆔 Documento: ---";
            lblTipoMarcacion.Text = "📋 Marcación: ---";
            lblEstadoMarcacion.Text = "📊 Estado: ---";
            imgHuella.Source = null;
        }

        private void MostrarMensaje(string mensaje)
        {
            Dispatcher.Invoke(() => txtEstadoHuella.Text = mensaje);
        }

        private void MostrarImagenHuella(Bitmap imagen)
        {
            Dispatcher.Invoke(() =>
            {
                imgHuella.Source = ConvertirBitmapToImageSource(imagen);
            });
        }

        private ImageSource ConvertirBitmapToImageSource(Bitmap bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        /// <summary>
        /// Valida la configuración del sistema antes de procesar
        /// </summary>
        private bool ValidarConfiguracionSistema()
        {
            try
            {
                // Verificar que el sistema esté configurado
                if (!ConfiguracionSistema.EstaConfigurado)
                {
                    Logger.Agregar("❌ Sistema no configurado: Empresa y sede no están configuradas");
                    return false;
                }

                // Verificar que haya empleados con huellas en la base de datos
                using var db = AppSettings.GetContextUno();
                var empleadosConHuella = db.Empleados.Where(e => e.Huella != null && e.Estado == true).Count();
                
                if (empleadosConHuella == 0)
                {
                    Logger.Agregar("❌ No hay empleados con huellas registradas");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error validando configuración: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Valida datos de un empleado antes de procesar marcación
        /// </summary>
        private bool ValidarDatosEmpleado(EmpleadoModel empleado)
        {
            try
            {
                if (empleado == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(empleado.Nombres) || string.IsNullOrEmpty(empleado.Apellidos))
                {
                    return false;
                }

                if (empleado.Documento <= 0)
                {
                    return false;
                }

                if (!empleado.Estado)
                {
                    return false;
                }

                if (empleado.Huella == null || empleado.Huella.Length == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Solo registrar errores críticos de validación
                System.Diagnostics.Debug.WriteLine($"Error validando empleado: {ex.Message}");
                return false;
            }
        }

        private void ProcesarHuellaVerificacion(Sample sample)
        {
            // PREVENIR PROCESAMIENTO MÚLTIPLE
            if (_procesandoVerificacion)
            {
                Logger.Agregar("⚠ Verificación ya en proceso, ignorando nueva muestra");
                return;
            }

            // VERIFICACIÓN PREVENTIVA ANTES DE PROCESAR
            if (!_capturaService.VerificarLector())
            {
                Logger.Agregar($"❌ Lector no disponible para procesar muestra. Estado: {_capturaService.EstadoLector}");
                MostrarMensaje("❌ Lector no disponible. Verifique la conexión.");
                return;
            }

            // VALIDACIÓN DE CONFIGURACIÓN DEL SISTEMA
            if (!ValidarConfiguracionSistema())
            {
                MostrarMensaje("❌ Sistema no configurado correctamente.");
                ReproducirSonido("Sonidos/error.wav");
                return;
            }

            _procesandoVerificacion = true;
            _capturaService.DetenerCaptura();

            MensajeWindow buscandoWindow = null;

            Dispatcher.Invoke(async () =>
            {
                try
                {
                    Logger.Agregar("🧠 Procesando muestra de huella digital...");

                    var extractor = new DPFP.Processing.FeatureExtraction();
                    var feedback = DPFP.Capture.CaptureFeedback.None;
                    FeatureSet features = new FeatureSet();
                    extractor.CreateFeatureSet(sample, DPFP.Processing.DataPurpose.Verification, ref feedback, ref features);

                    if (features == null || feedback != DPFP.Capture.CaptureFeedback.Good)
                    {
                        Logger.Agregar("❌ No se pudo leer la huella correctamente.");
                        MostrarMensaje("❌ No se pudo leer la huella correctamente.");
                        ReproducirSonido("Sonidos/error.wav");

                        new MensajeWindow("❌ Lectura de huella inválida. Intente nuevamente.", 2, "error")
                        {
                            Owner = this,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }.ShowDialog();
                        await ReiniciarCaptura();
                        return;
                    }

                    buscandoWindow = new MensajeWindow("🔍 Verificando identidad en la base de datos...", false, true);
                    buscandoWindow.Show();

                    using var db = AppSettings.GetContextUno();
                    var empleados = db.Empleados.Where(e => e.Huella != null && e.Estado == true).ToList();
                    var verificador = new DPFP.Verification.Verification();
                    var resultado = new DPFP.Verification.Verification.Result();
                    
                    int empleadosVerificados = 0;
                    int empleadosValidos = 0;
                    int erroresSdk = 0;
                    EmpleadoModel empleadoEncontrado = null;

                    Logger.Agregar($"🔍 Iniciando verificación de huella en {empleados.Count} empleados registrados...");

                    foreach (var empleado in empleados)
                    {
                        empleadosVerificados++;
                        
                        try
                        {
                            // VALIDACIÓN DE DATOS DEL EMPLEADO
                            if (!ValidarDatosEmpleado(empleado))
                            {
                                continue; // Saltar empleado inválido
                            }

                            // VERIFICACIÓN PREVENTIVA DEL TEMPLATE
                            if (empleado.Huella == null || empleado.Huella.Length == 0)
                            {
                                continue; // Saltar empleado sin huella
                            }

                            empleadosValidos++;
                            var templateBD = new Template(new MemoryStream(empleado.Huella));
                            verificador.Verify(features, templateBD, ref resultado);

                            if (resultado.Verified)
                            {
                                empleadoEncontrado = empleado;
                                break; // Salir del bucle al encontrar coincidencia
                            }
                        }
                        catch (Exception ex)
                        {
                            // Si es un error específico del SDK, intentar recuperar
                            if (ex.Message.Contains("0xFFFFFFF8") || ex.Message.Contains("0xFFFFFFFE"))
                            {
                                erroresSdk++;
                                Logger.Agregar("🚨 Error crítico del SDK detectado. Intentando recuperar lector...");
                                
                                if (_capturaService.IntentarRecuperarLector())
                                {
                                    Logger.Agregar("✅ Lector recuperado. Continuando verificación...");
                                    continue; // Continuar con el siguiente empleado
                                }
                                else
                                {
                                    Logger.Agregar("❌ No se pudo recuperar el lector. Deteniendo verificación.");
                                    break; // Salir del bucle
                                }
                            }
                            
                            // Para otros errores, continuar con el siguiente empleado
                            continue;
                        }
                    }

                    // REGISTRAR RESUMEN DE LA VERIFICACIÓN
                    if (empleadoEncontrado != null)
                    {
                        Logger.Agregar($"✅ Huella verificada exitosamente: {empleadoEncontrado.Nombres} {empleadoEncontrado.Apellidos} ({empleadoEncontrado.Documento}) - Verificados: {empleadosVerificados} empleados");
                        buscandoWindow?.Close();
                        MostrarDatosEmpleado(empleadoEncontrado);
                        ReproducirSonido("Sonidos/correcto.wav");
                        Dispatcher.InvokeAsync(() => DeterminarTipoMarcacion(empleadoEncontrado));
                        return;
                    }
                    else
                    {
                        Logger.Agregar($"❌ Huella no reconocida - Verificados: {empleadosVerificados} empleados, Válidos: {empleadosValidos}, Errores SDK: {erroresSdk}");
                    }
                    buscandoWindow?.Close();

                    Dispatcher.BeginInvoke(async () =>
                    {
                        MostrarMensaje("❌ Huella no reconocida - Intente nuevamente");
                        ReproducirSonido("Sonidos/error.wav");
                        new MensajeWindow(
                            "❌ Huella no reconocida en el sistema.\n\n💡 Verifique que:\n• Su huella esté registrada\n• Coloque el mismo dedo registrado\n• El dedo esté limpio y seco", 3, "advertencia")
                        {
                            Owner = this,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }.ShowDialog();

                        await ReiniciarCaptura();
                    });
                }
                catch (Exception ex)
                {
                    Logger.Agregar($"❌ Error general en verificación: {ex.Message}");
                    buscandoWindow?.Close();
                    MostrarMensaje("❌ Error en verificación. Reintentando...");
                    await ReiniciarCaptura();
                }
                finally
                {
                    _procesandoVerificacion = false;
                }
            });
        }

        private void MostrarDatosEmpleado(EmpleadoModel empleado)
        {
            lblNombreEmpleado.Text = $"👤 Nombre: {empleado.Nombres} {empleado.Apellidos}";
            lblDocumento.Text = $"🆔 Documento: {empleado.Documento}";
            lblTipoMarcacion.Text = "⏳ Procesando marcación...";
            lblEstadoMarcacion.Text = "🔄 Verificando horario";
        }

        private void DeterminarTipoMarcacion(EmpleadoModel empleado)
        {
            Dispatcher.InvokeAsync(async() =>
            {
                try
                {
                    // VALIDACIÓN FINAL DE DATOS ANTES DE MARCAR
                    if (!ValidarDatosEmpleado(empleado))
                    {
                        MostrarMensaje("❌ Datos de empleado inválidos.");
                        await ReiniciarCaptura();
                        return;
                    }

                    using var db = AppSettings.GetContextUno();

                    var hoy = DateTime.Now;
                    var diaSemana = (int)hoy.DayOfWeek;
                    diaSemana = diaSemana == 0 ? 1 : diaSemana;

                    var asignacion = db.AsignacionHorarios
                        .FirstOrDefault(a => a.IdEmpleado == empleado.IdEmpleado && a.Estado);

                    if (asignacion == null)
                    {
                        Logger.Agregar($"⚠️ {empleado.Nombres} no tiene una asignación de horario activa.");
                        ReproducirSonido("Sonidos/advertencia.wav");
                        new MensajeWindow("⚠ No hay horario asignado para este colaborador.\nContacte al administrador.", 2, "advertencia")
                        {
                            Owner = this,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }.ShowDialog();
                        await ReiniciarCaptura();
                        return;
                    }

                    var detalle = db.DetalleHorarios
                        .FirstOrDefault(d => d.IdAsignacion == asignacion.Id && d.DiaSemana == diaSemana);

                    if (detalle == null)
                    {
                        Logger.Agregar($"⚠️ No se encontró detalle de horario para el día {diaSemana}.");

                        var ventana = new MensajeWindow("⚠ No hay horario configurado para hoy.", 3, "advertencia")
                        {
                            Owner = this,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        };
                        ventana.Closed += async (s, e) =>
                        {
                            await ReiniciarCaptura();
                        };
                        ventana.ShowDialog();
                        return;
                    }

                    TimeOnly horaActual = TimeOnly.FromDateTime(hoy);
                    TimeOnly entrada = detalle.HoraInicio;
                    TimeOnly salida = detalle.HoraFin;

                    var yaMarcoHoy = db.Marcaciones.Any(m =>
                        m.IdEmpleado == empleado.IdEmpleado &&
                        m.FechaHora.Date == hoy.Date);

                    int tipoMarcacion;
                    string tipoTexto;
                    
                    // Si está dentro del rango de salida o después de la salida → Salida
                    if (horaActual >= salida.AddHours(-1))
                    {
                        tipoMarcacion = 2;
                        tipoTexto = "🚪 Salida";
                    }
                    else if (!yaMarcoHoy)
                    {
                        tipoMarcacion = 1;
                        tipoTexto = "✅ Entrada";
                    }
                    else
                    {
                        tipoMarcacion = 3;
                        tipoTexto = "⚠️ Novedad";
                        Logger.Agregar($"⚠️ {empleado.Nombres} realizó una marcación fuera de horario. Se registrará como NOVEDAD.");
                    }

                    var cincoMinutosAtras = hoy.AddMinutes(-5);

                    var ultima = db.Marcaciones
                    .Where(m => m.IdEmpleado == empleado.IdEmpleado && m.FechaHora >= cincoMinutosAtras)
                     .OrderByDescending(m => m.FechaHora)
                    .FirstOrDefault();

                    if (ultima != null)
                    {
                        var diferencia = hoy - ultima.FechaHora;
                        string mensaje = $"⚠ {empleado.Nombres} ya marcó hace {diferencia.Minutes} min {diferencia.Seconds} seg.\n⏳ Debe esperar 5 minutos.";
                        Logger.Agregar(mensaje);
                        ReproducirSonido("Sonidos/advertencia.wav");

                        new MensajeWindow(mensaje, 2, "advertencia")
                        {
                            Owner = this,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }.ShowDialog();

                        await ReiniciarCaptura();
                        return;
                    }

                    if (!ConfiguracionSistema.EstaConfigurado)
                    {
                        new MensajeWindow("⚠ La empresa y sede no están configuradas.\nNo se puede registrar la marcación.", 3, "error").ShowDialog();
                        return;
                    }

                    var marcacion = new Marcacione
                    {
                        IdEmpleado = empleado.IdEmpleado,
                        FechaHora = hoy,
                        IdEmpresa = empleado.IdEmpresa,
                        IdSede = ConfiguracionSistema.IdSedeActual.Value,
                        IdTipoMarcacion = tipoMarcacion,
                        IdAsignacion = asignacion.Id
                    };

                    db.Marcaciones.Add(marcacion);
                    db.SaveChanges();

                    lblTipoMarcacion.Text = $"Marcación: {tipoTexto}";
                    lblEstadoMarcacion.Text = "🎉 Registrado Exitosamente";

                    Logger.Agregar($"📝 {tipoTexto} registrada para {empleado.Nombres} ({empleado.Documento})");

                    var ventanaConfirmacion = new MensajeWindow($"🎉 ¡Marcación Registrada!\n\n📋 Tipo: {tipoTexto}\n🕐 Hora: {hoy:HH:mm:ss}\n📅 Fecha: {hoy:dd/MM/yyyy}", 3);
                    ventanaConfirmacion.Closed += async (s, e) =>
                    {
                        await ReiniciarCaptura();
                    };
                    ventanaConfirmacion.Show();
                }
                catch (Exception ex)
                {
                    Logger.Agregar($"❌ Error en marcación: {ex.Message}");
                    MostrarMensaje("❌ Error al registrar marcación.");
                    LimpiarFormulario();
                }
                finally
                {
                    // VERIFICACIÓN PREVENTIVA ANTES DE REINICIAR
                    if (_capturaService.VerificarLector())
                    {
                        _capturaService.IniciarCaptura();
                    }
                    else
                    {
                        Logger.Agregar("❌ No se pudo reiniciar captura. Lector no disponible.");
                        MostrarMensaje("❌ Lector no disponible. Verifique la conexión.");
                    }
                }
            });
        }
        
        private async Task ReiniciarCaptura()
        {
            LimpiarFormulario();
            await Task.Delay(500);
            
            // VERIFICACIÓN PREVENTIVA ANTES DE REINICIAR
            if (_capturaService.VerificarLector())
            {
                _capturaService.IniciarCaptura();
            }
            else
            {
                Logger.Agregar("❌ No se pudo reiniciar captura. Lector no disponible.");
                MostrarMensaje("❌ Lector no disponible. Verifique la conexión.");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _capturaService.DetenerCaptura();
            _capturaService.Dispose(); // Liberar recursos correctamente
            base.OnClosed(e);
        }

        private void ReproducirSonido(string archivo)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(archivo);
                player.Play();
            }
            catch (Exception ex)
            {
                Logger.Agregar("Error al reproducir sonido: " + ex.Message);
            }
        }
    }
}
