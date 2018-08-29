namespace Instagraph.DataProcessor.ImportDtos
{
    using System.Xml.Serialization;

    [XmlType("post")]
    public class PostElementDto
    {
        [XmlAttribute("id")]
        public int PostId { get; set; }
    }
}