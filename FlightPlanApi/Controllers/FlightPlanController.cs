using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightPlanApi.Data;
using FlightPlanApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlightPlanApi.Controllers
{
    [Route("api/v1/flightplan")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private IDatabaseAdapter _database;

        public FlightPlanController(IDatabaseAdapter database)
        {
            _database = database;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FlightPlanList()
        {
            var flightPlanList = await _database.GetAllFlightPlans();
            if (flightPlanList.Count == 0)
            {
                return NoContent();
            }

            return Ok(flightPlanList);
        }

        [HttpGet]
        [Authorize]
        [Route("{flightPlanId}")]
        public async Task<IActionResult> GetFlightFlightPlanById(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId); 
            if (flightPlan.FlightPlanId != flightPlanId)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(flightPlan);
        }

        [HttpPost]
        [Authorize]
        [Route("file")]
        public async Task<IActionResult> FileFlightPlan(FlightPlan flightPlan)
        {
            var transactionResult = await _database.FileFlightPlan(flightPlan);
            switch(transactionResult)
            {
                case TransactionResult.Success:
                    return Ok();
                case TransactionResult.BadRequest:
                    return StatusCode(StatusCodes.Status400BadRequest);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateFlightPlan(FlightPlan flightPlan)
        {
            var updateResult = await _database.UpdateFlightPlan(flightPlan.FlightPlanId, flightPlan);
            switch (updateResult)
            {
                case TransactionResult.Success:
                    return Ok();
                case TransactionResult.NotFound:
                    return StatusCode(StatusCodes.Status404NotFound);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{flightPlanId}")]
        public async Task<IActionResult> DeleteFlightPlan(string flightPlanId)
        {
            var result = await _database.DeleteFlightPlanById(flightPlanId);
            if (result)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpGet]
        [Authorize]
        [Route("airport/departure/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanDepartureAirport(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);
            
            if (flightPlan == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(flightPlan.DepartureAirport);
        }

        [HttpGet]
        [Authorize]
        [Route("route/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanRoute(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);
            if (flightPlan.FlightPlanId != flightPlanId)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(flightPlan.Route);
        }

        [HttpGet]
        [Authorize]
        [Route("time/enroute/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
        {
            var flightPlan = await _database.GetFlightPlanById(flightPlanId);
            if (flightPlan == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            
            var estimatedTimeEnroute = flightPlan.ArrivalTime - flightPlan.DepartureTime;

            return Ok(estimatedTimeEnroute);
        }
        
    }
}
