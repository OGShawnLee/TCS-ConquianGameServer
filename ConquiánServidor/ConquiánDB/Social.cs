using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class Social
{
    public int IdSocial { get; set; }

    public string UserLink { get; set; }

    public int? IdPlayer { get; set; }

    public int? IdSocialType { get; set; }

    public virtual Player IdPlayerNavigation { get; set; }

    public virtual SocialType IdSocialTypeNavigation { get; set; }
}
