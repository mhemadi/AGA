namespace AltaGasAssignment.Shared.DTOs
{
    public class TripEventResponseDto
    {
        public Guid Id { get; set; }
        public int EquipmentEventTypeId { get; set; }
        public required string EquipmentEventTypeDescription { get; set; }
        public DateTime EventDate { get; set; }
        public int CityId { get; set; }
        public required string CityName { get; set; }
        public Guid TripId { get; set; }
    }
}
