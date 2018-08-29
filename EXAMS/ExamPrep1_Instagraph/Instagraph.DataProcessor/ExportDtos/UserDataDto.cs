namespace Instagraph.DataProcessor.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserDataDto
    {
        [XmlElement("Username")]
        public string Username { get; set; }

        [XmlElement("MostComments")]
        public int MostComments { get; set; }
    }
}