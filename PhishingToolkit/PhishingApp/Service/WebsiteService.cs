using PhishingApp.Database;
using PhishingApp.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PhishingApp.Service
{
    public class WebsiteService
    {
        private readonly PhishingAppDbContext _dbContext;

        public WebsiteService(PhishingAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task IncrementEmailsSent(int id, int emailsSent)
        {
            var website = await _dbContext.Websites.FirstAsync(w => w.Id == id);
            await _dbContext.Database.ExecuteSqlCommandAsync($"UPDATE Websites SET EmailsSent = EmailsSent + {emailsSent} WHERE Id = 1");
            await _dbContext.SaveChangesAsync();
        }

        public async Task IncrementFormsFilled(int id)
        {
            var website = await _dbContext.Websites.FirstAsync(w => w.Id == id);
            await _dbContext.Database.ExecuteSqlCommandAsync("UPDATE Websites SET FormsFilled = FormsFilled + 1 WHERE Id = 1");
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Website> GetWebsiteById(int id)
        {
            return await _dbContext.Websites.FirstAsync(w => w.Id == id);
        }
    }
}
