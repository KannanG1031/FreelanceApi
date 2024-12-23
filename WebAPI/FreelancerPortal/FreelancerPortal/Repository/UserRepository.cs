using AutoMapper;
using FreelancerPortal.Data;
using FreelancerPortal.DTO;
using FreelancerPortal.Models;
using FreelancerPortal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FreelancerPortal.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext dbContext;
        //private readonly IMapper _mapper;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext appDbContext, ILogger<UserRepository> logger)
        {
            dbContext = appDbContext;
            //_mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all freelancers users from database.");
            return await dbContext.Users
                .Include(u => u.Skillsets)
                .Include(u => u.Hobbies)
                .ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching freelancer user with ID: {id} from database.");
            return await dbContext.Users
                .Include(u => u.Skillsets)
                .Include(u => u.Hobbies)
                .FirstOrDefaultAsync(u => u.Id == id);


        }

        public async Task AddUserAsync(User user)
        {
            _logger.LogInformation("Adding a new freelancer user to database.");
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            _logger.LogInformation($"Updating freelancer user with ID: {user.Id} in database.");
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            _logger.LogWarning($"Deleting freelancer user with ID: {id} from database.");
            var user = await dbContext.Users.FindAsync(id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task ArchiveFreelancerAsync(int id)
        {
            var archiveUser = await dbContext.Users.FindAsync(id);
            if (archiveUser != null)
            {
                _logger.LogInformation($"Archiving freelancer user with ID: {id} in database.");
                archiveUser.IsArchived = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UnarchiveFreelancerAsync(int id)
        {
            var unarchiveUser = await dbContext.Users.FindAsync(id);
            if (unarchiveUser != null)
            {
                _logger.LogInformation($"Unarchiving freelancer user with ID: {id} in database.");
                unarchiveUser.IsArchived = false;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> SearchFreelancersAsync(string query)
        {
            _logger.LogInformation($"Searching freelancer user in database with query: {query}");
            //return await dbContext.Users.Where(f => f.Username.Contains(query) || f.Email.Contains(query)).ToListAsync();
            
            return await dbContext.Users.FromSqlInterpolated($"sp_SearchFreelancers {query}").ToListAsync();

        }

    }
}
