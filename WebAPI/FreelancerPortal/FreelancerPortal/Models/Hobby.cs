using FreelancerPortal.DTO;

namespace FreelancerPortal.Models
{
    public class Hobby
    {
        public int Id { get; set; }
        public string HobbyName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
      

    }
}
