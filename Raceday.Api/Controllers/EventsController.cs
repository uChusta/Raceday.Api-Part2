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
    public class EventsController : ControllerBase
    {
        // private field for the database context
        private readonly RaceDayDbContext _context;

        // constructor for dependency injection of the database context
        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        //can be accesssed by both organisers and participants
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            // Get all events with status "Published"
            var events = await _context.Events.Where(e => e.status == "Published").ToListAsync();
            return Ok(events);
        }

        //only accessed by organisers
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateEvent([FromBody] Event eventDto)
        {
            // Set the status to "Draft" by default
            eventDto.status = "Draft";
            _context.Events.Add(eventDto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllEvents), new { id = eventDto.eventID }, eventDto);
        }

        //only accessed by participants
        [HttpPost("{eventID}/enrol")]
        [Authorize(Roles = "Participant")]
        public IActionResult EnrolInEvent(int eventID)
        {
            return Ok(new { message = $"Participant enrolled in event {eventID} successfully." });
        }

        //retrive details of a specific event using eventID, can be accessed by both organisers and participants
        [HttpGet("{eventID}")]
        public async Task<IActionResult> GetEventById(int eventID)
        {
            var eventDetails = await _context.Events.Include(e => e.Categories).FirstOrDefaultAsync(e => e.eventID == eventID);
            if (eventDetails == null)
            {
                return NotFound($"Event with ID {eventID} not found.");
            }
            return Ok(eventDetails);
        }

        //Organiser can create new event
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto eventDto)
        { 
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int organiserId))
            { 
                return Unauthorized(new {message = "Invalid user ID."});
            }

            var newEvent = new Event
            {
                title = eventDto.title,
                description = eventDto.description,
                event_type = eventDto.event_type,
                event_date = eventDto.event_date,
                registration_deadline = eventDto.registration_deadline,
                venue = eventDto.venue,
                city = eventDto.city,
                province = eventDto.province,
                status = "Draft", // Default status
                organiserID = organiserId // Set the organiser ID
            };
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, newEvent);
        }

        //Organiser can update existing event
        [HttpPut("{eventID}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateEvent(int eventID, [FromBody] EventUpdateDto eventDto)
        {
        
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int organiserId))
            {
                return Unauthorized(new { message = "Invalid user ID." });
            }

            var eventItem = await _context.Events.FindAsync(eventID);
            if (eventItem == null)
            {
                return NotFound(new { message = $"Event with ID {eventID} not found." });
            }

            //check if the organiser is the owner of the event
            if (eventItem.organiserID != organiserId)
            {
                return Forbid();
            }

            eventItem.title = eventDto.title;
            eventItem.description = eventDto.description;
            eventItem.event_type = eventDto.event_type;
            eventItem.event_date = eventDto.event_date;
            eventItem.registration_deadline = eventDto.registration_deadline;
            eventItem.venue = eventDto.venue;
            eventItem.city = eventDto.city;
            eventItem.province = eventDto.province;
            eventItem.status = eventDto.status;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Event with ID {eventID} updated successfully.", eventID = eventItem.eventID });
        }

        //Organiser can delete existing event
        [HttpDelete("{eventID}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteEvent(int eventID)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int organiserId))
            {
                return Unauthorized(new { message = "Invalid user ID." });
            }

            var eventItem = await _context.Events.FindAsync(eventID);
            if (eventItem == null)
            {
                return NotFound(new { message = $"Event with ID {eventID} not found." });
            }

            //check if the organiser is the owner of the event
            if (eventItem.organiserID != organiserId)
            {
                return Forbid();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Event with ID {eventID} deleted successfully." });
        }
    }
}
