using ConquiánServidor.ConquiánDB.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ConquiánServidor.ConquiánDB.Repositories
{
    public class LobbyRepository : ILobbyRepository
    {
        private readonly ConquiánContext context;

        public LobbyRepository(ConquiánContext context)
        {
            this.context = context;
        }

        public void AddLobby(Lobby lobby)
        {
            context.Lobbies.Add(lobby);
        }

        public async Task<bool> DoesRoomCodeExistAsync(string roomCode)
        {
            return await context.Lobbies.AnyAsync(l => l.RoomCode == roomCode);
        }

        public async Task<Lobby> GetLobbyByRoomCodeAsync(string roomCode)
        {
            return await context.Lobbies
                .Include(l => l.IdHostPlayerNavigation)
                .Include(l => l.IdStatusLobbyNavigation)
                .Include(l => l.IdGamemodeNavigation)
                .FirstOrDefaultAsync(l => l.RoomCode == roomCode);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}