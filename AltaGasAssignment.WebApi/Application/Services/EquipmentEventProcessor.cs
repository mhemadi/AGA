using AltaGasAssignment.WebApi.Data.Entities;
using AltaGasAssignment.WebApi.Data;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace AltaGasAssignment.WebApi.Application.Services
{
    /// <summary>
    /// Processes a file containing equipment events and stores the data in the database
    /// The public methods must be called in order. Load, Process and save
    /// </summary>
     

    //TODO: Refactor this class into smaller components
    public class EquipmentEventProcessor
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EquipmentEventProcessor> _logger;
        private string[]? fileLines;
        private List<Trip> trips = new();
        private List<EquipmentEvent> equipmentEvents = new();
        private Dictionary<int, TimeZoneInfo> cityTimeZoneMap = new();
        private Dictionary<int, City> cityMap = new();
        private Dictionary<string, EquipmentEventType> eventTypeMap = new();

        public EquipmentEventProcessor(AppDbContext dbContext, ILogger<EquipmentEventProcessor> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        //TODO: Implement a more complex return type. This will do for now
        public async Task<(bool Success, string Message)> Load(Guid fileId)
        {
            var fileLoaded = await LoadFile(fileId);
            if (!fileLoaded)
            {
                return (false, "Failed to load uploaded file");
            }

            var cityLoaded = await LoadCities();
            if (!cityLoaded)
            {
                return (false, "Failed to load cities");
            }

            var eventTypesLoaded = await LoadEventTypes();
            if (!eventTypesLoaded)
            {
                return (false, "Failed to load event types");
            }

            bool first = true;
            foreach (var fileLine in fileLines!)
            {
                if (first)
                {
                    first = false;
                    continue;
                }

                var cs = fileLine.Split(',');
                if (cs.Length != 4)
                {
                    _logger.LogError("Each line is required to have 4 comma separated values, {FileLine}",
                        fileLine);

                    return (false, "Wrong files structure");
                }

                if (!int.TryParse(cs[3], out var cityId))
                {
                    _logger.LogError("City id {CityId} is not a valid integer", cs[3]);
                    return (false, "Error processing the city ids");
                }

                if (!cityTimeZoneMap.ContainsKey(cityId))
                {
                    _logger.LogError("City id {CityId} could not be found", cityId);
                    return (false, "Error processing the cities");
                }

                if (!eventTypeMap.ContainsKey(cs[1].Trim()))
                {
                    _logger.LogError("Event code {EventCode} could not be found", cs[1]);
                    return (false, "Error processing the event types");
                }

                DateTimeOffset dateTimeOffset;
                try
                {
                    DateTime parsedDateTime = DateTime.ParseExact(cs[2].Trim(), "yyyy-MM-dd HH:mm",
                        CultureInfo.InvariantCulture);

                    dateTimeOffset = new DateTimeOffset(parsedDateTime,
                        cityTimeZoneMap[cityId].GetUtcOffset(parsedDateTime));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Even time {EventTimeStr} could not be parsed", cs[2]);
                    return (false, "Error processing dates");
                }

                equipmentEvents.Add(new EquipmentEvent()
                {
                    EquipmentId = cs[0].Trim(),
                    CityId = cityMap[cityId].Id,
                    EquipmentEventTypeId = eventTypeMap[cs[1].Trim()].Id,
                    EquipmentEventType = eventTypeMap[cs[1].Trim()],
                    EventDate = dateTimeOffset.UtcDateTime,
                    City = cityMap[cityId],
                    Id = Guid.NewGuid()
                });
            }

            return (true, "Success");
        }

        public (bool Success, string Message) Process()
        {
            Dictionary<string, Guid> equipmentIdTripMap = new();
            Dictionary<Guid, Trip> tripMap = new();
            equipmentEvents = equipmentEvents.OrderBy(x => x.EventDate).ToList();

            //To be even more strict we could try to identify duplicate or over lapping events
            foreach (var eqEvent in equipmentEvents)
            {
                if (!equipmentIdTripMap.ContainsKey(eqEvent.EquipmentId))
                {
                    if (eqEvent.EquipmentEventType!.Code != "W")
                    {
                        _logger.LogError("Trip should start with a W {@EquipmentEvent}", eqEvent);
                        return (false, "Error processing the file, wrong order");
                    }

                    Trip trip = new Trip()
                    {
                        EquipmentId = eqEvent.EquipmentId,
                        OriginCityId = eqEvent.CityId,
                        Id = Guid.NewGuid(),
                        StartDate = eqEvent.EventDate
                    };

                    tripMap.Add(trip.Id, trip);
                    eqEvent.TripId = trip.Id;
                    equipmentIdTripMap.Add(trip.EquipmentId, trip.Id);
                }
                else
                {
                    if (eqEvent.EquipmentEventType!.Code == "W")
                    {
                        _logger.LogError("Trip is already started and can not have another W event" +
                            " {@EquipmentEvent}", eqEvent);

                        return (false, "Error processing the file, wrong order");
                    }

                    var tripId = equipmentIdTripMap[eqEvent.EquipmentId];
                    eqEvent.TripId = tripId;

                    if (eqEvent.EquipmentEventType!.Code == "Z")
                    {
                        Trip trip = tripMap[tripId];
                        trip.DestinationCityId = eqEvent.CityId;
                        trip.EndDate = eqEvent.EventDate;
                        trip.TotalTripMinutes = Convert.ToInt32((trip.EndDate - trip.StartDate).Value
                            .TotalMinutes);

                        equipmentIdTripMap.Remove(trip.EquipmentId);
                    }
                }
            }

            trips = tripMap.Values.ToList();
            return (true, "Success");
        }

        public async Task<(bool Success, string Message)> Save()
        {
            _dbContext.EquipmentEvents.AddRange(equipmentEvents);
            _dbContext.Trips.AddRange(trips);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed saving to the db");
                return (false, "Failed to save");
            }

            return (true, "Success");
        }

        //Could cache the city list
        private async Task<bool> LoadCities()
        {
            List<City> cities = new List<City>();
            try
            {
                cities = await _dbContext.Cities.ToListAsync();
                if (!cities.Any())
                {
                    _logger.LogError("No city found in the database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load cities from database");
                return false;
            }

            foreach (var city in cities)
            {
                try
                {
                    //It seems that linux uses different time zone names, keep that into consideration
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(city.TimeZoneStr);
                    cityTimeZoneMap.Add(city.CityId, timeZone);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Can not deduce the time zone for city {CityName} and time zone " +
                        "string {TimeZoneStr}", city.Name, city.TimeZoneStr);

                    return false;
                }

                cityMap.Add(city.CityId, city);
            }

            return true;
        }

        //Could cache the event type list
        private async Task<bool> LoadEventTypes()
        {
            List<EquipmentEventType> eventTypes = new List<EquipmentEventType>();
            try
            {
                eventTypes = await _dbContext.EquipmentEventTypes.ToListAsync();
                if (!eventTypes.Any())
                {
                    _logger.LogError("No event type found in the database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load event types from database");
                return false;
            }

            eventTypeMap = eventTypes.ToDictionary(x => x.Code, x => x);
            return true;
        }

        private async Task<bool> LoadFile(Guid fileId)
        {
            try
            {
                fileLines = await File.ReadAllLinesAsync(@"Data\Uploads\" + fileId);
            }
            catch (Exception ex) //Could be more specific
            {
                _logger.LogError(ex, "Failed reading file {FileId}", fileId);
                return false;
            }

            if (fileLines is null || fileLines.Length == 0)
            {
                _logger.LogError("File {FileId} loaded but is empty", fileId);
                return false;
            }

            return true;
        }
    }
}
