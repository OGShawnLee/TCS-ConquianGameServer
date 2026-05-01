using System;
using System.Collections.Generic;

namespace ConquiánServidor.ConquiánDB;

public partial class LevelRule
{
    public int LevelNumber { get; set; }

    public string RankName { get; set; }

    public int MinPointsReward { get; set; }

    public int MaxPointsReward { get; set; }

    public int PointsRequired { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
