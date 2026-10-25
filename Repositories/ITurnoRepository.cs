using Turnos.API.Models;

namespace Turnos.API.Repositories
{
    public interface ITurnoRepository
    {
        Task<IEnumerable<Turno>> ObtenerTodosAsync();
        Task<Turno?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Turno>> ObtenerPorDiaAsync(DateTime fecha);
        Task<IEnumerable<Turno>> ObtenerPorClienteAsync(int clienteId);
        Task<bool> ExisteTurnoEnHorarioAsync(int profesionalId, DateTime fechaHora);
        Task CrearAsync(Turno turno);
        Task ActualizarAsync(Turno turno);
        Task EliminarAsync(int id);
    }
}