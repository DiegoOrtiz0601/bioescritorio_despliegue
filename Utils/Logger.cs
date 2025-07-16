using System.IO;
using System.Text;
using System.Windows.Threading;

namespace BiomentricoHolding.Utils
{
    public static class Logger
    {
        private static readonly StringBuilder _contenido = new();
        private static readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_sistema.txt");

        public static event Action? LogActualizado;

        static Logger()
        {
            // Cargar contenido desde archivo al iniciar
            if (File.Exists(_logFilePath))
            {
                var contenidoInicial = File.ReadAllText(_logFilePath);
                _contenido.Append(contenidoInicial);
            }
        }

        public static void Agregar(string mensaje)
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensaje}";
            _contenido.AppendLine(log);

            // También guardar directamente en archivo
            try
            {
                File.AppendAllText(_logFilePath, log + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Silenciar errores de escritura
                System.Diagnostics.Debug.WriteLine("Error al escribir en el log: " + ex.Message);
            }

            // Invocar evento de forma thread-safe
            try
            {
                if (LogActualizado != null)
                {
                    // Si estamos en el hilo de la UI, invocar directamente
                    if (System.Windows.Application.Current?.Dispatcher != null && 
                        System.Windows.Application.Current.Dispatcher.CheckAccess())
                    {
                        LogActualizado.Invoke();
                    }
                    else
                    {
                        // Si estamos en otro hilo, invocar en el hilo de la UI
                        System.Windows.Application.Current?.Dispatcher?.BeginInvoke(DispatcherPriority.Normal, LogActualizado);
                    }
                }
            }
            catch (Exception ex)
            {
                // Silenciar errores de invocación de eventos
                System.Diagnostics.Debug.WriteLine("Error al invocar evento LogActualizado: " + ex.Message);
            }
        }

        public static string ObtenerContenido() => _contenido.ToString();

        public static void GuardarEnArchivo(string ruta)
        {
            File.WriteAllText(ruta, _contenido.ToString());
        }

        public static void Limpiar()
        {
            _contenido.Clear();
            try
            {
                if (File.Exists(_logFilePath))
                {
                    File.Delete(_logFilePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar archivo de log: " + ex.Message);
            }

            // Invocar evento de forma thread-safe
            try
            {
                if (LogActualizado != null)
                {
                    // Si estamos en el hilo de la UI, invocar directamente
                    if (System.Windows.Application.Current?.Dispatcher != null && 
                        System.Windows.Application.Current.Dispatcher.CheckAccess())
                    {
                        LogActualizado.Invoke();
                    }
                    else
                    {
                        // Si estamos en otro hilo, invocar en el hilo de la UI
                        System.Windows.Application.Current?.Dispatcher?.BeginInvoke(DispatcherPriority.Normal, LogActualizado);
                    }
                }
            }
            catch (Exception ex)
            {
                // Silenciar errores de invocación de eventos
                System.Diagnostics.Debug.WriteLine("Error al invocar evento LogActualizado: " + ex.Message);
            }
        }
    }
}
