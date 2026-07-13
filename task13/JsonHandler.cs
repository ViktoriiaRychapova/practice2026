using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public static class JsonHandler
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new DateTimeConverter() } 
        };

        public static string SerializeStudent(Student student)
        {
            return JsonSerializer.Serialize(student, options);
        }

        public static Student DeserializeStudent(string json)
        {
            try
            {
                var student = JsonSerializer.Deserialize<Student>(json, options);
                if (string.IsNullOrEmpty(student.FirstName) || string.IsNullOrEmpty(student.LastName))
                {
                    throw new InvalidOperationException("Имя и фамилия должны быть указаны.");
                }
                return student;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                return null;
            }
        }

        public static void SaveToFile(string json, string filename)
        {
            File.WriteAllText(filename, json);
        }

        public static string LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                return File.ReadAllText(filename);
            }
            throw new FileNotFoundException($"Файл {filename} не найден");
        }
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (DateTime.TryParseExact(stringValue, format, null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            throw new JsonException($"Дата не в формате {format}: {stringValue}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(format));
        }
    }
}