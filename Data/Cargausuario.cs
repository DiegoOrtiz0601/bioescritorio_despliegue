using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Cargausuario
{
    public byte IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public string Correo { get; set; } = null!;

    public byte Estado { get; set; }

    public byte IdRolUsuario { get; set; }
}
