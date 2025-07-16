using BiomentricoHolding.Services;
using DPFP;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BiomentricoHolding.Views.Empleado
{
    public partial class CapturarHuellaWindow : Window
    {
        private readonly CapturaHuellaService capturaService;

        public Template ResultadoTemplate { get; private set; }

        public ImageSource UltimaHuellaCapturada { get; private set; }

        public CapturarHuellaWindow()
        {
            InitializeComponent();

            capturaService = new CapturaHuellaService();

            capturaService.DetenerCaptura(); // 🛑 Siempre detener captura previa
            capturaService.Modo = ModoCaptura.Registro;

            capturaService.Mensaje += MostrarMensajeTexto;
            capturaService.TemplateGenerado += HuellaCapturada;
            capturaService.MuestraProcesadaImagen += DibujarHuella;
            capturaService.IntentoFallido += MostrarFalloYReintentar;

            Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(500);
                capturaService.IniciarCaptura(); // ✅ Iniciar después de espera
            });
        }

        private void MostrarMensajeTexto(string mensaje)
        {
            Dispatcher.Invoke(() =>
            {
                txtEstado.Text = mensaje;

                // Mostrar ventana de mensaje para errores importantes
                if (mensaje.StartsWith("❌"))
                {
                    new MensajeWindow(mensaje, false, "Entendido", "").ShowDialog();
                }
                else if (mensaje.Contains("no coinciden") || mensaje.Contains("no es clara"))
                {
                    new MensajeWindow(mensaje, false, "Reintentar", "").ShowDialog();
                }
                else if (mensaje.Contains("Error del lector") || mensaje.Contains("Error técnico"))
                {
                    new MensajeWindow(mensaje, false, "Cerrar", "").ShowDialog();
                }
            });
        }

        private void MostrarAlerta(string mensaje)
        {
            Dispatcher.Invoke(() =>
            {
                new MensajeWindow(mensaje, false, "Aceptar", "").ShowDialog();
            });
        }

        private void HuellaCapturada(Template template)
        {
            ResultadoTemplate = template;

            Dispatcher.Invoke(() =>
            {
                // No cerrar automáticamente, solo mostrar confirmación
                txtEstado.Text = "✅ Huella capturada correctamente. Presiona 'Confirmar' para continuar.";
                
                // Habilitar botón de confirmar
                btnConfirmar.Visibility = Visibility.Visible;
                btnConfirmar.IsEnabled = true;
                
                // Detener captura ya que tenemos el template
                capturaService.DetenerCaptura();
            });
        }

        private void DibujarHuella(Bitmap bmp)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    IntPtr hBitmap = bmp.GetHbitmap();

                    var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight(100, 100)
                    );

                    UltimaHuellaCapturada = bitmapSource;

                    var image = new System.Windows.Controls.Image
                    {
                        Source = bitmapSource,
                        Width = 100,
                        Height = 100,
                        Margin = new Thickness(5)
                    };

                    panelHuellas.Children.Add(image);

                    DeleteObject(hBitmap);
                }
                catch (Exception ex)
                {
                    MostrarAlerta("❌ Error al mostrar imagen: " + ex.Message);
                }
            });
        }

        private void MostrarFalloYReintentar()
        {
            Dispatcher.Invoke(() =>
            {
                new MensajeWindow("🛑 Las huellas capturadas no coinciden entre sí.\n\n💡 Recomendaciones:\n• Asegúrese de colocar el mismo dedo en todas las capturas\n• Limpie el dedo y el lector antes de intentar\n• Mantenga el dedo firme y centrado en el lector\n• Intente con otro dedo si el problema persiste", false, "Reintentar", "").ShowDialog();
                panelHuellas.Children.Clear();
                txtEstado.Text = "Coloca tu dedo nuevamente en el lector.";
            });
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            capturaService.DetenerCaptura(); // ✅ Liberar el lector
            this.Close();
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            // Cerrar la ventana y retornar el template para verificación
            this.DialogResult = true;
            this.Close();
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        protected override void OnClosed(EventArgs e)
        {
            capturaService.DetenerCaptura(); // Seguridad adicional al cerrar
            base.OnClosed(e);
        }
    }
}
