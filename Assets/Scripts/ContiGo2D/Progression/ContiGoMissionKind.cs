/// <summary>Tipos de condição de missão (hit vs vitória). Fácil de estender com novos valores.</summary>
public enum ContiGoMissionKind
{
    /// <summary>Iniciar uma partida (evento único quando o jogo entra em "Play").</summary>
    MatchStart,

    /// <summary>Acertar <see cref="ContiGoMissionDefinition.Param1"/> com tempo desde o início da partida ≤ Param2 (segundos).</summary>
    HitNumberWithinSecondsFromMatchStart,

    /// <summary>Acertar o valor Param1 (pelo menos uma vez na sessão).</summary>
    HitNumber,

    /// <summary>Acertar Param1 com tempo desde o início ≥ Param2 (segundos).</summary>
    HitNumberAfterSecondsFromMatchStart,

    /// <summary>Pelo menos Param1 acertos corretos na mesma partida.</summary>
    TotalCorrectHitsAtLeast,

    /// <summary>Sequência atual (ou melhor) ≥ Param1 acertos seguidos.</summary>
    ConsecutiveHitsAtLeast,

    /// <summary>Vencer a partida (tabuleiro completo), qualquer nível.</summary>
    VictoryCompleteBoard,

    /// <summary>Vencer no nível indicado em <see cref="ContiGoMissionDefinition.Param1"/> (valor de <see cref="ContiGo2DLevelId"/>).</summary>
    VictoryOnLevel,

    /// <summary>Vencer com ≤ Param1 erros na sessão.</summary>
    VictoryWithErrorsAtMost,

    /// <summary>Vencer com ≥ Param1 vidas restantes.</summary>
    VictoryWithLivesAtLeast,

    /// <summary>Vencer com tempo principal restante ≥ Param1 (segundos).</summary>
    VictoryWithTimerRemainingAtLeast,

    /// <summary>Acertar Param1 depois de pelo menos um erro nesta partida.</summary>
    HitNumberAfterAtLeastOneError,
}
