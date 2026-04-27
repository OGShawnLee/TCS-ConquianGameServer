using System.ServiceModel;
using CoreWCF;

namespace ConquiánServidor.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IInvitationCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnInvitationReceived(string senderNickname, string roomCode);
    }
}