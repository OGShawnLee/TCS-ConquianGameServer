using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class Game
{
    public int IdGame { get; set; }

    public int GameTime { get; set; }

    public DateTime? DatePlayed { get; set; }

    public int IdGamemode { get; set; }

    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

    public virtual Gamemode IdGamemodeNavigation { get; set; }
}
