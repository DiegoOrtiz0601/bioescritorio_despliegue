using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class RolPermiso
{
    public int Id { get; set; }

    public int IdRol { get; set; }

    public int IdPermiso { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Permiso IdPermisoNavigation { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;
}
