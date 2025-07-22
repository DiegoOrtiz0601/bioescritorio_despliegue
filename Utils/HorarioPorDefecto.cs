using System.Text.Json;
using System.IO;
using BiomentricoHolding.Data;

namespace BiomentricoHolding.Utils
{
    /// <summary>
    /// Clase para manejar la configuración de horarios por defecto
    /// </summary>
    public class HorarioPorDefecto
    {
        public List<DiaHorario> Dias { get; set; } = new();
        public DateTime FechaModificacion { get; set; }
        public string ModificadoPor { get; set; } = "";

        /// <summary>
        /// Obtiene los valores por defecto del sistema
        /// </summary>
        public static HorarioPorDefecto ObtenerValoresPorDefecto()
        {
            return new HorarioPorDefecto
            {
                Dias = new List<DiaHorario>
                {
                    new() { Id = 1, HoraInicio = "07:00", HoraFin = "17:30" }, // Domingo
                    new() { Id = 2, HoraInicio = "07:00", HoraFin = "17:30" }, // Lunes
                    new() { Id = 3, HoraInicio = "07:00", HoraFin = "17:30" }, // Martes
                    new() { Id = 4, HoraInicio = "07:00", HoraFin = "17:30" }, // Miércoles
                    new() { Id = 5, HoraInicio = "07:00", HoraFin = "17:30" }, // Jueves
                    new() { Id = 6, HoraInicio = "07:00", HoraFin = "16:30" }, // Viernes
                    new() { Id = 7, HoraInicio = "07:00", HoraFin = "16:30" }  // Sábado
                },
                FechaModificacion = DateTime.Now,
                ModificadoPor = "Sistema"
            };
        }

        /// <summary>
        /// Carga la configuración desde el archivo JSON
        /// </summary>
        public static HorarioPorDefecto CargarDesdeArchivo()
        {
            try
            {
                string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "horario_por_defecto.json");
                
                if (!File.Exists(rutaArchivo))
                {
                    Logger.Agregar("📁 Archivo de configuración de horario no encontrado. Usando valores por defecto.");
                    return ObtenerValoresPorDefecto();
                }

                string json = File.ReadAllText(rutaArchivo);
                var config = JsonSerializer.Deserialize<ConfiguracionHorario>(json);
                
                if (config?.HorarioPorDefecto != null)
                {
                    Logger.Agregar("✅ Configuración de horario cargada desde archivo.");
                    return config.HorarioPorDefecto;
                }
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al cargar configuración de horario: {ex.Message}");
            }

            Logger.Agregar("⚠️ Usando valores por defecto del sistema.");
            return ObtenerValoresPorDefecto();
        }

        /// <summary>
        /// Guarda la configuración en el archivo JSON
        /// </summary>
        public void GuardarEnArchivo()
        {
            try
            {
                string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
                string rutaArchivo = Path.Combine(directorio, "horario_por_defecto.json");

                // Crear directorio si no existe
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                var config = new ConfiguracionHorario
                {
                    HorarioPorDefecto = this
                };

                var opciones = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string json = JsonSerializer.Serialize(config, opciones);
                File.WriteAllText(rutaArchivo, json);

                Logger.Agregar("✅ Configuración de horario guardada correctamente.");
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al guardar configuración de horario: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtiene el nombre del día desde la base de datos
        /// </summary>
        public static string ObtenerNombreDia(int idDia)
        {
            try
            {
                using var context = AppSettings.GetContextUno();
                var dia = context.DiasDeLaSemanas.FirstOrDefault(d => d.Id == idDia);
                return dia?.Nombre ?? $"Día {idDia}";
            }
            catch (Exception ex)
            {
                Logger.Agregar($"❌ Error al obtener nombre del día {idDia}: {ex.Message}");
                return $"Día {idDia}";
            }
        }

        /// <summary>
        /// Valida que todos los horarios sean correctos
        /// </summary>
        public bool ValidarHorarios()
        {
            foreach (var dia in Dias)
            {
                if (!TimeOnly.TryParse(dia.HoraInicio, out var inicio) ||
                    !TimeOnly.TryParse(dia.HoraFin, out var fin))
                {
                    Logger.Agregar($"❌ Formato de hora inválido para el día {dia.Id}");
                    return false;
                }

                if (fin <= inicio)
                {
                    Logger.Agregar($"❌ La hora de fin debe ser posterior a la hora de inicio para el día {dia.Id}");
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Clase para representar el horario de un día específico
    /// </summary>
    public class DiaHorario
    {
        public int Id { get; set; }
        public string HoraInicio { get; set; } = "07:00";
        public string HoraFin { get; set; } = "17:30";

        /// <summary>
        /// Obtiene el nombre del día desde la base de datos
        /// </summary>
        public string NombreDia => HorarioPorDefecto.ObtenerNombreDia(Id);
    }

    /// <summary>
    /// Clase para la estructura del archivo JSON
    /// </summary>
    public class ConfiguracionHorario
    {
        public HorarioPorDefecto HorarioPorDefecto { get; set; } = new();
    }
} 