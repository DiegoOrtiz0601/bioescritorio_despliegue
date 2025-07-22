using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Role
{
    public int IdRol { get; set; }

    public string? Rol { get; set; }

    public string? Descripcion { get; set; }

    public bool EsEditable { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
