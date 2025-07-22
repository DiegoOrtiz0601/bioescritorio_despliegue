using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BiomentricoHolding.Utils;
using BiomentricoHolding.Data;
using BiomentricoHolding.Views;
using System.IO;

namespace BiomentricoHolding.Views.Configuracion
{
    public partial class ConfiguracionHorarioWindow : Window
    {
        private HorarioPorDefecto horarioActual;
        private List<DiasDeLaSemana> diasDeLaSemana;

        public ConfiguracionHorarioWindow()
        {
            InitializeComponent();
            try
            {
                CargarDatos();
                GenerarControlesPorDia();
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al inicializar ventana de configuración: {ex.Message}");
                new MensajeWindow($"❌ Error al inicializar la ventana de configuración:\n\n{ex.Message}", false, "Cerrar", "").ShowDialog();
                Close();
            }
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar horario actual
                horarioActual = ConfiguracionSistema.HorarioPorDefecto ?? HorarioPorDefecto.ObtenerValoresPorDefecto();

                // Cargar días de la semana desde la base de datos
                using var context = AppSettings.GetContextUno();
                diasDeLaSemana = context.DiasDeLaSemanas.OrderBy(d => d.Id).ToList();

                if (diasDeLaSemana == null || diasDeLaSemana.Count == 0)
                {
                    throw new Exception("No se encontraron días de la semana en la base de datos");
                }

                Logger.Agregar("✅ Datos cargados para configuración de horario por defecto");
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al cargar datos: {ex.Message}");
                new MensajeWindow($"❌ Error al cargar los datos:\n\n{ex.Message}", false, "Cerrar", "").ShowDialog();
                throw; // Re-lanzar la excepción para que el constructor la maneje
            }
        }

        private void GenerarControlesPorDia()
        {
            panelDias.Children.Clear();

            if (diasDeLaSemana == null || diasDeLaSemana.Count == 0)
            {
                Logger.Agregar("❌ No se pudieron cargar los días de la semana");
                new MensajeWindow("❌ Error: No se pudieron cargar los días de la semana desde la base de datos.", false, "Cerrar", "").ShowDialog();
                return;
            }

            foreach (var dia in diasDeLaSemana)
            {
                var horarioDia = horarioActual.Dias.FirstOrDefault(d => d.Id == dia.Id) ?? new DiaHorario { Id = dia.Id };

                // Contenedor principal del día
                var borderDia = new Border
                {
                    Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F8F9FA")),
                    BorderBrush = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#E5E7EB")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 8, 0, 8)
                };

                var gridDia = new Grid();
                gridDia.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                gridDia.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Columna izquierda - Nombre del día
                var stackDia = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 15, 0)
                };

                stackDia.Children.Add(new TextBlock
                {
                    Text = GetIconoDia(dia.Id),
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 8, 0)
                });

                stackDia.Children.Add(new TextBlock
                {
                    Text = dia.Nombre,
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1F2937")),
                    VerticalAlignment = VerticalAlignment.Center,
                    MinWidth = 100
                });

                Grid.SetColumn(stackDia, 0);
                gridDia.Children.Add(stackDia);

                // Columna derecha - Controles de tiempo
                var stackControles = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Hora de inicio
                stackControles.Children.Add(new TextBlock
                {
                    Text = "Inicio:",
                    FontSize = 14,
                    FontWeight = FontWeights.Medium,
                    Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#6B7280")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 8, 0)
                });

                var inicioHora = CrearComboBox($"inicioHora_{dia.Id}", 0, 23);
                var inicioMin = CrearComboBox($"inicioMin_{dia.Id}", 0, 59);
                
                // Establecer valores actuales
                if (TimeOnly.TryParse(horarioDia.HoraInicio, out var horaInicio))
                {
                    inicioHora.SelectedItem = horaInicio.Hour.ToString("D2");
                    inicioMin.SelectedItem = horaInicio.Minute.ToString("D2");
                }
                else
                {
                    inicioHora.SelectedItem = "07";
                    inicioMin.SelectedItem = "00";
                }

                stackControles.Children.Add(inicioHora);
                stackControles.Children.Add(new TextBlock
                {
                    Text = ":",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(4, 0, 4, 0)
                });
                stackControles.Children.Add(inicioMin);

                // Separador
                stackControles.Children.Add(new TextBlock
                {
                    Text = "  →  ",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DC2626")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(15, 0, 15, 0)
                });

                // Hora de fin
                stackControles.Children.Add(new TextBlock
                {
                    Text = "Fin:",
                    FontSize = 14,
                    FontWeight = FontWeights.Medium,
                    Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#6B7280")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 8, 0)
                });

                var finHora = CrearComboBox($"finHora_{dia.Id}", 0, 23);
                var finMin = CrearComboBox($"finMin_{dia.Id}", 0, 59);
                
                // Establecer valores actuales
                if (TimeOnly.TryParse(horarioDia.HoraFin, out var horaFin))
                {
                    finHora.SelectedItem = horaFin.Hour.ToString("D2");
                    finMin.SelectedItem = horaFin.Minute.ToString("D2");
                }
                else
                {
                    finHora.SelectedItem = "17";
                    finMin.SelectedItem = "30";
                }

                stackControles.Children.Add(finHora);
                stackControles.Children.Add(new TextBlock
                {
                    Text = ":",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(4, 0, 4, 0)
                });
                stackControles.Children.Add(finMin);

                Grid.SetColumn(stackControles, 1);
                gridDia.Children.Add(stackControles);

                borderDia.Child = gridDia;
                panelDias.Children.Add(borderDia);
            }
        }

        private ComboBox CrearComboBox(string name, int min, int max)
        {
            var combo = new ComboBox
            {
                Name = name,
                Width = 60,
                Height = 36,
                Margin = new Thickness(4, 0, 4, 0),
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F9FAFB")),
                BorderBrush = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#E5E7EB")),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(8, 6, 8, 6),
                FontSize = 14,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            for (int i = min; i <= max; i++)
                combo.Items.Add(i.ToString("D2"));

            return combo;
        }

        private string GetIconoDia(int idDia)
        {
            return idDia switch
            {
                1 => "1️⃣", // Domingo
                2 => "2️⃣", // Lunes
                3 => "3️⃣", // Martes
                4 => "4️⃣", // Miércoles
                5 => "5️⃣", // Jueves
                6 => "6️⃣", // Viernes
                7 => "7️⃣", // Sábado
                _ => "📅"
            };
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarHorarios())
                {
                    new MensajeWindow("⚠️ Error de validación:\n\nPor favor, verifica que todos los horarios sean válidos:\n• Formato HH:mm\n• Hora de fin debe ser posterior a la hora de inicio", false, "Entendido", "").ShowDialog();
                    return;
                }

                // Actualizar horario con los valores de los controles
                ActualizarHorarioDesdeControles();

                // Guardar configuración de manera robusta
                ConfiguracionSistema.GuardarHorarioPorDefectoRobusto(horarioActual);

                Logger.Agregar("✅ Configuración de horario por defecto guardada correctamente");
                new MensajeWindow("✅ Configuración guardada correctamente.\n\nLos horarios por defecto han sido actualizados y se aplicarán a todos los empleados con horario genérico.", false, "Perfecto", "").ShowDialog();
                
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al guardar configuración: {ex.Message}");
                new MensajeWindow($"❌ Error al guardar la configuración:\n\n{ex.Message}", false, "Cerrar", "").ShowDialog();
            }
        }

        private void BtnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            var resultado = new MensajeWindow(
                "🔄 Confirmar restauración:\n\n¿Estás seguro que deseas restaurar los valores por defecto?\n\n⚠️ Esta acción no se puede deshacer y afectará a todos los empleados con horario genérico.",
                true, 
                "Sí, Restaurar", 
                "Cancelar");

            bool? decision = resultado.ShowDialog();

            if (decision == true && resultado.Resultado)
            {
                try
                {
                    ConfiguracionSistema.RestaurarValoresPorDefecto();
                    horarioActual = ConfiguracionSistema.HorarioPorDefecto;
                    GenerarControlesPorDia();

                    Logger.Agregar("✅ Valores por defecto restaurados correctamente");
                    new MensajeWindow("✅ Valores por defecto restaurados correctamente.\n\nLos horarios han sido restablecidos a la configuración inicial del sistema.", false, "Entendido", "").ShowDialog();
                }
                catch (Exception ex)
                {
                    Logger.Agregar($"❌ Error al restaurar valores: {ex.Message}");
                    new MensajeWindow($"❌ Error al restaurar los valores:\n\n{ex.Message}", false, "Cerrar", "").ShowDialog();
                }
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidarHorarios()
        {
            foreach (var dia in diasDeLaSemana)
            {
                var inicioHora = FindComboBox($"inicioHora_{dia.Id}");
                var inicioMin = FindComboBox($"inicioMin_{dia.Id}");
                var finHora = FindComboBox($"finHora_{dia.Id}");
                var finMin = FindComboBox($"finMin_{dia.Id}");

                if (inicioHora?.SelectedItem == null || inicioMin?.SelectedItem == null || 
                    finHora?.SelectedItem == null || finMin?.SelectedItem == null)
                {
                    return false;
                }

                var inicioHoraInt = int.Parse(inicioHora.SelectedItem.ToString());
                var inicioMinInt = int.Parse(inicioMin.SelectedItem.ToString());
                var finHoraInt = int.Parse(finHora.SelectedItem.ToString());
                var finMinInt = int.Parse(finMin.SelectedItem.ToString());

                var inicio = new TimeSpan(inicioHoraInt, inicioMinInt, 0);
                var fin = new TimeSpan(finHoraInt, finMinInt, 0);

                if (fin <= inicio)
                {
                    return false;
                }
            }

            return true;
        }

        private void ActualizarHorarioDesdeControles()
        {
            foreach (var dia in diasDeLaSemana)
            {
                var inicioHora = FindComboBox($"inicioHora_{dia.Id}");
                var inicioMin = FindComboBox($"inicioMin_{dia.Id}");
                var finHora = FindComboBox($"finHora_{dia.Id}");
                var finMin = FindComboBox($"finMin_{dia.Id}");

                if (inicioHora?.SelectedItem != null && inicioMin?.SelectedItem != null && 
                    finHora?.SelectedItem != null && finMin?.SelectedItem != null)
                {
                    var horarioDia = horarioActual.Dias.FirstOrDefault(d => d.Id == dia.Id);
                    if (horarioDia == null)
                    {
                        horarioDia = new DiaHorario { Id = dia.Id };
                        horarioActual.Dias.Add(horarioDia);
                    }

                    var inicioHoraInt = int.Parse(inicioHora.SelectedItem.ToString());
                    var inicioMinInt = int.Parse(inicioMin.SelectedItem.ToString());
                    var finHoraInt = int.Parse(finHora.SelectedItem.ToString());
                    var finMinInt = int.Parse(finMin.SelectedItem.ToString());

                    horarioDia.HoraInicio = $"{inicioHoraInt:D2}:{inicioMinInt:D2}";
                    horarioDia.HoraFin = $"{finHoraInt:D2}:{finMinInt:D2}";
                }
            }

            horarioActual.FechaModificacion = DateTime.Now;
            horarioActual.ModificadoPor = "Usuario";
        }

        private ComboBox FindComboBox(string name)
        {
            return FindVisualChild<ComboBox>(panelDias, cb => cb.Name == name);
        }

        private T FindVisualChild<T>(DependencyObject parent, Func<T, bool> predicate) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is T result && predicate(result))
                    return result;

                var descendant = FindVisualChild<T>(child, predicate);
                if (descendant != null)
                    return descendant;
            }

            return null;
        }
    }
} 