/// <summary>Resultado de desbloqueio para UI (popup / som).</summary>
public readonly struct ContiGoUnlockEvent
{
    public readonly string MissionId;
    public readonly int CardId;
    public readonly string FantasyName;

    public ContiGoUnlockEvent (string missionId, int cardId, string fantasyName)
    {
        MissionId = missionId;
        CardId = cardId;
        FantasyName = fantasyName;
    }
}
