using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Domain.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    /// <summary>
    /// Controlador con los métodos CRUD para la administración del histórico de posiciones de un empleado.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PositionHistoryController : Controller
    {
        private readonly PositionHistoryService _positionHistoryService;
        private readonly IRequestHandler<GetPositionHistoryRequest, PositionHistory> _getPositionHistoryHandler; 

        public PositionHistoryController(PositionHistoryService positionHistoryService, IRequestHandler<GetPositionHistoryRequest, PositionHistory> getPositionHistoryHandler) 
        {
            _positionHistoryService = positionHistoryService;
            _getPositionHistoryHandler = getPositionHistoryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPositionHistory()
        {
            var positionHistoryList = await _positionHistoryService.GetAllPositionHistoryAsync();
            return Ok(positionHistoryList);
        }

        [HttpGet("ById/{Id}")]
        public async Task<IActionResult> GetPositionHistoryById(int Id)
        {
            var positionHistory = await _positionHistoryService.GetById(Id);
            return Ok(positionHistory);
        }

        [HttpGet("ByEmployee/{EmployeeId}")]
        public async Task<IActionResult> GetPositionHistoryByEmployeeId(int EmployeeId)
        {
            var positionHistory = await _positionHistoryService.GetListByEmployeeId(EmployeeId);
            return Ok(positionHistory);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePositionHistory([FromBody] PositionHistoryDto positionHistoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var positionHistory = await _positionHistoryService.CreatePositionHistoryAsync(positionHistoryDto.EmployeeId, positionHistoryDto.Position, positionHistoryDto.StartDate, positionHistoryDto.EndDate);

            return Ok(positionHistory);
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] PositionHistoryDto positionHistoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedEmployee = await _positionHistoryService.UpdateEmployeeAsync(id, positionHistoryDto.EmployeeId, positionHistoryDto.Position, positionHistoryDto.StartDate, positionHistoryDto.EndDate);
            if (updatedEmployee == null)
            {
                return NotFound();
            }

            return Ok(updatedEmployee);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _positionHistoryService.DeletePositionHistoryAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
