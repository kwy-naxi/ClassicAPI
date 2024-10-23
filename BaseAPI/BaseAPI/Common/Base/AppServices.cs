using BaseAPI.Common.DataTypeUtility;
using BaseAPI.Common.Web.Util;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BaseAPI
{
    public static class AppServices
    {
        static IServiceProvider services = null;

        /// <summary>
        /// Provides static access to the framework's services provider
        /// </summary>
        public static IServiceProvider Services
        {
            get { return services; }
            set
            {
                if (services != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                services = value;
            }
        }

        public static HttpContext HttpContextCurrent
        {
            get
            {
                IHttpContextAccessor httpContextAccessor = services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                return httpContextAccessor?.HttpContext;
            }
        }

        public static IWebHostEnvironment HostingEnvironment
        {
            get
            {
                return services.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;
            }
        }

        public static IMemoryCache Cache
        {
            get
            {
                return services.GetService(typeof(IMemoryCache)) as IMemoryCache;
            }
        }

        public static IWebHelper WebHelper
        {
            get
            {
                return services.GetService(typeof(IWebHelper)) as IWebHelper;
            }
        }

        public static IDataTypeUtility DataTypeUtility
        {
            get
            {
                return services.GetService(typeof(IDataTypeUtility)) as IDataTypeUtility;
            }
        }

        public static AppSettings Config
        {
            get
            {
                var s = services.GetService(typeof(IOptionsMonitor<AppSettings>)) as IOptionsMonitor<AppSettings>;
                AppSettings config = s.CurrentValue;
                return config;
            }
        }

        public static IDistributedCache CacheDistributed
        {
            get
            {
                return services.GetService(typeof(IDistributedCache)) as IDistributedCache;
            }
        }
    }
}
