using AltaGasAssignment.Shared.DTOs;
using AltaGasAssignment.Shared.Enums;
using AltaGasAssignment.WebApi.Data.Entities;
using AltaGasAssignment.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Application.Services
{
    public class TripService
    {
        private readonly AppDbContext _dbContext;

        public TripService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TripListResponseDto?> GetTrips(TripListRequestDto requestDto)
        {
            IQueryable<Trip> query = _dbContext.Trips
                .Include(x => x.OriginCity)
                .Include(x => x.DestinationCity);

            query = requestDto?.OrderBy switch
            {
                TripListRequestOrderBy.RecentStartDate => query.OrderByDescending(x => x.StartDate),
                TripListRequestOrderBy.RecentEndDate => query.OrderByDescending(x => x.EndDate),
                TripListRequestOrderBy.ShortestTrip => query.OrderBy(x => x.TotalTripMinutes),
                TripListRequestOrderBy.LongestTrip => query.OrderByDescending(x => x.TotalTripMinutes),
                _ => query.OrderBy(x => x.EquipmentId)
            };

            query = query.Skip((requestDto!.PageNumber - 1) * requestDto.PageSize).Take(requestDto.PageSize);

            var trips = await query.ToListAsync();

            var tripListDto = trips.Select(x => new TripResponseDto()
            {
                DestinationCityId = x.DestinationCityId,
                EquipmentId = x.EquipmentId,
                DestinationCityName = x.DestinationCity?.Name,
                EndDate = x.EndDate,
                Id = x.Id,
                OriginCityId = x.OriginCityId,
                OriginCityName = x.OriginCity!.Name,
                StartDate = x.StartDate,
                TotalTripHours = Convert.ToInt32(Math.Round(x.TotalTripMinutes / 60.0))
            }).ToList();

            return new TripListResponseDto() { Trips = tripListDto };
        }
    }
}
