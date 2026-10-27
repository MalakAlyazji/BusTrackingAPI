using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusTrackingAPI.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _AdminService;
        public AdminController(IAdmin admin)
        {
            _AdminService = admin;
        }
        [HttpPost("CreateNewAdmin", Name = "CreateNewAdmin")]
        [ProducesResponseType(typeof(AdminDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AdminDto>> CreateNewAdmin(CreateNewAdminDto dto)
        {
            var Result = await _AdminService.CreateNewAdmin(dto);
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            if (Result.Data?.Id == 0 || Result.Data?.Id == null)
            {
                return Problem(detail: "Admin ID not generated", statusCode: 500);
            }
            return CreatedAtRoute("GetAdminById", new { id = Result.Data.Id }, Result.Data);
        }

        [HttpPut("updateAdmin/{id}", Name = "updateAdmin")]
        [ProducesResponseType(typeof(AdminDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminDto>> UpdateAdmin(updateAdminDto dto, int id)
        {
            var Result = await _AdminService.UpdateAdmin(id, dto);
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            return Ok(Result.Data);
        }
        [HttpDelete("DeleteAdmin/{id}", Name = "DeleteAdmin")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteAdmin(int id)
        {
            var Result = await _AdminService.DeleteAdmin(id);
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            return Ok("Admin deleted successfully");
        }
        [HttpGet("GetAdminById/{id}", Name = "GetAdminById")]
        [ProducesResponseType(typeof(AdminDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminDto>> GetAdminById(int id)
        {
            var Result = await _AdminService.GetAdminById(id);
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            return Ok(Result.Data);
        }
        [HttpGet("GetAllAdmins", Name = "GetAllAdmins")]
        [ProducesResponseType(typeof(IEnumerable<AdminDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAllAdmins()
        {
            var Result = await _AdminService.GetAllAdmins();
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            return Ok(Result.Data);
        }

        [HttpGet("GetActiveAdmins", Name = "GetActiveAdmins")]
        [ProducesResponseType(typeof(IEnumerable<AdminDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetActionResultAsync()
        {
            var Result = await _AdminService.GetAllActiveAdmins();
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            return Ok(Result.Data);
        }

        [HttpPost("LoginAdmin", Name = "LoginAdmin")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<LoginResponseDto>> LoginAdmin(LoginDto dto)
        {
            var Result = await _AdminService.Login(dto);
            if (!Result.IsSuccess)
            {
                return Problem(detail: Result.ErrorMessage, statusCode: Result.ErrorCode);
            }
            return Ok("Login successfuly!");
        }

        [HttpPut("ChangeAdminPassword", Name = "ChangeAdminPassword")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ChangeAdminPassword(changePasswordDto dto)
        {
            var result=await _AdminService.ChangePassword(dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok("password changes successfuly");
        }
        [HttpPut("ActivateAdmin", Name = "ActivateAdmin")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ActivateAdmin(int id)
        {
            var result = await _AdminService.ActivateAdmin(id);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok("Admin activated successfuly");
        }
    }
}
