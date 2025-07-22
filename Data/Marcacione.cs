using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Marcacione
{
    public int Id { get; set; }

    public string? Documento { get; set; }

    public DateTime FechaHora { get; set; }

    public int IdEmpresaMarcacion { get; set; }

    public int IdSedeMarcacion { get; set; }

    public int IdTipoMarcacion { get; set; }

    public int IdAsignacion { get; set; }

    public virtual TiposMarcacion IdTipoMarcacionNavigation { get; set; } = null!;
}
