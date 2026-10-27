using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Services;
using BusTrackingDL.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace BusTrackingBL.Mapper
{
    public static class ConfirmationMapper
    {
       public static BusTrackingDL.Models.Entites.StudentConfirmation MapToEntity(this CreateConfirmationDto dto)
        {
            return new BusTrackingDL.Models.Entites.StudentConfirmation
            {
                StudentId = dto.StudentId,
                TripId = dto.TripId,
                ConfirmedAt = DateTime.UtcNow,
                Notes = dto.Notes
            };
        }
        public static ConfirmDto MapToConfirmDto(this BusTrackingDL.Models.Entites.StudentConfirmation Entity)
        {
            return new ConfirmDto
            {
                ConfirmationId = Entity.Id,
                StudentId = Entity.StudentId,
                TripId = Entity.TripId,
                ConfirmedAt = Entity.ConfirmedAt,
                Notes = Entity.Notes
            };
        }
    }
}
