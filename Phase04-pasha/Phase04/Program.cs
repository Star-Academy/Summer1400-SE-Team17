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
        }

        private static void DisplayTopThreeStudents()
        {
            List<Student> orderedList = Student.AllStudents.OrderBy(x => (-x.GetAverageScore())).ToList();
            for (int i = 0; i < 3; ++i)
                DisplayStudent(orderedList[i]);
        }

        private static void InitStudentsAndCourses()
        {
            string studentData = FileReader.GetWholeFileAsString(StudentsPath);
            Student.AllStudents = JsonManager.Deserialize<List<Student>>(studentData);
            string coursesData = FileReader.GetWholeFileAsString(ScoresPath);
            Course.AllStudentsCourses = JsonManager.Deserialize<List<Course>>(coursesData);
            foreach (var student in Student.AllStudents)
            {
                Console.WriteLine(student.StudentNumber);
            }
            foreach (var course in Course.AllStudentsCourses)
            {
                Console.WriteLine(course.StudentNumber);
            }
            // foreach (var course in Course.AllStudentsCourses)
            // {
            //     Console.WriteLine(course.StudentNumber);
            //     Student.AllStudents.Find(x => x.StudentNumber == course.StudentNumber).Courses.Add(course);
            // }
        }
        private static void DisplayStudent(Student student)
        {
            System.Console.WriteLine($"student name : {student.FirstName} {student.LastName} average score : {student.GetAverageScore()}");
        }
    }
}