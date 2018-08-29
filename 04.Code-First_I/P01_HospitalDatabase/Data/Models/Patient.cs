namespace P01_HospitalDatabase.Data.Models

{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Patient
    {
        public Patient()
        {
            this.Prescriptions = new HashSet<PatientMedicament>();
            this.Visitations = new HashSet<Visitation>();
            this.Diagnoses = new HashSet<Diagnose>();
        }

        public int PatientId { get; set; }

        [Column("FirstName", TypeName = "NVARCHAR(50)")]
        public string FirstName { get; set; }

        [Column("LastName", TypeName = "NVARCHAR(50)")]
        public string LastName { get; set; }

        [Column("Address", TypeName = "NVARCHAR(250)")]
        public string Address { get; set; }

        [Column("Email", TypeName = "VARCHAR(80)")]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }
    }
}