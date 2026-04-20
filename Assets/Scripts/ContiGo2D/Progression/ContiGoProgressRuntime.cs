using System.Collections.Generic;

/// <summary>Motor de missões + desbloqueio de cartas (liga catálogo, avaliador e persistência).</summary>
public static class ContiGoProgressRuntime
{
    static HashSet<int> s_unlockedCards;
    static HashSet<string> s_doneMissions;
    static bool s_loaded;

    static void EnsureLoaded ()
    {
        if (s_loaded)
            return;
        s_unlockedCards = ContiGoProgressPersistence.LoadUnlockedCards ();
        s_doneMissions = ContiGoProgressPersistence.LoadCompletedMissions ();
        s_loaded = true;
    }

    public static void ReloadFromDisk ()
    {
        s_loaded = false;
        EnsureLoaded ();
    }

    public static bool IsMissionCompleted (string missionId)
    {
        EnsureLoaded ();
        return s_doneMissions.Contains (missionId);
    }

    public static bool IsCardUnlocked (int cardId)
    {
        EnsureLoaded ();
        return s_unlockedCards.Contains (cardId);
    }

    public static IReadOnlyCollection<int> UnlockedCardIds ()
    {
        EnsureLoaded ();
        return s_unlockedCards;
    }

    /// <summary>Chamar após cada acerto (já com o hit registado em <paramref name="session"/>).</summary>
    /// <param name="errorsBeforeHit">Valor de <see cref="ContiGoMatchSessionState.Errors"/> antes deste acerto.</param>
    public static List<ContiGoUnlockEvent> ProcessAfterHit (
        ContiGoMatchSessionState session,
        int lastHitValue,
        int errorsBeforeHit)
    {
        EnsureLoaded ();
        var events = new List<ContiGoUnlockEvent> ();
        bool changed = false;

        foreach (ContiGoMissionDefinition def in ContiGoMissionsCatalog.All) {
            if (s_doneMissions.Contains (def.Id))
                continue;
            if (!ContiGoMissionEvaluator.IsHitPhase (def.Kind))
                continue;
            if (!ContiGoMissionEvaluator.EvaluateHit (def, session, lastHitValue, errorsBeforeHit))
                continue;
            TryCompleteMission (def, ref changed, events);
        }

        if (changed)
            ContiGoProgressPersistence.Save (s_unlockedCards, s_doneMissions);
        return events;
    }

    /// <summary>Chamar quando o jogador inicia uma partida (primeiro "Play").</summary>
    public static List<ContiGoUnlockEvent> ProcessAfterMatchStart (ContiGoMatchSessionState session)
    {
        EnsureLoaded ();
        var events = new List<ContiGoUnlockEvent> ();
        bool changed = false;

        foreach (ContiGoMissionDefinition def in ContiGoMissionsCatalog.All) {
            if (s_doneMissions.Contains (def.Id))
                continue;
            if (def.Kind != ContiGoMissionKind.MatchStart)
                continue;
            if (!ContiGoMissionEvaluator.EvaluateMatchStart (def, session))
                continue;
            TryCompleteMission (def, ref changed, events);
        }

        if (changed)
            ContiGoProgressPersistence.Save (s_unlockedCards, s_doneMissions);
        return events;
    }

    /// <summary>Chamar na vitória (tabuleiro completo), antes ou depois do ecrã de vitória.</summary>
    public static List<ContiGoUnlockEvent> ProcessAfterVictory (
        ContiGoMatchSessionState session,
        float mainTimerRemaining,
        int livesRemaining)
    {
        EnsureLoaded ();
        var events = new List<ContiGoUnlockEvent> ();
        bool changed = false;

        foreach (ContiGoMissionDefinition def in ContiGoMissionsCatalog.All) {
            if (s_doneMissions.Contains (def.Id))
                continue;
            if (ContiGoMissionEvaluator.IsHitPhase (def.Kind))
                continue;
            if (!ContiGoMissionEvaluator.EvaluateVictory (def, session, mainTimerRemaining, livesRemaining))
                continue;
            TryCompleteMission (def, ref changed, events);
        }

        if (changed)
            ContiGoProgressPersistence.Save (s_unlockedCards, s_doneMissions);
        return events;
    }

    static void TryCompleteMission (ContiGoMissionDefinition def, ref bool changed, List<ContiGoUnlockEvent> events)
    {
        if (s_doneMissions.Contains (def.Id))
            return;
        s_doneMissions.Add (def.Id);
        changed = true;

        bool wasUnlocked = s_unlockedCards.Contains (def.TargetCardId);
        if (!wasUnlocked)
            s_unlockedCards.Add (def.TargetCardId);

        if (!wasUnlocked) {
            string name = ContiGoFantasyNames.GetFantasyName (def.TargetCardId);
            events.Add (new ContiGoUnlockEvent (def.Id, def.TargetCardId, name));
        }
    }
}
