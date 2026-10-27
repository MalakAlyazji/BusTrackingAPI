using BusTrackingBL.DTOs;
using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingBL.Mapper;
using BusTrackingDL.Models.Data;
using BusTrackingDL.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BusTrackingBL.Services
{
    public class DriverService : IDriver
    {
        private readonly PasswordHasher<Driver> _passwordHasher;
        private readonly AppDBContext _context;
        public DriverService(AppDBContext context)
        {
            _context=context;
            _passwordHasher = new PasswordHasher<Driver>();
        }
        public async Task<Result<DriverDto>> AddDriver(AddDriverDto dto)
        {
            var checkDto=await ValidateAddDto(dto);
            if (!checkDto.IsSuccess)
            {
                return Result<DriverDto>.Failure(checkDto.ErrorMessage, checkDto.ErrorCode);
            }
            var NewDriver = dto.MapAddDtoToEntity();
            NewDriver.PasswordHash = _passwordHasher.HashPassword(NewDriver,
                dto.password);
            _context.Drivers.AddAsync(NewDriver);
            await _context.SaveChangesAsync();
            return Result<DriverDto>.Success(NewDriver.MapToDriverDto());
        }
        private async Task<Result<bool>> ValidateAddDto(AddDriverDto dto)
        {
            if (dto == null)
            {
                return Result<bool>.Failure("Invlaid Driver details!",400);
            }
            var driverExist =await _context.Drivers.AnyAsync(t => t.Phone == dto.Phone);
            if (driverExist)
            {
                return Result<bool>.Failure("A driver with this phone number already exists!", 400);
            }
            if (!string.IsNullOrEmpty(dto.BusNumber)) {
                var busAssigned=await _context.Drivers.AnyAsync(t=>t.BusNumber == dto.BusNumber);
                if (busAssigned)
                {
                    return Result<bool>.Failure("bus is assigned to different driver!", 400);
                }
            }
            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> DeactivateDriver(int driverId)
        {
            if (driverId <= 0)
            {
                return Result<bool>.Failure("Invalid Driver", 400);
            }

            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
            {
                return Result<bool>.Failure("Driver doesn't exist!", 404);
            }

            if (!driver.IsActive)
            {
                return Result<bool>.Failure("Driver is already inactive!", 400);
            }
            driver.IsActive = false;
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        public async Task<Result<DriverDto>> GetDriverById(int driverId)
        {
            if (driverId <= 0)
            {
                return Result<DriverDto>.Failure("Invalid Driver", 400);
            }

            var ExistDriver = await _context.Drivers.FirstOrDefaultAsync(t => t.Id == driverId);

            if (ExistDriver == null)
            {
                return Result<DriverDto>.Failure("Driver doesn't exist!", 404);
            }
            if (!ExistDriver.IsActive)
            {
                return Result<DriverDto>.Failure("Driver is inactive!", 404);
            }
            return Result<DriverDto>.Success(
                ExistDriver.MapToDriverDto());
        }
        public async Task<Result<IEnumerable<DriverDto>>> GetActiveDrivers()
        {
            var drivers=await _context.Drivers.AsNoTracking().
                Where(t=>t.IsActive).OrderBy(t=>t.Id).Select(t=>t.MapToDriverDto()).ToListAsync();
            return Result<IEnumerable<DriverDto>>.Success(drivers);
        }
        public async Task<Result<DriverDto>> UpdateDriver(int driverId, UpdateDriverDto dto)
        {
            if (dto == null)
            {
                return Result<DriverDto>.Failure("Invlaid Driver details!", 400);
            }
            var driverExist = await _context.Drivers.FirstOrDefaultAsync(t => t.Id == driverId);
            if (driverExist==null)
            {
                return Result<DriverDto>.Failure($"A driver with  ID {driverId} does not exist!", 400);
            }
            if (!driverExist.IsActive)
            {
                return Result<DriverDto>.Failure($"A driver with  ID {driverId} is inactive!", 400);
            }
            dto.MapUpdateDtoToEntity(driverExist);
            await _context.SaveChangesAsync();
            return Result<DriverDto>.Success(driverExist.MapToDriverDto());
        }
        public async Task<Result<bool>> ActivateDriver(int driverId)
        {
            if (driverId <= 0)
            {
                return Result<bool>.Failure("Invalid Driver", 400);
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
            {
                return Result<bool>.Failure(
                    "Driver doesn't exist!", 404);
            }

            if (driver.IsActive)
            {
                return Result<bool>.Failure(
                    "The driver is already active!", 400);
            }

            driver.IsActive = true;

            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> ChangePassword(changePasswordDto dto)
        {
            if (dto == null)
            {
                return Result<bool>.Failure("invalid input!", 400);
            }
            if (dto.newPassword == dto.oldPassword)
            {
                return Result<bool>.Failure("New password must be different from old password", 400);
            }
            var driverFound = await _context.Drivers.FirstOrDefaultAsync(t => t.DriverName == dto.userName);
            if (driverFound == null)
            {
                return Result<bool>.Failure("Invalid username or password", 400);
            }
            
            var results = _passwordHasher.VerifyHashedPassword(driverFound, driverFound.PasswordHash, dto.oldPassword);
            if (results == PasswordVerificationResult.Failed)
            {
                return Result<bool>.Failure("Invalid username or password", 400);
            }
            driverFound.PasswordHash = _passwordHasher.HashPassword(driverFound, dto.newPassword);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        public async Task<Result<LoginResponseDto>> Login(LoginDto dto)
        {
            if (dto == null)
            {
                return Result<LoginResponseDto>.Failure("Invalid credinials", 400);
            }
            var driverFound = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverName == dto.UserName);
            if (driverFound == null)
            {
                return Result<LoginResponseDto>.Failure("Invalid username or password", 401);
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(driverFound, driverFound.PasswordHash, dto.Password);
            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Result<LoginResponseDto>.Failure("Invalid credentials", 401);
            }
            if (!driverFound.IsActive)
            {
                return Result<LoginResponseDto>.Failure(
                    "driver account is inactive", 403);
            }
            return Result<LoginResponseDto>.Success(new LoginResponseDto { UserName = driverFound.DriverName });
        }


    }
}
