using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class SocialType
{
    public int IdSocialType { get; set; }

    public string Type { get; set; }

    public virtual ICollection<Social> Socials { get; set; } = new List<Social>();
}
