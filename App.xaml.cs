using BiomentricoHolding.Utils;
using BiomentricoHolding.Views.Configuracion;
using BiomentricoHolding.Views;
using System.Windows;
using System.IO;

namespace BiomentricoHolding
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                ShutdownMode = ShutdownMode.OnExplicitShutdown;

                // 1. Intentar cargar configuración
                ConfiguracionSistema.CargarConfiguracion();

                // 2. Si no hay configuración, abrir ventana de bienvenida
                if (!ConfiguracionSistema.EstaConfigurado)
                {

                    var bienvenida = new BienvenidaInicialWindow();
                    MainWindow = bienvenida;
                    bool? resultado = bienvenida.ShowDialog();

                    // 3. Volver a cargar configuración por si fue guardada dentro del flujo
                    ConfiguracionSistema.CargarConfiguracion();



                    // 4. Si aún no está configurado, cerrar la app
                    if (resultado != true || !ConfiguracionSistema.EstaConfigurado)
                    {
                        new MensajeWindow("❌ El sistema no puede iniciar sin configuración.", false, "Cerrar", "").ShowDialog();
                        return;
                    }
                }

                // 5. Cargar MainWindow si todo está bien



                MainWindow = new MainWindow();
                MainWindow.Show();

                DispatcherUnhandledException += (sender, args) =>
                {
                    new MensajeWindow("💥 Excepción no controlada:\n\n" +
                   $"Mensaje: {args.Exception.Message}\n\n" +
                   $"StackTrace:\n{args.Exception.StackTrace}",
                   false, "Cerrar", "").ShowDialog();
                    args.Handled = true;
                };
            }
            catch (Exception ex)
            {
                new MensajeWindow($"Error crítico: {ex.Message}", false, "Cerrar", "").ShowDialog();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                // La configuración se guarda automáticamente en GuardarConfiguracion()
                // No necesitamos hacer nada adicional aquí
                Logger.Agregar("✅ Aplicación cerrada correctamente");
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al cerrar la aplicación: {ex.Message}");
            }

            base.OnExit(e);
        }
    }
}
