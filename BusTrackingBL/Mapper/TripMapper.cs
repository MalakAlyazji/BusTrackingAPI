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
    public static class TripMapper
    {
         public static TripDto MapToTripDto(this Trip entity)
         {
            return new TripDto
            {
                TripId= entity.Id,
                RouteId= entity.RouteId,
                DriverId= entity.DriverId,
                TripTime= entity.TripTime,
                TripStatus = entity.TripStatus.ToString(),
                TripDay= entity.TripDay.ToString(),
            };
         }
         public static Trip MapToEntity(this CreateTripDto dto)
         {
            return new Trip
            {
                RouteId=dto.RouteId,
                DriverId=dto.DriverId,
                TripTime=dto.TripTime,
                AdminId=dto.AdminId,
                TripDay=dto.TripDay 
            };     
         }
        public static void MapUpdateDto(this Trip entity, UpdateTripDto dto)
        {
            entity.UpdatedAt=DateTime.Now;
            entity.DriverId=dto.DriverId;
            entity.RouteId=dto.RouteId;
            entity.TripTime=dto.TripTime;
            entity.TripDay = dto.TripDay;
        }
    }
}
