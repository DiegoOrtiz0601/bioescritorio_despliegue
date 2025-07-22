using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public string Correo { get; set; } = null!;

    public bool Estado { get; set; }

    public int? IdRolUsuario { get; set; }

    public virtual Role? IdRolUsuarioNavigation { get; set; }
}
