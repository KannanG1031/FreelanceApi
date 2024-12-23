using System.ComponentModel.DataAnnotations;

namespace FreelancerPortal.Models
{
    public class User
    {
       
        public int Id { get; set; }
        public  string Username { get; set; }
        public  string Email { get; set; }
        public  string PhoneNumber { get; set; }

        // Navigation properties
        public ICollection<Skillset> Skillsets { get; set; }
        public ICollection<Hobby> Hobbies { get; set; }
        public bool IsArchived { get; set; }
    }
}
