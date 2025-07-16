using BiomentricoHolding.Utils;
using System;
using System.Windows;

namespace BiomentricoHolding.Views.Configuracion
{
    public partial class ConfiguracionHorarioWindow : Window
    {
        public ConfiguracionHorarioWindow()
        {
            InitializeComponent();
            Logger.Agregar("🕒 Abriendo configuración de horario");
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
} 