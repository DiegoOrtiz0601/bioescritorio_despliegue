using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Documento { get; set; } = null!;

    // Campos requeridos por la base de datos pero no usados para relaciones
    // Las relaciones ahora se manejan a través de SedeCiudadEmpresaArea
    public int IdEmpresa { get; set; }
    public int IdSede { get; set; }
    public int IdArea { get; set; }

    public byte[]? Huella { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaIngreso { get; set; }

    public int IdEnrolador { get; set; }

    public int IdTipoEmpleado { get; set; }

    public int? IdJefe { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
