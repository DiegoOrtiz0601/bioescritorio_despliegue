using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class DiasDeLaSemana
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<DetalleHorario> DetalleHorarios { get; set; } = new List<DetalleHorario>();
}
