using System;
using task13;

class Program
{
    static void Main()
    {
        var student = new Student
        {
            FirstName = "Ирина",
            LastName = "Кузнецова",
            BirthDate = new DateTime(2001, 3, 12),
            Grades = new List<Subject>
            {
                new Subject { Name = "Математика", Grade = 5 },
                new Subject { Name = "История", Grade = 4 }
            }
        };

        string json = JsonHandler.SerializeStudent(student);
        Console.WriteLine("Сериализация в JSON:");
        Console.WriteLine(json);
        Console.WriteLine();

        string filename = "student.json";
        JsonHandler.SaveToFile(json, filename);
        Console.WriteLine($"Данные успешно сохранены в файл: {filename}");
        Console.WriteLine();

        string jsonFromFile = JsonHandler.LoadFromFile(filename);
        Console.WriteLine("Загруженный JSON из файла:");
        Console.WriteLine(jsonFromFile);
        Console.WriteLine();

        var studentFromJson = JsonHandler.DeserializeStudent(jsonFromFile);
        Console.WriteLine("Десериализация обратно в объект:");
        Console.WriteLine($"Имя: {studentFromJson.FirstName}");
        Console.WriteLine($"Фамилия: {studentFromJson.LastName}");
        Console.WriteLine($"Дата рождения: {studentFromJson.BirthDate.ToString("yyyy-MM-dd")}");
        Console.WriteLine("Оценки:");
        foreach (var subj in studentFromJson.Grades)
        {
            Console.WriteLine($"  {subj.Name}: {subj.Grade}");
        }
    }
}