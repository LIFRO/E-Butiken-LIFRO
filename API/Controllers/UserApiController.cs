using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace API.Controllers;

[ApiController]

public class UserApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public UserApiController(AppDbContext db) => _db = db;

    [HttpPost("Register")]
    public async Task<ActionResult<AppUser>> Register([FromBody] UserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.email))
            return BadRequest("User mail empty");
        if (string.IsNullOrWhiteSpace(user.Password))
            return BadRequest("User password empty");

            var exists = await _db.Users.AnyAsync(u => u.UserMail == user.email);
            if (exists)
                return Conflict("A user with that email already exists");

            var hash = HashSHA256(user.Password); 
            var newUser = new AppUser {UserMail = user.email, Name = user.name, PasswordHash = hash};

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Login), new { id = newUser.id }, newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> Login([FromBody] UserDto user)
    {
        if(string.IsNullOrWhiteSpace(user.email) || string.IsNullOrWhiteSpace(user.Password))
        return BadRequest("Email and password required");

        var exist = await _db.Users.FirstOrDefaultAsync(u => u.UserMail == user.email);
        if (exist == null) 
        return Unauthorized("Invalid credentials");

        if(HashSHA256(user.Password) != exist.PasswordHash)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(exist);
    }

    private static string HashSHA256(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input + "lalalal123");
        var hash = SHA256.Create().ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

public class UserDto
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
