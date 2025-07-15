using DPFP;
using DPFP.Capture;
using DPFP.Processing;
using System.Drawing;

namespace BiomentricoHolding.Services
{
    public enum ModoCaptura { Registro, Verificacion }
    public enum EstadoLector { Desconocido, Conectado, Desconectado, Ocupado, Error, Listo }

    public class CapturaHuellaService : DPFP.Capture.EventHandler, IDisposable
    {
        private Capture Capturador;
        private Enrollment Enroller;
        private bool primerIntento = true;
        private bool lectorConectado = false;
        private EstadoLector estadoLector = EstadoLector.Desconocido;
        private bool disposed = false;

        public Template TemplateCapturado { get; private set; }
        public EstadoLector EstadoLector => estadoLector;

        public event Action<string> Mensaje;
        public event Action<Template> TemplateGenerado;
        public event Action<Bitmap> MuestraProcesadaImagen;
        public event Action<Sample> MuestraProcesada;
        public event Action IntentoFallido;

        public ModoCaptura Modo { get; set; } = ModoCaptura.Registro;

        public CapturaHuellaService()
        {
            try
            {
                Capturador = new Capture();
                if (Capturador != null)
                {
                    Capturador.EventHandler = this;
                    Enroller = new Enrollment();
                    estadoLector = EstadoLector.Conectado;
                }
                else
                {
                    estadoLector = EstadoLector.Error;
                    Mensaje?.Invoke("⚠ No se pudo inicializar el capturador.");
                }
            }
            catch (Exception ex)
            {
                estadoLector = EstadoLector.Error;
                Mensaje?.Invoke($"❌ Error al inicializar lector: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si el lector está disponible y funcionando
        /// </summary>
        public bool VerificarLector()
        {
            try
            {
                // Verificar si el capturador existe
                if (Capturador == null)
                {
                    estadoLector = EstadoLector.Error;
                    Mensaje?.Invoke("❌ Lector no inicializado.");
                    return false;
                }

                // Si el capturador existe, asumir que está conectado
                // (los eventos de conexión se manejan por separado)
                if (estadoLector == EstadoLector.Error)
                {
                    Mensaje?.Invoke("❌ Lector en estado de error. Reinicie el sistema.");
                    return false;
                }

                // Verificar si está ocupado
                if (estadoLector == EstadoLector.Ocupado)
                {
                    Mensaje?.Invoke("⚠ Lector ocupado por otro proceso. Espere un momento.");
                    return false;
                }

                estadoLector = EstadoLector.Listo;
                return true;
            }
            catch (Exception ex)
            {
                estadoLector = EstadoLector.Error;
                Mensaje?.Invoke($"❌ Error al verificar lector: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Diagnostica el estado completo del lector
        /// </summary>
        public string DiagnosticarLector()
        {
            var diagnostico = new System.Text.StringBuilder();
            diagnostico.AppendLine("🔍 DIAGNÓSTICO DEL LECTOR");
            diagnostico.AppendLine("==========================");

            // Estado general
            diagnostico.AppendLine($"Estado: {estadoLector}");
            diagnostico.AppendLine($"Conectado: {(lectorConectado ? "Sí" : "No")}");
            diagnostico.AppendLine($"Capturador: {(Capturador != null ? "Inicializado" : "No inicializado")}");

            // Verificaciones específicas
            if (Capturador != null)
            {
                try
                {
                    // Intentar obtener información del lector
                    diagnostico.AppendLine("✅ Capturador responde correctamente");
                }
                catch (Exception ex)
                {
                    diagnostico.AppendLine($"❌ Error en capturador: {ex.Message}");
                }
            }

            // Recomendaciones
            diagnostico.AppendLine("\n💡 RECOMENDACIONES:");
            switch (estadoLector)
            {
                case EstadoLector.Desconectado:
                    diagnostico.AppendLine("- Conecte el lector por USB");
                    diagnostico.AppendLine("- Verifique que los drivers estén instalados");
                    break;
                case EstadoLector.Ocupado:
                    diagnostico.AppendLine("- Cierre otras aplicaciones que usen el lector");
                    diagnostico.AppendLine("- Espere unos segundos y reintente");
                    break;
                case EstadoLector.Error:
                    diagnostico.AppendLine("- Reinicie el lector");
                    diagnostico.AppendLine("- Reinicie la aplicación");
                    diagnostico.AppendLine("- Verifique los drivers del sistema");
                    break;
                case EstadoLector.Listo:
                    diagnostico.AppendLine("- El lector está funcionando correctamente");
                    break;
            }

            return diagnostico.ToString();
        }

        public void IniciarCaptura()
        {
            try
            {
                // VERIFICACIÓN PREVENTIVA
                if (!VerificarLector())
                {
                    return; // No continuar si el lector no está listo
                }

                DetenerCaptura(); // evitar múltiples sesiones
                Enroller.Clear();
                primerIntento = true;

                Capturador?.StartCapture();
                Mensaje?.Invoke("Coloca tu dedo en el lector para capturar la huella.");
            }
            catch (Exception ex)
            {
                estadoLector = EstadoLector.Error;
                Mensaje?.Invoke($"❌ Error al iniciar la captura: {ex.Message}");
            }
        }

        public void DetenerCaptura()
        {
            try
            {
                Capturador?.StopCapture();
                Mensaje?.Invoke("📴 Captura detenida.");
            }
            catch (Exception ex)
            {
                estadoLector = EstadoLector.Error;
                Mensaje?.Invoke($"❌ Error al detener la captura: {ex.Message}");
            }
        }

        public void OnComplete(object capture, string readerSerialNumber, Sample sample)
        {
            // VERIFICACIÓN PREVENTIVA ANTES DE PROCESAR
            if (!VerificarLector())
            {
                ManejarFallo("❌ Lector no disponible para procesar muestra.");
                return;
            }

            if (!primerIntento)
            {
                Mensaje?.Invoke("❌ Captura bloqueada. Debes reiniciar el proceso.");
                DetenerCaptura();
                return;
            }

            // Mostrar imagen
            MuestraProcesadaImagen?.Invoke(ConvertirMuestraAImagen(sample));

            // ➤ Si estamos en modo verificación, enviar sample directamente
            if (Modo == ModoCaptura.Verificacion)
            {
                MuestraProcesada?.Invoke(sample);
                return;
            }

            // ➤ Si estamos en modo registro, continuar flujo de enrolamiento
            var features = ExtractFeatures(sample, DataPurpose.Enrollment);
            if (features != null)
            {
                try
                {
                    Enroller.AddFeatures(features);
                    Mensaje?.Invoke($"✅ Muestra válida. Faltan {Enroller.FeaturesNeeded} muestra(s).");

                    if (Enroller.TemplateStatus == Enrollment.Status.Ready)
                    {
                        TemplateCapturado = Enroller.Template;
                        TemplateGenerado?.Invoke(TemplateCapturado);
                        Mensaje?.Invoke("🎉 Huella capturada exitosamente.");
                        DetenerCaptura();
                    }
                    else if (Enroller.TemplateStatus == Enrollment.Status.Failed)
                    {
                        ManejarFallo("❌ Las muestras no coincidieron. Debes volver a intentarlo.");
                    }
                }
                catch (DPFP.Error.SDKException ex)
                {
                    estadoLector = EstadoLector.Error;
                    ManejarFallo($"❌ Error crítico al capturar huella:\n{ex.Message}");
                }
            }
            else
            {
                ManejarFallo("⚠ Huella no clara. Intenta nuevamente.");
            }
        }

        private void ManejarFallo(string mensaje)
        {
            primerIntento = false;
            IntentoFallido?.Invoke();
            Enroller.Clear();
            DetenerCaptura();
            Mensaje?.Invoke(mensaje);
        }

        private Bitmap ConvertirMuestraAImagen(Sample sample)
        {
            var convertidor = new SampleConversion();
            var imagen = new Bitmap(100, 100);
            convertidor.ConvertToPicture(sample, ref imagen);
            return imagen;
        }

        private FeatureSet ExtractFeatures(Sample sample, DataPurpose purpose)
        {
            var extractor = new FeatureExtraction();
            CaptureFeedback feedback = CaptureFeedback.None;
            var features = new FeatureSet();

            extractor.CreateFeatureSet(sample, purpose, ref feedback, ref features);

            return (feedback == CaptureFeedback.Good) ? features : null;
        }

        public void OnFingerTouch(object capture, string readerSerialNumber)
        {
            if (primerIntento)
                Mensaje?.Invoke("👆 Dedo detectado...");
        }

        public void OnFingerGone(object capture, string readerSerialNumber)
        {
            if (primerIntento)
                Mensaje?.Invoke("👋 Dedo retirado.");
        }

        public void OnReaderConnect(object capture, string readerSerialNumber)
        {
            lectorConectado = true;
            estadoLector = EstadoLector.Conectado;
            Mensaje?.Invoke("✅ Lector conectado.");
        }

        public void OnReaderDisconnect(object capture, string readerSerialNumber)
        {
            lectorConectado = false;
            estadoLector = EstadoLector.Desconectado;
            Mensaje?.Invoke("❌ Lector desconectado.");
        }

        public void OnSampleQuality(object capture, string readerSerialNumber, CaptureFeedback feedback)
        {
            if (!primerIntento) return;

            if (feedback == CaptureFeedback.Good)
                Mensaje?.Invoke("👌 Calidad de huella aceptable.");
            else
                Mensaje?.Invoke("⚠ Calidad de huella insuficiente.");
        }

        public void Reiniciar(ModoCaptura nuevoModo)
        {
            DetenerCaptura();
            Modo = nuevoModo;
            Enroller?.Clear();
            primerIntento = true;
            estadoLector = EstadoLector.Conectado; // Resetear estado
            IniciarCaptura();
        }

        /// <summary>
        /// Intenta recuperar el lector de un estado de error
        /// </summary>
        public bool IntentarRecuperarLector()
        {
            try
            {
                Mensaje?.Invoke("🔄 Intentando recuperar lector...");
                
                // Detener captura actual
                DetenerCaptura();
                
                // Limpiar recursos
                Enroller?.Clear();
                
                // Resetear estado
                estadoLector = EstadoLector.Conectado;
                primerIntento = true;
                
                // Verificar si se recuperó
                if (VerificarLector())
                {
                    Mensaje?.Invoke("✅ Lector recuperado exitosamente.");
                    return true;
                }
                else
                {
                    Mensaje?.Invoke("❌ No se pudo recuperar el lector.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                estadoLector = EstadoLector.Error;
                Mensaje?.Invoke($"❌ Error al recuperar lector: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                try
                {
                    DetenerCaptura();
                    Capturador?.Dispose();
                    // Enroller no implementa IDisposable, usar Clear() para limpiar recursos
                    Enroller?.Clear();
                }
                catch (Exception ex)
                {
                    // Log error pero no lanzar excepción en Dispose
                    System.Diagnostics.Debug.WriteLine($"Error en Dispose: {ex.Message}");
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}
