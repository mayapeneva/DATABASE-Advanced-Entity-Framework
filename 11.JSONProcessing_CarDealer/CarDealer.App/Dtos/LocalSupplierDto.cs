namespace CarDealer.App.Dtos
{
    public class LocalSupplierDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual int PartsCount { get; set; }
    }
}