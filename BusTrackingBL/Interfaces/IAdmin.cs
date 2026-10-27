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
    public interface IAdmin
    {
        public Task<Result<AdminDto>> CreateNewAdmin(CreateNewAdminDto dto);
        Task<Result<AdminDto>> GetAdminById(int id);
        Task<Result<IEnumerable<AdminDto>>> GetAllAdmins();
        public Task<Result<AdminDto>> UpdateAdmin(int id, updateAdminDto dto);
        public Task<Result<bool>> DeleteAdmin(int id);
        public Task<Result<LoginResponseDto>> Login(LoginDto dto);
        public Task<Result<IEnumerable<AdminDto>>> GetAllActiveAdmins();
        public Task<Result<bool>> ChangePassword(changePasswordDto dto);
        public Task<Result<bool>> ActivateAdmin(int id);
    }
}
