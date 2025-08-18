using HealthTrackerSolution.DataContext;
using HealthTrackerSolution.Interface;
using HealthTrackerSolution.Model;
using HealthTrackerSolution.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackerSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly dataContext _context;
        private readonly IUser _userRepository;


        public UserController(dataContext context, IUser userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpPut]
        [Route("SignUp")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User data is required.");


            await _userRepository.UserExistsAsync(user.PhoneNumber).ContinueWith(task =>
            {
                if (task.Result)
                {
                    throw new Exception("User with this phone number already exists.");
                }
            });

            await _userRepository.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok("User creeated successfully");
        }

        [HttpPost]
        [Route("Login/{phoneNumber}")]
        public async Task<IActionResult> login([FromBody] string password, long phoneNumber)
        {
            if (password == null)
                return BadRequest("User data is required.");

            await _userRepository.UserExistsAsync(phoneNumber).ContinueWith(task =>
            {
                if (!task.Result)
                {
                    throw new Exception("User with this phone number doesn't exists.");
                }
            });
            
            await _userRepository.ComparePasswordAsync(phoneNumber, password).ContinueWith(task =>
            {
                if (!task.Result)
                {
                    throw new Exception("Password is incorrect.");
                }
            });

            return Ok("Login successful");
        }
    }
}

