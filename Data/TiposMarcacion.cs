using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class TiposMarcacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Marcacione> Marcaciones { get; set; } = new List<Marcacione>();
}
