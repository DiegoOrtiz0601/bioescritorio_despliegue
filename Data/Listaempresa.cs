using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Listaempresa
{
    public byte IdEmpresa { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public byte Estado { get; set; }
}
