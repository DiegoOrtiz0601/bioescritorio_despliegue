using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Ciudad
{
    public int IdCiudad { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<SedeCiudadEmpresaArea> SedeCiudadEmpresaAreas { get; set; } = new List<SedeCiudadEmpresaArea>();
}
