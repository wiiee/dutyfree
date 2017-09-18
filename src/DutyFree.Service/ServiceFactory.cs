namespace DutyFree.Service
{
    using Context;
    using Microsoft.Extensions.Logging;
    using Platform.Context;
    using Platform.Util;
    using System;

    //Resolve Service之间互相引用导致死循环的问题
    public class ServiceFactory
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<ServiceFactory>();

        private ServiceFactory()
        {
        }

        private static readonly Lazy<ServiceFactory> lazy = new Lazy<ServiceFactory>(() => new ServiceFactory());

        public static ServiceFactory Instance { get { return lazy.Value; } }

        public T GetService<T>()
            where T : IService
        {
            try
            {
                Type type = typeof(T);
                return (T)Activator.CreateInstance(type, ServiceContextRepository.Instance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(T);
            }
        }
    }
}
