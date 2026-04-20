using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nomes fantasia por valor de casa (CONTI GO). Lookup O(1) em memória; não altera regras nem matemática.
/// Campos extra (raridade, ícone, lore, desbloqueio) ficam preparados para evolução futura.
/// </summary>
public sealed class ContiGoFantasyData
{
    public string Name { get; }
    public string Gender { get; }
    /// <summary>Reservado: p.ex. "common", "rare".</summary>
    public string Rarity { get; }
    /// <summary>Reservado: id de recurso ou caminho de sprite (Resources).</summary>
    public string IconAssetId { get; }
    /// <summary>História do personagem (PT). Vazio = texto procedural em <see cref="ContiGoFantasyNames.GetCardLore"/>.</summary>
    public string LorePt { get; }
    /// <summary>História do personagem (EN).</summary>
    public string LoreEn { get; }
    /// <summary>Reservado: coleção / progressão.</summary>
    public bool Unlocked { get; }

    public ContiGoFantasyData (
        string name,
        string gender,
        string rarity = null,
        string iconAssetId = null,
        string lorePt = null,
        string loreEn = null,
        bool unlocked = true)
    {
        Name = name;
        Gender = gender;
        Rarity = rarity;
        IconAssetId = iconAssetId;
        LorePt = lorePt;
        LoreEn = loreEn;
        Unlocked = unlocked;
    }
}

public static class ContiGoFantasyNames
{
    public const bool USE_FANTASY_NAMES = true;

    /// <summary>
    /// Quando true, as células do tabuleiro mostram nome fantasia; por defeito false (apenas números, como no original).
    /// O catálogo <see cref="GetFantasyName"/> / <see cref="GetFantasyData"/> mantém-se sempre disponível.
    /// </summary>
    public const bool USE_FANTASY_NAMES_ON_BOARD = false;

    /// <summary>Retrato placeholder (832×1248) até haver arte por valor.</summary>
    public const string CardPortraitPlaceholderResource = "Imagens/Cartas/zeraphina";

    /// <summary>Retrato da carta 1 — Primarion.</summary>
    public const string CardPortrait1Resource = "Imagens/Cartas/primarion";

    /// <summary>Retrato da carta 2 — Duovelya.</summary>
    public const string CardPortrait2Resource = "Imagens/Cartas/duovelya";

    /// <summary>Retrato da carta 3 — Triandor.</summary>
    public const string CardPortrait3Resource = "Imagens/Cartas/triandor";

    /// <summary>Retrato da carta 4 — Quadrina.</summary>
    public const string CardPortrait4Resource = "Imagens/Cartas/quadrina";

    /// <summary>Retrato da carta 5 — Quintaros.</summary>
    public const string CardPortrait5Resource = "Imagens/Cartas/quintaros";

    /// <summary>Retrato da carta 6 — Hexalyn.</summary>
    public const string CardPortrait6Resource = "Imagens/Cartas/hexalyn";

    /// <summary>Retrato da carta 10 — decalya.</summary>
    public const string CardPortrait10Resource = "Imagens/Cartas/decalya";

    static readonly Dictionary<int, ContiGoFantasyData> s_map = BuildMap ();
    static readonly int[] s_sortedValueIds = CreateSortedValueIds ();

    static int[] CreateSortedValueIds ()
    {
        var list = new List<int> (s_map.Keys);
        list.Sort ();
        return list.ToArray ();
    }

    /// <summary>Todos os valores com nome fantasia (coleção / cartas), ordenados.</summary>
    public static IReadOnlyList<int> AllRegisteredValueIdsSorted => s_sortedValueIds;

    static Dictionary<int, ContiGoFantasyData> BuildMap ()
    {
        var d = new Dictionary<int, ContiGoFantasyData> (96);
        void Add (int key, string name, string gender, string lorePt = null, string loreEn = null, string iconResourcePath = null)
        {
            string icon = string.IsNullOrEmpty (iconResourcePath) ? CardPortraitPlaceholderResource : iconResourcePath;
            d[key] = new ContiGoFantasyData (name, gender, null, icon, lorePt, loreEn, true);
        }

        Add (
            0,
            "Zeraphina",
            "F",
            "No silêncio do vazio sereno, Zeraphina guarda o ponto onde tudo termina… e tudo pode começar novamente.\n\n" +
            "Zeraphina é uma entidade silenciosa e enigmática, cuja presença parece ao mesmo tempo existir e se desfazer no ar, " +
            "como se fosse o ponto de origem de tudo e o destino final de qualquer forma. Seu corpo é envolto por um halo circular " +
            "de energia suave, que pulsa com um vazio sereno, onde nada pesa e tudo pode nascer. Seus movimentos são leves, quase " +
            "imperceptíveis, e ao seu redor o espaço se curva sutilmente, como se obedecesse a uma lógica invisível. Ela não carrega " +
            "excesso nem falta — apenas um equilíbrio absoluto, onde toda força se anula e se renova. Dizem que, ao cruzar seu caminho, " +
            "até as maiores certezas se dissolvem, deixando apenas um estado puro de possibilidade, onde tudo pode começar outra vez.",
            "In the silence of the serene void, Zeraphina guards the point where everything ends… and everything can begin again.\n\n" +
            "Zeraphina is a silent, enigmatic entity whose presence seems to exist and dissolve into the air at the same time, " +
            "as if she were the point of origin of everything and the final destination of any form. Her body is wrapped in a circular " +
            "halo of gentle energy that pulses with a serene emptiness, where nothing weighs and anything may be born. Her movements are " +
            "light—almost imperceptible—and around her space subtly bends, as if obeying an invisible logic. She carries neither excess nor " +
            "lack—only an absolute balance, where every force cancels and renews itself. They say that, when you cross her path, even the " +
            "greatest certainties dissolve, leaving only a pure state of possibility, where everything can begin once more.");
        Add (1, "Primarion", "M", null, null, CardPortrait1Resource);
        Add (2, "Duovelya", "F", null, null, CardPortrait2Resource);
        Add (3, "Triandor", "M", null, null, CardPortrait3Resource);
        Add (4, "Quadrina", "F", null, null, CardPortrait4Resource);
        Add (5, "Quintaros", "M", null, null, CardPortrait5Resource);
        Add (6, "Hexalyn", "F", null, null, CardPortrait6Resource);
        Add (7, "Septurion", "M");
        Add (8, "Octavira", "F");
        Add (9, "Nontheris", "M");

        Add (10, "Decalya", "F", null, null, CardPortrait10Resource);
        Add (11, "Undecor", "M");
        Add (12, "Dodecira", "F");
        Add (13, "Trezalon", "M");
        Add (14, "Quattuora", "F");
        Add (15, "Quinzelor", "M");
        Add (16, "Sexdoria", "F");
        Add (17, "Septenor", "M");
        Add (18, "Octavelyn", "F");
        Add (19, "Noventhor", "M");

        Add (20, "Vigessia", "F");
        Add (21, "Vintarion", "M");
        Add (22, "Duovintia", "F");
        Add (23, "Trivintor", "M");
        Add (24, "Quadrivela", "F");
        Add (25, "Quintovar", "M");
        Add (26, "Hexavina", "F");
        Add (27, "Septavion", "M");
        Add (28, "Octalira", "F");
        Add (29, "Novarion", "M");

        Add (30, "Trizena", "F");
        Add (31, "Trionyx", "M");
        Add (32, "Duotrina", "F");
        Add (33, "Triatrix", "M");
        Add (34, "Quadrya", "F");
        Add (35, "Quinzor", "M");
        Add (36, "Hexatria", "F");
        Add (37, "Septorin", "M");
        Add (38, "Octaryn", "F");
        Add (39, "Novatrix", "M");

        Add (40, "Quadrinae", "F");
        Add (41, "Quadrion", "M");
        Add (42, "Duoquadria", "F");
        Add (44, "Quadrorix", "M");
        Add (45, "Quinquadra", "F");
        Add (48, "Octaquadrix", "M");
        Add (50, "Quintessara", "F");
        Add (54, "Quinhexor", "M");
        Add (55, "Quinquinara", "F");

        Add (60, "Hexagor", "M");
        Add (64, "Hexaquara", "F");
        Add (66, "Hexhexor", "M");
        Add (72, "Septadua", "F");
        Add (75, "Septaquor", "M");
        Add (80, "Octogena", "F");
        Add (90, "Novarix", "M");
        Add (96, "Novexa", "F");
        Add (100, "Centorion", "M");
        Add (108, "Centavira", "F");

        Add (120, "Centovintor", "M");
        Add (125, "Centoquinara", "F");
        Add (144, "Centoquadrix", "M");
        Add (150, "Centoquinella", "F");
        Add (180, "Centoctarion", "M");

        return d;
    }

    /// <summary>Lore para coleção / modal. Usa texto do catálogo se existir; senão variações procedurais por valor.</summary>
    public static string GetCardLore (int id, bool portuguese)
    {
        if (!s_map.TryGetValue (id, out ContiGoFantasyData data))
            return "";
        if (portuguese) {
            if (!string.IsNullOrEmpty (data.LorePt))
                return data.LorePt;
            return BuildProceduralLorePt (id, data.Name);
        }
        if (!string.IsNullOrEmpty (data.LoreEn))
            return data.LoreEn;
        return BuildProceduralLoreEn (id, data.Name);
    }

    /// <summary>Retrato da carta em <c>Resources</c> (<see cref="ContiGoFantasyData.IconAssetId"/>).</summary>
    public static Sprite TryLoadCardPortrait (int id)
    {
        if (!s_map.TryGetValue (id, out ContiGoFantasyData data))
            return null;
        if (string.IsNullOrEmpty (data.IconAssetId))
            return null;
        Sprite sp = Resources.Load<Sprite> (data.IconAssetId);
        if (sp != null)
            return sp;
        if (!data.IconAssetId.StartsWith ("Imagens/", StringComparison.Ordinal))
            return Resources.Load<Sprite> ("Imagens/" + data.IconAssetId);
        return null;
    }

    static string BuildProceduralLorePt (int v, string n)
    {
        switch (Mathf.Abs (v) % 8) {
        case 0:
            return n + " personifica o " + v + ": um eco quieto no tabuleiro — lembre que até o silêncio tem forma.";
        case 1:
            return "Nas crônicas de Conti Go, " + n + " surge quando alguém enxerga o " + v + " escondido entre sinais e atalhos.";
        case 2:
            return n + " não disputa atenção: acalma o " + v + " até ele parecer óbvio, como um nome que encaixa na boca.";
        case 3:
            return "Dizem que " + n + " carrega o " + v + " como emblema — não força vitórias, apenas revela caminhos.";
        case 4:
            return "Entre acertos e erros, " + n + " deixa o " + v + " como lembrete: paciência é também estratégia.";
        case 5:
            return "O tabuleiro respeita " + n + ": onde há " + v + ", há rota — mesmo que estreita, mesmo que exija conta.";
        case 6:
            return n + " nasceu da vontade de nomear o " + v + "; nomes dão coragem ao cálculo e memória ao risco.";
        default:
            return "Poucos escutam " + n + " de primeira; quem escuta reconhece o " + v + " antes do próximo movimento.";
        }
    }

    static string BuildProceduralLoreEn (int v, string n)
    {
        switch (Mathf.Abs (v) % 8) {
        case 0:
            return n + " embodies " + v + ": a quiet center on the board — proof that stillness can have shape.";
        case 1:
            return "In Conti Go tales, " + n + " appears when someone spots " + v + " hiding between signs and shortcuts.";
        case 2:
            return n + " never begs for attention: it settles " + v + " until it feels obvious, like a name that fits.";
        case 3:
            return "They say " + n + " wears " + v + " like a badge — not forcing wins, only revealing paths.";
        case 4:
            return "Between hits and misses, " + n + " leaves " + v + " as a reminder: patience is strategy too.";
        case 5:
            return "The board respects " + n + ": where " + v + " exists, a route exists — narrow, but real.";
        case 6:
            return n + " was born from the need to name " + v + "; names give courage to calculation and memory to risk.";
        default:
            return "Few hear " + n + " at first; those who do recognize " + v + " before the next move.";
        }
    }

    /// <summary>Nome fantasia ou o número em texto se não houver entrada.</summary>
    public static string GetFantasyName (int number)
    {
        if (s_map.TryGetValue (number, out ContiGoFantasyData data))
            return data.Name;
        return number.ToString ();
    }

    /// <summary>Dados completos ou null se o valor não estiver no catálogo.</summary>
    public static ContiGoFantasyData GetFantasyData (int number)
    {
        if (s_map.TryGetValue (number, out ContiGoFantasyData data))
            return data;
        return null;
    }

    public static bool TryGetFantasyData (int number, out ContiGoFantasyData data)
    {
        return s_map.TryGetValue (number, out data);
    }

    /// <summary>Texto da célula do tabuleiro (número puro ou nome + número, conforme <see cref="USE_FANTASY_NAMES_ON_BOARD"/>).</summary>
    public static string FormatBoardCell (int value)
    {
        if (!USE_FANTASY_NAMES_ON_BOARD)
            return value.ToString ();
        if (!USE_FANTASY_NAMES)
            return value.ToString ();
        if (!s_map.TryGetValue (value, out ContiGoFantasyData data))
            return value.ToString ();
        return data.Name + "\n(" + value + ")";
    }

    /// <summary>Mensagem de acerto com nome e valor, ex.: "Você acertou Octavira (8)".</summary>
    public static string FormatHitFeedback (int value, bool portuguese)
    {
        if (!USE_FANTASY_NAMES) {
            return portuguese
                ? "acertou!"
                : "correct!";
        }
        string name = GetFantasyName (value);
        if (portuguese)
            return "Você acertou " + name + " (" + value + ")";
        return "You got " + name + " (" + value + ")";
    }

    /// <summary>Uma entrada para listas (vírgula ou hífen); números puros se a feature estiver desligada.</summary>
    public static string FormatListEntry (int value)
    {
        if (!USE_FANTASY_NAMES)
            return value.ToString ();
        if (!s_map.TryGetValue (value, out ContiGoFantasyData data))
            return value.ToString ();
        return data.Name + " (" + value + ")";
    }

    /// <summary>Tamanho de fonte sugerido para o rótulo da célula (nomes longos em grelhas densas).</summary>
    public static float GetSuggestedCellFontSize (float cellSide, int gridSide)
    {
        if (!USE_FANTASY_NAMES_ON_BOARD || !USE_FANTASY_NAMES) {
            return cellSide > 56f ? 30f : (gridSide <= 4 ? 28f : 24f);
        }
        float baseFs = cellSide > 56f ? 26f : (gridSide <= 4 ? 24f : 20f);
        if (gridSide >= 8)
            baseFs = Mathf.Min (baseFs, 14f);
        else if (gridSide >= 6)
            baseFs = Mathf.Min (baseFs, 17f);
        return Mathf.Max (10f, Mathf.Min (baseFs, cellSide * 0.28f));
    }
}
