using BusTrackingBL.DTOs.Requests;
using BusTrackingBL.DTOs.Responses;
using BusTrackingBL.Helper;
using BusTrackingBL.Interfaces;
using BusTrackingBL.Mapper;
using BusTrackingDL.Models.Data;
using BusTrackingDL.Models.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BusTrackingBL.Services
{
    public class AdminServices: IAdmin
    {
        private readonly AppDBContext _context;
        private readonly IPasswordHasher<Admin> _passwordHasher;
        public AdminServices(AppDBContext context, IPasswordHasher<Admin> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<Result<AdminDto>> CreateNewAdmin(CreateNewAdminDto dto)
        {
            if (dto == null)
            {
                return Result<AdminDto>.Failure("invalid Admin details!", 400);
            }
           var CheckAdmin=await _context.Admins.AnyAsync(d=>d.UserName==dto.UserName);
            if (CheckAdmin)
            {
                return Result<AdminDto>.Failure("Admin already exist!", 400);
            }
            var AdminEntity = dto.MapCreatetoEntity();
            AdminEntity.PasswordHash=_passwordHasher.HashPassword(AdminEntity,dto.Password);
            await _context.Admins.AddAsync(AdminEntity);
            await _context.SaveChangesAsync();
            return Result<AdminDto>.Success(AdminEntity.MapToAdminDtoo());
        }
        public async Task<Result<bool>> DeleteAdmin(int id)
        {
            if (id <= 0)
            {
                return Result<bool>.Failure("Invalid id", 400);
            }
            var AdminFound= await _context.Admins.FirstOrDefaultAsync(x => x.Id == id);
            if (AdminFound == null)
                return Result<bool>.Failure($"Admin with id= {id} is not found!", 404);
            if (!AdminFound.IsActive)
                return Result<bool>.Failure($"Admin with id= {id} is inactive!", 400);
            AdminFound.IsActive = false;
           await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }//deactivate
        public async Task<Result<AdminDto>> GetAdminById(int id)
        {
            if (id <= 0)
            {
                return Result<AdminDto>.Failure("Invalid id", 400);
            }
            var Admin=await _context.Admins.FirstOrDefaultAsync(x => x.Id == id);
            if (Admin == null)
            {
                return Result<AdminDto>.Failure("Cant find admin", 400);
            }
            return Result<AdminDto>.Success(Admin.MapToAdminDtoo());
        }
        public async Task<Result<IEnumerable<AdminDto>>> GetAllAdmins()
        {
            var Admins =await _context.Admins.AsNoTracking().Select(t=>t.MapToAdminDtoo()).ToListAsync();
           if (!Admins.Any())
            {
                return Result<IEnumerable<AdminDto>>.Failure("No Admin found",404);
            }

            return Result<IEnumerable<AdminDto>>.Success(Admins);
        }
        public async Task<Result<LoginResponseDto>> Login(LoginDto dto)
        {
            if (dto==null)
            {
                return Result<LoginResponseDto>.Failure("Invalid credinials", 400);
            }
            var admin=await _context.Admins.FirstOrDefaultAsync(d=>d.UserName == dto.UserName);
            if (admin == null)
            {
               return Result<LoginResponseDto>.Failure("Invalid credentials", 401);
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, dto.Password);
            if (passwordResult == PasswordVerificationResult.Failed)
            {
               return Result<LoginResponseDto>.Failure("Invalid credentials", 401);
            }
            if (!admin.IsActive)
            {
                return Result<LoginResponseDto>.Failure(
                    "Admin account is inactive", 403);
            }
            return Result<LoginResponseDto>.Success(new LoginResponseDto{UserName=admin.UserName});
        }
        public async Task<Result<AdminDto>> UpdateAdmin(int id, updateAdminDto dto)//change username
        {
            var AdminFound=await _context.Admins.FirstOrDefaultAsync(s=>s.Id == id);
            if (AdminFound==null)
            {
                return Result<AdminDto>.Failure( "Can't find admin", 404);
            }
            AdminFound.UserName = dto.UserName;
           
            await _context.SaveChangesAsync();
            return Result<AdminDto>.Success(AdminFound.MapToAdminDtoo());
        }
        public async Task<Result<IEnumerable<AdminDto>>> GetAllActiveAdmins()
        {
            var Admins = await _context.Admins.Where(t=>t.IsActive).AsNoTracking().Select(t => t.MapToAdminDtoo()).ToListAsync();
            if (!Admins.Any())
            {
                return Result<IEnumerable<AdminDto>>.Failure("No Admin found", 404);
            }

            return Result<IEnumerable<AdminDto>>.Success(Admins);
        }
        public async Task<Result<bool>>ChangePassword(changePasswordDto dto)
        {
            if(dto == null)
            {
                return Result<bool>.Failure("invalid input!", 400);
            }
            if (dto.newPassword == dto.oldPassword)
            {
                return Result<bool>.Failure("New password must be different from old password", 400);
            }
            var admin = await _context.Admins.FirstOrDefaultAsync(t => t.UserName == dto.userName);
            if (admin == null)
            {
                return Result<bool>.Failure("Invalid username or password", 400);
            }
            var results= _passwordHasher.VerifyHashedPassword(admin,admin.PasswordHash,dto.oldPassword);
            if (results == PasswordVerificationResult.Failed)
            {
                return Result<bool>.Failure("Invalid username or password", 400);
            }
            admin.PasswordHash = _passwordHasher.HashPassword(admin,dto.newPassword);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> ActivateAdmin(int id)
        {
            var adminFound=await GetAdminById(id);
            if(adminFound == null)
            {
                return Result<bool>.Failure(adminFound.ErrorMessage,adminFound.ErrorCode);
            }
            var admin=await _context.Admins.FirstOrDefaultAsync(d=>d.Id==id);
            if (admin == null)
            {
                return Result<bool>.Failure($"Admin with id= {id} is not found!", 404);
            }
            if (admin.IsActive)
            {
                return Result<bool>.Failure($"Admin with id= {id} is already active!", 404);
            }
            admin.IsActive = true;
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }    
    }
}
