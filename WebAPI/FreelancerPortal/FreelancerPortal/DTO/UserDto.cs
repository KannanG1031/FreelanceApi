using FreelancerPortal.Models;

namespace FreelancerPortal.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<SkillsetDto> Skillsets { get; set; }
        public ICollection<HobbyDto> Hobbies { get; set; }
        public bool IsArchived { get; set; }
    }
}
