using CoreWCF;

namespace ConquiánServidor.Contracts.ServiceContracts
{
    [ServiceContract(CallbackContract = typeof(IPresenceCallback))]
    public interface IPresence
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe(int idPlayer);

        [OperationContract(IsOneWay = true)]
        void Unsubscribe(int idPlayer);
    }
}
