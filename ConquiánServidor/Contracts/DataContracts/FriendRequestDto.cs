using System.Runtime.Serialization;

namespace ConquiánServidor.Contracts.DataContracts
{
    [DataContract]
    public class FriendRequestDto
    {
        [DataMember]
        public int IdFriendship { get; set; }
        [DataMember]
        public string Nickname { get; set; }
    }
}
