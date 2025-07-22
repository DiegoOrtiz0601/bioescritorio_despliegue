using System.IO;
using System.Text.Json;

namespace BiomentricoHolding.Utils
{
    public static class ConfiguracionSistema
    {
        private static readonly string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static int? IdEmpresaActual { get; private set; }
        public static string NombreEmpresaActual { get; private set; }
        public static int? IdSedeActual { get; private set; }
        public static string NombreSedeActual { get; private set; }

        public static bool EstaConfigurado => IdEmpresaActual.HasValue && IdSedeActual.HasValue;

        // Configuración de horario por defecto
        public static HorarioPorDefecto HorarioPorDefecto { get; private set; } = new();

        public static void EstablecerConfiguracion(int idEmpresa, string nombreEmpresa, int idSede, string nombreSede)
        {
            IdEmpresaActual = idEmpresa;
            NombreEmpresaActual = nombreEmpresa;
            IdSedeActual = idSede;
            NombreSedeActual = nombreSede;

            GuardarConfiguracion();
        }

        public static void CargarConfiguracion()
        {
            if (!File.Exists(rutaArchivo))
            {
                IdEmpresaActual = null;
                NombreEmpresaActual = null;
                IdSedeActual = null;
                NombreSedeActual = null;
                // Cargar horarios por defecto del sistema
                HorarioPorDefecto = HorarioPorDefecto.ObtenerValoresPorDefecto();
                return;
            }

            try
            {
                var json = File.ReadAllText(rutaArchivo);
                var datos = JsonSerializer.Deserialize<ConfiguracionData>(json);

                IdEmpresaActual = datos?.IdEmpresa;
                NombreEmpresaActual = datos?.NombreEmpresa;
                IdSedeActual = datos?.IdSede;
                NombreSedeActual = datos?.NombreSede;
                
                // Cargar horarios por defecto desde el archivo
                if (datos?.HorarioPorDefecto != null)
                {
                    HorarioPorDefecto = datos.HorarioPorDefecto;
                    Logger.Agregar("✅ Configuración de horario por defecto cargada desde config.json");
                }
                else
                {
                    HorarioPorDefecto = HorarioPorDefecto.ObtenerValoresPorDefecto();
                    Logger.Agregar("⚠️ No se encontró configuración de horario, usando valores por defecto");
                }
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al cargar configuración: {ex.Message}");
                IdEmpresaActual = null;
                NombreEmpresaActual = null;
                IdSedeActual = null;
                NombreSedeActual = null;
                HorarioPorDefecto = HorarioPorDefecto.ObtenerValoresPorDefecto();
            }
        }

        private static void GuardarConfiguracion()
        {
            var datos = new ConfiguracionData
            {
                IdEmpresa = IdEmpresaActual,
                NombreEmpresa = NombreEmpresaActual,
                IdSede = IdSedeActual,
                NombreSede = NombreSedeActual,
                HorarioPorDefecto = HorarioPorDefecto
            };

            var json = JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(rutaArchivo, json);
            Logger.Agregar("✅ Configuración guardada en config.json");
        }

        private class ConfiguracionData
        {
            public int? IdEmpresa { get; set; }
            public string NombreEmpresa { get; set; }
            public int? IdSede { get; set; }
            public string NombreSede { get; set; }
            public HorarioPorDefecto HorarioPorDefecto { get; set; }
        }

        #region Métodos para Horario por Defecto

        /// <summary>
        /// Carga la configuración de horario por defecto
        /// </summary>
        public static void CargarHorarioPorDefecto()
        {
            // Ya se carga en CargarConfiguracion()
        }

        /// <summary>
        /// Guarda la configuración de horario por defecto
        /// </summary>
        public static void GuardarHorarioPorDefecto(HorarioPorDefecto horario)
        {
            try
            {
                HorarioPorDefecto = horario;
                GuardarConfiguracion(); // Usar el mismo método que empresa/sede
                Logger.Agregar("✅ Configuración de horario por defecto guardada correctamente.");
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al guardar horario por defecto: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Guarda la configuración de horario por defecto de manera robusta
        /// </summary>
        public static void GuardarHorarioPorDefectoRobusto(HorarioPorDefecto horario)
        {
            GuardarHorarioPorDefecto(horario);
        }

        /// <summary>
        /// Obtiene la hora de inicio para un día específico
        /// </summary>
        public static TimeOnly ObtenerHoraInicio(int diaSemana)
        {
            try
            {
                var dia = HorarioPorDefecto.Dias.FirstOrDefault(d => d.Id == diaSemana);
                if (dia != null && TimeOnly.TryParse(dia.HoraInicio, out var hora))
                {
                    return hora;
                }
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al obtener hora de inicio para día {diaSemana}: {ex.Message}");
            }

            // Fallback a valores por defecto
            return diaSemana == 6 || diaSemana == 7 ? TimeOnly.Parse("07:00") : TimeOnly.Parse("07:00");
        }

        /// <summary>
        /// Obtiene la hora de fin para un día específico
        /// </summary>
        public static TimeOnly ObtenerHoraFin(int diaSemana)
        {
            try
            {
                var dia = HorarioPorDefecto.Dias.FirstOrDefault(d => d.Id == diaSemana);
                if (dia != null && TimeOnly.TryParse(dia.HoraFin, out var hora))
                {
                    return hora;
                }
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al obtener hora de fin para día {diaSemana}: {ex.Message}");
            }

            // Fallback a valores por defecto
            return diaSemana == 6 || diaSemana == 7 ? TimeOnly.Parse("16:30") : TimeOnly.Parse("17:30");
        }

        /// <summary>
        /// Restaura los valores por defecto del sistema
        /// </summary>
        public static void RestaurarValoresPorDefecto()
        {
            try
            {
                HorarioPorDefecto = HorarioPorDefecto.ObtenerValoresPorDefecto();
                GuardarConfiguracion(); // Usar el mismo método que empresa/sede
                Logger.Agregar("🔄 Valores por defecto del sistema restaurados correctamente.");
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al restaurar valores por defecto: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtiene el nombre del día desde la base de datos
        /// </summary>
        public static string ObtenerNombreDia(int diaSemana)
        {
            return HorarioPorDefecto.ObtenerNombreDia(diaSemana);
        }

        #endregion
    }
}
