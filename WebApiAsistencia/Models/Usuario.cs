using System;
using System.Collections.Generic;

namespace WebApiAsistencia.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string DocumentoIdentidad { get; set; } = null!;

    public int IdSucursal { get; set; }

    public int IdHorario { get; set; }

    public int IntentosFallidosGps { get; set; }

    public bool BloqueadoPorGps { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool EsPrimerIngreso { get; set; }

    public bool PermiteCambioClave { get; set; }

    public bool Estado { get; set; }

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual Sucursale IdSucursalNavigation { get; set; } = null!;

    public virtual ICollection<Marcacione> Marcaciones { get; set; } = new List<Marcacione>();
}
