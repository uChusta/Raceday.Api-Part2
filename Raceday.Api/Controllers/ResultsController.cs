using Raceday.Api.Models;
using RaceDay.Contracts;
using Raceday.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Raceday.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        // private field for the database context
        private readonly RaceDayDbContext _context;

        // constructor for dependency injection of the database context
        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        //Organiser can capture/create a participant's result for a race
        [HttpPost("results")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CaptureResult([FromBody] ResultDto dto)
        {
            var enrolment = await _context.Enrolments.FindAsync(dto.enrolmentID);
            if (enrolment == null)
            {
                return NotFound(new { message = $"Enrolment with ID {dto.enrolmentID} not found." });
            }

            var result = new Result
            {
                enrolmentID = dto.enrolmentID,
                finish_time = dto.finish_time,
                category_position = dto.category_position,
                status = dto.status
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, result);
        }

         //Organiser can update a participant's result for a race
        [HttpPut("results/{resultID}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateResult(int resultID, [FromBody] ResultDto dto)
        {
            var result = await _context.Results.FindAsync(resultID);
            if (result == null)
            {
               return NotFound(new { message = $"Result with ID {resultID} not found." });
            }
          result.finish_time = dto.finish_time;
          result.category_position = dto.category_position;
          result.status = dto.status;
          
          await _context.SaveChangesAsync();
          
          return Ok(new { message = $"Result with ID {resultID} updated successfully." });

        }

        //Participant can view their result
        [HttpGet("results/my-results")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int participantId))
            {
                return Unauthorized(new { message = "Invalid participant ID." });
            }
            var results = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Category)
                .ThenInclude(c => c.Event)
                .Where(r => r.Enrolment.participantID == participantId)
                .ToListAsync();

            return Ok(results);
        }

        //participant can view their result for an event
        [HttpGet("events/{eventId}/results/")]
        public async Task<IActionResult> GetResultsForEvent(int eventId)
        {
            var results = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e.Category)
                .Where(r => r.Enrolment.Category.eventID == eventId)
                .ToListAsync();

            return Ok(results);
        }

    }
}
