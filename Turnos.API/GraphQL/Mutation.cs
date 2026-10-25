using Turnos.API.Models;
using Turnos.API.Services;
using HotChocolate;

namespace Turnos.API.GraphQL
{
    public record TurnoPayload(bool Exito, string Mensaje, Turno? Turno);

    public class Mutation
    {
        public async Task<TurnoPayload> SacarTurno(
            int profesionalId,
            int clienteId,
            DateTime fechaHora,
            [Service] ITurnoService turnoService)
        {
            var turno = new Turno
            {
                ProfesionalId = profesionalId,
                ClienteId = clienteId,
                FechaHora = fechaHora
            };

            var (exito, mensaje) = await turnoService.CrearTurnoAsync(turno);
            return new TurnoPayload(exito, mensaje, exito ? turno : null);
        }

        public async Task<TurnoPayload> CancelarTurno(int id, [Service] ITurnoService turnoService)
        {
            var (exito, mensaje) = await turnoService.CancelarTurnoAsync(id);
            var turno = exito ? await turnoService.ObtenerPorIdAsync(id) : null;
            return new TurnoPayload(exito, mensaje, turno);
        }
    }
}