using System;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Phase04_sia
{
    public class Executor
    {
        private static readonly string FilePath = "../../../resources/";
        private static Executor _executor = new Executor();

        public static Executor Executorr
        {
            get => _executor;
        }
        
        public IReader<string, string> Reader;

        public IWriter<string> Writer;
        
        private Executor()
        {
            Reader = new FileReader();
            Writer = new DefaultConsoleWriter();
        }


        public void Execute()
        {
            string studentJson = Reader.Read(FilePath + "Students.json");
            string gradesJson = Reader.Read(FilePath + "Scores.json");
            var students = JsonSerializer.Deserialize<Student[]>(studentJson);
            var scores = JsonSerializer.Deserialize<LessonsScore[]>(gradesJson);
            PrintResults(students, scores);
        }

        private void PrintResults(Student[] students, LessonsScore[] scores)
        {
            var results = students.OrderByDescending(s => GetAverage(scores, s))
                .Select(student => new {Name = student.ToString(), Average = GetAverage(scores, student)}).Take(3).ToList();

            foreach (var result in results)
            {
                Writer.Write($"{result.Name} {result.Average}");
            }
        }

        private double GetAverage(LessonsScore[] scores, Student s)
        {
            return scores.Where(score => score.StudentNumber == s.StudentNumber).Average(score => score.Score);
        }
    }

    class FileReader : IReader<string, string>
    {
        public string Read(string filePath)
        {
            return File.ReadAllText(path: filePath);
        }
    }

    class DefaultConsoleWriter : IWriter<string>
    {
        public void Write(string data)
        {
            Console.WriteLine(data);
        }
    }

    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StudentNumber { get; set; }

        public override string ToString()
        {
            return FirstName + "  " + LastName;
        }
    }

    class LessonsScore
    {
        public int StudentNumber { get; set; }
        public string Lesson { get; set; }
        public double Score { get; set; }
    }
}