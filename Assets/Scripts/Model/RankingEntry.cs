using System;
using UnityEngine;

[Serializable]
public class RankingEntry
{
    public int points;
    /// <summary>
    /// Tempo gasto no timer principal (segundos). Menor = mais rápido.
    /// </summary>
    public int timeSpentSeconds;
    // Campo legado (versões anteriores desta sessão gravaram tempo restante).
    public int timeRemainingSeconds;

    public RankingEntry () { }

    public RankingEntry (int points, int timeSpentSeconds)
    {
        this.points = points;
        this.timeSpentSeconds = timeSpentSeconds;
    }

    public static int Compare (RankingEntry a, RankingEntry b)
    {
        if (ReferenceEquals (a, b))
            return 0;
        if (a == null)
            return 1; // nulls por último
        if (b == null)
            return -1;
        // Mais pontos primeiro; em empate, menor tempo gasto (mais rápido) primeiro.
        int p = b.points.CompareTo (a.points);
        if (p != 0)
            return p;
        return a.timeSpentSeconds.CompareTo (b.timeSpentSeconds);
    }
}

