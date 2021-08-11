using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;

namespace Phase04
{
    public class Student
    {
        
        [JsonPropertyName("StudentNumber")] public int StudentNumber;
        [JsonPropertyName("FirstName")] public string FirstName;
        [JsonPropertyName("LastName")] public string LastName;
        public List<Course> Courses = new List<Course>();

        public Student()
        {
        }

        public double GetAverageScore()
        {
            if (Courses.Count == 0)
            {
                return 0;
            }
            return Courses.Average(x => x.Score);
        }
    }
}