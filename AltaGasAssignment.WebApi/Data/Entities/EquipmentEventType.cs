namespace AltaGasAssignment.WebApi.Data.Entities
{
    public class EquipmentEventType
    {
        public int Id { get; set; } //Could have potentially used the Code as primary key
        public required string Code { get; set; }
        public required string EventDescription { get; set; }
        public string? LongDescription { get; set; }
    }
}
