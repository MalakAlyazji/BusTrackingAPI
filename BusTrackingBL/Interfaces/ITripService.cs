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
    public interface ITripService
    {
       public Task< Result< TripDto> > CreateNewTrip(CreateTripDto dto);
        public Task<Result<TripDto>> StartWaiting(int TripId);
        public Task<Result<TripDto>> MarkEveryoneHere(int TripId);
        public Task<Result<IEnumerable<TripDto>>> GetAllComingTrips();
        public  Task<Result<IEnumerable<TripDto>>> GetAllTrips();
        public  Task<Result<TripDto>> GetTripDtoById(int TripId);
        Task<Result<TripDto>> UpdateTripAsync(int tripId, UpdateTripDto dto);
        public Task<Result<bool>> CancelTrip(int tripId);
        public Task<Result<bool>> CompleteTrip(int tripId);
        public Task<Result<IEnumerable<TripDto>>> GetDriverTrips(int DriverId);
        public Task<Result<int>> AutoDepartTrips();
    }
}
