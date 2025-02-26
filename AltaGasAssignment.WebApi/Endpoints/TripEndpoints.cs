using AltaGasAssignment.Shared.DTOs;
using AltaGasAssignment.WebApi.Application.Services;
using FluentValidation;

namespace AltaGasAssignment.WebApi.Endpoints
{
    public static class TripEndpoints
    {
        public static IEndpointRouteBuilder MapTripEndpoints(this IEndpointRouteBuilder routes)
        {
            //TODO: Implement filtering
            routes.MapGet("/trips", async (TripService tripService, [AsParameters]
                TripListRequestDto requestDto, IValidator<TripListRequestDto> validator) =>
            {
                var validationResult = validator.Validate(requestDto);
                if(validationResult.IsValid == false)
                {
                    return Results.BadRequest(validationResult.Errors); //Could also use problem
                }

                //TODO: Exception handling
                var response = await tripService.GetTrips(requestDto);
                return Results.Ok(response);
            });

            return routes;
        }
    }
}
