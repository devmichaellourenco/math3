using System.Collections.Generic;

/// <summary>Catálogo inicial de missões (20). Adicionar entradas aqui mantém o resto desacoplado.</summary>
public static class ContiGoMissionsCatalog
{
    static readonly ContiGoMissionDefinition[] s_all = Build ();

    public static IReadOnlyList<ContiGoMissionDefinition> All => s_all;

    static ContiGoMissionDefinition[] Build ()
    {
        int L (ContiGo2DLevelId id) => (int)id;

        return new[] {
            new ContiGoMissionDefinition (
                "unlock_150_fast",
                "Marque 150 nos primeiros 10 s após o início da partida.",
                "Mark 150 within the first 10 seconds after the match starts.",
                150,
                ContiGoMissionKind.HitNumberWithinSecondsFromMatchStart,
                150,
                10),

            new ContiGoMissionDefinition (
                "unlock_0_complete_board",
                "Vença no nível Mestre (8×8).",
                "Win on Master (8×8).",
                0,
                ContiGoMissionKind.VictoryOnLevel,
                L (ContiGo2DLevelId.Mestre)),

            new ContiGoMissionDefinition (
                "unlock_8_quick",
                "Marque 8 nos primeiros 15 s de jogo.",
                "Mark 8 within the first 15 seconds.",
                8,
                ContiGoMissionKind.HitNumberWithinSecondsFromMatchStart,
                8,
                15),

            new ContiGoMissionDefinition (
                "unlock_streak_5",
                "Consiga 5 acertos seguidos na mesma partida.",
                "Get 5 consecutive correct marks in one match.",
                25,
                ContiGoMissionKind.ConsecutiveHitsAtLeast,
                5),

            new ContiGoMissionDefinition (
                "unlock_hits_10",
                "Faça 10 jogadas certas na mesma partida.",
                "Make 10 correct moves in one match.",
                10,
                ContiGoMissionKind.TotalCorrectHitsAtLeast,
                10),

            new ContiGoMissionDefinition (
                "unlock_perfect_run",
                "Vença sem errar nenhuma vez (0 erros).",
                "Win with zero mistakes.",
                144,
                ContiGoMissionKind.VictoryWithErrorsAtMost,
                0),

            new ContiGoMissionDefinition (
                "unlock_lives_2_end",
                "Termine uma partida com pelo menos 2 vidas.",
                "Finish a match with at least 2 lives left.",
                50,
                ContiGoMissionKind.VictoryWithLivesAtLeast,
                2),

            new ContiGoMissionDefinition (
                "unlock_72_slow",
                "Marque 72 após pelo menos 45 s desde o início.",
                "Mark 72 at least 45 seconds after the match starts.",
                72,
                ContiGoMissionKind.HitNumberAfterSecondsFromMatchStart,
                72,
                45),

            new ContiGoMissionDefinition (
                "unlock_level_beginner",
                "Vença no nível Iniciante (2×2).",
                "Win on Beginner (2×2).",
                3,
                ContiGoMissionKind.VictoryOnLevel,
                L (ContiGo2DLevelId.Iniciante)),

            new ContiGoMissionDefinition (
                "unlock_level_pro",
                "Vença no nível Profissional (4×4).",
                "Win on Professional (4×4).",
                16,
                ContiGoMissionKind.VictoryOnLevel,
                L (ContiGo2DLevelId.Profissional)),

            new ContiGoMissionDefinition (
                "unlock_level_sage",
                "Vença no nível Sábio (6×6).",
                "Win on Sage (6×6).",
                64,
                ContiGoMissionKind.VictoryOnLevel,
                L (ContiGo2DLevelId.Sabio)),

            new ContiGoMissionDefinition (
                "unlock_level_master",
                "Vença no nível Mestre (8×8).",
                "Win on Master (8×8).",
                180,
                ContiGoMissionKind.VictoryOnLevel,
                L (ContiGo2DLevelId.Mestre)),

            new ContiGoMissionDefinition (
                "unlock_hit_1",
                "Marque o valor 1.",
                "Mark the value 1.",
                1,
                ContiGoMissionKind.HitNumber,
                1),

            new ContiGoMissionDefinition (
                "unlock_timer_120_left",
                "Vença com pelo menos 120 s restantes no relógio principal.",
                "Win with at least 120 seconds left on the main timer.",
                125,
                ContiGoMissionKind.VictoryWithTimerRemainingAtLeast,
                120),

            new ContiGoMissionDefinition (
                "unlock_7_after_error",
                "Marque 7 depois de cometer pelo menos um erro.",
                "Mark 7 after at least one mistake.",
                7,
                ContiGoMissionKind.HitNumberAfterAtLeastOneError,
                7),

            new ContiGoMissionDefinition (
                "unlock_66",
                "Marque o valor 66.",
                "Mark the value 66.",
                66,
                ContiGoMissionKind.HitNumber,
                66),

            new ContiGoMissionDefinition (
                "unlock_hits_20",
                "Faça 20 acertos corretos na mesma partida.",
                "Make 20 correct marks in one match.",
                108,
                ContiGoMissionKind.TotalCorrectHitsAtLeast,
                20),

            new ContiGoMissionDefinition (
                "unlock_42",
                "Marque o valor 42.",
                "Mark the value 42.",
                42,
                ContiGoMissionKind.HitNumber,
                42),

            new ContiGoMissionDefinition (
                "unlock_streak_3",
                "Consiga 3 acertos seguidos.",
                "Get 3 consecutive correct marks.",
                55,
                ContiGoMissionKind.ConsecutiveHitsAtLeast,
                3),

            new ContiGoMissionDefinition (
                "unlock_96",
                "Marque o valor 96.",
                "Mark the value 96.",
                96,
                ContiGoMissionKind.HitNumber,
                96),
        };
    }
}
