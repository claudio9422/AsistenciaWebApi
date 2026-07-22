using System;
using System.Collections.Generic;

namespace WebApiAsistencia.Models;

public partial class Marcacione
{
    public int IdMarcacion { get; set; }

    public int IdUsuario { get; set; }

    public int? IdSucursal { get; set; }

    public int? IdTipoMarcacion { get; set; }

    public DateTime FechaHoraRegistro { get; set; }

    public string TipoMarcacion { get; set; } = null!;

    public decimal LatitudMarcacion { get; set; }

    public decimal LongitudMarcacion { get; set; }

    public string RutaFoto { get; set; } = null!;

    public bool EsValida { get; set; }

    public int DistanciaCalculadaMetros { get; set; }

    public virtual Sucursale? IdSucursalNavigation { get; set; }

    public virtual TiposMarcacion? IdTipoMarcacionNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
