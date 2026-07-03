// Файл: DirectorySizeCommand.cs
using System;
using System.IO;
using System.Linq;
using CommandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _path;

        public DirectorySizeCommand(string path)
        {
            _path = path;
        }

        public void Execute()
        {
            if (!Directory.Exists(_path))
            {
                Console.WriteLine($"Ошибка: Директория {_path} не найдена.");
                return;
            }

            long size = Directory.GetFiles(_path, "*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f).Length)
                .Sum();

            Console.WriteLine($"Размер каталога {_path}: {size} байт");
        }
    }
}