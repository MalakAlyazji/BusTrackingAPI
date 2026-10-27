
using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackingAPI.Controllers
{
    [Route("api/Route")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _RouteService;
        public RouteController(IRouteService RouteService)
        {
            _RouteService = RouteService;
        }


        [HttpDelete("DeleteRoute/{id}", Name = "DeleteRoute")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteRoute(int id)
        {
            var result = await _RouteService.DeleteRoute(id);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }


        [HttpGet("GetRouteById/{Id}", Name = "GetRouteById")]
        [ProducesResponseType(typeof(RouteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RouteDto>> GetRouteById(int Id)
        {
            var result = await _RouteService.GetRouteById(Id);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }


        [HttpPut("UpdateRoute/{Id}", Name = "UpdateRoute")]
        [ProducesResponseType(typeof(RouteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RouteDto>> UpdateRoute(int Id, RouteDto dto)
        {
            var result = await _RouteService.UpdateRoute(Id, dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }


        [HttpGet("GetAllRoutes", Name = "GetAllRoutes")]
        [ProducesResponseType(typeof(IEnumerable<RouteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RouteDto>>> GetAllRoutes()
        {
            var result = await _RouteService.GetAllRoutes();
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }


        [HttpPost("CreateRoute",Name = "CreateRoute")]
        [ProducesResponseType(typeof(RouteDto),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RouteDto>> CreateRoute(CreateRouteDto dto)
        {
            var result = await _RouteService.CreateRoute(dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return StatusCode(StatusCodes.Status201Created,result.Data);
        }
    }
}
