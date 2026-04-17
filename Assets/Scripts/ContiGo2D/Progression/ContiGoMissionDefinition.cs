/// <summary>Definição estática de uma missão (sem lambdas — avaliação por <see cref="ContiGoMissionEvaluator"/>).</summary>
public readonly struct ContiGoMissionDefinition
{
    public readonly string Id;
    public readonly string DescriptionPt;
    public readonly string DescriptionEn;
    public readonly int TargetCardId;
    public readonly ContiGoMissionKind Kind;
    /// <summary>Significado depende de <see cref="Kind"/> (número-alvo, limite de tempo, contagem, ou (int)<see cref="ContiGo2DLevelId"/>).</summary>
    public readonly int Param1;
    public readonly int Param2;

    public ContiGoMissionDefinition (
        string id,
        string descriptionPt,
        string descriptionEn,
        int targetCardId,
        ContiGoMissionKind kind,
        int param1 = 0,
        int param2 = 0)
    {
        Id = id;
        DescriptionPt = descriptionPt;
        DescriptionEn = descriptionEn;
        TargetCardId = targetCardId;
        Kind = kind;
        Param1 = param1;
        Param2 = param2;
    }
}
