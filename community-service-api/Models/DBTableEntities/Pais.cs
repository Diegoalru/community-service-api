using System;
using System.Collections.Generic;

namespace community_service_api.Models.DBTableEntities;

public partial class Pais
{
    public int IdPais { get; set; }

    public string Nombre { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<Actividad> Actividad { get; set; } = new List<Actividad>();

    public virtual ICollection<Organizacion> Organizacion { get; set; } = new List<Organizacion>();

    public virtual ICollection<Perfil> Perfil { get; set; } = new List<Perfil>();

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
