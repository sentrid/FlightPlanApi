using MongoDB.Bson;
using MongoDB.Driver;
using FlightPlanApi.Models;

namespace FlightPlanApi.Data
{
    public class MongoDbDatabase : IDatabaseAdapter
    {
        public async Task<List<FlightPlan>> GetAllFlightPlans()
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var documents = collection.Find(_ => true).ToListAsync();

            var flightPlanList = new List<FlightPlan>();

            if (documents == null) return flightPlanList;

            foreach (var document in await documents)
            {
                flightPlanList.Add(ConvertBsonToFlightPlan(document));
            }

            return flightPlanList;
        }

        public async Task<FlightPlan> GetFlightPlanById(string flightPlanId)
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var flightPlanCursor = await collection.FindAsync(
                Builders<BsonDocument>.Filter.Eq("flight_plan_id", flightPlanId));
            var document = flightPlanCursor.FirstOrDefault();
            var flightPlan = ConvertBsonToFlightPlan(document);

            if (flightPlan == null)
            {
                return new FlightPlan();
            }

            return new FlightPlan();
        }

        public async Task<bool> FileFlightPlan(FlightPlan flightPlan)
        {
            var collection = GetCollection("pluralsight", "flight_plans");

            var document = new BsonDocument
            {
                {"flight_plan_id", Guid.NewGuid().ToString("N") },
                {"altitude", flightPlan.Altitude },
                {"airspeed", flightPlan.Airspeed },
                {"aircraft_identification", flightPlan.AircraftIdentification },
                {"aircraft_type", flightPlan.AircraftType },
                {"arrival_airport", flightPlan.ArrivalAirport },
                {"flight_type", flightPlan.FlightType },
                {"departing_airport", flightPlan.DepartureAirport },
                {"departure_time", flightPlan.DepartureTime },
                {"estimated_arrival_time", flightPlan.ArrivalTime },
                {"route", flightPlan.Route },
                {"remarks", flightPlan.Remarks },
                {"fuel_hours", flightPlan.FuelHours },
                {"fuel_minutes", flightPlan.FuelMinutes },
                {"number_onboard", flightPlan.NumberOnBoard }
            };

            try
            {
                await collection.InsertOneAsync(document);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteFlightPlanById(string flightPlanId)
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var result = await collection.DeleteOneAsync(
                Builders<BsonDocument>.Filter.Eq("flight_plan_id", flightPlanId));

            return result.DeletedCount > 0;
        }

        public async Task<bool> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan)
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var filter = Builders<BsonDocument>.Filter.Eq("flight_plan_id", flightPlanId);
            var update = Builders<BsonDocument>.Update
                .Set("altitude", flightPlan.Altitude)
                .Set("airspeed", flightPlan.Airspeed)
                .Set("aircraft_identification", flightPlan.AircraftIdentification)
                .Set("aircraft_type", flightPlan.AircraftType)
                .Set("arrival_airport", flightPlan.ArrivalAirport)
                .Set("flight_type", flightPlan.FlightType)
                .Set("departing_airport", flightPlan.DepartureAirport)
                .Set("departure_time", flightPlan.DepartureTime)
                .Set("estimated_arrival_time", flightPlan.ArrivalTime)
                .Set("route", flightPlan.Route)
                .Set("remarks", flightPlan.Remarks)
                .Set("fuel_hours", flightPlan.FuelHours)
                .Set("fuel_minutes", flightPlan.FuelMinutes)
                .Set("numberOnBoard", flightPlan.NumberOnBoard);
            var result = await collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        private IMongoCollection<BsonDocument> GetCollection(
            string databaseName, string collectionName)
        {
            var client = new MongoClient();
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            return collection;
        }

        private FlightPlan ConvertBsonToFlightPlan(BsonDocument document)
        {
            if (document == null) return null;

            return new FlightPlan
            {
                FlightPlanId = document["flight_plan_id"].AsString,
                Altitude = document["altitude"].AsInt32,
                Airspeed = document["airspeed"].AsInt32,
                AircraftIdentification = document["aircraft_identification"].AsString,
                AircraftType = document["aircraft_type"].AsString,
                ArrivalAirport = document["arrival_airport"].AsString,
                FlightType = document["flight_type"].AsString,
                DepartureAirport = document["departing_airport"].AsString,
                DepartureTime = document["departure_time"].AsBsonDateTime.ToUniversalTime(),
                ArrivalTime = document["estimated_arrival_time"].AsBsonDateTime.ToUniversalTime(),
                Route = document["route"].AsString,
                Remarks = document["remarks"].AsString,
                FuelHours = document["fuel_hours"].AsInt32,
                FuelMinutes = document["fuel_minutes"].AsInt32,
                NumberOnBoard = document["number_onboard"].AsInt32
            };
        }
    }
}
