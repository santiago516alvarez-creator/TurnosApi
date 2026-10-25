using Turnos.API.Models;
using Turnos.API.Repositories;
using Turnos.API.Services;
using HotChocolate;

namespace Turnos.API.GraphQL
{
    public class Query
    {
        public Task<IEnumerable<Turno>> GetTurnos([Service] ITurnoService turnoService)
            => turnoService.ObtenerTodosAsync();

        public Task<Turno?> GetTurno(int id, [Service] ITurnoService turnoService)
            => turnoService.ObtenerPorIdAsync(id);

        public Task<IEnumerable<Turno>> GetTurnosPorDia(DateTime fecha, [Service] ITurnoService turnoService)
            => turnoService.ObtenerPorDiaAsync(fecha);

        public Task<IEnumerable<Turno>> GetTurnosPorCliente(int clienteId, [Service] ITurnoService turnoService)
            => turnoService.ObtenerPorClienteAsync(clienteId);

        public Task<IEnumerable<Profesional>> GetProfesionales([Service] IProfesionalRepository repo)
            => repo.ObtenerTodosAsync();

        public Task<IEnumerable<Cliente>> GetClientes([Service] IClienteRepository repo)
            => repo.ObtenerTodosAsync();
    }
}