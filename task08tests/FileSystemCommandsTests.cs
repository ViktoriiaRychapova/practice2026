using System;
using System.IO;
using Xunit;
using FileSystemCommands;

namespace task08tests
{
    public class FileSystemCommandsTests
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir_Size");
            if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello"); // 5 bytes
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");  // 5 bytes

            var command = new DirectorySizeCommand(testDir);
            
            var exception = Record.Exception(() => command.Execute());
            Assert.Null(exception);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir_Find");
            if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");

            var exception = Record.Exception(() => command.Execute());
            Assert.Null(exception);

            Directory.Delete(testDir, true);
        }
    }
}