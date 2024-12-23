using FreelancerPortal.DTO;
using FreelancerPortal.Models;

namespace FreelancerPortal.Service
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task AddUserAsync(UserDto userDto);
        Task UpdateUserAsync(int id, UserDto userDto);
        Task DeleteUserAsync(int id);
        Task ArchiveFreelancerAsync(int id);
        Task UnarchiveFreelancerAsync(int id);
        Task<IEnumerable<User>> SearchFreelancersAsync(string query);
    }
}
