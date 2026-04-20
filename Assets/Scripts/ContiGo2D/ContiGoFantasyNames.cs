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
        Add (
            1,
            "Primarion",
            "M",
            "Primarion é a primeira vontade do universo: firme, inteiro e destinado a abrir caminhos onde nada existia.\n\n" +
            "Primarion manifesta-se como a primeira afirmação da existência, uma presença firme que se ergue como um pilar de luz clara " +
            "no vasto silêncio do universo. Sua forma é simples e imponente, marcada por linhas retas e puras que transmitem direção e " +
            "propósito. No centro de seu peito arde um núcleo luminoso e constante, uma chama indivisível que jamais se dispersa, mantendo " +
            "sempre sua essência plena e íntegra. Antigos traços de energia percorrem sua superfície como marcas primordiais, lembrando os " +
            "primeiros sinais que seres conscientes gravaram para registrar presença e conquista. Ao seu redor, o espaço parece alinhar-se " +
            "naturalmente, como se tudo reconhecesse nele o ponto onde o movimento começa. Sua postura transmite decisão, coragem e liderança " +
            "silenciosa — não pela força que impõe, mas pela clareza com que inaugura caminhos. Onde Primarion surge, a inércia se desfaz e " +
            "o universo encontra sua primeira direção, como se cada jornada precisasse, antes de tudo, de alguém disposto a dar o primeiro passo.",
            "Primarion is the universe's first will: steady, whole, and bound to open paths where nothing existed.\n\n" +
            "Primarion manifests as the first affirmation of existence—a firm presence rising like a pillar of clear light within the vast " +
            "silence of the universe. His form is simple yet imposing, traced by straight, pure lines that convey direction and purpose. At " +
            "the center of his chest burns a constant luminous core—an indivisible flame that never scatters—keeping his essence complete and " +
            "intact. Ancient currents of energy run across his surface like primordial markings, recalling the first signs conscious beings carved " +
            "to record presence and conquest. Around him, space seems to align naturally, as if everything recognizes in him the point where motion " +
            "begins. His posture carries resolve, courage, and quiet leadership—not through force, but through the clarity with which he opens the way. " +
            "Where Primarion appears, inertia dissolves and the universe finds its first direction, as if every journey, before anything else, needs " +
            "someone willing to take the very first step.",
            CardPortrait1Resource);
        Add (
            2,
            "Duovelya",
            "F",
            "Duovelya une forças opostas em perfeita harmonia, revelando que o verdadeiro poder nasce da união.\n\n" +
            "Duovelya manifesta-se como a guardiã da harmonia entre forças que, sozinhas, jamais encontrariam paz. Sua presença flui " +
            "em duas correntes de energia entrelaçadas que percorrem seu corpo como fitas luminosas, movendo-se em perfeita sincronia, " +
            "ora se afastando, ora se aproximando, como se dançassem em constante diálogo. Seus olhos refletem duas luzes suaves e " +
            "complementares, revelando uma percepção capaz de compreender lados opostos sem permitir que se tornem conflito. Ao seu redor, " +
            "o espaço parece se dividir e se recompor em equilíbrio, como se cada força encontrasse naturalmente sua contraparte. Pequenas " +
            "marcas luminosas surgem em pares ao longo de sua pele, lembrando antigos sinais gravados para representar união e companhia. " +
            "Sua voz carrega uma serenidade conciliadora, capaz de transformar tensão em entendimento. Onde Duovelya passa, o isolamento se " +
            "desfaz e vínculos se formam — pois sua essência não é caminhar sozinha, mas revelar que toda existência encontra sentido quando " +
            "duas presenças aprendem a coexistir em harmonia.",
            "Duovelya unites opposing forces in perfect harmony, revealing that true power is born from unity.\n\n" +
            "Duovelya manifests as the guardian of harmony between forces that, alone, would never find peace. Her presence flows in two " +
            "intertwined currents of energy that run across her body like luminous ribbons, moving in perfect synchrony—sometimes drifting " +
            "apart, sometimes drawing close—as if dancing in constant dialogue. Her eyes reflect two gentle, complementary lights, revealing " +
            "a perception able to understand opposing sides without allowing them to become conflict. Around her, space seems to split and " +
            "recompose in balance, as if each force naturally found its counterpart. Small luminous marks appear in pairs along her skin, " +
            "echoing ancient symbols carved to represent union and companionship. Her voice carries a conciliatory serenity, capable of " +
            "turning tension into understanding. Wherever Duovelya passes, isolation dissolves and bonds are formed—for her essence is not " +
            "to walk alone, but to reveal that existence finds meaning when two presences learn to coexist in harmony.",
            CardPortrait2Resource);
        Add (
            3,
            "Triandor",
            "M",
            "Triandor sustenta o equilíbrio da criação, onde três forças se encontram para dar forma à expressão e à vida.\n\n" +
            "Triandor manifesta-se como uma presença vibrante e criativa, cuja energia se organiza em três correntes luminosas que giram ao " +
            "redor de seu corpo em perfeita harmonia. Sua forma parece sustentar um equilíbrio natural, como se cada movimento seu fosse apoiado " +
            "por três pontos invisíveis que estabilizam tudo ao seu redor. Sobre sua cabeça pairam três fragmentos de luz cristalina que orbitam " +
            "lentamente, formando um padrão triangular que pulsa com ritmo constante, lembrando passado, presente e futuro entrelaçados em um mesmo " +
            "instante. Sua voz ecoa com clareza e inspiração, carregando uma força expressiva capaz de despertar ideias, histórias e caminhos onde antes " +
            "havia silêncio. Marcas sutis em forma de triângulos percorrem sua pele como símbolos ancestrais de sabedoria e completude. Ao caminhar, o " +
            "espaço parece ganhar estrutura e fluidez ao mesmo tempo, como se a realidade encontrasse em sua presença a combinação perfeita entre estabilidade " +
            "e criação. Triandor não apenas sustenta o equilíbrio — ele o transforma em expressão, fazendo do universo uma obra viva que se comunica, se reinventa " +
            "e continua a se expandir.",
            "Triandor upholds the balance of creation, where three forces meet to give shape to expression and life.\n\n" +
            "Triandor manifests as a vibrant, creative presence, his energy arranged into three luminous currents that spin around his body in perfect harmony. " +
            "His form sustains a natural equilibrium, as if each movement were supported by three invisible points that stabilize everything nearby. Above his head, " +
            "three crystalline fragments of light orbit slowly, forming a triangular pattern that pulses with a steady rhythm, evoking past, present, and future braided " +
            "into a single instant. His voice echoes with clarity and inspiration, carrying an expressive force capable of awakening ideas, stories, and paths where there was " +
            "once silence. Subtle triangular markings run across his skin like ancestral symbols of wisdom and completeness. As he walks, space seems to gain structure and flow at " +
            "once, as if reality found in him the perfect blend of stability and creation. Triandor does not merely hold balance—he turns it into expression, making the universe a living " +
            "work that speaks, reinvents itself, and continues to expand.",
            CardPortrait3Resource);

        Add (
            4,
            "Quadrina",
            "F",
            "Quadrina é o alicerce da realidade, onde quatro forças se unem para sustentar ordem, direção e estabilidade.\n\n" +
            "Quadrina ergue-se como a guardiã da estrutura e da ordem que sustenta o mundo. Sua presença transmite firmeza serena, como se o próprio espaço " +
            "encontrasse nela seus pontos de apoio. Quatro pilares de energia cristalina orbitam lentamente ao seu redor, formando um campo estável que organiza tudo " +
            "o que toca. Em sua pele brilham marcas geométricas que lembram ângulos perfeitos e formas sólidas, como se cada traço carregasse a essência das fundações " +
            "invisíveis da realidade. Seus passos são precisos e seguros, como alguém que compreende a importância de cada base antes de qualquer construção. Ao seu redor, " +
            "as forças do universo se alinham como pontos cardeais encontrando direção, e até o caos parece adquirir forma e propósito. Quadrina não busca dominar o mundo — " +
            "ela o sustenta, mantendo a ordem silenciosa que permite que todas as coisas existam, cresçam e permaneçam firmes diante do tempo.",
            "Quadrina is the foundation of reality, where four forces unite to sustain order, direction, and stability.\n\n" +
            "Quadrina rises as the guardian of structure and order that holds the world together. Her presence carries a serene firmness, as if space itself found in her its points of support. " +
            "Four pillars of crystalline energy orbit slowly around her, forming a stable field that organizes everything it touches. Geometric marks glow on her skin, recalling perfect angles and solid " +
            "forms, as if each line carried the essence of reality's invisible foundations. Her steps are precise and sure, like someone who understands the importance of every base before any construction. " +
            "Around her, the forces of the universe align like cardinal points finding direction, and even chaos seems to gain shape and purpose. Quadrina does not seek to dominate the world—she sustains it, " +
            "maintaining the silent order that allows all things to exist, grow, and remain firm against time.",
            CardPortrait4Resource);

        Add (
            5,
            "Quintaros",
            "M",
            "Quintaros é o espírito da mudança, guiado por cinco forças que despertam liberdade, curiosidade e novos caminhos.\n\n" +
            "Quintaros caminha como a própria personificação do movimento e da descoberta. Sua presença nunca permanece estática; ao redor de seu corpo giram cinco " +
            "correntes de energia viva que se expandem em diferentes direções, como se explorassem o mundo através de sentidos invisíveis. Seus olhos carregam um brilho inquieto " +
            "e curioso, sempre atentos aos detalhes do universo, percebendo texturas, sons e vibrações que poucos conseguem notar. Em suas mãos surgem marcas luminosas que se abrem " +
            "como uma estrela de cinco pontas, refletindo sua ligação com as forças naturais que permeiam todas as coisas. Cada passo de Quintaros parece despertar novas possibilidades, " +
            "como se o próprio espaço se abrisse para caminhos inesperados. Ele não se prende a rotas fixas nem a estruturas rígidas; sua essência é experimentar, adaptar e avançar. Onde " +
            "Quintaros passa, o mundo se torna mais vasto, mais vivo e cheio de possibilidades — pois sua natureza é lembrar que a verdadeira existência se revela quando se tem coragem de " +
            "explorar o desconhecido.",
            "Quintaros is the spirit of change, guided by five forces that awaken freedom, curiosity, and new paths.\n\n" +
            "Quintaros walks as the very embodiment of movement and discovery. His presence never remains static; around his body spin five currents of living energy, spreading in different directions as if " +
            "exploring the world through unseen senses. His eyes carry a restless, curious glow, always alert to the universe's details—perceiving textures, sounds, and vibrations few can notice. In his hands, " +
            "luminous marks unfold like a five-pointed star, reflecting his bond with the natural forces that permeate all things. Each step seems to awaken new possibilities, as if space itself opened into unexpected " +
            "routes. He clings to neither fixed paths nor rigid structures; his essence is to experiment, adapt, and move forward. Wherever Quintaros passes, the world becomes wider, more alive, and full of possibility—" +
            "for his nature is to remind us that true existence is revealed when we dare to explore the unknown.",
            CardPortrait5Resource);

        Add (
            6,
            "Hexalyn",
            "F",
            "Hexalyn é a guardiã da perfeição serena, onde cada parte encontra seu lugar e o mundo se mantém em harmonia.\n\n" +
            "Hexalyn possui uma presença acolhedora e firme, como uma guardiã silenciosa que sustenta aquilo que precisa permanecer inteiro. Seus passos são calmos e seguros, e " +
            "por onde caminha o ambiente parece se organizar naturalmente, como se cada coisa encontrasse seu lugar correto. Seu manto apresenta padrões geométricos delicados que " +
            "lembram colmeias e estruturas naturais, símbolos de uma ordem construída com paciência e cooperação. Seu olhar transmite uma serenidade profunda, a de alguém que compreende " +
            "o valor de proteger, nutrir e manter o equilíbrio entre as partes. Hexalyn não age com pressa nem com imposição; sua força está na constância e na responsabilidade. Ao seu redor, " +
            "conflitos se suavizam e excessos se acomodam, como se sua presença lembrasse ao mundo que toda criação precisa de cuidado, estrutura e harmonia para continuar existindo.",
            "Hexalyn is the guardian of serene perfection, where every part finds its place and the world remains in harmony.\n\n" +
            "Hexalyn has a welcoming yet steadfast presence, like a silent guardian who holds together what must remain whole. Her steps are calm and sure, and wherever she walks the environment seems to organize itself " +
            "naturally, as if each thing found its rightful place. Her cloak bears delicate geometric patterns reminiscent of honeycombs and natural structures—symbols of an order built through patience and cooperation. Her gaze " +
            "conveys deep serenity, that of someone who understands the value of protecting, nurturing, and keeping balance among all parts. Hexalyn acts neither in haste nor by imposition; her strength lies in constancy and responsibility. " +
            "Around her, conflicts soften and excesses settle, as if her presence reminded the world that every creation needs care, structure, and harmony to continue existing.",
            CardPortrait6Resource);

        Add (
            7,
            "Septurion",
            "M",
            "Septurion é o guardião do mistério, onde sabedoria, sorte e espiritualidade se encontram.\n\n" +
            "Septurion caminha como um viajante entre o visível e o invisível, carregando consigo uma aura de mistério sereno. Seu manto profundo parece tecido com fragmentos de céu noturno, " +
            "pontilhado por pequenas luzes que lembram constelações antigas, como se cada uma guardasse um segredo do universo. Seu olhar é contemplativo e distante, não por indiferença, mas por " +
            "enxergar camadas da realidade que poucos percebem. Em seu bastão de madeira antiga estão gravados sete símbolos arcanos, marcas de conhecimento acumulado através de eras e dimensões. Ao " +
            "seu redor, o silêncio adquire significado, como se cada pausa fosse parte de uma linguagem invisível. Septurion não impõe respostas — ele inspira reflexão. Onde ele passa, mentes inquietas " +
            "encontram sabedoria, e perguntas esquecidas voltam a despertar. Sua presença lembra que o universo não é feito apenas de matéria, mas também de mistério, consciência e caminhos interiores ainda por explorar.",
            "Septurion is the guardian of mystery, where wisdom, fortune, and spirituality meet.\n\n" +
            "Septurion walks like a traveler between the visible and the invisible, carrying an aura of serene mystery. His deep mantle seems woven from fragments of the night sky, speckled with small lights that recall ancient constellations, " +
            "as if each held a secret of the universe. His gaze is contemplative and distant—not from indifference, but from seeing layers of reality few perceive. On his staff of ancient wood are carved seven arcane symbols, marks of knowledge " +
            "gathered through ages and dimensions. Around him, silence gains meaning, as if each pause were part of an unseen language. Septurion does not impose answers—he inspires reflection. Wherever he passes, restless minds find wisdom, and " +
            "forgotten questions awaken again. His presence reminds us that the universe is not made only of matter, but also of mystery, consciousness, and inner paths still waiting to be explored.");
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
