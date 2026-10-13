using Microsoft.AspNetCore.Mvc;
using Raceday.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Raceday.Api.Models;
using RaceDay.Contracts;
using BCrypt.Net;


namespace Raceday.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // private field for the database context
        private readonly RaceDayDbContext _context;

        // constructor for dependency injection of the database context
        public AuthController(RaceDayDbContext context)
        {
            _context = context;
        }

        // register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto.user_role != "Organiser" && dto.user_role != "Participant")
            {
                return BadRequest("Invalid user role. Must be either 'Organiser' or 'Participant'.");
            }
            if (await _context.Users.AnyAsync(u => u.email == dto.email))
            {
                return Conflict("Email already exists.");
            }

            //password hashing
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.user_password);

            // create a new user entity
            var user = new User
            {
                fullname = dto.fullname,
                surname = dto.surname,
                gender = dto.gender,
                email = dto.email,
                user_password = hashedPassword,
                phone_number = dto.phone_number,
                user_role = dto.user_role,
                date_of_birth = dto.date_of_birth,
                clubID = dto.clubID
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, new { message = "User registered successfully.", userID = user.userID });
        }

        // login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == dto.email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.user_password, user.user_password))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.userID.ToString()),
                new Claim(ClaimTypes.Name, user.fullname),
                new Claim(ClaimTypes.Role, user.user_role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            // sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            //store session data
            HttpContext.Session.SetString("userID", user.userID.ToString());
            HttpContext.Session.SetString("user_role", user.user_role);

            return Ok(new { message = "Login successful.", role = user.user_role });
        }

        // logout endpoint
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Ok(new { message = "Logout successful." });
        }
    }
}
