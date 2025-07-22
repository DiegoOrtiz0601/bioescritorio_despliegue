using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class AsignacionHorario
{
    public int Id { get; set; }

    public string Documento { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public DateTime FechaCreacion { get; set; }

    public int CreadoPor { get; set; }

    public bool Estado { get; set; }

    public int TipoHorario { get; set; }

    public virtual ICollection<DetalleHorario> DetalleHorarios { get; set; } = new List<DetalleHorario>();
}
