using MongoDB.Bson;
using MongoDB.Driver;
using FlightPlanApi.Models;

namespace FlightPlanApi.Data
{
    public class MongoDbDatabase
    {

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
