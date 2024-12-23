using AutoMapper;
using FreelancerPortal.DTO;
using FreelancerPortal.Models;


namespace FreelancerPortal
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
                CreateMap<UserDto, User>();
                CreateMap<SkillsetDto, Skillset>();
                CreateMap<HobbyDto, Hobby>();
                
        }
    }
}
