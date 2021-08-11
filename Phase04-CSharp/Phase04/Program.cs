using System;
using System.Linq;
using System.Collections.Generic;

namespace Phase04
{
    class Program
    {
        private const string StudentsPath = "../../../resources/Students.json";
        private const string ScoresPath = "../../../resources/Scores.json";

        static void Main(string[] args)
        {
            InitStudentsAndCourses();
            DisplayTopThreeStudents();
            // test();
        }

        static void test()
        {
            List<int> xx = new List<int>();
            xx.Add(10);
            Console.WriteLine(xx.Aggregate((a, b) => a + b));
        }


        private static void DisplayTopThreeStudents()
        {
            List<Student> orderedList = Database.AllStudents.OrderBy(x => (-x.GetAverageScore())).ToList();
            for (int i = 0; i < Math.Min(orderedList.Count, 3); ++i)
                DisplayStudent(orderedList[i]);
        }

        private static void InitStudentsAndCourses()
        {
            LoadCoursesData();
            LoadStudentsData();
            foreach (var course in Course.AllStudentsCourses)
            {
                Database.AllStudents.Find(x => x.StudentNumber == course.StudentNumber).Courses.Add(course);
            }
        }

        private static void LoadStudentsData()
        {
            string studentData = FileReader.GetWholeFileAsString(StudentsPath);
            JsonManager<List<Student>> jsonManager = new JsonManager<List<Student>>();
            Database.AllStudents = jsonManager.Deserialize(studentData);
        }

        private static void LoadCoursesData()
        {
            string coursesData = FileReader.GetWholeFileAsString(ScoresPath);
            JsonManager<List<Course>> jsonManager = new JsonManager<List<Course>>();
            Course.AllStudentsCourses = jsonManager.Deserialize(coursesData);
        }

        private static void DisplayStudent(Student student)
        {
            Console.WriteLine(
                $"student name : {student.FirstName} {student.LastName} average score : {student.GetAverageScore()}");
        }
    }
}