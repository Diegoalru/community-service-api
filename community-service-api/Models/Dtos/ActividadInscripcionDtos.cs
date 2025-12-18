using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class InscribirUsuarioActividadRequestDto
{
    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public int IdOrganizacion { get; set; }

    [Required]
    public int IdActividad { get; set; }

    [Required]
    public int IdHorarioActividad { get; set; }
}

public class InscripcionActividadResponseDto
{
    public int IdParticipanteActividad { get; set; }
    public int IdUsuario { get; set; }
    public int IdOrganizacion { get; set; }
    public int IdActividad { get; set; }
    public int IdHorarioActividad { get; set; }
    public DateTime FechaInscripcion { get; set; }
    public int CuposRestantes { get; set; }
}

public class DesinscripcionActividadResponseDto
{
    public int IdUsuario { get; set; }
    public int IdOrganizacion { get; set; }
    public int IdActividad { get; set; }
    public int IdHorarioActividad { get; set; }
    public DateTime FechaRetiro { get; set; }
    public int CuposRestantes { get; set; }
}


