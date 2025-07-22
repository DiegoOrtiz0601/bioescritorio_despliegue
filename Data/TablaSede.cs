using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class TablaSede
{
    public byte IdSede { get; set; }

    public string Nombre { get; set; } = null!;

    public byte IdEmpresa { get; set; }

    public byte IdCiudad { get; set; }

    public byte Estado { get; set; }

    public byte IdUsuario { get; set; }
}
