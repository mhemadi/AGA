using AltaGasAssignment.Shared.Enums;

namespace AltaGasAssignment.Shared.DTOs
{
    public class TripListRequestDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public TripListRequestOrderBy? OrderBy { get; set; }
    }
}
