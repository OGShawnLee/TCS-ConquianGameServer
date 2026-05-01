using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class GamePlayer
{
    public int IdGamePlayer { get; set; }

    public int IdGame { get; set; }

    public int IdPlayer { get; set; }

    public int Score { get; set; }

    public bool IsWinner { get; set; }

    public virtual Game IdGameNavigation { get; set; }

    public virtual Player IdPlayerNavigation { get; set; }
}
