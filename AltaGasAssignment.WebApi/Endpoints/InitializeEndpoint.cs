using AltaGasAssignment.WebApi.Data;
using AltaGasAssignment.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Endpoints
{
    public static class InitializeEndpoint
    {
        public static IEndpointRouteBuilder MapInitializeEndpoints(this IEndpointRouteBuilder routes)
        {
            //Need to run this to initialize the database. 
            routes.MapGet("/init", async (AppDbContext dbContext) =>
            {
                if (!Directory.Exists(@"Data\DB"))
                {
                    Directory.CreateDirectory(@"Data\DB");
                }

                dbContext.Database.Migrate();

                var cityFileLines = await File.ReadAllLinesAsync(@"Assets\canadian_cities.csv");
                var eventTypeFileLines = await File.ReadAllLinesAsync(@"Assets\event_code_definitions.csv");

                bool first = true;
                foreach (var cityLine in cityFileLines)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    var cs = cityLine.Split(',');
                    dbContext.Cities.Add(new City()
                    {
                        CityId = Convert.ToInt32(cs[0]),
                        Name = cs[1].Trim(),
                        TimeZoneStr = cs[2].Trim()
                    });
                }

                first = true;
                foreach (var eventTypeLine in eventTypeFileLines)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    var cs = eventTypeLine.Split(",");
                    dbContext.EquipmentEventTypes.Add(new EquipmentEventType()
                    {
                        Code = cs[0].Trim(),
                        EventDescription = cs[1].Trim(),
                        LongDescription = cs[2].Trim()
                    });
                }

                await dbContext.SaveChangesAsync();
            });

            return routes;
        }
    }
}
