/// <summary>Avalia condições de missão com base no estado da sessão (sem efeitos laterais).</summary>
public static class ContiGoMissionEvaluator
{
    public static bool IsHitPhase (ContiGoMissionKind kind)
    {
        switch (kind) {
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
