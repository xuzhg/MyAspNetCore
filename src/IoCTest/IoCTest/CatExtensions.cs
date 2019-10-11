using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoCTest
{
    public static class CatExtensions
    {
        public static Cat Register(this Cat cat, Type from, Type to, Lifetime lifetime)
        {
            Func<Cat, Type[], object> factory = (_, arguments) => Create(_, to, arguments);
            cat.Register(new ServiceRegistry(from, lifetime, factory));
            return cat;
        }

        public static Cat Register<TFrom, TTo>(this Cat cat, Lifetime lifetime) where TTo : TFrom
            => cat.Register(typeof(TFrom), typeof(TTo), lifetime);

        public static Cat Register<TServiceType>(this Cat cat, TServiceType instance)
        {
            Func<Cat, Type[], object> factory = (_, arguments) => instance;
            cat.Register(new ServiceRegistry(typeof(TServiceType), Lifetime.Root, factory));
            return cat;
        }

        public static Cat Register<TServiceType>(this Cat cat, Func<Cat, TServiceType> factory, Lifetime lifetime)
        {
            cat.Register(new ServiceRegistry(typeof(TServiceType), lifetime, (_, arguments) => factory(_)));
            return cat;
        }

        public static bool HasRegistry<T>(this Cat cat) => cat.HasRegistry(typeof(T));

        public static bool HasRegistry(this Cat cat, Type serviceType) => cat._root._registries.ContainsKey(serviceType);

        private static object Create(Cat cat, Type type, Type[] genericArguments)
        {
            if (genericArguments.Length > 0)
            {
                type = type.MakeGenericType(genericArguments);
            }

            //var constrcotrs = type.GetConstructors(System.Reflection.BindingFlags.Instance);
            var constrcotrs = type.GetConstructors();
            if (constrcotrs.Length == 0)
            {
                throw new InvalidOperationException($"Cannot create the instance of {type} which does not have an public constructor.");
            }

            var constrcotor = constrcotrs.FirstOrDefault(it => it.GetCustomAttributes(false).OfType<InjectionAttribute>().Any());
            constrcotor = constrcotor ?? constrcotrs.First();
            var parameters = constrcotor.GetParameters();
            if (parameters.Length == 0)
            {
                return Activator.CreateInstance(type);
            }

            var arguments = new object[parameters.Length];
            for (int index = 0; index < arguments.Length; index++)
            {
                var parameter = parameters[index];
                var parameterType = parameter.ParameterType;
                if (cat.HasRegistry(parameterType))
                {
                    arguments[index] = cat.GetService(parameterType);
                }
                else if (parameter.HasDefaultValue)
                {
                    arguments[index] = parameter.DefaultValue;
                }
                else
                {
                    throw new InvalidOperationException($"Cannot create the instance of {type} whose constructor has non-registered parameter type(s)");
                }
            }

            return Activator.CreateInstance(type, arguments);
        }

        public static IEnumerable<T> GetServices<T>(this Cat cat) => cat.GetService<IEnumerable<T>>();
        

        public static T GetService<T>(this Cat cat) => (T)cat.GetService(typeof(T));
    }
}
