namespace AltaGasAssignment.WebApi.Data.Entities
{
    public class City
    {
        public int Id { get; set; }  //Could have potentially used the CityId as primary key
        public int CityId { get; set; }
        public required string Name { get; set; }
        public required string TimeZoneStr { get; set; }
    }
}
