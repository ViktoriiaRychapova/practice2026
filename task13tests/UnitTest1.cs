using Xunit;
using task13;
using System;
using System.IO;

namespace task13tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeAndDeserializeStudent_ReturnsEquivalentObject()
        {
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Иванов",
                BirthDate = new DateTime(2000, 1, 15),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 5 },
                    new Subject { Name = "Physics", Grade = 4 }
                }
            };

            string json = JsonHandler.SerializeStudent(student);
            Assert.True(!string.IsNullOrEmpty(json));

            string filename = "student_test.json";

            JsonHandler.SaveToFile(json, filename);
            Assert.True(File.Exists(filename));

            string loadedJson = JsonHandler.LoadFromFile(filename);
            Assert.Equal(json, loadedJson);

            var deserializedStudent = JsonHandler.DeserializeStudent(loadedJson);
            Assert.NotNull(deserializedStudent);
            Assert.Equal(student.FirstName, deserializedStudent.FirstName);
            Assert.Equal(student.LastName, deserializedStudent.LastName);
            Assert.Equal(student.BirthDate, deserializedStudent.BirthDate);
            Assert.Equal(student.Grades.Count, deserializedStudent.Grades.Count);

            File.Delete(filename);
        }
    }
}