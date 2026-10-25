using Microsoft.AspNetCore.Mvc;
using Turnos.API.Models;
using Turnos.API.Repositories;

namespace Turnos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesionalesController : ControllerBase
    {
        private readonly IProfesionalRepository _profesionalRepository;

        public ProfesionalesController(IProfesionalRepository profesionalRepository)
        {
            _profesionalRepository = profesionalRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profesional>>> ObtenerTodos()
        {
            var profesionales = await _profesionalRepository.ObtenerTodosAsync();
            return Ok(profesionales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Profesional>> ObtenerPorId(int id)
        {
            var profesional = await _profesionalRepository.ObtenerPorIdAsync(id);
            if (profesional == null) return NotFound();
            return Ok(profesional);
        }

        [HttpPost]
        public async Task<ActionResult<Profesional>> Crear(Profesional profesional)
        {
             await _profesionalRepository.CrearAsync(profesional);
             return CreatedAtAction(nameof(ObtenerPorId), new { id = profesional.Id }, profesional);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, Profesional profesional)
        {
            if (id != profesional.Id) return BadRequest();
            await _profesionalRepository.ActualizarAsync(profesional);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _profesionalRepository.EliminarAsync(id);
            return NoContent();
        }
    }
}