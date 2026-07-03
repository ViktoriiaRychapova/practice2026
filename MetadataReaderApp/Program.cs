using System;
using System.Reflection;
using System.Linq;

namespace MetadataReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Ошибка: Не указан путь к библиотеке (.dll).");
                Console.WriteLine("Использование: dotnet run -- <путь_к_dll>");
                return;
            }

            string assemblyPath = args[0];

            if (!System.IO.File.Exists(assemblyPath))
            {
                Console.WriteLine($"Ошибка: Файл по пути '{assemblyPath}' не найден.");
                return;
            }

            try
            {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                
                Console.WriteLine(new string('=', 50));
                Console.WriteLine($"АНАЛИЗ СБОРКИ: {assembly.FullName}");
                Console.WriteLine(new string('=', 50));

                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                
                {
                    Console.WriteLine($"\n[Class: {type.Name}]");

                    var attributes = type.GetCustomAttributes(false);
                    if (attributes.Length > 0)
                    {
                        Console.WriteLine("  Attributes:");
                        foreach (var attr in attributes)
                        {
                            Console.WriteLine($"    - {attr.GetType().Name}");
                        }
                    }

                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    if (methods.Length > 0)
                    {
                        Console.WriteLine("  Methods:");
                        foreach (var method in methods)
                        {
                            if (method.DeclaringType == typeof(object)) continue;

                            var parameters = method.GetParameters();
                            string paramString = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                            
                            Console.WriteLine($"    - {method.Name}({paramString})");
                        }
                    }
                    var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    if (constructors.Length > 0)
                    {
                        Console.WriteLine("  Constructors:");
                        foreach (var ctor in constructors)
                        {
                            var ctorParams = ctor.GetParameters();
                            string ctorParamString = string.Join(", ", ctorParams.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                            
                            Console.WriteLine($"    - {ctor.Name}({ctorParamString})");
                        }
                    }
                }

                Console.WriteLine("\n" + new string('-', 50));
                Console.WriteLine("Анализ завершен успешно.");
            }
            catch (BadImageFormatException)
            {
                Console.WriteLine("Ошибка: Указанный файл не является корректной .NET сборкой.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }
        }
    }
}