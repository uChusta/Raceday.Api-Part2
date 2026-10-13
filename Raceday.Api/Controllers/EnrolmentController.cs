using Raceday.Api.Models;
using RaceDay.Contracts;
using Raceday.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data.SqlTypes;

namespace Raceday.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentController : ControllerBase
    {
        // private field for the database context
        private readonly RaceDayDbContext _context;

        // constructor for dependency injection of the database context
        public EnrolmentController(RaceDayDbContext context)
        {
            _context = context;
        }

        //Participants can enrol in a specific category of an event category
        [HttpPost("enrolments")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> EnrolInCategory([FromBody] EnrolmentDto dto)
        {
          var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
          if(!int.TryParse(userIdString, out int participantId))
            {
                return Unauthorized(new {message = "Invalid participant ID."});
            }
          var category = await _context.Categories.FindAsync(dto.categoryID);
          
          if (category == null)
          {
              return NotFound(new { message = $"Category with ID {dto.categoryID} not found." });
          }
            //if already enrolled, return conflict
            bool alreadyEnrolled = await _context.Enrolments.AnyAsync(e => e.participantID == participantId && e.categoryID == dto.categoryID);

            if (alreadyEnrolled)
            {
                return Conflict(new { message = $"Participant with ID {participantId} is already enrolled in this category" });
            }

            var enrolment = new Enrolment
            {
                participantID = participantId,
                categoryID = dto.categoryID,
                racer_number = dto.racer_number,
                payment_status = "Pending",
            };
            
            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();
            
            return StatusCode(StatusCodes.Status201Created, enrolment);
        }

        //Participants can view their enrolments
        [HttpGet("enrolments/my-enrolments")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int participantId))
            {
                return Unauthorized(new { message = "Invalid participant ID." });
            }

            var enrolments = await _context.Enrolments
                .Include(e => e.Category)
                .ThenInclude(c => c.Event)
                .Where(e => e.participantID == participantId)
                .ToListAsync();
            
            return Ok(enrolments);
        }

        //Organiser can view all enrolments for their event
        [HttpGet("events/{eventId}/enrolments")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEnrolmentsForEvent(int eventId)
        {
            var enrolments = await _context.Enrolments
                .Include(e => e.Category)
                .ThenInclude(c => c.Event)
                .Where(e => e.Category.eventID == eventId)
                .ToListAsync();

            return Ok(enrolments);
        }
    }
}
