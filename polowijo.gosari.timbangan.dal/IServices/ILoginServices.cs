
using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polowijo.gosari.timbangan.dal.IServices
{
    public interface ILoginServices 
    {
        List<LoginDto> GetAll();
        LoginDto GetById(object id);
        LoginDto Save(LoginDto Login);
        LoginDto VerificationLogin(LoginDto Login);
        void ChangePassword(string UserId, string PasswordBaru);
        void SetLastOnline(string UserId);
        DateTime? GetLastOnline(string UserId);
    }
}
