// Файл: Program.cs
using System;
using System.Reflection;
using System.Linq;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");

            if (!File.Exists(dllPath))
            	Console.WriteLine("DLL плагина не найдена. Сначала соберите проект FileSystemCommands.");
                return;

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);

                var commandTypes = assembly.GetTypes()
                    .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in commandTypes)
                {
                    Console.WriteLine($"\nОбнаружена команда: {type.Name}");

                    
                    if (type.Name == "DirectorySizeCommand")
                    {
                        var instance = Activator.CreateInstance(type, Path.GetTempPath()) as ICommand;
                        instance?.Execute();
                    }
                    else if (type.Name == "FindFilesCommand")
                    {
                        var instance = Activator.CreateInstance(type, Path.GetTempPath(), "*.txt") as ICommand;
                        instance?.Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
            }
        }
    }
}