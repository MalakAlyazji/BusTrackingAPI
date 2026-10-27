using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingDL.Models.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace BusTrackingAPI.Controllers
{
    [Route("api/Trip")]
    [ApiController]
    public class TripController : ControllerBase
    {
        readonly ITripService _tripService;
        public TripController(ITripService tripService)
        {
            _tripService= tripService;
        }
        [HttpPost("AddNewTrip", Name = "AddNewTrip")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TripDto>> AddNewTrip(CreateTripDto dto)
        {
            var result=await _tripService.CreateNewTrip(dto);
            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: result.ErrorCode,
                    detail: result.ErrorMessage);
            }
            return StatusCode(
                StatusCodes.Status201Created,
                result.Data);
        }
        [HttpPost("StartDeparting/{tripId}", Name = "StartDeparting")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TripDto>> StartDeparting(int tripId)
        {
            var result=await _tripService.MarkEveryoneHere(tripId);
            if (!result.IsSuccess)
            {
                return Problem(statusCode:result.ErrorCode,detail:result.ErrorMessage);
            }
            return Ok(result.Data);
        }
        [HttpPost("StartWaiting/{tripId}", Name = "StartWaiting")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TripDto>> StartWaiting(int tripId)
        {
            var result = await _tripService.StartWaiting(tripId);
            if (!result.IsSuccess)
            {
                return Problem(statusCode: result.ErrorCode, detail: result.ErrorMessage);
            }
            return Ok(result.Data);
        }
        [HttpGet("GetAllTrips", Name = "GetAllTrips")]
        [ProducesResponseType(typeof(IEnumerable<TripDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetAllTrips()
        {
            var result = await _tripService.GetAllTrips();
            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: result.ErrorCode,
                    detail: result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        [HttpGet("GetAllComingTrips", Name = "GetAllComingTrips")]
        [ProducesResponseType(typeof(IEnumerable<TripDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetAllComingTrips()
        {
            var result = await _tripService.GetAllComingTrips();
            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: result.ErrorCode,
                    detail: result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        [HttpGet("GetTripById/{id}", Name = "GetTripById")]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TripDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TripDto>>GetTripById(int id)
        {
            var trip=await _tripService.GetTripDtoById(id);
            if (!trip.IsSuccess)
            {
                return Problem(
                   statusCode: trip.ErrorCode,
                   detail: trip.ErrorMessage);
            }
            return Ok(trip.Data);
        }
        [HttpPut("UpdateTripAsync/{tripId}", Name = "UpdateTripAsync")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TripDto>> UpdateTripAsync(int tripId,UpdateTripDto trip) {
            var result=await _tripService.UpdateTripAsync(tripId, trip);
            if (!result.IsSuccess)
            {
                return Problem(
                   statusCode: result.ErrorCode,
                   detail: result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        [HttpGet("CancelTrip",Name = "CancelTrip")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> CancelTrip(int tripId)
        {
            var result = await _tripService.CancelTrip(tripId);
            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: result.ErrorCode,
                   detail: result.ErrorMessage);
            }
            return Ok("trip cancelled successfully!");
        }
        [HttpGet("CompleteTrip", Name = "CompleteTrip")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> CompleteTrip(int tripId)
        {
            var result = await _tripService.CompleteTrip(tripId);
            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: result.ErrorCode,
                   detail: result.ErrorMessage);
            }
            return Ok("trip completed successfully!");
        }

        [HttpGet("GetDriverTrips/{DriverId}", Name = "GetDriverTrips")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetDriverTrips(int DriverId)
        {
            var result = await _tripService.GetDriverTrips(DriverId);
            if (!result.IsSuccess)
            {
                return Problem(
                    statusCode: result.ErrorCode,
                   detail: result.ErrorMessage);
            }
            return Ok(result.Data);
        }
    }
}
