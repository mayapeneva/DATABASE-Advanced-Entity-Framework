namespace P01_HospitalDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Doctor
    {
        public Doctor()
        {
            this.Visitations = new HashSet<Visitation>();
        }

        public int DoctorId { get; set; }

        [Column("Name", TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }

        [Column("Speciality", TypeName = "NVARCHAR(100)")]
        public string Speciality { get; set; }

        public ICollection<Visitation> Visitations { get; set; }
    }
}