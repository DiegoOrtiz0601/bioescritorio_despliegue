using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Permiso
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Modulo { get; set; } = null!;

    public string Accion { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}
