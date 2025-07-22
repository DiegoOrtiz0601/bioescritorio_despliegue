using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class SedeCiudadEmpresaArea
{
    public int Id { get; set; }

    public int IdSede { get; set; }

    public int IdCiudad { get; set; }

    public int IdEmpresa { get; set; }

    public int IdArea { get; set; }

    public virtual Area IdAreaNavigation { get; set; } = null!;

    public virtual Ciudad IdCiudadNavigation { get; set; } = null!;

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
