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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Services
{
    public class StudentConfirmation:IStudentConfirmation
    {
        private readonly AppDBContext _Context;
        public StudentConfirmation(AppDBContext Context) {
            _Context = Context;
        }
        public async Task<Result<ConfirmDto>> CreateConfirmStudent(CreateConfirmationDto dto)
        {
           var checkValidation=await validateConfirmation(dto);
            if (!checkValidation.IsSuccess)
            {
                return Result<ConfirmDto>.Failure(checkValidation.ErrorMessage,checkValidation.ErrorCode);
            }
            var NewStudentConfirmation = dto.MapToEntity();
            _Context.StudentConfirmations.Add(NewStudentConfirmation);
            await _Context.SaveChangesAsync();
            return Result<ConfirmDto>.Success(NewStudentConfirmation.MapToConfirmDto());
        }
        async Task<Result<bool>> validateConfirmation(CreateConfirmationDto dto)
        {
            if (dto == null)
            {
                return Result<bool>.Failure("Confirmation details are required", 404);
            }
            var student = await _Context.Students.FirstOrDefaultAsync(t => t.Id == dto.StudentId);
            if (student == null)
            {
                return Result<bool>.Failure("student does not exist.", 404);
            }
            var trip = await _Context.Trips.FirstOrDefaultAsync(t => t.Id == dto.TripId);
            if (trip == null)
            {
                return Result<bool>.Failure("trip does not exist.", 404);
            }
            if (trip.TripStatus == enStatus.Cancelled)
            {
                return Result<bool>.Failure(
                    "Cannot confirm a cancelled trip.",
                    400);
            }

            if (trip.TripStatus == enStatus.Departed ||
                trip.TripStatus == enStatus.completed)
            {
                return Result<bool>.Failure(
                    "Cannot confirm a trip that has already departed.",
                    400);
            }
            if (trip.TripStatus == enStatus.Cancelled)
            {
                return Result<bool>.Failure("Cant confirm cancelled trip.", 404);
            }
            if (!IsConfirmationOpen(trip))
            {
                return Result<bool>.Failure("you have to confirm within the Range", 404);
            }
            var alreadyConfirmed = await _Context.StudentConfirmations.AnyAsync(sc =>
        sc.StudentId == dto.StudentId &&sc.TripId == dto.TripId &&!sc.IsCancelled);

            if (alreadyConfirmed)
            {
                return Result<bool>.Failure(
                    "Student has already confirmed this trip.",
                    400);
            }
            return Result<bool>.Success(true);
        }
        private TimeSpan GetConfirmationDeadline(Trip trip)
        {
            if (trip.TripTime == new TimeSpan(7, 0, 0))
            {
                return trip.TripTime + TimeSpan.FromMinutes(5);
            }
            return trip.TripTime + TimeSpan.FromMinutes(10);
        }
        private bool IsConfirmationOpen(Trip trip)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            TimeSpan confirmationStart =
                trip.TripTime - TimeSpan.FromHours(12);

            TimeSpan confirmationDeadline =
                GetConfirmationDeadline(trip);

            return currentTime >= confirmationStart &&
                   currentTime <= confirmationDeadline;
        }
        public async Task<Result<bool>> CancelConfirmation(CancelConfirmationDto dto)
        {
            if (dto == null)
            {
                return Result<bool>.Failure(
                    "Confirmation details are required.",
                    400);
            }
            var confirmation = await _Context.StudentConfirmations
                .FirstOrDefaultAsync(sc =>
                    sc.StudentId == dto.StudentId &&
                    sc.TripId == dto.TripId &&
                    !sc.IsCancelled);

            if (confirmation == null)
            {
                return Result<bool>.Failure(
                    "Active confirmation does not exist.",
                    404);
            }
            confirmation.IsCancelled = true;
            await _Context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        public async Task<Result< IEnumerable<ConfirmDto>>> GetConfirmationsByTripId(int tripId)
        {
            if(tripId <= 0)
            {
                return Result<IEnumerable<ConfirmDto>>.Failure("invalid ID",400);
            }
            var trip= await _Context.Trips.Include(t=>t.StudentConfirmations).FirstOrDefaultAsync(t=>t.Id == tripId);
            if (trip == null)
            {
                return Result<IEnumerable<ConfirmDto>>.Failure("trip does not exist.", 400);
            }
            var confirmations=await _Context.StudentConfirmations.AsNoTracking().Where(t=>t.TripId== tripId && !t.IsCancelled).Select(p=>p.MapToConfirmDto()).ToListAsync();
            if (!confirmations .Any())
            {
                return Result<IEnumerable<ConfirmDto>>.Failure("No confirmations found!", 400);
            }
            return Result<IEnumerable<ConfirmDto>>.Success(confirmations);
        }
        public async Task<Result<bool>> CancelConfirmationById(int id)
        {
            if (id <= 0)
            {
                return Result<bool>.Failure("invalid id!",400);
            }
            var confirmation = await _Context.StudentConfirmations.FirstOrDefaultAsync(x => x.Id == id);

            if (confirmation == null)
            {
                return Result<bool>.Failure( "Confirmation doesn't exist!", 404);
            }

            if (confirmation.IsCancelled)
            {
                return Result<bool>.Failure("Confirmation is already cancelled!", 400);
            }
            confirmation.IsCancelled = true;
            await _Context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
