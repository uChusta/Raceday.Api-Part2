using Raceday.Api.Models;
using RaceDay.Contracts;
using Raceday.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Raceday.Api
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
            var eventDetails = await _context.Events.FindAsync(eventID);
            if (eventDetails == null)
            {
                return NotFound($"Event with ID {eventID} not found.");
            }
            return Ok(eventDetails);
        }

        //Organiser can create new event
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task <IActionResult> CreateEvent([FromBody] ) 
    }
}
