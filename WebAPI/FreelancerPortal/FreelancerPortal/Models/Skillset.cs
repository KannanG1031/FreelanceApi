namespace FreelancerPortal.Models
{
    public class Skillset
    {
        public int Id { get; set; }
        public string SkillName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
