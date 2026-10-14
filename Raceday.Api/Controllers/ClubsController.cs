using System.Collections.Generic;
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
    public class ClubsController : ControllerBase
    {
        // private field for the database context
        private readonly RaceDayDbContext _context;

        // constructor for dependency injection of the database context
        public ClubsController(RaceDayDbContext context)
        {
            _context = context;
        }

        //Gets a list of all clubs
        [HttpGet]
        public async Task<IActionResult> GetAllClubs()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Users)
                .ToListAsync();
            return Ok(clubs);
        }

        //get club by ID
        [HttpGet("{clubID}")]
        public async Task<IActionResult> GetClubById(int clubID)
        {
            var club = await _context.Clubs
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.clubID == clubID);

            if (club == null)
            {
                return NotFound(new { message = $"Club with ID {clubID} not found." });
            }
            return Ok(club);
        }

        //Organiser can create a new club
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateClub(int clubID, [FromBody] ClubDto dto)
        {
            var club = await _context.Clubs.FindAsync(clubID);
            if (club != null)
            {
                return NotFound(new { message = "Club not found." });
            }

            var validsport_types = new List<string> { "Running", "Cycling", "Walking","Multi-Sport" };
            if (!validsport_types.Contains(dto.sport_type))
            {
                return BadRequest(new { message = $"Invalid sport type. Valid options are: {string.Join(", ", validsport_types)}" });
            }

            club.club_name = dto.club_name;
            club.province = dto.province;
            club.city = dto.city;
            club.contact_email = dto.contact_email;
            club.sport_type = dto.sport_type;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Club created successfully.", club = club.clubID });
        }

        //Delete a club by ID
        [HttpDelete("{clubID}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteClub(int clubID)
        {
            var club = await _context.Clubs.FindAsync(clubID);
            if (club == null)
            {
                return NotFound(new { message = $"Club with ID {clubID} not found." });
            }
            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Club with ID {clubID} deleted successfully." });
        }

    }
}
