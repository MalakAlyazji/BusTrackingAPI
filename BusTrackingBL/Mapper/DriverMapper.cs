using BusTrackingBL.DTOs.Requests;
using BusTrackingDL.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BusTrackingBL.DTOs.Responses;
namespace BusTrackingBL.Mapper
{
    public static class DriverMapper
    {
       public static Driver MapAddDtoToEntity(this AddDriverDto dto)
        {
            return new Driver { BusNumber=dto.BusNumber ,DriverName=dto.DriverName,
                Phone=dto.Phone
            };
        }
       public static DriverDto MapToDriverDto(this Driver entity)
        {
            return new DriverDto
            {
                DriverName = entity.DriverName,
                ID = entity.Id,
                Phone = entity.Phone,
                BusNumber = entity.BusNumber,
                IsActive = true
            };
        }
        public static void MapUpdateDtoToEntity(this UpdateDriverDto dto, Driver entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.BusNumber = dto.BusNumber;
            entity.DriverName = dto.DriverName;
            entity.Phone = dto.Phone;
        }
    }
}
