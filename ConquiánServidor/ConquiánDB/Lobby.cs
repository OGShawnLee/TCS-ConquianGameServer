using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class Lobby
{
    public int IdLobby { get; set; }

    public string RoomCode { get; set; }

    public int IdHostPlayer { get; set; }

    public int IdStatusLobby { get; set; }

    public DateTime CreationDate { get; set; }

    public int? IdGamemode { get; set; }

    public virtual Gamemode IdGamemodeNavigation { get; set; }

    public virtual Player IdHostPlayerNavigation { get; set; }

    public virtual StatusLobby IdStatusLobbyNavigation { get; set; }
}
