using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightPlanApi.Data;
using FlightPlanApi.Models;

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
        public async Task<IActionResult> FlightPlanList()
        {
            var flightPlanList = await _database.GetAllFlightPlans();
            if (flightPlanList.Count == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return Ok(flightPlanList);
        }

        [HttpGet]
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
        [Route("file")]
        public async Task<IActionResult> FileFlightPlan(FlightPlan flightPlan)
        {
            var result = await _database.FileFlightPlan(flightPlan);
            if (result)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{flightPlan}")]
        public async Task<IActionResult> UpdateFlightPlan(FlightPlan flightPlan)
        {
            var updateResult = await _database.UpdateFlightPlan(flightPlan.FlightPlanId, flightPlan);
            if (updateResult)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete]
        [Route("{flightPlanId}")]
        public async Task<IActionResult> DeleteFlightPlan(string flightPlanId)
        {
            var result = await _database.DeleteFlightPlanById(flightPlanId);
            if (result)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
