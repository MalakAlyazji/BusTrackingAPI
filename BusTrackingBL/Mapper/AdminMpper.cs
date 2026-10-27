using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingDL.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Mapper
{
    public static class AdminMapper
    {
        public static AdminDto MapToAdminDtoo(this Admin Entity)
        {
            return new AdminDto { Id=Entity.Id ,UserName=Entity.UserName};
        }
       public static Admin MaptoEntity(this AdminDto dto)
        {
            return new Admin { UserName=dto.UserName, Id=dto.Id};
        }
        public static Admin MapCreatetoEntity(this CreateNewAdminDto dto)
        {
            return new Admin { UserName = dto.UserName };
        }
        public static void MapUpdateEntitytoDto(this updateAdminDto dto,Admin entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UserName=dto.UserName;
            entity.UpdatedAt=DateTime.Now;
        }
    }
}
