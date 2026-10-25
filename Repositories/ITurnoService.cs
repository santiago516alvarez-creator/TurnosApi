using Turnos.API.Models;

namespace Turnos.API.Services
{
    public interface ITurnoService
    {
        Task<IEnumerable<Turno>> ObtenerTodosAsync();
        Task<Turno?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Turno>> ObtenerPorDiaAsync(DateTime fecha);
        Task<IEnumerable<Turno>> ObtenerPorClienteAsync(int clienteId);
        Task<(bool Exito, string Mensaje)> CrearTurnoAsync(Turno turno);
        Task<(bool Exito, string Mensaje)> CancelarTurnoAsync(int id);
    }
}