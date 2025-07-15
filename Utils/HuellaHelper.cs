using DPFP;
using System.IO;

namespace BiomentricoHolding.Utils
{
    public static class HuellaHelper
    {
        /// <summary>
        /// Convierte un template a bytes para almacenamiento
        /// </summary>
        public static byte[] ConvertirTemplateABytes(Template template)
        {
            try
            {
                if (template == null)
                {
                    throw new ArgumentNullException(nameof(template), "Template no puede ser nulo");
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    template.Serialize(stream);
                    var bytes = stream.ToArray();
                    
                    // Validar que el template serializado no esté vacío
                    if (bytes.Length == 0)
                    {
                        throw new InvalidOperationException("Template serializado está vacío");
                    }
                    
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al convertir template a bytes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Convierte bytes a template para uso
        /// </summary>
        public static Template ConvertirBytesATemplate(byte[] datos)
        {
            try
            {
                if (datos == null || datos.Length == 0)
                {
                    throw new ArgumentException("Datos del template no pueden ser nulos o vacíos");
                }

                Template template = new Template();
                using (MemoryStream stream = new MemoryStream(datos))
                {
                    template.DeSerialize(stream);
                }
                
                return template;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al convertir bytes a template: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Valida si un template es válido y no está corrupto
        /// </summary>
        public static bool ValidarTemplate(byte[] datos)
        {
            try
            {
                if (datos == null || datos.Length == 0)
                {
                    return false;
                }

                // Verificar tamaño mínimo (templates válidos tienen al menos 100 bytes)
                if (datos.Length < 100)
                {
                    return false;
                }

                // Verificar tamaño máximo (templates no deberían ser mayores a 10KB)
                if (datos.Length > 10240)
                {
                    return false;
                }

                // Intentar deserializar para verificar integridad
                var template = ConvertirBytesATemplate(datos);
                
                // Verificar que el template no sea nulo
                if (template == null)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene información detallada de un template
        /// </summary>
        public static string ObtenerInformacionTemplate(byte[] datos)
        {
            try
            {
                if (datos == null)
                {
                    return "Template: NULO";
                }

                var info = new System.Text.StringBuilder();
                info.AppendLine($"Tamaño: {datos.Length} bytes");
                
                if (ValidarTemplate(datos))
                {
                    info.AppendLine("Estado: ✅ VÁLIDO");
                    
                    // Intentar obtener más información del template
                    try
                    {
                        var template = ConvertirBytesATemplate(datos);
                        info.AppendLine("Deserialización: ✅ EXITOSA");
                    }
                    catch (Exception ex)
                    {
                        info.AppendLine($"Deserialización: ❌ ERROR - {ex.Message}");
                    }
                }
                else
                {
                    info.AppendLine("Estado: ❌ INVÁLIDO/CORRUPTO");
                    
                    if (datos.Length == 0)
                    {
                        info.AppendLine("Razón: Template vacío");
                    }
                    else if (datos.Length < 100)
                    {
                        info.AppendLine("Razón: Tamaño insuficiente");
                    }
                    else if (datos.Length > 10240)
                    {
                        info.AppendLine("Razón: Tamaño excesivo");
                    }
                    else
                    {
                        info.AppendLine("Razón: Error de deserialización");
                    }
                }

                return info.ToString();
            }
            catch (Exception ex)
            {
                return $"Error al analizar template: {ex.Message}";
            }
        }

        /// <summary>
        /// Limpia templates corruptos de la base de datos
        /// </summary>
        public static int LimpiarTemplatesCorruptos()
        {
            try
            {
                using var db = AppSettings.GetContextUno();
                var empleadosConTemplates = db.Empleados.Where(e => e.Huella != null).ToList();
                int templatesCorruptos = 0;

                foreach (var empleado in empleadosConTemplates)
                {
                    if (!ValidarTemplate(empleado.Huella))
                    {
                        empleado.Huella = null; // Limpiar template corrupto
                        templatesCorruptos++;
                    }
                }

                if (templatesCorruptos > 0)
                {
                    db.SaveChanges();
                }

                return templatesCorruptos;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al limpiar templates corruptos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Diagnostica todos los templates en la base de datos
        /// </summary>
        public static string DiagnosticarTemplates()
        {
            try
            {
                using var db = AppSettings.GetContextUno();
                var empleados = db.Empleados.Where(e => e.Huella != null).ToList();
                
                var diagnostico = new System.Text.StringBuilder();
                diagnostico.AppendLine("🔍 DIAGNÓSTICO DE TEMPLATES");
                diagnostico.AppendLine("============================");
                diagnostico.AppendLine($"Total empleados con templates: {empleados.Count}");
                
                int templatesValidos = 0;
                int templatesCorruptos = 0;
                var templatesCorruptosDetalle = new List<string>();

                foreach (var empleado in empleados)
                {
                    if (ValidarTemplate(empleado.Huella))
                    {
                        templatesValidos++;
                    }
                    else
                    {
                        templatesCorruptos++;
                        templatesCorruptosDetalle.Add($"{empleado.Nombres} {empleado.Apellidos} ({empleado.Documento})");
                    }
                }

                diagnostico.AppendLine($"Templates válidos: {templatesValidos}");
                diagnostico.AppendLine($"Templates corruptos: {templatesCorruptos}");
                
                if (templatesCorruptos > 0)
                {
                    diagnostico.AppendLine("\n📋 TEMPLATES CORRUPTOS:");
                    foreach (var detalle in templatesCorruptosDetalle.Take(10)) // Mostrar solo los primeros 10
                    {
                        diagnostico.AppendLine($"- {detalle}");
                    }
                    
                    if (templatesCorruptos > 10)
                    {
                        diagnostico.AppendLine($"... y {templatesCorruptos - 10} más");
                    }
                    
                    diagnostico.AppendLine("\n💡 RECOMENDACIÓN: Ejecute LimpiarTemplatesCorruptos() para limpiar templates corruptos.");
                }

                return diagnostico.ToString();
            }
            catch (Exception ex)
            {
                return $"Error en diagnóstico: {ex.Message}";
            }
        }
    }
}
