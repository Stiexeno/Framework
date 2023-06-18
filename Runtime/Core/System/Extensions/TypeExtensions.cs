using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;

namespace Framework
{
    public static class TypeExtensions
    {
        static readonly Dictionary<Type, Type[]> interfaces = new Dictionary<Type, Type[]>();

        private static readonly HashSet<Type> excludedInterfaces = new HashSet<Type>
        {
            typeof(IProcessable),
            typeof(ITickable),
            typeof(IDisposable),
            typeof(IFixedProcessable),
            typeof(IPreInstall)
        };
        
        public static Type[] Interfaces(this Type type)
        {
            if (interfaces.TryGetValue(type, out Type[] result) == false)
            {
                result = type.GetInterfaces().Except(excludedInterfaces).ToArray();
                interfaces.Add(type, result);
            }
            
            return result;
        }
        
        public static Type[] ModiferInterfaces(this Type type)
        {
            return type.GetInterfaces().Intersect(excludedInterfaces).ToArray();
        }
    }
}
