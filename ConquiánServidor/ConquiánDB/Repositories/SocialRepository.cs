using ConquiánServidor.ConquiánDB.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ConquiánServidor.ConquiánDB.Repositories
{
    public class SocialRepository : ISocialRepository
    {
        private readonly ConquiánContext context;

        public SocialRepository(ConquiánContext context)
        {
            this.context = context;
        }

        public async Task<bool> DoesPlayerExistAsync(int idPlayer)
        {
            return await context.Players.AnyAsync(p => p.IdPlayer == idPlayer);
        }

        public async Task<List<Social>> GetSocialsByPlayerIdAsync(int idPlayer)
        {
            return await context.Socials.AsNoTracking().Where(s => s.IdPlayer == idPlayer).ToListAsync();
        }

        public void AddSocial(Social social)
        {
            context.Socials.Add(social);
        }

        public void RemoveSocialsRange(IEnumerable<Social> socials)
        {
            context.Socials.RemoveRange(socials);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}