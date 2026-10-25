using Turnos.API.Models;

namespace Turnos.API.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task CrearAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(int id);
    }
}