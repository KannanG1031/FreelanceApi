using FreelancerPortal.DTO;
using FreelancerPortal.Models;
using FreelancerPortal.Repository;

namespace FreelancerPortal.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all freelancers.");
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(u => new UserDto
            {
                Username = u.Username,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Skillsets = u.Skillsets.Select(s => new SkillsetDto { SkillName = s.SkillName }).ToList(),
                Hobbies = u.Hobbies.Select(h => new HobbyDto { HobbyName = h.HobbyName }).ToList(),
                IsArchived=u.IsArchived
            });
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching freelancer with ID: {id}");
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Skillsets = user.Skillsets.Select(s => new SkillsetDto { SkillName = s.SkillName }).ToList(),
                Hobbies = user.Hobbies.Select(h => new HobbyDto { HobbyName = h.HobbyName }).ToList(),
                IsArchived = user.IsArchived
            };
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            _logger.LogInformation("Registering a new freelancer.");

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Skillsets = userDto.Skillsets.Select(s => new Skillset { SkillName = s.SkillName }).ToList(),
                Hobbies = userDto.Hobbies.Select(h => new Hobby { HobbyName = h.HobbyName }).ToList()
            };

            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(int id, UserDto userDto)
        {
            _logger.LogInformation($"Updating freelancer with ID: {userDto.Id}");

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return;

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Skillsets = userDto.Skillsets.Select(s => new Skillset { SkillName = s.SkillName, UserId = id }).ToList();
            user.Hobbies = userDto.Hobbies.Select(h => new Hobby { HobbyName = h.HobbyName, UserId = id }).ToList();

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            _logger.LogWarning($"Deleting freelancer with ID: {id}");
            await _userRepository.DeleteUserAsync(id);
        }

        public Task ArchiveFreelancerAsync(int id)
        {
            _logger.LogInformation($"Archiving freelancer with ID: {id}");
            return _userRepository.ArchiveFreelancerAsync(id);
        }

        public Task UnarchiveFreelancerAsync(int id)
        {
            _logger.LogInformation($"Unarchiving freelancer with ID: {id}");
            return _userRepository.UnarchiveFreelancerAsync(id);
        }

        public Task<IEnumerable<User>> SearchFreelancersAsync(string query)
        {
            _logger.LogInformation($"Searching freelancers with query: {query}");
            return _userRepository.SearchFreelancersAsync(query);
        }
    }

}
