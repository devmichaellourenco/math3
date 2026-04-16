using System;
using System.Collections.Generic;

/// <summary>
/// Definições dos níveis 2x2 … 8x8. O Mestre (8x8) usa os 64 valores já balanceados do jogo original.
/// </summary>
public static class ContiGo2DLevelCatalog
{
    /// <summary>64 valores do tabuleiro mestre (mesma lista que existia em ContiGoMatch2D).</summary>
    public static readonly int[] MasterBoardValues64 = {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 44, 45, 48, 50, 54, 55, 60, 64, 66, 72, 75, 80, 90, 96, 100, 108, 120, 125, 144, 150, 180
    };

    public static int GridSide (ContiGo2DLevelId id)
    {
        switch (id) {
        case ContiGo2DLevelId.Iniciante:
            return 2;
        case ContiGo2DLevelId.Profissional:
            return 4;
        case ContiGo2DLevelId.Sabio:
            return 6;
        case ContiGo2DLevelId.Mestre:
            return 8;
        default:
            return 8;
        }
    }

    public static int CellCount (ContiGo2DLevelId id)
    {
        int g = GridSide (id);
        return g * g;
    }

    /// <summary>Tempo principal em segundos para concluir o desafio (ajustável por nível).</summary>
    public static int MainTimeSeconds (ContiGo2DLevelId id)
    {
        switch (id) {
        case ContiGo2DLevelId.Iniciante:
            return 75;
        case ContiGo2DLevelId.Profissional:
            return 150;
        case ContiGo2DLevelId.Sabio:
            return 240;
        case ContiGo2DLevelId.Mestre:
            return 300;
        default:
            return 300;
        }
    }

    /// <summary>Valores das casas (sem embaralhar). Tamanho = GridSide².</summary>
    public static int[] GetBoardValues (ContiGo2DLevelId id)
    {
        int n = CellCount (id);
        if (n > MasterBoardValues64.Length)
            n = MasterBoardValues64.Length;
        int[] v = new int[n];
        Array.Copy (MasterBoardValues64, v, n);
        return v;
    }

    public static string SceneFileName (ContiGo2DLevelId id)
    {
        switch (id) {
        case ContiGo2DLevelId.Iniciante:
            return "GamePlay2D_Iniciante";
        case ContiGo2DLevelId.Profissional:
            return "GamePlay2D_Profissional";
        case ContiGo2DLevelId.Sabio:
            return "GamePlay2D_Sabio";
        case ContiGo2DLevelId.Mestre:
        default:
            return "GamePlay2D";
        }
    }

    public static List<int> ShuffleCopy (IReadOnlyList<int> src)
    {
        var list = new List<int> (src);
        var rng = new System.Random ();
        for (int i = list.Count - 1; i > 0; i--) {
            int j = rng.Next (i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
}
