using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;

namespace Backend.LoginRepository
{
    public class LoginRepository : ILoginRepository
    {
        private ICacheManager<LoginModel> _cacheManager;
        private string key = "LoggedUsers";

        public LoginRepository(ICacheManager<LoginModel> cacheManager)
        {
            _cacheManager = cacheManager;
            _cacheManager.Set(key, new List<LoginModel>(), 24);
        }

        public bool Login(LoginModel user)
        {
            return LogAUser(user);
        }

        public bool Logout(LoginModel user)
        {
            return LogoutAUser(user);
        }

        public bool IsLoggedIn(LoginModel user)
        {
            bool result = false;
            Dictionary<string, LoginModel> loggedUsers = _cacheManager.Get("LoggedUsers").ToDictionary(u => u.Username, u => u);
            {
                LoginModel loggedUser;
                if ((loggedUser = loggedUsers.SingleOrDefault(u => u.Key == user.Username).Value) != null)
                {
                    if (loggedUser.Username == user.Username && loggedUser.Password == user.Password)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool LogoutAUser(LoginModel user)
        {
            bool result = false;
            Dictionary<string, LoginModel> loggedUsers = _cacheManager.Get("LoggedUsers").ToDictionary(u => u.Username, u => u);           
            if (loggedUsers.ContainsKey(user.Username))
            {
                loggedUsers.Remove(user.Username);
                result = true;
            }
            _cacheManager.Set(key, loggedUsers.Values, 24);
            return result;
        }

        private bool LogAUser(LoginModel user)
        {
            Dictionary<string, LoginModel> loggedUsers = _cacheManager.Get("LoggedUsers").ToDictionary(u => u.Username, u => u);
            if (!loggedUsers.ContainsKey(user.Username))
            {
                loggedUsers.Add(user.Username, user);
            }
            _cacheManager.Set(key, loggedUsers.Values, 24);
            return true;
        }
    }
}