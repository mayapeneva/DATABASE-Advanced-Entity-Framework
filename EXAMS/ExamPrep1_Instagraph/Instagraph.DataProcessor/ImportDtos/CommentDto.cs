namespace Instagraph.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlType("comment")]
    public class CommentDto
    {
        [XmlElement("content")]
        [MaxLength(250)]
        [Required]
        public string Content { get; set; }

        public int UserId { get; set; }

        [XmlElement("user")]
        [Required]
        public string User { get; set; }

        [XmlElement("post")]
        [Required]
        public PostElementDto PostElement { get; set; }
    }
}