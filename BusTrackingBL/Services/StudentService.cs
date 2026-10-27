using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingBL.Mapper;
using BusTrackingDL.Models.Data;
using BusTrackingDL.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Services
{
    public class StudentService:IStudent
    {
        private readonly  AppDBContext  _context;
        public StudentService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Result<StudentDto>> CreateNewStudent(NewStudentDto dto)
        {
            var checkStudent=await validateNewStudent(dto);
            if (!checkStudent.IsSuccess)
            {
                return Result<StudentDto>.Failure(checkStudent.ErrorMessage,checkStudent.ErrorCode);
            }
            Student NewStudent= dto.MapNewDtoToStudent();
             _context.Students.Add(NewStudent);
            await _context.SaveChangesAsync();
            return Result<StudentDto>.Success(NewStudent.MapEntitytoDto());
        }
        async Task<Result<bool>> validateNewStudent(NewStudentDto dto)
        {
            if(dto == null)
            {
                return Result<bool>.Failure("Invalid Student details.");
            }
            var student=await _context.Students.AnyAsync(t=>t.Phone==dto.Phone);
            if (student)
            {
                return Result<bool>.Failure($"student with phone {dto.Phone} already exist!", 400);
            }
            return Result<bool>.Success(true);
        }
        public async Task<Result<IEnumerable< StudentDto>>> GetAllStudents()
        {
            var students=await _context.Students.AsNoTracking().Select(t=>t.MapEntitytoDto()).ToListAsync();
            return Result<IEnumerable<StudentDto>>.Success(students);
        }
        public async Task<Result<StudentDto>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return Result<StudentDto>.Failure("Invalid id", 400);
            }
            var student=await _context.Students.FirstOrDefaultAsync(t=>t.Id==id);
            if (student == null)
            {
                return Result<StudentDto>.Failure("Student not found", 400);
            }
            return Result<StudentDto>.Success(student.MapEntitytoDto());
        }
        public async Task<Result<StudentDto>> UpdateStudentById( int id, updateStudentDto dto)
        {
            if (dto == null)
            {
                return Result<StudentDto>.Failure("Invalid student details.",400);
            }
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return Result<StudentDto>.Failure( $"Student with id {id} not found.", 404);
            }
            dto.MapUpdate(student);
            await _context.SaveChangesAsync();
            return Result<StudentDto>.Success(student.MapEntitytoDto());
        }
    }
}
