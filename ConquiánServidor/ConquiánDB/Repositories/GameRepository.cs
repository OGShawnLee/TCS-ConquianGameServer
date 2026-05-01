using ConquiánServidor.ConquiánDB.Abstractions;

namespace ConquiánServidor.ConquiánDB.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ConquiánContext context;

        public GameRepository(ConquiánContext context)
        {
            this.context = context;
        }

        public async Task AddGameAsync(Game game)
        {
            context.Games.Add(game);
            await context.SaveChangesAsync();
        }
    }
}
