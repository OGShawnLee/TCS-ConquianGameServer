using Microsoft.EntityFrameworkCore;
using ConquiánServidor.ConquiánDB.Abstractions;

namespace ConquiánServidor.ConquiánDB.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private const int FRIENDSHIP_STATUS_ACCEPTED = 1;
        private const int FRIENDSHIP_STATUS_PENDING = 3;

        private readonly ConquiánContext context;

        public FriendshipRepository(ConquiánContext context)
        {
            this.context = context;
        }

        public async Task<List<Player>> GetFriendsAsync(int idPlayer)
        {
            var friends = await context.Friendships
                .Where(f => (f.IdOrigen == idPlayer || f.IdDestino == idPlayer)
                         && f.IdStatus == FRIENDSHIP_STATUS_ACCEPTED)
                .Select(f => f.IdOrigen == idPlayer ? f.IdDestinoNavigation : f.IdOrigenNavigation)
                .ToListAsync();

            return friends;
        }

        public async Task<Friendship> GetPendingRequestByIdAsync(int idFriendship)
        {
            var pendingRequest = await context.Friendships
                .FirstOrDefaultAsync(f => f.IdFriendship == idFriendship
                                       && f.IdStatus == FRIENDSHIP_STATUS_PENDING);

            return pendingRequest;
        }

        public async Task<List<Friendship>> GetFriendRequestsAsync(int idPlayer)
        {
            var friendRequests = await context.Friendships
                .Where(f => f.IdDestino == idPlayer
                         && f.IdStatus == FRIENDSHIP_STATUS_PENDING)
                .Include(f => f.IdOrigenNavigation)
                .ToListAsync();

            return friendRequests;
        }

        public async Task<Friendship> GetExistingRelationshipAsync(int idPlayer, int idFriend)
        {
            var existingRelationship = await context.Friendships
                .FirstOrDefaultAsync(f => (f.IdOrigen == idPlayer && f.IdDestino == idFriend)
                                       || (f.IdOrigen == idFriend && f.IdDestino == idPlayer));

            return existingRelationship;
        }

        public async Task<Friendship> GetPendingRequestAsync(int receiverId, int senderId)
        {
            var pendingRequest = await context.Friendships
                .FirstOrDefaultAsync(f => f.IdOrigen == receiverId
                                       && f.IdDestino == senderId
                                       && f.IdStatus == FRIENDSHIP_STATUS_PENDING);

            return pendingRequest;
        }

        public async Task<Friendship> GetAcceptedFriendshipAsync(int idPlayer, int idFriend)
        {
            var acceptedFriendship = await context.Friendships
                .FirstOrDefaultAsync(f => ((f.IdOrigen == idPlayer && f.IdDestino == idFriend)
                                        || (f.IdOrigen == idFriend && f.IdDestino == idPlayer))
                                        && f.IdStatus == FRIENDSHIP_STATUS_ACCEPTED);

            return acceptedFriendship;
        }

        public void AddFriendship(Friendship friendship)
        {
            context.Friendships.Add(friendship);
        }

        public void RemoveFriendship(Friendship friendship)
        {
            context.Friendships.Remove(friendship);
        }

        public async Task<int> SaveChangesAsync()
        {
            int changesSaved = await context.SaveChangesAsync();
            return changesSaved;
        }
    }
}