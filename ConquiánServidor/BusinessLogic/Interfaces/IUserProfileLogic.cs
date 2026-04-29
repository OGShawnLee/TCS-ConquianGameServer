using ConquiánServidor.Contracts.DataContracts;

namespace ConquiánServidor.BusinessLogic.Interfaces
{
    public interface IUserProfileLogic
    {
        Task<PlayerDto> GetPlayerByIdAsync(int idPlayer);
        Task<List<SocialDto>> GetPlayerSocialsAsync(int idPlayer);
        Task UpdatePlayerAsync(PlayerDto playerDto);
        Task UpdatePlayerSocialsAsync(int idPlayer, List<SocialDto> socialDtos);
        Task UpdateProfilePictureAsync(int idPlayer, string newPath);
        Task<List<GameHistoryDto>> GetPlayerGameHistoryAsync(int idPlayer);
    }
}