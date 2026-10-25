namespace Turnos.API.Models
{
    public enum EstadoTurno
    {
        Pendiente,
        Cancelado,
        Atendido
    }
    public class Turno
    {
        public int Id {get;set;}
        public int ProfesionalId {get; set;}
        public Profesional? Profesional {get; set;} 
        public int ClienteId {get;set;}
        public Cliente? Cliente {get;set;}
        public DateTime FechaHora {get;set;}
        public EstadoTurno Estado {get;set;} = EstadoTurno.Pendiente;
    }
}