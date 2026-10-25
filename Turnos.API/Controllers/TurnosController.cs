using Microsoft.AspNetCore.Mvc;
using Turnos.API.Models;
using Turnos.API.Services;

namespace Turnos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnosController : ControllerBase
    {
        private readonly ITurnoService _turnoService;

        public TurnosController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turno>>> ObtenerTodos()
        {
            var turnos = await _turnoService.ObtenerTodosAsync();
            return Ok(turnos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Turno>> ObtenerPorId(int id)
        {
            var turno = await _turnoService.ObtenerPorIdAsync(id);
            if (turno == null)
            {
                return NotFound("El turno no fue encontrado.");
            }
            return Ok(turno);
        }

        [HttpGet("dia/{fecha}")]
        public async Task<ActionResult<IEnumerable<Turno>>> ObtenerPorDia(DateTime fecha)
        {
            var turnos = await _turnoService.ObtenerPorDiaAsync(fecha);
            return Ok(turnos);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Turno turno)
        {
     var resultado = await _turnoService.CrearTurnoAsync(turno);
            if (!resultado.Exito)
            {
                if (resultado.Mensaje.Contains("horario", StringComparison.OrdinalIgnoreCase))
                {
                    return Conflict(resultado.Mensaje);
                }
                return BadRequest(resultado.Mensaje);
            }

            return Ok(resultado.Mensaje);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var resultado = await _turnoService.CancelarTurnoAsync(id);
            if (!resultado.Exito)
            {
                return BadRequest(resultado.Mensaje);
            }

            return Ok(resultado.Mensaje);
        }
    }
}