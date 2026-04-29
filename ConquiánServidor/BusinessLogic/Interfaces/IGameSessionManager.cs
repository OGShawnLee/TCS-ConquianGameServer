using ConquiánServidor.BusinessLogic.Game;
using ConquiánServidor.Contracts.DataContracts;

namespace ConquiánServidor.BusinessLogic.Interfaces
{
    public interface IGameSessionManager
    {
        void CreateGame(string roomCode, int gamemodeId, List<PlayerDto> players);
        GameLogic GetGame(string roomCode);
        void RemoveGame(string roomCode);
        void CheckAndClearActiveSessions(int playerId);
    }
}