using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Phase04
{
    public class Course
    {
        public static List<Course> AllStudentsCourses;
        public int StudentNumber;
        public string Lesson;
        public double Score;
    }
}