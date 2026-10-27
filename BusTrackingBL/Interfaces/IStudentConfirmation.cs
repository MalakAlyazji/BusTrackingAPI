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
    public interface IStudentConfirmation
    {
        public Task<Result<ConfirmDto>> CreateConfirmStudent(CreateConfirmationDto dto);
        public Task<Result<bool>> CancelConfirmation(CancelConfirmationDto dto);
        public Task<Result<bool>> CancelConfirmationById(int id);
        public Task<Result<IEnumerable<ConfirmDto>>> GetConfirmationsByTripId(int tripId);
    }
}
