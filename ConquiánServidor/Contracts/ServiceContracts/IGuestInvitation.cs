using ConquiánServidor.Contracts.DataContracts;
using CoreWCF;

namespace ConquiánServidor.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IGuestInvitation
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFaultDto))]
        Task SendGuestInviteAsync(string roomCode, string email);
    }
}