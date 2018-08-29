namespace P01_StudentSystem.Data.Models
{
    using Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Resource
    {
        public int ResourceId { get; set; }

        [Column("Name", TypeName = "NVARCHAR(50)")]
        [Required]
        public string Name { get; set; }

        [Column("Url", TypeName = "VARCHAR(MAX)")]
        [Required]
        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}