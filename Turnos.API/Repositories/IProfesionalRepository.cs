using Turnos.API.Models;

namespace Turnos.API.Repositories
{
    public interface IProfesionalRepository
    {
        Task<IEnumerable<Profesional>> ObtenerTodosAsync();
        Task<Profesional?> ObtenerPorIdAsync(int id);
        Task CrearAsync(Profesional profesional);
        Task ActualizarAsync(Profesional profesional);
        Task EliminarAsync(int id);
    }
}