using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class Gamemode
{
    public int IdGamemode { get; set; }

    public string Gamemode1 { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Lobby> Lobbies { get; set; } = new List<Lobby>();
}
