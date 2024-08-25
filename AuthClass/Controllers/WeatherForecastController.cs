using AuthClass.Context;
using AuthClass.Model;
using AuthClass.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthClass.Controllers
{
    [ApiController]
    [Route("")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserContext _context;

        public WeatherForecastController(UserContext context,ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("home")]
        public async Task <ActionResult<string>> Home()
        {
            return Ok("welcome");
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User loginUser)
        {
            if(loginUser == null)
            {
                return BadRequest("invalid");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u =>u.Username== loginUser.Username
            && u.Password == loginUser.Password);

            if(user== null)
            {
                return Unauthorized("username is incorrect");
            }
            string token =  Guid.NewGuid().ToString(); 

            user.Token = token;
            await _context.SaveChangesAsync(); 

            Response.Cookies.Append("Auth", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
            return Ok();
        }


        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            Response.Cookies.Delete("Auth");

            return Ok("Auth Deleted");
        }






    }
}
