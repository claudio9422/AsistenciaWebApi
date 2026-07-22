using System;
using System.Collections.Generic;

namespace WebApiAsistencia.Models;

public partial class TiposMarcacion
{
    public int IdTipoMarcacion { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Marcacione> Marcaciones { get; set; } = new List<Marcacione>();
}
