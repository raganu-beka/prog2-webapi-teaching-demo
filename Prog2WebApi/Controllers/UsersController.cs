using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Prog2WebApi.Data;
using Prog2WebApi.Models;
using Prog2WebApi.Models.Requests;
using Prog2WebApi.Models.Responses;
using Prog2WebApi.Services;

namespace Prog2WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly AuthenticationService _authService;

        public UsersController(AppDbContext dbContext, AuthenticationService authService)
        {
            _db = dbContext;
            _authService = authService;
        }

        [HttpPost("/register")]
        public IActionResult CreateUser(UserRequest request)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Username == request.Username);
            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            var user = new User()
            {
                Username = request.Username
            };

            // Izveidojam hasher objektu un hash'ojam paroli
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(new { id = user.Id });
        }

        [HttpPost("/login")]
        public IActionResult LoginUser(UserRequest request)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Username == request.Username);
            if (existingUser == null)
            {
                return Unauthorized("Incorrect credentials.");
            }

            // Izveidojam hasher objektu un salīdzinam ievadītās paroles hash ar DB hash
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(existingUser, existingUser.Password, request.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized("Incorrect credentials.");
            }

            var token = _authService.CreateJwtToken(existingUser);
            return Ok(new { token });
        }
    }
}
