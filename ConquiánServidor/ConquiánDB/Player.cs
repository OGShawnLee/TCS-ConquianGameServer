using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class Player
{
    public int IdPlayer { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int CurrentPoints { get; set; }

    public string PathPhoto { get; set; }

    public string VerificationCode { get; set; }

    public DateTime? CodeExpiryDate { get; set; }

    public int IdLevel { get; set; }

    public virtual ICollection<Friendship> FriendshipIdDestinoNavigations { get; set; } = new List<Friendship>();

    public virtual ICollection<Friendship> FriendshipIdOrigenNavigations { get; set; } = new List<Friendship>();

    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

    public virtual LevelRule IdLevelNavigation { get; set; }

    public virtual ICollection<Lobby> Lobbies { get; set; } = new List<Lobby>();

    public virtual ICollection<Social> Socials { get; set; } = new List<Social>();
}
