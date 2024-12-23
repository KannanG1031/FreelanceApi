using FreelancerPortal.DTO;
using FreelancerPortal.Models;
using FreelancerPortal.Repository;
using FreelancerPortal.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPortal.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get list of all Users
        /// </summary>
        /// <returns>list of all freelancer users</returns>
        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            _logger.LogInformation("API call to fetch all freelancers.");
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            _logger.LogInformation($"API call to fetch freelancer with ID: {id}");
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
            {
                _logger.LogWarning($"Freelancer with ID: {id} not found.");
                return NotFound(new { message = "Freelancer not found." });
            }
            
            return Ok(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST api/v1/user/
        /// {
        ///  "username": "Testing",
        ///  "email": "Testing1@test.com",
        ///  "phoneNumber": "123456789",
        ///  "skillsets": [
        ///    {
        ///      "skillName": "C"
        ///    }
        ///  ],
        ///  "hobbies": [
        ///    {
        ///      "hobbyName": "Reading"
        ///    }
        ///  ],
        ///  "isArchived": true
        ///}
        ///"isArchived": true}
        /// </remarks>
        /// <param name="userDto"></param>
           


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserDto userDto)
        {
            _logger.LogInformation("API call to register a new freelancer.");
            await _userService.AddUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                _logger.LogWarning($"Freelancer ID mismatch: {id} vs {userDto.Id}");
                return BadRequest(new { message = "Freelancer ID mismatch." });
            }

            _logger.LogInformation($"API call to update freelancer with ID: {id}");
            await _userService.UpdateUserAsync(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogWarning($"API call to delete freelancer with ID: {id}");
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        [HttpGet("search")]
       
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchFreelancers([FromQuery] string query)
        {
            _logger.LogInformation($"API call to search freelancers with query: {query}");
            var user = await _userService.SearchFreelancersAsync(query);

            if (user == null || user.Count()==0)
            {
                _logger.LogWarning($"No data found with the search creteria: {query} .");
                return NotFound(new { message = $"No data found with the search creteria: {query} ." });
            }

            return Ok(user);
                    
        }

        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ArchiveFreelancer(int id)
        {
            _logger.LogInformation($"API call to archive freelancer with ID: {id}");
            await _userService.ArchiveFreelancerAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/unarchive")]
        public async Task<IActionResult> UnarchiveFreelancer(int id)
        {
            _logger.LogInformation($"API call to unarchive freelancer with ID: {id}");

            await _userService.UnarchiveFreelancerAsync(id);
            return NoContent();
        }


    }
       
    
}
