namespace Models
{
    public class TeamEvent
    {
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}