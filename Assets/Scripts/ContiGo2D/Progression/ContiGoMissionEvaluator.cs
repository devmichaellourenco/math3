using System.Collections.Generic;

/// <summary>Avalia condições de missão com base no estado da sessão (sem efeitos laterais).</summary>
public static class ContiGoMissionEvaluator
{
    static readonly HashSet<int> s_unlock2EvenMasterSet = new HashSet<int> {
        0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 48, 50, 54, 60, 64, 66, 72, 80, 90, 96, 100, 108, 120, 144, 150, 180
    };

    static readonly HashSet<int> s_unlock3Prime3Set = new HashSet<int> {
        8, 12, 18, 27, 45, 50, 75, 125
    };

    static readonly HashSet<int> s_unlock5VowelPositionsSet = new HashSet<int> {
        1, 5, 9, 15, 21
    };

    static readonly HashSet<int> s_unlock6MultiplesOf6Set = new HashSet<int> {
        6, 12, 18, 24, 30, 36, 42, 48, 54, 60, 66, 72, 90, 96, 108, 120, 144, 150, 180
    };

    static readonly HashSet<int> s_unlock7Make7Set = new HashSet<int> {
        7, 16, 17, 18, 25, 29, 34, 43, 72, 108, 125, 144, 180
    };

    // Múltiplos de 9 presentes no tabuleiro Mestre, sem incluir o 180 (que deve ser o último).
    static readonly HashSet<int> s_unlock180MultiplesOf9Without180 = new HashSet<int> {
        0, 9, 18, 27, 36, 45, 54, 72, 90, 96, 108, 144
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
            if (def.Param1 >= 0 && (int)s.LevelId != def.Param1)
                return false;
            if (t > def.Param2 + 0.001f)
                return false;
            // Conjunto de valores definido por missão (Id).
            if (def.Id == "unlock_2_even_master_240") {
                if (!s_unlock2EvenMasterSet.Contains (lastHitValue))
                    return false;
                return ContainsAll (s, s_unlock2EvenMasterSet);
            }
            if (def.Id == "unlock_3_prime3_master_125") {
                if (!s_unlock3Prime3Set.Contains (lastHitValue))
                    return false;
                return ContainsAll (s, s_unlock3Prime3Set);
            }
            if (def.Id == "unlock_5_vowels_25") {
                if (!s_unlock5VowelPositionsSet.Contains (lastHitValue))
                    return false;
                return ContainsAll (s, s_unlock5VowelPositionsSet);
            }
            if (def.Id == "unlock_6_mult6_master_300") {
                if (!s_unlock6MultiplesOf6Set.Contains (lastHitValue))
                    return false;
                return ContainsAll (s, s_unlock6MultiplesOf6Set);
            }
            if (def.Id == "unlock_7_make7_master_300") {
                if (!s_unlock7Make7Set.Contains (lastHitValue))
                    return false;
                return ContainsAll (s, s_unlock7Make7Set);
            }
            if (def.Id == "unlock_180_master_mult9_last_180") {
                // 180 deve ser o último acerto que completa a missão.
                if (lastHitValue != 180)
                    return false;
                return ContainsAll (s, s_unlock180MultiplesOf9Without180);
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
