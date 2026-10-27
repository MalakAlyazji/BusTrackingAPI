using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingBL.Mapper;
using BusTrackingDL.Models.Data;
using BusTrackingDL.Models.Entites;
using BusTrackingDL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Services
{
    public class RouteService : IRouteService
    {
        readonly AppDBContext _Context;
        public RouteService(AppDBContext Context)
        {
            _Context = Context;
        }
        public async Task<Result<RouteDto>> CreateRoute(CreateRouteDto dto)
        {
            var validation = await ValidateRoute(dto);
            if (!validation.IsSuccess)
            {
                return Result<RouteDto>.Failure(validation.ErrorMessage, validation.ErrorCode);
            }
            if (!Enum.TryParse<enLocation>( dto.StartingPoint,true,out var startingPoint))
            {
                return Result<RouteDto>.Failure("Invalid starting point", 400);
            }
            if (!Enum.TryParse<enLocation>(dto.EndingPoint, true,out var endingPoint))
            {
                return Result<RouteDto>.Failure("Invalid ending point", 400);
            }
            Route NewRoute = dto.MapCreatetoRouteEntity();
            await _Context.Routes.AddAsync(NewRoute);
            await _Context.SaveChangesAsync();
            return Result<RouteDto>.Success(NewRoute.MaptoDto());
        }
        private async Task<Result<bool>> ValidateRoute(CreateRouteDto dto)
        {
            if (!Enum.TryParse<enLocation>(
                dto.StartingPoint, true, out var startingPoint))
            {
                return Result<bool>.Failure("Invalid starting point", 400);
            }
            if (!Enum.TryParse<enLocation>(
                dto.EndingPoint, true, out var endingPoint))
            {
                return Result<bool>.Failure("Invalid ending point", 400);
            }

            var exists = await _Context.Routes.AnyAsync(r =>
                r.StartingPoint == startingPoint &&
                r.EndingPoint == endingPoint);

            if (exists)
            {
                return Result<bool>.Failure("This route already exists!", 400);
            }

            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> DeleteRoute(int id)
        {
           var CheckRoute = await GetRouteById(id);
            if (!CheckRoute.IsSuccess)
            {
                return Result<bool>.Failure(CheckRoute.ErrorMessage,CheckRoute.ErrorCode);
            }
           var deleted=await _Context.Routes.Where(t=>t.Id==id).ExecuteDeleteAsync();
            if (deleted==0)
            {
                return Result<bool>.Failure("Route could not be deleted.",400);
            }
            return Result<bool>.Success(true);
        }
        public async Task<Result<IEnumerable<RouteDto>>> GetAllRoutes()
        {
           var routes= await _Context.Routes.AsNoTracking().Select(t=>t.MaptoDto()).ToListAsync();
            return Result<IEnumerable<RouteDto>>.Success(routes);
        }
        public async Task<Result<RouteDto>> GetRouteById(int id)
        {
            if(id <= 0)
            {
                return Result<RouteDto>.Failure("Id is not valid",400);
            }
            var RouteExist =await _Context.Routes.FirstOrDefaultAsync(t=>t.Id==id);
            if (RouteExist==null)
            {
                return Result<RouteDto>.Failure("Route with id doesnt exist!", 400);
            }
            return Result<RouteDto>.Success(RouteExist.MaptoDto());
        }
        public async Task<Result<RouteDto>> UpdateRoute(int id, RouteDto routeDto)
        {
            var CheckRoute = await GetRouteById(id);
            if (!CheckRoute.IsSuccess)
            {
                return Result<RouteDto>.Failure(CheckRoute.ErrorMessage, CheckRoute.ErrorCode);
            }
            var Route=await _Context.Routes.FirstOrDefaultAsync(t=>t.Id == id);
            routeDto.MapUpdateToDto(Route);
            await _Context.SaveChangesAsync();
            return Result<RouteDto>.Success(Route.MaptoDto());
        }
    }
}
