/// <summary>Avalia condições de missão com base no estado da sessão (sem efeitos laterais).</summary>
public static class ContiGoMissionEvaluator
{
    static readonly HashSet<int> s_unlock2EvenMasterSet = new HashSet<int> {
        0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 48, 50, 54, 60, 64, 66, 72, 80, 90, 96, 100, 108, 120, 144, 150, 180
    };

    static bool ContainsAll (ContiGoMatchSessionState s, HashSet<int> required)
    {
        if (required == null || required.Count == 0)
            return true;
        foreach (int v in required) {
            if (!s.HitNumber (v))
                return false;
        }
        return true;
    }

    public static bool EvaluateMatchStart (ContiGoMissionDefinition def, ContiGoMatchSessionState s)
    {
        switch (def.Kind) {
        case ContiGoMissionKind.MatchStart:
            return true;
        default:
            return false;
        }
    }

    public static bool IsHitPhase (ContiGoMissionKind kind)
    {
        switch (kind) {
        case ContiGoMissionKind.MatchStart:
        case ContiGoMissionKind.VictoryCompleteBoard:
        case ContiGoMissionKind.VictoryOnLevel:
        case ContiGoMissionKind.VictoryWithErrorsAtMost:
        case ContiGoMissionKind.VictoryWithLivesAtLeast:
        case ContiGoMissionKind.VictoryWithTimerRemainingAtLeast:
            return false;
        default:
            return true;
        }
    }

    public static bool EvaluateHit (
        ContiGoMissionDefinition def,
        ContiGoMatchSessionState s,
        int lastHitValue,
        int errorsBeforeThisHit)
    {
        float t = s.TimeSinceMatchStart ();
        switch (def.Kind) {
        case ContiGoMissionKind.HitValueSetWithinSecondsOnLevel:
            // Param1: (int)ContiGo2DLevelId, Param2: seconds
            if ((int)s.LevelId != def.Param1)
                return false;
            if (t > def.Param2 + 0.001f)
                return false;
            // Conjunto de valores definido por missão (Id).
            if (def.Id == "unlock_2_even_master_240") {
                if (!s_unlock2EvenMasterSet.Contains (lastHitValue))
                    return false;
                return ContainsAll (s, s_unlock2EvenMasterSet);
            }
            return false;

        case ContiGoMissionKind.HitNumberWithinSecondsFromMatchStart:
            return lastHitValue == def.Param1 && t <= def.Param2 + 0.001f;

        case ContiGoMissionKind.HitNumber:
            return lastHitValue == def.Param1;

        case ContiGoMissionKind.HitNumberAfterSecondsFromMatchStart:
            return lastHitValue == def.Param1 && t >= def.Param2 - 0.001f;

        case ContiGoMissionKind.TotalCorrectHitsAtLeast:
            return s.Hits.Count >= def.Param1;

        case ContiGoMissionKind.ConsecutiveHitsAtLeast:
            return s.ConsecutiveHits >= def.Param1;

        case ContiGoMissionKind.HitNumberAfterAtLeastOneError:
            return lastHitValue == def.Param1 && errorsBeforeThisHit >= 1;

        default:
            return false;
        }
    }

    public static bool EvaluateVictory (
        ContiGoMissionDefinition def,
        ContiGoMatchSessionState s,
        float mainTimerRemaining,
        int livesRemaining)
    {
        if (!s.CompletedBoard)
            return false;

        switch (def.Kind) {
        case ContiGoMissionKind.VictoryCompleteBoard:
            return true;

        case ContiGoMissionKind.VictoryOnLevel:
            return (int)s.LevelId == def.Param1;

        case ContiGoMissionKind.VictoryWithErrorsAtMost:
            return s.Errors <= def.Param1;

        case ContiGoMissionKind.VictoryWithLivesAtLeast:
            return livesRemaining >= def.Param1;

        case ContiGoMissionKind.VictoryWithTimerRemainingAtLeast:
            return mainTimerRemaining >= def.Param1 - 0.001f;

        default:
            return false;
        }
    }
}
