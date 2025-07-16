using BiomentricoHolding.Data.DataBaseRegistro_Test;
using BiomentricoHolding.Utils;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace BiomentricoHolding.Views.Configuracion
{
    public partial class ConfiguracionControl : UserControl
    {
        private readonly DataBaseRegistro_TestDbContext _context = AppSettings.GetContextUno();

        public ConfiguracionControl()
        {
            InitializeComponent();
            ConfiguracionSistema.CargarConfiguracion();
            CargarSedesDesdeBD();
            MostrarConfiguracionActual();
            MostrarLogEnPantalla();
            Logger.LogActualizado += MostrarLogEnPantalla;
        }

        private void CargarSedesDesdeBD()
        {
            try
            {
                var sedes = _context.Sedes.OrderBy(s => s.Nombre).ToList();
                cmbSedes.ItemsSource = sedes;
                cmbSedes.DisplayMemberPath = "Nombre";
                cmbSedes.SelectedValuePath = "IdSede";
            }
            catch (Exception ex)
            {
                Logger.Agregar("❌ Error al cargar las sedes: " + ex.Message);
                MostrarLogEnPantalla();
                MostrarToast("❌ Error al cargar las sedes desde la base de datos");
            }
        }

        private void MostrarConfiguracionActual()
        {
            if (ConfiguracionSistema.EstaConfigurado)
            {
                cmbSedes.SelectedValue = ConfiguracionSistema.IdSedeActual;
                Logger.Agregar($"✔️ Sede actual cargada: {ConfiguracionSistema.NombreSedeActual}");
            }
        }

        private void BtnGuardarSede_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSedes.SelectedItem is not Sede sedeSeleccionada)
            {
                MostrarToast("⚠️ Por favor selecciona una sede antes de guardar.");
                return;
            }

            ConfiguracionSistema.EstablecerConfiguracion(
                ConfiguracionSistema.IdEmpresaActual ?? 0,
                ConfiguracionSistema.NombreEmpresaActual ?? "Empresa desconocida",
                sedeSeleccionada.IdSede,
                sedeSeleccionada.Nombre
            );

            Logger.Agregar($"✅ Sede guardada: {sedeSeleccionada.Nombre}");
            MostrarLogEnPantalla();

            MostrarToast($"✅ Sede '{sedeSeleccionada.Nombre}' guardada correctamente");
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
                    ToastBorder.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2563EB")); // Azul
                    ToastIcon.Text = mensaje.StartsWith("📥") ? "📥" : "🧹";
                }
                else
                {
                    ToastBorder.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#059669")); // Verde por defecto
                    ToastIcon.Text = "✅";
                }
                
                ToastContainer.Visibility = Visibility.Visible;

                await Task.Delay(3000); // Espera 3 segundos

                ToastContainer.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                // Silenciar errores de actualización de UI
                System.Diagnostics.Debug.WriteLine("Error al mostrar toast: " + ex.Message);
            }
        }
        private void BtnLimpiarLog_Click(object sender, RoutedEventArgs e)
        {
            var resultado = MessageBox.Show("¿Estás seguro que deseas limpiar el log del sistema?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                Logger.Limpiar();
                MostrarLogEnPantalla();
                MostrarToast("🧹 Log del sistema limpiado correctamente");
            }
        }

    }
}
