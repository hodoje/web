﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Backend.LoginRepository
{
    public class CacheManager<T> : ICacheManager<T> where T : class
    {
        public ObjectCache CachedData => MemoryCache.Default;

        public IEnumerable<T> Get(string key)
        {
            IEnumerable<T> list = new List<T>();
            if (!String.IsNullOrEmpty(key))
            {
                list = (IEnumerable<T>)CachedData[key];
            }
            return list;
        }

        public bool IsSet(string key)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(key))
            {
                result = CachedData.Contains(key);
            }
            return result;
        }

        public void Remove(string key)
        {
            CachedData.Remove(key);
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (data != null && !String.IsNullOrEmpty(key))
            {
                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now + TimeSpan.FromHours(cacheTime)
                };

                CachedData.Set(new CacheItem(key, data), policy);
            }
        }
    }
}