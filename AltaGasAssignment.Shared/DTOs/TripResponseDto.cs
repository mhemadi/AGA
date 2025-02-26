namespace AltaGasAssignment.Shared.DTOs
{
    public class TripResponseDto
    {
        public Guid Id { get; set; }
        public required string EquipmentId { get; set; }
        public int OriginCityId { get; set; }
        public required string OriginCityName { get; set; } //Could have use nested objects
        public int? DestinationCityId { get; set; }
        public string? DestinationCityName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalTripHours { get; set; }

        //Could have returned events along side the trip
    }
}
