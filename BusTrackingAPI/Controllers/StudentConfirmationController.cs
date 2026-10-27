using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingBL.Services;
using BusTrackingDL.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackingAPI.Controllers
{
    [Route("api/StudentConfirmation")]
    [ApiController]
    public class StudentConfirmationController : ControllerBase
    {
        readonly IStudentConfirmation _IStudentConfirmation;
        public StudentConfirmationController(IStudentConfirmation IStudentConfirmation)
        {
            _IStudentConfirmation = IStudentConfirmation;
        }
       
        [HttpPost("CreateConfirmStudent",Name = "CreateConfirmStudent")]
        [ProducesResponseType(typeof(CreateConfirmationDto),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreateConfirmationDto>> CreateConfirmStudent(CreateConfirmationDto dto)
        {
            var result=await _IStudentConfirmation.CreateConfirmStudent(dto);
            if (!result.IsSuccess)
            {
                return Problem(statusCode: result.ErrorCode,detail:result.ErrorMessage);
            }
            return StatusCode(StatusCodes.Status201Created,result.Data);
        }
       
        [HttpDelete("CancelConfirmation", Name =("CancelConfirmation"))]
        [ProducesResponseType(typeof(bool),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),statusCode:StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), statusCode:StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CancelConfirmation(CancelConfirmationDto dto)
        {
            var result = await _IStudentConfirmation.CancelConfirmation(dto);
            if (!result.IsSuccess)
            {
                return Problem(statusCode: result.ErrorCode, detail: result.ErrorMessage);
            }
            return Ok(result.Data);
        }
        
        [HttpGet("GetConfirmations/{tripId}", Name = "GetConfirmations")]
        [ProducesResponseType(typeof(IEnumerable<ConfirmDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ConfirmDto>>> GetConfirmationsByTripId(int tripId)
        {
            var result = await _IStudentConfirmation.GetConfirmationsByTripId(tripId);
            if (!result.IsSuccess)
            {
                return Problem(statusCode: result.ErrorCode, detail: result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpDelete("CancelConfirmationById/{id}", Name = "CancelConfirmationById")]
        [ProducesResponseType(typeof(IEnumerable<ConfirmDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ConfirmDto>>> CancelConfirmationById(int id)
        {
            var result = await _IStudentConfirmation.CancelConfirmationById(id);
            if (!result.IsSuccess)
            {
                return Problem(statusCode: result.ErrorCode, detail: result.ErrorMessage);
            }
            return Ok("condirmation cancelled successfully!");
        }
    }
}