using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<SedeCiudadEmpresaArea> SedeCiudadEmpresaAreas { get; set; } = new List<SedeCiudadEmpresaArea>();
}
