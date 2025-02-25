namespace AltaGasAssignment.WebApi.Data.Entities
{
    public class EquipmentEvent
    {
        public Guid Id { get; set; }
        public required string EquipmentId { get; set; }
        public int EquipmentEventTypeId { get; set; }
        public EquipmentEventType? EquipmentEventType { get; set; }
        public DateTime EventDate { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
        public Guid TripId { get; set; }
        public Trip? Trip { get; set; }
    }
}
