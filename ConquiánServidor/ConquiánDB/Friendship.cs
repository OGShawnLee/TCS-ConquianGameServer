using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class Friendship
{
    public int IdFriendship { get; set; }

    public int? IdOrigen { get; set; }

    public int? IdDestino { get; set; }

    public int? IdStatus { get; set; }

    public virtual Player IdDestinoNavigation { get; set; }

    public virtual Player IdOrigenNavigation { get; set; }

    public virtual Status IdStatusNavigation { get; set; }
}
