namespace HospitalDatabase
{
    using System.ComponentModel.DataAnnotations;
    using P01_HospitalDatabase.Data;
    using P01_HospitalDatabase.Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new HospitalContext())
            {
                DatabaseInitializer.InitialSeed(context);
            }
        }
    }
}