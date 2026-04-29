using System.Runtime.Serialization;

namespace ConquiánServidor.Contracts.DataContracts
{
    [DataContract]
    public class LobbyDto
    {
        [DataMember]
        public string RoomCode { get; set; }

        [DataMember]
        public int idHostPlayer { get; set; }

        [DataMember]
        public List<PlayerDto> Players { get; set; }

        [DataMember]
        public string GameMode { get; set; }

        [DataMember]
        public string StatusLobby { get; set; }

        [DataMember]
        public List<MessageDto> ChatMessages { get; set; }

        [DataMember]
        public int? idGamemode { get; set; }
    }
}
