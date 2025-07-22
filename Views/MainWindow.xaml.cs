using BiomentricoHolding.Utils;
using BiomentricoHolding.Views;
using BiomentricoHolding.Views.Configuracion;
using BiomentricoHolding.Views.Empleado;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using WpfAnimatedGif;
using BiomentricoHolding.Views.Reportes;
using System.Reflection;
using BiomentricoHolding.Data;

namespace BiomentricoHolding
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.Loaded += MainWindow_Loaded; // ⚡ Ejecutar la lógica principal cuando la ventana esté cargada completamente

                MostrarVersionAplicativo(); // ⚡ Mostrar versión en la pantalla
                RegistrarVersionSiNoExiste(); // ⚡ Registrar versión en la base de datos si no existe
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error en el constructor: " + ex.ToString());
                throw;
            }
        }
        private void RegistrarVersionSiNoExiste()
        {
            try
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                if (version == null) return;

                string versionTexto = $"{version.Major}.{version.Minor}.{version.Build}";

                // TODO: VersionSistema no existe en el nuevo contexto
                // using var db = AppSettings.GetContextUno();
                // bool existe = db.VersionSistema.Any(v => v.NumeroVersion == versionTexto);

                // if (!existe)
                // {
                //     db.VersionSistema.Add(new VersionSistema
                //     {
                //         NumeroVersion = versionTexto,
                //         FechaPublicacion = DateTime.Now,
                //         Comentarios = "Registro automático"
                //     });
                //     db.SaveChanges();
                //     Logger.Agregar($"✅ Versión registrada automáticamente: {versionTexto}");
                // }
            }
            catch (Exception ex)
            {
                Logger.Agregar("❌ Error registrando versión: " + ex.Message);
            }
        }
        

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Cargar el GIF animado
                string gifPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "logo.gif");

                if (File.Exists(gifPath) && imgBienvenida != null)
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(gifPath, UriKind.Absolute);
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    ImageBehavior.SetAnimatedSource(imgBienvenida, image);
                    ImageBehavior.SetRepeatBehavior(imgBienvenida, System.Windows.Media.Animation.RepeatBehavior.Forever);
                    ImageBehavior.SetAutoStart(imgBienvenida, true);

                    imgBienvenida.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("❌ No se encontró el GIF o imgBienvenida es null.");
                    if (imgBienvenida != null)
                        imgBienvenida.Visibility = Visibility.Collapsed;
                }

                // Iniciar animaciones de fondo
                IniciarAnimacionesFondo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ No se pudo cargar el GIF animado.\n\n" + ex.Message, "Error al cargar logo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IniciarAnimacionesFondo()
        {
            try
            {
                // Partícula 1 - Movimiento suave y lento
                var animacion1X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 80,
                    Duration = TimeSpan.FromSeconds(25),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion1Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = -60,
                    Duration = TimeSpan.FromSeconds(30),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Partícula 2 - Movimiento circular suave
                var animacion2X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(35),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion2Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 80,
                    Duration = TimeSpan.FromSeconds(28),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Partícula 3 - Movimiento ondulante
                var animacion3X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = -70,
                    Duration = TimeSpan.FromSeconds(22),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion3Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = -40,
                    Duration = TimeSpan.FromSeconds(18),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Partícula 4 - Movimiento muy lento
                var animacion4X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 120,
                    Duration = TimeSpan.FromSeconds(40),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion4Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 90,
                    Duration = TimeSpan.FromSeconds(32),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Partícula 5 - Movimiento sutil
                var animacion5X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 50,
                    Duration = TimeSpan.FromSeconds(20),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion5Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = -30,
                    Duration = TimeSpan.FromSeconds(25),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Aplicar animaciones en múltiples ejes
                TransformCirculo1.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion1X);
                TransformCirculo1.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion1Y);
                
                TransformCirculo2.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion2X);
                TransformCirculo2.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion2Y);
                
                TransformCirculo3.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion3X);
                TransformCirculo3.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion3Y);
                
                TransformCirculo4.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion4X);
                TransformCirculo4.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion4Y);
                
                TransformCirculo5.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion5X);
                TransformCirculo5.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion5Y);

                // Círculo 6 - Movimiento flotante suave
                var animacion6X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = -70,
                    Duration = TimeSpan.FromSeconds(13),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion6Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 100,
                    Duration = TimeSpan.FromSeconds(17),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Círculo 7 - Movimiento espiral
                var animacion7X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 160,
                    Duration = TimeSpan.FromSeconds(18),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.QuarticEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion7Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = -80,
                    Duration = TimeSpan.FromSeconds(22),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.ElasticEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Círculo 8 - Movimiento pulsante
                var animacion8X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = 110,
                    Duration = TimeSpan.FromSeconds(7),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.BounceEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion8Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0,
                    To = -90,
                    Duration = TimeSpan.FromSeconds(11),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.QuadraticEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Aplicar animaciones de los nuevos círculos
                TransformCirculo6.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion6X);
                TransformCirculo6.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion6Y);
                
                TransformCirculo7.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion7X);
                TransformCirculo7.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion7Y);
                
                TransformCirculo8.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion8X);
                TransformCirculo8.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion8Y);

                // Animaciones para partículas adicionales (más simples y suaves)
                var animacion9X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = 60, Duration = TimeSpan.FromSeconds(26),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };
                var animacion9Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = -45, Duration = TimeSpan.FromSeconds(33),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion10X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = -80, Duration = TimeSpan.FromSeconds(29),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };
                var animacion10Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = 70, Duration = TimeSpan.FromSeconds(24),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion11X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = 40, Duration = TimeSpan.FromSeconds(31),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };
                var animacion11Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = -55, Duration = TimeSpan.FromSeconds(27),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                var animacion12X = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = -65, Duration = TimeSpan.FromSeconds(23),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };
                var animacion12Y = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0, To = 85, Duration = TimeSpan.FromSeconds(34),
                    RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever,
                    AutoReverse = true,
                    EasingFunction = new System.Windows.Media.Animation.SineEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut }
                };

                // Aplicar animaciones de partículas adicionales
                TransformCirculo9.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion9X);
                TransformCirculo9.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion9Y);
                TransformCirculo10.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion10X);
                TransformCirculo10.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion10Y);
                TransformCirculo11.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion11X);
                TransformCirculo11.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion11Y);
                TransformCirculo12.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animacion12X);
                TransformCirculo12.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animacion12Y);



                Logger.Agregar("✨ Animaciones de partículas iniciadas correctamente");
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error iniciando animaciones de fondo: {ex.Message}");
            }
        }

        private void BtnRegistrarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            var login = new MiniLoginWindow();
            bool? resultado = login.ShowDialog();

            if (resultado == true && login.AccesoPermitido)
            {
                MainContent.Content = new RegistrarEmpleado();
                imgBienvenida.Visibility = Visibility.Collapsed;
                MainContent.Visibility = Visibility.Visible;
            }
            
           
                else
                {
                    Logger.Agregar("🚫 Acceso denegado a Configuración");

                    var mensaje = new MensajeWindow("⚠️ Acceso denegado o cancelado.", false, "Cerrar", "")
                    {
                        Owner = this // 👉 Asegura que la ventana salga al frente
                    };

                    mensaje.ShowDialog();
                }
            
        }

        private void BtnControlAcceso_Click(object sender, RoutedEventArgs e)
        {
            CapturaEntradaSalidaWindow ventanaCaptura = new CapturaEntradaSalidaWindow();
            ventanaCaptura.ShowDialog();
        }





        private void BtnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            var login = new MiniLoginWindow();
            bool? resultado = login.ShowDialog();

            if (resultado == true && login.AccesoPermitido)
            {
                Logger.Agregar("🔐 Acceso autorizado a Configuración");
                MainContent.Content = new ConfiguracionControl();
                imgBienvenida.Visibility = Visibility.Collapsed;
                MainContent.Visibility = Visibility.Visible;
            }
            else
            {
                Logger.Agregar("🚫 Acceso denegado a Configuración");

                var mensaje = new MensajeWindow("⚠️ Acceso denegado o cancelado.", false, "Cerrar", "")
                {
                    Owner = this // 👉 Asegura que la ventana salga al frente
                };

                mensaje.ShowDialog();
            }
        }




        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public void MostrarGifBienvenida()
        {
            if (FindName("imgBienvenida") is Image img)
            {
                string rutaGif = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "logo.gif");

                if (File.Exists(rutaGif))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(rutaGif, UriKind.Absolute);
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    ImageBehavior.SetAnimatedSource(img, image);
                    img.Visibility = Visibility.Visible;
                }

                if (FindName("MainContent") is ContentControl contenedor)
                {
                    contenedor.Content = null;
                    contenedor.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void MostrarVersionAplicativo()
        {
            try
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                if (version != null)
                {
                    string versionTexto = $"{version.Major}.{version.Minor}.{version.Build}";
                    lblVersion.Text = $"Versión: {versionTexto}";
                }

                // Obtener fecha de publicación desde la fecha de compilación del assembly
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var buildDate = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyMetadataAttribute), false)
                    .OfType<System.Reflection.AssemblyMetadataAttribute>()
                    .FirstOrDefault(a => a.Key == "BuildDate");

                if (buildDate != null && DateTime.TryParse(buildDate.Value, out DateTime fechaPublicacion))
                {
                    lblFechaPublicacion.Text = $"Fecha Publicación: {fechaPublicacion:dd/MM/yyyy}";
                }
                else
                {
                    // Si no hay fecha de compilación, usar la fecha actual
                    lblFechaPublicacion.Text = $"Fecha Publicación: {DateTime.Now:dd/MM/yyyy}";
                }
            }
            catch (Exception ex)
            {
                Logger.Agregar("❌ Error mostrando versión y fecha: " + ex.Message);
            }
        }

    }
}
