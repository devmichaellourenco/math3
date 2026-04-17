using System.Collections.Generic;
using UnityEngine;

/// <summary>Estado de uma partida para missões (não altera regras do jogo). Relógio inicia em <see cref="OnPlayStarted"/>.</summary>
public sealed class ContiGoMatchSessionState
{
    public float MatchStartUnscaledTime { get; private set; }
    public readonly List<int> Hits = new List<int> (64);
    public int Errors { get; private set; }
    public int ConsecutiveHits { get; private set; }
    public int BestConsecutiveHits { get; private set; }
    public bool CompletedBoard { get; private set; }
    public ContiGo2DLevelId LevelId { get; private set; }

    public void ResetForNewBoard (ContiGo2DLevelId level)
    {
        LevelId = level;
        Hits.Clear ();
        Errors = 0;
        ConsecutiveHits = 0;
        BestConsecutiveHits = 0;
        CompletedBoard = false;
        MatchStartUnscaledTime = 0f;
    }

    public void OnPlayStarted ()
    {
        MatchStartUnscaledTime = Time.unscaledTime;
    }

    public float TimeSinceMatchStart ()
    {
        if (MatchStartUnscaledTime <= 0f)
            return 0f;
        return Mathf.Max (0f, Time.unscaledTime - MatchStartUnscaledTime);
    }

    public bool HitNumber (int n)
    {
        return Hits.Contains (n);
    }

    public void RegisterCorrectHit (int value)
    {
        Hits.Add (value);
        ConsecutiveHits++;
        if (ConsecutiveHits > BestConsecutiveHits)
            BestConsecutiveHits = ConsecutiveHits;
    }

    public void RegisterError ()
    {
        Errors++;
        ConsecutiveHits = 0;
    }

    public void MarkVictory ()
    {
        CompletedBoard = true;
    }
}
