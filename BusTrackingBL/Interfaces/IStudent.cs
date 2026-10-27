using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Interfaces
{
    public interface IStudent
    {
        public Task<Result<StudentDto>> CreateNewStudent(NewStudentDto dto);
        public  Task<Result<IEnumerable<StudentDto>>> GetAllStudents();
        public Task<Result<StudentDto>> UpdateStudentById(int id, updateStudentDto dto);
        public Task<Result<StudentDto>> GetStudentById(int id);
    }
}
