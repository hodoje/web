using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.LoginRepository
{
    public interface ILoginRepository
    {
        bool Login(LoginModel user);
        bool Logout(LoginModel user);
    }
}
