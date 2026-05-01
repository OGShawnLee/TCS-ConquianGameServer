using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class StatusLobby
{
    public int IdStatusLobby { get; set; }

    public string StatusName { get; set; }

    public virtual ICollection<Lobby> Lobbies { get; set; } = new List<Lobby>();
}
