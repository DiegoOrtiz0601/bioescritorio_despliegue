using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class TiposEmpleado
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }
}
