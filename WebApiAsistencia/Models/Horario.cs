using System;
using System.Collections.Generic;

namespace WebApiAsistencia.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public string NombreHorario { get; set; } = null!;

    public TimeOnly HoraEntrada { get; set; }

    public TimeOnly? HoraSalidaRefrigerio { get; set; }

    public TimeOnly? HoraRetornoRefrigerio { get; set; }

    public TimeOnly HoraSalida { get; set; }

    public int? ToleraEntradaMinutos { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
