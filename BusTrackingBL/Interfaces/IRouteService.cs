using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Interfaces
{
    public interface IRouteService
    {
        public Task<Result<RouteDto>> CreateRoute(CreateRouteDto dto);
        public Task<Result<RouteDto>> UpdateRoute(int id, RouteDto routeDto);
        public Task<Result<bool>> DeleteRoute(int id);
        public Task<Result<RouteDto>> GetRouteById(int id);
        public Task<Result<IEnumerable<RouteDto>>> GetAllRoutes();
    }
}
