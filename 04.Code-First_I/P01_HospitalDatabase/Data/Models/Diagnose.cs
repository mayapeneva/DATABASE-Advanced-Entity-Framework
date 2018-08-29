namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Diagnose
    {
        public int DiagnoseId { get; set; }

        [Column("Name", TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        [Column("Comments", TypeName = "NVARCHAR(250)")]
        public string Comments { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}