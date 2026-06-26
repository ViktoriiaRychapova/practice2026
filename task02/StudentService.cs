using System;
using System.Collections.Generic;
using System.Linq;

namespace task02;

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students)
    {
        _students = students ?? new List<Student>();
    }

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        => _students.Where(s => s.Faculty == faculty);

    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
        => _students.Where(s => s.Grades != null
                             && s.Grades.Any()
                             && s.Grades.Average() >= minAverageGrade);

    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(s => s.Name);

    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(s => s.Faculty);

    public string GetFacultyWithHighestAverageGrade()
    {
        return _students
            .Where(s => !string.IsNullOrWhiteSpace(s.Faculty))
            .GroupBy(s => s.Faculty)
            .Select(g => new
            {
                Faculty = g.Key,
                Average = g.SelectMany(s => s.Grades ?? Enumerable.Empty<int>())
                           .DefaultIfEmpty()
                           .Average()
            })
            .OrderByDescending(x => x.Average)
            .Select(x => x.Faculty)
            .FirstOrDefault() ?? string.Empty;
    }
}

