using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Area
{
    public int IdArea { get; set; }

    public string? Nombre { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<SedeCiudadEmpresaArea> SedeCiudadEmpresaAreas { get; set; } = new List<SedeCiudadEmpresaArea>();
}
