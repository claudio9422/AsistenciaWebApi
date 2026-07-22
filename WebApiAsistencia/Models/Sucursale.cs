using System;
using System.Collections.Generic;

namespace WebApiAsistencia.Models;

public partial class Sucursale
{
    public int IdSucursal { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public int? RadioPermitidoMetros { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Marcacione> Marcaciones { get; set; } = new List<Marcacione>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
