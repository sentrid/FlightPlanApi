using System.Collections.Generic;
using System.Threading.Tasks;
using FlightPlanApi.Models;

namespace FlightPlanApi.Data
{
    public interface IDatabaseAdapter
    {
        Task<List<FlightPlan>> GetAllFlightPlans();

        Task<FlightPlan> GetFlightPlanById(string flightPlanId);

        Task<bool> FileFlightPlan(FlightPlan flightPlan);

        Task<bool> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan);

        Task<bool> DeleteFlightPlanById(string flightPlanId);
    }
}
