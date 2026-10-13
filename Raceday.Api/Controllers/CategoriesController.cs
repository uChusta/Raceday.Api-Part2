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
    public class CategoriesController : ControllerBase
    {
        // private field for the database context
        private readonly RaceDayDbContext _context;

        // constructor for dependency injection of the database context
        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }


        //get all categories for a specific event using eventID, can be accessed by both organisers and participants
        [HttpGet("event/{eventID}/categories")]
        public async Task<IActionResult> GetCategoriesForEvent(int eventID)
        { 
        var categories = await _context.Categories.Where(c => c.eventID == eventID).ToListAsync();
        return Ok(categories);
        }

        //Organisers can create a category for a specific event using eventID
        [HttpPost("event/{eventID}/categories")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateCategory(int eventID, [FromBody] CategoryDto dto)
        {
            var eventItem = await _context.Events.FindAsync(eventID);
            if (eventItem == null)
            {
                return NotFound(new { message = $"Event with ID {eventID} not found." });
            }
            var category = new Category
            {
                name = dto.name,
                distance_KM = dto.distance_KM,
                start_time = dto.start_time,
                entry_fee = dto.entry_fee,
                age_category = dto.age_category,
                max_entries = dto.max_entries,
                eventID = eventID
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, category);
        }

        //Organisers can update a category for a specific event using categoryID
        [HttpPut("categories/{categoryID}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateCategory(int categoryID, [FromBody] CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(categoryID);
            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {categoryID} not found." });
            }
            category.name = dto.name;
            category.distance_KM = dto.distance_KM;
            category.start_time = dto.start_time;
            category.entry_fee = dto.entry_fee;
            category.age_category = dto.age_category;
            category.max_entries = dto.max_entries;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Category updated successfully." , categoryID = category.categoryID});
        }

        //Organisers can delete a category for a specific event using categoryID
        [HttpDelete("categories/{categoryID}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteCategory(int categoryID)
        {
            var category = await _context.Categories.FindAsync(categoryID);
            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {categoryID} not found." });
            }
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Category deleted successfully." });
        }
    }
}
