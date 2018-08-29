namespace Instagraph.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlType("post")]
    public class PostDto
    {
        [XmlElement("caption")]
        [Required]
        public string Caption { get; set; }

        public int UserId { get; set; }

        [XmlElement("user")]
        [Required]
        public string User { get; set; }

        public int PictureId { get; set; }

        [XmlElement("picture")]
        [Required]
        public string Picture { get; set; }
    }
}