namespace P01_StudentSystem
{
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new StudentSystemContext())
            {
                //var course1 = new Course() { Name = "C# Fundamental", StartDate = new DateTime(18 / 09 / 2017), EndDate = new DateTime(17 / 01 / 2018), Price = 320, Resources = new List<Resource>(new Resource() { Name = }) };
                //var student = new Student()
                //{
                //    Name = "Gosho",
                //    Birthday = new DateTime(04 / 12 / 1972),
                //    PhoneNumber = "0888225555",
                //    RegisteredOn = new DateTime(11 / 09 / 2017),
                //};
                //context.Students.Add()
            }
        }
    }
}