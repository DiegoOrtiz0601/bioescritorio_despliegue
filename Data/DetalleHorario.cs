using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class DetalleHorario
{
    public int Id { get; set; }

    public int IdAsignacion { get; set; }

    public int DiaSemana { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual DiasDeLaSemana DiaSemanaNavigation { get; set; } = null!;

    public virtual AsignacionHorario IdAsignacionNavigation { get; set; } = null!;
}
