using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingDL.Models.Entites;
using BusTrackingDL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Mapper
{
    public static class RouteMapper
    {
       public static Route MapCreatetoRouteEntity(this CreateRouteDto dto)
        {
            return new Route
            {
                StartingPoint = Enum.Parse<enLocation>(dto.StartingPoint,true),
                EndingPoint = Enum.Parse<enLocation>(dto.EndingPoint,true)
            };
        }
       public static RouteDto MaptoDto(this Route entity)
        {
            return new RouteDto
            {
                RouteId=entity.Id,
                EndingPoint = entity.EndingPoint.ToString(),
                StartingPoint = entity.StartingPoint.ToString()
            };
        }
        public static void MapUpdateToDto(this RouteDto dto, Route entity)
        {
            entity.EndingPoint = Enum.Parse<enLocation>(dto.EndingPoint, true);
            entity.StartingPoint = Enum.Parse<enLocation>(dto.StartingPoint, true);
        }
    }
}
