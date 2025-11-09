using LeaveManagement.Application.DTOs;
using LeaveManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestsController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LeaveRequestDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LeaveRequestDto>> CreateLeaveRequest([FromBody] CreateLeaveRequestDto createDto)
        {
            try
            {
                var result = await _leaveRequestService.CreateLeaveRequestAsync(createDto);
                return CreatedAtAction(nameof(GetLeaveRequest), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message, code = "VALIDATION_ERROR" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message, code = "INVALID_DATE" });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LeaveRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeaveRequestDto>> GetLeaveRequest(Guid id)
        {
            try
            {
                var result = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LeaveRequestDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetAllLeaveRequests()
        {
            var result = await _leaveRequestService.GetAllLeaveRequestsAsync();
            return Ok(result);
        }

        [HttpGet("employee/{employeeId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<LeaveRequestDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> GetLeaveRequestsByEmployee(Guid employeeId)
        {
            var result = await _leaveRequestService.GetLeaveRequestsByEmployeeIdAsync(employeeId);
            return Ok(result);
        }

        [HttpPost("{id:guid}/approve")]
        [ProducesResponseType(typeof(LeaveRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeaveRequestDto>> ApproveLeaveRequest(Guid id, [FromBody] UpdateLeaveRequestStatusDto updateDto)
        {
            try
            {
                var result = await _leaveRequestService.ApproveLeaveRequestAsync(id, updateDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id:guid}/reject")]
        [ProducesResponseType(typeof(LeaveRequestDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeaveRequestDto>> RejectLeaveRequest(Guid id, [FromBody] UpdateLeaveRequestStatusDto updateDto)
        {
            try
            {
                var result = await _leaveRequestService.RejectLeaveRequestAsync(id, updateDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}