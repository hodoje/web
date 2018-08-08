using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.AccessServices;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;

namespace Backend.AccessServices
{
    public class AccessService : IAccessService
    {
        private ICacheManager<string, LoginModel> _cacheManager;
        private HashGenerator _hashGenerator;
        private string key = "LoggedUsers";
        static object lockk = new object();

        public AccessService(ICacheManager<string, LoginModel> cacheManager, HashGenerator hashGenerator)
        {
            _cacheManager = cacheManager;
            _cacheManager.Set(key, new Dictionary<string, LoginModel>(), 24);
            _hashGenerator = hashGenerator;
        }

        public ApiMessage<string, LoginModel> Login(ApiMessage<string, LoginModel> user, IUnitOfWork unitOfWork)
        {
            ApiMessage<string, LoginModel> returnMessage = null;
            Dictionary<string, LoginModel> loggedUsers = (Dictionary<string, LoginModel>)_cacheManager.Get("LoggedUsers");

            // This lock handles multiple fast same parameter logins
            lock (lockk)
            {
                string hash = _hashGenerator.GenerateHash(user.Data);

                user.Data.Role = ((Role)unitOfWork.UserRepository.Find(u => u.Username == user.Data.Username).FirstOrDefault().Role).ToString();

                if (!loggedUsers.ContainsKey(hash))
                {
                    if (loggedUsers.Values.FirstOrDefault(u => u.Username == user.Data.Username) == null)
                    {
                        loggedUsers.Add(hash, user.Data);

                        returnMessage = new ApiMessage<string, LoginModel>();
                        returnMessage.Key = hash;
                        returnMessage.Data = user.Data;

                        _cacheManager.Set(key, loggedUsers, 24);
                        return returnMessage;
                    }
                }
            }
            return returnMessage;
        }

        public bool Logout(string hash)
        {
            bool result = false;
            Dictionary<string, LoginModel> loggedUsers = (Dictionary<string, LoginModel>)_cacheManager.Get("LoggedUsers");
            if (loggedUsers.ContainsKey(hash))
            {
                loggedUsers.Remove(hash);
                result = true;
            }
            _cacheManager.Set(key, loggedUsers, 24);
            return result;
        }

        public bool IsLoggedIn(string hash)
        {
            bool result = false;
            Dictionary<string, LoginModel> loggedUsers = _cacheManager.Get("LoggedUsers").ToDictionary(u => u.Key, u => u.Value);
            {
                if (loggedUsers.ContainsKey(hash))
                {
                    result = true;
                }
            }
            return result;
        }

        public ApiMessage<string, LoginModel> GetLoginData(string userHash, IUnitOfWork unitOfWork)
        {
            ApiMessage<string, LoginModel> loginData = null;
            Dictionary<string, LoginModel> loggedUsers =
                (Dictionary<string, LoginModel>)_cacheManager.Get("LoggedUsers");
            if (loggedUsers.ContainsKey(userHash))
            {
                loginData = new ApiMessage<string, LoginModel>
                {
                    Key = userHash,
                    Data = loggedUsers[userHash]
                };
            }
            return loginData;
        }
        public bool BlockUser(string username)
        {
            throw new NotImplementedException();
        }

        public bool UnblockUser(string username)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthorized(string hash, LoginModel user)
        {
            throw new NotImplementedException();
        }
    }
}