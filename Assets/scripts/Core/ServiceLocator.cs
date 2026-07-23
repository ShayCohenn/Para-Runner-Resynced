using System;
using System.Collections.Generic;

namespace ParaRunner.Core
{
    //  A simple, zero-allocation service locator to avoid Singleton clutter.
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T service)
        {
            Type type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                _services.Add(type, service);
            }
            else
            {
                _services[type] = service;
            }
        }

        public static T Get<T>()
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"[ServiceLocator] Service {type.Name} is not registered.");
        }

        public static bool TryGet<T>(out T service)
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object rawService))
            {
                service = (T)rawService;
                return true;
            }
            service = default;
            return false;
        }

        public static void Reset() => _services.Clear();
    }
}