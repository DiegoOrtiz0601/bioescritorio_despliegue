using BiomentricoHolding.Data;
using BiomentricoHolding.Utils;
using System.Windows;
using System.Windows.Controls;

namespace BiomentricoHolding.Views.Configuracion
{
    public partial class SeleccionEmpresaSedeWindow : Window
    {
        private readonly BiometricoDbContext _context = AppSettings.GetContextUno();

        public SeleccionEmpresaSedeWindow()
        {
            InitializeComponent();
            CargarEmpresas();
        }

        private void CargarEmpresas()
        {
            var empresas = _context.Empresas.OrderBy(e => e.Nombre).ToList();
            cmbEmpresas.ItemsSource = empresas;
            cmbEmpresas.DisplayMemberPath = "Nombre";
            cmbEmpresas.SelectedValuePath = "IdEmpresa";
        }

        private void cmbEmpresas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEmpresas.SelectedValue is int idEmpresa)
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
                
                Logger.Agregar($"🔍 Configuración inicial - Sedes cargadas: {sedes.Count} sedes para empresa {idEmpresa}");
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // VALIDACIÓN
            if (cmbEmpresas.SelectedItem is not Empresa empresa)
            {
                MessageBox.Show("Debe seleccionar una empresa.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbSedes.SelectedItem is not Sede sede)
            {
                MessageBox.Show("Debe seleccionar una sede.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // GUARDAR EN CONFIGURACIÓN GLOBAL
            ConfiguracionSistema.EstablecerConfiguracion(
                empresa.IdEmpresa,
                empresa.Nombre,
                sede.IdSede,
                sede.Nombre
            );

            MessageBox.Show("✅ Configuración guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
