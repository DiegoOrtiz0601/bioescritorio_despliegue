using System;
using System.Collections.Generic;

namespace BiomentricoHolding.Data;

public partial class Sede
{
    public int IdSede { get; set; }

    public string Nombre { get; set; } = null!;

    // Nota: IdEmpresa e IdCiudad ya no existen en la base de datos
    // Las relaciones ahora se manejan a través de SedeCiudadEmpresaArea

    public bool? Estado { get; set; }

    public int IdUsuario { get; set; }

    public virtual ICollection<SedeCiudadEmpresaArea> SedeCiudadEmpresaAreas { get; set; } = new List<SedeCiudadEmpresaArea>();
}
