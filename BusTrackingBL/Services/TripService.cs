using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingDL.Models.Data;
using BusTrackingDL.Models.Entites;
using BusTrackingBL.Mapper;
using Microsoft.EntityFrameworkCore;
using BusTrackingDL.Models.Enums;
using System.Threading.Tasks;
namespace BusTrackingBL.Services
{
    public class TripService : ITripService
    {
        private readonly AppDBContext _Context;
        public TripService(AppDBContext context)
        {
            _Context = context;
        }
        public async Task<Result<TripDto>> CreateNewTrip(CreateTripDto dto)
        {
            var ValidateResult = await ValidateCreateTrip(dto);
            if (!ValidateResult.IsSuccess)
            {
                return Result<TripDto>.Failure(ValidateResult.ErrorMessage, ValidateResult.ErrorCode);
            }
            Trip NewTrip = dto.MapToEntity();
            _Context.Trips.Add(NewTrip);
            await _Context.SaveChangesAsync();
            return Result<TripDto>.Success(NewTrip.MapToTripDto());
        }
        private async Task<Result<bool>> ValidateCreateTrip(CreateTripDto dto)
        {
            if (dto == null)
            {
                return Result<bool>.Failure("Invalid trip details!", 400);
            }
            var driver = await _Context.Drivers.FirstOrDefaultAsync(t => t.Id == dto.DriverId);
            if (driver == null)
            {
                return Result<bool>.Failure("Driver does not exist.", 404);
            }
            if (!driver.IsActive)
            {
                return Result<bool>.Failure("This driver is not active!", 400);
            }
            var route = await _Context.Routes.FirstOrDefaultAsync(r => r.Id == dto.RouteId);

            if (route == null)
            {
                return Result<bool>.Failure(
                    "Route does not exist.", 404);
            }
            if (dto.AdminId.HasValue && !await _Context.Admins.AnyAsync(a => a.Id == dto.AdminId.Value))
            {
                return Result<bool>.Failure("Admin does not exist.", 404);
            }
            if (dto.TripTime < new TimeSpan(8, 0, 0) || dto.TripTime > new TimeSpan(20, 0, 0))
            {
                return Result<bool>.Failure("Trip time must be between 8:00 AM and 8:00 PM.", 400);
            }

            if (await _Context.Trips.AnyAsync(t =>t.DriverId == dto.DriverId &&t.TripTime == dto.TripTime &&
      t.TripDay == dto.TripDay &&t.TripStatus != enStatus.Cancelled))
            {
                return Result<bool>.Failure(
                    "This driver already has a trip at the same time.",
                    400);
            }
            return Result<bool>.Success(true);
        }
        public async Task<Result<TripDto>> StartWaiting(int TripId)
        {
            if (TripId <= 0)
            {
                return Result<TripDto>.Failure("Id is not valid", 400);
            }
            var SelectedTrip = await GetTripById(TripId);
            if (!SelectedTrip.IsSuccess)
            {
                return Result<TripDto>.Failure("Trip does not exist.", 404);
            }
            TimeSpan tripTime = SelectedTrip.Data.TripTime;
            TimeSpan startWaiting = GetWaitingStartTime(tripTime);
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;
            if (!canStartWaiting(SelectedTrip.Data))
            {
                return Result<TripDto>.Failure("Trip is not schedualed!", 400);
            }
            if (!HasWaitingStarted(startWaiting))
            {
                return Result<TripDto>.Failure("out of waiting period", 400);
            }
            if (SelectedTrip.Data.TripDay != DateTime.Now.DayOfWeek)
            {
                return Result<TripDto>.Failure(
                    "This trip is not scheduled for today.",
                    400);
            }
            SelectedTrip.Data.TripStatus = enStatus.Waiting;
            SelectedTrip.Data.DriverArrivedAt = DateTime.Now;
            await _Context.SaveChangesAsync();
            return Result<TripDto>.Success(SelectedTrip.Data.MapToTripDto());
        }
        private async Task<Result< Trip>> GetTripById(int tripId)
        {
            var trip=await _Context.Trips.FirstOrDefaultAsync(t => t.Id == tripId);
            if (trip == null)
            {
                return Result<Trip>.Failure($"trip with id= {tripId} doesn't exist!", 400);
            }
            if (trip.TripStatus == enStatus.Cancelled)
            {
                return Result<Trip>.Failure("trip is cancelled!", 400);
            }
            return Result<Trip>.Success(trip);
        }
        private TimeSpan GetWaitingStartTime(TimeSpan tripTime) => tripTime - TimeSpan.FromMinutes(30);
        private bool HasWaitingStarted(TimeSpan startWaiting) => DateTime.Now.TimeOfDay >= startWaiting;
        private bool canStartWaiting(Trip trip) => trip.TripStatus == enStatus.Scheduled;
        private async Task<Result<TripDto>> CanDepart(Trip trip)
        {
            if (trip.TripStatus != enStatus.Waiting)
            {
                return Result<TripDto>.Failure(
                    "Trip must be in Waiting status before it can depart.",
                    400);
            }
            if (DateTime.Now.TimeOfDay < trip.TripTime)
            {
                return Result<TripDto>.Failure(
                    "Trip cannot depart before its scheduled departure time.",
                    400);
            }
            if (!await HasComingStudents(trip.Id))
            {
                return Result<TripDto>.Failure(
                   "Trip cannot depart while no students coming.",
                   400);
            }
            return Result<TripDto>.Success(
                trip.MapToTripDto());
        }
        private TimeSpan GetLatestDepartureTime(TimeSpan tripTime)
        {
            if (tripTime == new TimeSpan(7, 0, 0))
            {
                return tripTime + TimeSpan.FromMinutes(5);
            }
            return tripTime + TimeSpan.FromMinutes(10);
        }
        //Driver
        public async Task<Result<TripDto>> MarkEveryoneHere(int tripId)
        {
            if (tripId <= 0)
            {
                return Result<TripDto>.Failure("Id is not valid",400);
            }
            var selectedTrip = await GetTripById(tripId);
            if (!selectedTrip .IsSuccess)
            {
                return Result<TripDto>.Failure("Trip does not exist.",404);
            }
            var allowDepart =await CanDepart(selectedTrip.Data);
            if (!allowDepart.IsSuccess)
            {
                return allowDepart;
            }
            selectedTrip.Data.TripStatus = enStatus.Departed;
            selectedTrip.Data.ActualDepartureTime = DateTime.Now;
            await _Context.SaveChangesAsync();
            return Result<TripDto>.Success(selectedTrip.Data.MapToTripDto());
        }
        public async Task<Result<IEnumerable<TripDto>>> GetAllTrips()
        {
            var trips = await _Context.Trips.AsNoTracking().Where(d => d.TripStatus != enStatus.Cancelled).OrderBy(d => d.TripTime).Select(t => t.MapToTripDto()).ToListAsync();

            return Result<IEnumerable<TripDto>>.Success(trips);
        }
        public async Task<Result<IEnumerable<TripDto>>> GetAllComingTrips()
        {
            var currentTime = DateTime.Now.TimeOfDay;
            var currentDay = DateTime.Now.DayOfWeek;
            var trips = await _Context.Trips.AsNoTracking().
               Where(d => (d.TripStatus == enStatus.Scheduled || d.TripStatus == enStatus.Waiting)
           && d.TripTime > currentTime && d.TripDay == currentDay).OrderBy(d => d.TripTime).
                Select(t => t.MapToTripDto()).ToListAsync();
            return Result<IEnumerable<TripDto>>.Success(trips);
        }
        public async Task<Result<TripDto>> GetTripDtoById(int TripId)
        {
            if (TripId <= 0)
            {
                return Result<TripDto>.Failure("Invalid trip Id!", 400);
            }
            var trip = await GetTripById(TripId);
            if (!trip.IsSuccess)
            {
                return Result<TripDto>.Failure(trip.ErrorMessage, trip.ErrorCode);
            }
           
            return Result<TripDto>.Success(trip.Data.MapToTripDto());
        }
        async Task<Driver> GetActiveDriver(int DriverId) => await _Context.Drivers.FirstOrDefaultAsync(d => d.Id == DriverId && d.IsActive);
        async Task<Route> GetRoute(UpdateTripDto trip) => await _Context.Routes.FirstOrDefaultAsync(d => d.Id == trip.RouteId);
        async Task<bool> CheckConflictDriverWithTrip(int tripID, int DriverID, TimeSpan tripTime, DayOfWeek Day) => await _Context.Trips.AnyAsync
                (t => t.Id != tripID && t.DriverId == DriverID && t.TripTime == tripTime && t.TripDay == Day && t.TripStatus != enStatus.Cancelled);
        //By Admin or driver
        public async Task<Result<bool>> CancelTrip(int tripId)
        {
            if (tripId <= 0)
                return Result<bool>.Failure("Invalid trip ID.", 400);
            var trip = await GetTripById(tripId);
            if (!trip.IsSuccess)
                return Result<bool>.Failure(trip.ErrorMessage, trip.ErrorCode);
            var ValidateCancellation = await CanCancelTrip(trip.Data);
            if (!ValidateCancellation.IsSuccess)
            {
                return Result<bool>.Failure(ValidateCancellation.ErrorMessage,ValidateCancellation.ErrorCode);
            }
            trip.Data.TripStatus = enStatus.Cancelled;
            await _Context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        public async Task<Result<TripDto>> UpdateTripAsync(int tripId, UpdateTripDto trip)
        {
            if (tripId <= 0)
            {
                return Result<TripDto>.Failure(
                    "Id is not valid",
                    400);
            }
            var selectedTrip = await GetTripById(tripId);
            if (!selectedTrip.IsSuccess)
            {
                return Result<TripDto>.Failure("Trip does not exist.", 404);
            }
            if (!canStartWaiting(selectedTrip.Data))
            {
                return Result<TripDto>.Failure("Trip cannot update unless its scheduled departure time.", 400);
            }
            var driver =await GetActiveDriver(trip.DriverId);
            if (driver == null)
            {
                return Result<TripDto>.Failure("Driver does not exist or is inactive.", 404);
            }
            var route =await GetRoute(trip);
            if (route == null)
            {
                return Result<TripDto>.Failure("Route does not exist.", 404);
            }

            if (await CheckConflictDriverWithTrip(tripId, driver.Id, trip.TripTime, trip.TripDay))
            {
                return Result<TripDto>.Failure("driver has another trip!", 400);
            }
            selectedTrip.Data.MapUpdateDto(trip);
            await _Context.SaveChangesAsync();
            return Result<TripDto>.Success(selectedTrip.Data.MapToTripDto());
        }

        // CompleteTrip By driver
        public async Task<Result<bool>> CompleteTrip(int tripId)
        {
            if (tripId <= 0)
                return Result<bool>.Failure("Invalid trip ID.", 400);
            var trip = await GetTripById(tripId);
            if (!trip.IsSuccess)
                return Result<bool>.Failure(trip.ErrorMessage, trip.ErrorCode);
            if (trip.Data.TripStatus != enStatus.Departed)
            {
                return Result<bool>.Failure("Can't complete non-Departed trip!", 400);
            }
            trip.Data.TripStatus = enStatus.completed;
            int WaitingTime = await GetWaitingTime();
            if (WaitingTime >= 60)
            {
                var result = await CreateNewReturnTrip(trip.Data);
                if (!result.IsSuccess)
                {
                    return Result<bool>.Failure(
                        result.ErrorMessage,
                        result.ErrorCode);
                }
            }
            await _Context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        async Task<int> GetWaitingTime()
        {
            var currentTime = DateTime.Now.TimeOfDay;
            var currentDay = DateTime.Now.DayOfWeek;

            var nextTripTime = await _Context.Trips
                .Where(t =>
                    t.RouteInfo.EndingPoint == enLocation.University &&
                    t.TripDay == currentDay &&
                    t.TripTime > currentTime)
                .OrderBy(t => t.TripTime)
                .Select(t => t.TripTime)
                .FirstOrDefaultAsync();
            return (int)(nextTripTime - currentTime).TotalMinutes;
        }
        async Task<Result<Trip>> CreateNewReturnTrip(Trip tripEntity)
        {
            await _Context.Entry(tripEntity).Reference(t => t.RouteInfo).LoadAsync();
            var newRouteId = await _Context.Routes.Where(r =>
                    r.StartingPoint == tripEntity.RouteInfo.EndingPoint &&
                    r.EndingPoint == tripEntity.RouteInfo.StartingPoint)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
            if (newRouteId == 0)
            {
                return Result<Trip>.Failure(
                    "Return route does not exist.", 404);
            }
            var returnTrip = new Trip
            {
                RouteId = newRouteId,
                DriverId = tripEntity.DriverId,
                TripTime = DateTime.Now.TimeOfDay,
                TripStatus = enStatus.Waiting,
                DriverArrivedAt = DateTime.Now,
                TripDay = tripEntity.TripDay
            };
            _Context.Trips.Add(returnTrip);
            return Result<Trip>.Success(returnTrip);
        }        
        // Driver or admin
        public async Task<Result<IEnumerable<TripDto>>>GetDriverTrips(int DriverId)
        {
           var Driver=await GetActiveDriver(DriverId);
            if (Driver == null)
            {
                return Result<IEnumerable<TripDto>>.Failure(
                    "Driver does not exist or is inactive.",
                    404);
            }
            var trips=await _Context.Trips.AsNoTracking().Where(t => t.DriverId == DriverId).
                OrderBy(t=>t.TripTime).Select(t=>t.MapToTripDto()).ToListAsync();
            return Result<IEnumerable<TripDto>>.Success(trips);
        }
        private async Task<bool> HasComingStudents(int tripId)
        {
            return await _Context.StudentConfirmations
                .AnyAsync(sc =>
                    sc.TripId == tripId &&
                    !sc.IsCancelled);
            
        }
        public async Task<Result<int>> AutoDepartTrips()
        {
            DayOfWeek CurrentDay =DateTime.Now.DayOfWeek;
            TimeSpan CurrentTime=DateTime.Now.TimeOfDay;
            var trips = await _Context.Trips
     .Where(t => t.TripStatus == enStatus.Waiting
     && t.TripDay == CurrentDay
     && CurrentTime >= GetLatestDepartureTime(t.TripTime)).ToListAsync();
            int departedTrips = 0;
            foreach (var trip in trips) {
                if(await HasComingStudents(trip.Id))
                {
                    trip.TripStatus = enStatus.Departed;
                    trip.ActualDepartureTime = DateTime.Now;
                    departedTrips++;
                }
                else
                {
                    trip.TripStatus = enStatus.Cancelled;
                }
            }
            await _Context.SaveChangesAsync();
            return Result<int>.Success(departedTrips);
        }
        private async Task<Result<bool>> CanCancelTrip(Trip trip)
        {
            if (trip.TripStatus == enStatus.Scheduled)
            {
                return Result<bool>.Success(true);
            }

            if (trip.TripStatus == enStatus.Waiting)
            {
                if (await HasComingStudents(trip.Id))
                {
                    return Result<bool>.Failure(
                        "Cannot cancel trip because students are coming.",
                        400);
                }
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure( "Trip cannot be cancelled in its current status.",400);
        }
    }
}