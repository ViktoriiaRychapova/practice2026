using System;
using System.Reflection;
using System.Linq;

namespace task07
{
    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            if (type == null) return;

            Console.WriteLine($"--- Анализ типа: {type.Name} ---");

            var classDisplayName = type.GetCustomAttribute<DisplayNameAttribute>();
            if (classDisplayName != null)
            {
                Console.WriteLine($"Отображаемое имя класса: {classDisplayName.DisplayName}");
            }

            var versionAttr = type.GetCustomAttribute<VersionAttribute>();
            if (versionAttr != null)
            {
                Console.WriteLine($"Версия класса: {versionAttr.Major}.{versionAttr.Minor}");
            }

            Console.WriteLine("\nСвойства:");
            foreach (var prop in type.GetProperties())
            {
                var propDisplayName = prop.GetCustomAttribute<DisplayNameAttribute>();
                string name = propDisplayName?.DisplayName ?? prop.Name;
                Console.WriteLine($"- {name} ({prop.PropertyType.Name})");
            }

            Console.WriteLine("\nМетоды:");
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var methodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
                string name = methodDisplayName?.DisplayName ?? method.Name;
                Console.WriteLine($"- {name}");
            }
            
            Console.WriteLine(new string('-', 30));
        }
    }
}