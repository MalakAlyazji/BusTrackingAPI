using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingBL.Services;
using BusTrackingDL.Models.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace BusTrackingAPI.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudent _studentService;
        public StudentController(IStudent studentService)
        {
            _studentService = studentService;
        }
        [HttpPost("CreateNewStudent", Name = "CreateNewStudent")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> CreateNewStudent(NewStudentDto dto)
        {
            var result = await _studentService.CreateNewStudent(dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return CreatedAtRoute("GetStudentById", new {studentId=result.Data.Id},result.Data);
        }
        [HttpGet("GetAllStudents", Name = "GetAllStudents")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            var result = await _studentService.GetAllStudents();
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }
        [HttpPut("UpdateStudentById/{id}", Name = "UpdateStudentById")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> UpdateStudentById(int id, updateStudentDto dto)
        {
            var result = await _studentService.UpdateStudentById(id, dto);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }
        [HttpGet("GetStudentById/{id}", Name = "GetStudentById")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            var result = await _studentService.GetStudentById(id);
            if (!result.IsSuccess)
            {
                return Problem(detail: result.ErrorMessage, statusCode: result.ErrorCode);
            }
            return Ok(result.Data);
        }
    }
}
