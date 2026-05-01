using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class Status
{
    public int IdStatus { get; set; }

    public string Status1 { get; set; }

    public virtual ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();
}
