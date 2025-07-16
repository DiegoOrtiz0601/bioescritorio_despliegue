using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BiomentricoHolding.Views.Empleado
{
    public partial class AsignarHorarioWindow : Window
    {
        private int _idEmpleado;

        public Dictionary<int, (TimeSpan Inicio, TimeSpan Fin)> HorariosAsignados { get; private set; } = new();

        public AsignarHorarioWindow()
        {
            InitializeComponent();
            GenerarControlesPorDia();
        }

        // 🔄 Constructor adicional con ID de empleado
        public AsignarHorarioWindow(int idEmpleado) : this()
        {
            _idEmpleado = idEmpleado;
        }

        private void GenerarControlesPorDia()
        {
            string[] dias = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            string[] iconos = { "1️⃣", "2️⃣", "3️⃣", "4️⃣", "5️⃣", "6️⃣", "7️⃣" };

            for (int i = 0; i < dias.Length; i++)
            {
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
                    Text = iconos[i],
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 8, 0)
                });

                stackDia.Children.Add(new TextBlock
                {
                    Text = dias[i],
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

                stackControles.Children.Add(CrearComboBox($"InicioHora_{i}", 0, 23));
                stackControles.Children.Add(new TextBlock
                {
                    Text = ":",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(4, 0, 4, 0)
                });
                stackControles.Children.Add(CrearComboBox($"InicioMin_{i}", 0, 59));

                // Separador
                stackControles.Children.Add(new TextBlock
                {
                    Text = "  →  ",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#3B82F6")),
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

                stackControles.Children.Add(CrearComboBox($"FinHora_{i}", 0, 23));
                stackControles.Children.Add(new TextBlock
                {
                    Text = ":",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(4, 0, 4, 0)
                });
                stackControles.Children.Add(CrearComboBox($"FinMin_{i}", 0, 59));

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

            // Establecer valores por defecto según el tipo de control
            if (name.Contains("InicioHora"))
            {
                combo.SelectedItem = "07"; // 7:00 AM por defecto
            }
            else if (name.Contains("InicioMin"))
            {
                combo.SelectedItem = "00";
            }
            else if (name.Contains("FinHora"))
            {
                combo.SelectedItem = "17"; // 5:00 PM por defecto
            }
            else if (name.Contains("FinMin"))
            {
                combo.SelectedItem = "00";
            }
            else
            {
                combo.SelectedIndex = 0;
            }

            return combo;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HorariosAsignados.Clear();

                for (int i = 0; i < 7; i++)
                {
                    var border = (Border)panelDias.Children[i];
                    var grid = (Grid)border.Child;
                    var stackControles = (StackPanel)grid.Children[1]; // La columna derecha con los controles

                    // Los controles están en el orden: Inicio:, Hora, :, Min, →, Fin:, Hora, :, Min
                    var inicioHora = int.Parse(((ComboBox)stackControles.Children[1]).SelectedItem.ToString());
                    var inicioMin = int.Parse(((ComboBox)stackControles.Children[3]).SelectedItem.ToString());
                    var finHora = int.Parse(((ComboBox)stackControles.Children[6]).SelectedItem.ToString());
                    var finMin = int.Parse(((ComboBox)stackControles.Children[8]).SelectedItem.ToString());

                    var inicio = new TimeSpan(inicioHora, inicioMin, 0);
                    var fin = new TimeSpan(finHora, finMin, 0);

                    // Validar que la hora de fin sea posterior a la de inicio
                    if (fin <= inicio)
                    {
                        MessageBox.Show($"❌ Error en {GetNombreDia(i + 1)}: La hora de fin debe ser posterior a la hora de inicio.", 
                                      "Error de Horario", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    HorariosAsignados.Add(i + 1, (inicio, fin));
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar horarios: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetNombreDia(int diaSemana)
        {
            string[] dias = { "", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            return dias[diaSemana];
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
