using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingBL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackingAPI.Controllers
{
    [Route("api/Driver")]
    [ApiController]
    public class DriverController : ControllerBase
    {
         readonly IDriver _driverService;
        public DriverController(IDriver driver)
        {
            _driverService = driver;
        }
        [HttpGet("GetDriverById/{driverId}", Name = "GetDriverById")]
        [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DriverDto>> GetDriverById(int driverId)
        {
            var result = await _driverService.GetDriverById(driverId);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            } return Ok(result.Data);

        }




        [HttpGet("GetActiveDrivers", Name = "GetActiveDrivers")]
        [ProducesResponseType(typeof(IEnumerable<DriverDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DriverDto>>> GetActiveDrivers()
        {
            var result = await _driverService.GetActiveDrivers();
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }




        [HttpPost("AddDriver", Name = "AddDriver")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DriverDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverDto>> AddDriver(AddDriverDto dto)
        {
            var result = await _driverService.AddDriver(dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return CreatedAtRoute("GetDriverById",new { driverId = result.Data.ID } ,result.Data);
        }


        [HttpPut("UpdateDriver/{driverId}", Name = "UpdateDriver")]
        [ProducesResponseType(typeof(DriverDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DriverDto>> UpdateDriver(int driverId, UpdateDriverDto dto)
        {
            var result = await _driverService.UpdateDriver(driverId,dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }



        [HttpDelete("DeactivateDriver/{driverId}", Name = "DeactivateDriver")]
        [ProducesResponseType(typeof(DriverDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeactivateDriver(int driverId)
        {
            var result = await _driverService.DeactivateDriver(driverId);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok("Driver deactivated successfuly!");
        }
        



        [HttpPut("ActivateDriver/{driverId}", Name = "ActivateDriver")]
        [ProducesResponseType(typeof(DriverDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> ActivateDriver(int driverId)
        {
            var result = await _driverService.ActivateDriver(driverId);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok("Driver activated successfully!");
        }



        [HttpPut("ChangeDriverPassword", Name = "ChangeDriverPassword")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ChangeDriverPassword(changePasswordDto dto)
        {
            var result = await _driverService.ChangePassword(dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok("password changes successfuly");
        }

        [HttpPost("LoginDriver", Name = "LoginDriver")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LoginResponseDto>> LoginDriver(LoginDto dto)
        {
            var Result = await _driverService.Login(dto);
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            return Ok("Login successfuly!");
        }

    }
}
