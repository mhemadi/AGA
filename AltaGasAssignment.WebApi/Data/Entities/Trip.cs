namespace AltaGasAssignment.WebApi.Data.Entities
{
    public class Trip
    {
        public Guid Id { get; set; }
        public required string EquipmentId { get; set; } //Could have an Equipment table
        public int OriginCityId { get; set; }
        public City? OriginCity { get; set; }
        public int? DestinationCityId { get; set; } //Assuming we can have ongoing trips
        public City? DestinationCity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalTripMinutes { get; set; } //Since the data supports minutes
        public ICollection<EquipmentEvent>? EquipmentEvents { get; set; }
    }
}
