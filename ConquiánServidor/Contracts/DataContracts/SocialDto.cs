using System.Runtime.Serialization;

namespace ConquiánServidor.Contracts.DataContracts
{
    [DataContract]
    public class SocialDto
    {
        [DataMember]
        public int IdSocial { get; set; }

        [DataMember]
        public int IdSocialType { get; set; }

        [DataMember]
        public string UserLink { get; set; }

    }
}
