using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingDL.Models.Configration;
using BusTrackingDL.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.Interfaces
{
    public interface IDriver
    {
        public Task<Result<DriverDto>> GetDriverById(int driverId);

        Task<Result<IEnumerable<DriverDto>>> GetActiveDrivers();

        Task<Result<DriverDto>> AddDriver(AddDriverDto dto);

        Task<Result<DriverDto>> UpdateDriver(int driverId, UpdateDriverDto dto);

        public Task<Result<bool>> DeactivateDriver(int driverId);

        Task<Result<bool>> ActivateDriver(int driverId);
        public Task<Result<LoginResponseDto>> Login(LoginDto dto);
        public Task<Result<bool>> ChangePassword(changePasswordDto dto);
    }
}
