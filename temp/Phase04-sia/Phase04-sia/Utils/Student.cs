namespace Phase04_sia
{
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