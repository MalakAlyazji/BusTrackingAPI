using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Services;
using BusTrackingDL.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Mapper
{
    public static class StudentMapper
    {
        public static StudentDto MapEntitytoDto(this Student student)
        {
            return new StudentDto { StudentName = student.StudentName, Phone = student.Phone,Id=student.Id };
        }
        public static Student MapDtoToEntity(this StudentDto dto)
        {
            return new Student { Id = dto.Id, Phone = dto.Phone, StudentName = dto.StudentName };
        }
        public static Student MapNewDtoToStudent(this NewStudentDto dto)
        {
            return new Student { Phone = dto.Phone, StudentName = dto.StudentName };
        }
        public static void MapUpdate(this updateStudentDto dto, Student student)
        {
            student.UpdatedAt = DateTime.Now;
            student.StudentName=dto.StudentName;
            student.Phone=dto.Phone;
        }
    }
}
