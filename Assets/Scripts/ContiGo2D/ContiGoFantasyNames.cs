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
        Add (
            8,
            "Octavira",
            "F",
            "Octavira dobra o destino em ciclos perfeitos, onde equilíbrio e continuidade nunca se quebram.\n\n" +
            "Octavira surge onde padrões se repetem sem se desgastar. Sua presença é como um laço perfeito que se fecha e reabre, mantendo a mesma forma " +
            "mesmo quando o mundo muda ao redor. Em seu corpo, curvas gêmeas de luz percorrem caminhos espelhados, e cada passo deixa no ar uma sensação " +
            "de continuidade — como se a realidade lembrasse de respirar em ciclos. Ela não acelera nem interrompe: ela sustenta. Ao seu redor, forças opostas " +
            "encontram um ritmo comum, e o caos perde o excesso, tornando-se desenho. Octavira não promete finais; ela oferece permanência — a ideia de que aquilo " +
            "que é verdadeiro retorna, sempre, com a mesma clareza.",
            "Octavira bends fate into perfect cycles, where balance and continuity never break.\n\n" +
            "Octavira appears where patterns repeat without wearing thin. Her presence is a flawless loop that closes and opens again, keeping its shape even as " +
            "the world shifts around it. Twin arcs of light trace mirrored paths along her form, and every step leaves a feeling of continuity in the air—as if reality " +
            "remembered how to breathe in cycles. She neither rushes nor interrupts: she sustains. Around her, opposing forces find a shared rhythm, and chaos loses its " +
            "excess until it becomes design. Octavira doesn’t promise endings; she offers permanence—the certainty that what is true will return, again and again, with the same clarity.");

        Add (
            9,
            "Nontheris",
            "M",
            "Nontheris sela o que estava disperso, transformando esforço em sentido e fim em passagem.\n\n" +
            "Nontheris caminha como o guardião do acabamento — aquele instante em que tudo faz sentido e nada sobra. Sua aura é densa e serena, como uma noite que não pesa, " +
            "apenas encerra o ruído. Ele carrega marcas geométricas que parecem sempre \"fechadas\", como se cada traço fosse a última peça de um mosaico. Quando fala, as palavras " +
            "soam como veredito gentil: não é imposição, é clareza. Ao seu redor, tarefas inacabadas encontram forma, ideias dispersas se alinham e o impulso de continuar sem rumo " +
            "se dissolve. Nontheris não cria caminhos; ele os conclui — e, ao concluir, prepara silenciosamente o terreno para o próximo começo.",
            "Nontheris seals what was scattered, turning effort into meaning—and endings into passage.\n\n" +
            "Nontheris walks as the keeper of completion—the moment when everything makes sense and nothing is left dangling. His aura is dense yet calm, like a night that doesn’t " +
            "weigh you down, only quiets the noise. Geometric marks on his form always seem \"closed,\" as if each line were the final tile in a mosaic. When he speaks, his words land " +
            "like a gentle verdict: not force, but clarity. Around him, unfinished tasks find shape, scattered ideas align, and the urge to keep moving without direction dissolves. Nontheris " +
            "doesn’t forge paths; he finishes them—and in finishing, he silently prepares the ground for the next beginning.");

        Add (
            10,
            "Decalya",
            "F",
            "Decalya abre a passagem onde a intenção encontra escala — e o começo volta maior do que antes.\n\n" +
            "Decalya aparece como um portal entre o simples e o vasto. Ela não se limita a avançar: ela muda a escala do mundo. Sua presença tem duas naturezas " +
            "em equilíbrio — uma linha firme de intenção e um espaço amplo de possibilidade — como se carregasse, ao mesmo tempo, direção e abertura. Ao seu redor, " +
            "o ambiente parece reorganizar-se em \"degraus\", e aquilo que era pequeno passa a ter alcance. Ela fala pouco, mas quando age, tudo ganha medida: distâncias " +
            "ficam claras, metas se tornam contáveis, e o impulso vira plano. Decalya não é apenas progresso; é transição — o momento em que o caminho deixa de ser tentativa " +
            "e passa a ser jornada.",
            "Decalya opens the passage where intent meets scale—and beginnings return larger than before.\n\n" +
            "Decalya appears as a gateway between the simple and the vast. She doesn’t merely move forward—she changes the scale of the world. Her presence holds two " +
            "balanced natures: a firm line of intent and a wide space of possibility, carrying direction and openness at once. Around her, the environment seems to reorganize " +
            "into \"steps,\" and what was small suddenly gains reach. She speaks little, but when she acts, everything gains measure: distances become clear, goals turn countable, " +
            "and impulse becomes plan. Decalya is not just progress; she is transition—the moment when a path stops being trial and becomes journey.",
            CardPortrait10Resource);
        Add (
            11,
            "Undecor",
            "M",
            "Undecor abre um passo além do óbvio, onde a vontade aprende a continuar sem se repetir.\n\n" +
            "Undecor surge como a inquietação elegante de quem se recusa a encerrar uma jornada cedo demais. Sua presença não rompe o que existe; ela estende, " +
            "com delicadeza firme, a linha do possível. Em torno dele, faixas finas de luz percorrem trajetos que parecem sempre apontar para fora, como uma promessa " +
            "de continuação. Seus gestos são econômicos, mas carregam impulso: a sensação de que o caminho não termina quando a mente diz “basta”. Undecor é feito de " +
            "curiosidade disciplinada — aquela coragem silenciosa que dá mais um passo quando todos já pararam.",
            "Undecor opens a step beyond the obvious, where intent learns to continue without repeating itself.\n\n" +
            "Undecor appears as the elegant restlessness of one who refuses to end a journey too soon. His presence doesn’t break what exists; it extends the line of " +
            "the possible with steady delicacy. Around him, thin ribbons of light trace paths that always seem to point outward, like a promise of continuation. His " +
            "gestures are economical yet driven, carrying the sense that a path doesn’t end when the mind says “enough.” Undecor is made of disciplined curiosity—" +
            "the quiet courage that takes one more step when everyone else has stopped.");

        Add (
            12,
            "Dodecira",
            "F",
            "Dodecira costura ciclos completos, onde cada parte encontra o seu lugar e nada fica solto.\n\n" +
            "Dodecira manifesta-se como uma arquiteta do tempo e das rotas, com uma calma que organiza sem prender. Em sua pele, marcas delicadas lembram anéis e " +
            "segmentos, como mapas de um percurso que retorna ao início sem perder significado. Ao caminhar, o espaço parece dividir-se em etapas naturais, e cada " +
            "etapa ganha nome, cor e propósito. Ela não apressa: ela alinha. Onde Dodecira passa, o que era disperso vira sequência, e a sequência vira entendimento.",
            "Dodecira stitches complete cycles, where every part finds its place and nothing is left loose.\n\n" +
            "Dodecira manifests as an architect of time and routes, with a calm that organizes without trapping. On her skin, delicate markings resemble rings and segments, " +
            "like maps of a path that returns to its start without losing meaning. As she walks, space seems to divide into natural stages, and each stage gains a name, " +
            "a color, and a purpose. She doesn’t rush; she aligns. Wherever Dodecira passes, what was scattered becomes sequence, and sequence becomes understanding.");

        Add (
            13,
            "Trezalon",
            "M",
            "Trezalon transforma risco em descoberta, onde a ousadia encontra sentido e vira destino.\n\n" +
            "Trezalon chega como um vento que muda a direção do pensamento. Sua aura tem um brilho raro, como se carregasse um segredo prestes a ser revelado, e " +
            "ao seu redor a realidade ganha uma leve tensão — não de perigo, mas de novidade. Ele não procura conforto; procura verdade. Quando fala, as palavras " +
            "soam como convite à travessia. Onde Trezalon passa, o medo perde o comando e a coragem aprende a sorrir.",
            "Trezalon turns risk into discovery, where boldness finds meaning and becomes destiny.\n\n" +
            "Trezalon arrives like a wind that shifts the direction of thought. His aura holds a rare glow, as if carrying a secret about to be revealed, and around him " +
            "reality gains a subtle tension—not danger, but novelty. He doesn’t seek comfort; he seeks truth. When he speaks, his words sound like an invitation to cross. " +
            "Where Trezalon passes, fear loses its grip and courage learns to smile.");

        Add (
            14,
            "Quattuora",
            "F",
            "Quattuora firma o mundo em pontos seguros, onde direção e estabilidade se tornam confiança.\n\n" +
            "Quattuora ergue-se com serenidade resoluta, como alguém que conhece as bordas do caos e sabe onde apoiar as mãos. Em torno dela, linhas invisíveis " +
            "parecem traçar um campo de orientação — como ventos que apontam sempre para o norte interior. Seus passos são firmes, e cada passo estabelece um " +
            "apoio: não para limitar, mas para sustentar. Onde Quattuora permanece, o ambiente desacelera e encontra forma.",
            "Quattuora anchors the world in safe points, where direction and stability become trust.\n\n" +
            "Quattuora rises with resolute serenity, like one who knows the edges of chaos and where to place her hands. Around her, unseen lines seem to draw an " +
            "orienting field—like winds that always point toward an inner north. Her steps are steady, and each step establishes footing: not to limit, but to sustain. " +
            "Where Quattuora remains, the environment slows down and finds shape.");

        Add (
            15,
            "Quinzelor",
            "M",
            "Quinzelor revela caminhos ocultos, onde escolha e precisão transformam acaso em estratégia.\n\n" +
            "Quinzelor aparece com olhos atentos e silêncio calculado. Ele observa o mundo como quem lê sinais em camadas: o que está na superfície, o que se repete, " +
            "o que falha por um detalhe. Ao seu redor, brilhos curtos surgem como marcações de rota, indicando alternativas que poucos enxergam. Ele não força a sorte; " +
            "ele a direciona. Onde Quinzelor caminha, decisões pequenas viram vitórias grandes.",
            "Quinzelor reveals hidden paths, where choice and precision turn chance into strategy.\n\n" +
            "Quinzelor appears with attentive eyes and a measured silence. He reads the world in layers: what lies on the surface, what repeats, what fails by a single detail. " +
            "Around him, brief glints flare like route markers, pointing to alternatives few can see. He doesn’t force luck; he steers it. Where Quinzelor walks, small decisions " +
            "become big wins.");

        Add (
            16,
            "Sexdoria",
            "F",
            "Sexdoria harmoniza forma e função, onde beleza e utilidade caminham lado a lado.\n\n" +
            "Sexdoria manifesta-se como uma artesã do equilíbrio prático. Sua presença é calorosa sem ser frágil, precisa sem ser fria. Padrões geométricos " +
            "parecem surgir e encaixar-se ao redor dela, como peças que sabem exatamente onde pertencer. Ao seu toque, o que era bruto encontra acabamento, e o " +
            "que era confuso encontra método. Sexdoria não impõe ordem: ela inspira arranjo.",
            "Sexdoria harmonizes form and function, where beauty and usefulness walk side by side.\n\n" +
            "Sexdoria manifests as a craftswoman of practical balance. Her presence is warm without fragility, precise without coldness. Geometric patterns seem to appear and " +
            "interlock around her, like pieces that know exactly where they belong. At her touch, what was rough finds polish, and what was confusing finds method. Sexdoria " +
            "doesn’t impose order; she inspires arrangement.");

        Add (
            17,
            "Septenor",
            "M",
            "Septenor protege o que é raro, onde paciência e mistério cultivam sabedoria.\n\n" +
            "Septenor caminha com um peso leve — a gravidade de quem escuta antes de agir. Sua aura tem camadas, como uma névoa que guarda sentidos escondidos " +
            "e só se abre para quem persiste. Ao seu redor, o ruído diminui e o mundo parece sussurrar. Ele não oferece respostas rápidas; oferece perguntas " +
            "melhores. Onde Septenor passa, a mente desacelera e aprende a ver profundidade.",
            "Septenor protects what is rare, where patience and mystery cultivate wisdom.\n\n" +
            "Septenor walks with a gentle weight—the gravity of one who listens before acting. His aura has layers, like a mist that holds hidden meanings and opens only " +
            "to those who persist. Around him, noise fades and the world seems to whisper. He doesn’t offer quick answers; he offers better questions. Where Septenor passes, " +
            "the mind slows down and learns to see depth.");

        Add (
            18,
            "Octavelyn",
            "F",
            "Octavelyn refina o movimento em ritmo, onde repetição vira maestria e serenidade.\n\n" +
            "Octavelyn surge como música que não cansa. Sua presença estabelece um compasso discreto, e tudo ao redor começa a seguir um fluxo mais limpo, " +
            "mais preciso. Ela não prende o mundo em regras; ela oferece cadência. Em sua passagem, erros se tornam ajustes e ajustes se tornam arte. " +
            "Octavelyn é o treino que vira graça.",
            "Octavelyn refines motion into rhythm, where repetition becomes mastery and serenity.\n\n" +
            "Octavelyn appears like music that never tires. Her presence sets a subtle tempo, and everything around begins to follow a cleaner, more precise flow. She doesn’t " +
            "trap the world in rules; she offers cadence. In her wake, mistakes become adjustments, and adjustments become art. Octavelyn is practice turned into grace.");

        Add (
            19,
            "Noventhor",
            "M",
            "Noventhor guarda o limiar da mudança, onde o fim se curva para abrir espaço ao recomeço.\n\n" +
            "Noventhor manifesta-se como a última vigília antes da virada. Seu olhar reconhece padrões que já deram tudo o que tinham, e sua presença " +
            "traz uma calma estranha — a calma de quem sabe que a ruptura pode ser necessária. Ao seu redor, o que estava rígido começa a soltar, " +
            "e o que era insistência vira escolha. Noventhor não destrói; ele libera. Ele prepara o terreno para que o novo não nasça frágil.",
            "Noventhor guards the threshold of change, where endings bend to make room for beginnings.\n\n" +
            "Noventhor manifests as the final watch before the turn. His gaze recognizes patterns that have given all they could, and his presence brings a strange calm—the calm " +
            "of one who knows a break can be necessary. Around him, what was rigid begins to loosen, and what was stubbornness becomes choice. Noventhor doesn’t destroy; he releases. " +
            "He prepares the ground so the new won’t be born fragile.");

        Add (
            20,
            "Vigessia",
            "F",
            "Vigessia transforma disciplina em leveza, onde constância vira poder silencioso.\n\n" +
            "Vigessia caminha como quem conhece a força do hábito bem escolhido. Sua presença não chama atenção; ela estabiliza. Ao seu redor, o " +
            "mundo parece encontrar rotina sem monotonia, como se cada ação se encaixasse no momento certo. Em sua pele, brilhos suaves surgem como " +
            "marcas de percurso, lembrando que evolução é soma de pequenas escolhas repetidas. Ela não promete atalhos; promete continuidade. Onde Vigessia " +
            "passa, a ansiedade perde pressa e a mente ganha firmeza — o tipo de firmeza que aguenta o tempo.",
            "Vigessia turns discipline into lightness, where consistency becomes quiet power.\n\n" +
            "Vigessia walks like one who knows the strength of a well-chosen habit. Her presence doesn’t demand attention; it stabilizes. Around her, the world " +
            "finds routine without monotony, as if each action fit the right moment. On her skin, soft glints appear like journey-marks, reminding that growth is the sum " +
            "of small repeated choices. She doesn’t promise shortcuts; she promises continuity. Where Vigessia passes, anxiety loses its rush and the mind gains steadiness—" +
            "the kind that can withstand time.");
        Add (
            21,
            "Vintarion",
            "M",
            "Vintarion transforma impulso em propósito, onde avanço e direção caminham juntos.\n\n" +
            "Vintarion surge como um capitão de rotas invisíveis. Sua presença carrega a sensação de movimento constante, não como pressa, mas como orientação. " +
            "Ao seu redor, o ar parece abrir corredores de passagem, e o mundo se arranja para facilitar o próximo passo. Ele observa com calma ativa: vê o que vem " +
            "à frente sem desprezar o que ficou para trás. Em sua voz há um tom de comando gentil, capaz de fazer escolhas parecerem simples. Onde Vintarion passa, " +
            "hesitação vira ação e ação vira trajetória.",
            "Vintarion turns impulse into purpose, where progress and direction walk together.\n\n" +
            "Vintarion appears like a captain of invisible routes. His presence carries the feeling of constant motion—not haste, but guidance. Around him, the air seems " +
            "to open passageways, and the world rearranges itself to ease the next step. He watches with active calm: seeing what lies ahead without dismissing what came before. " +
            "His voice holds a gentle command that makes choices feel simple. Where Vintarion passes, hesitation becomes action, and action becomes trajectory.");

        Add (
            22,
            "Duovintia",
            "F",
            "Duovintia junta caminhos distantes, onde encontros improváveis viram alianças duradouras.\n\n" +
            "Duovintia manifesta-se como uma ponte de luz entre margens que não se tocavam. Sua presença é acolhedora, mas firme, como quem sabe que união exige " +
            "tanto coragem quanto cuidado. Ao redor dela, pares de símbolos surgem e se conectam, formando mapas de ligação. Ela não força vínculos; ela cria espaço " +
            "para que eles aconteçam. Onde Duovintia caminha, o isolamento perde força e o mundo aprende a cooperar.",
            "Duovintia brings distant paths together, where unlikely meetings become lasting alliances.\n\n" +
            "Duovintia manifests as a bridge of light between shores that never touched. Her presence is welcoming yet steady, as one who knows unity requires courage as much " +
            "as care. Around her, pairs of symbols appear and connect, forming maps of linkage. She doesn’t force bonds; she makes room for them to happen. Where Duovintia walks, " +
            "isolation weakens and the world learns to cooperate.");

        Add (
            23,
            "Trivintor",
            "M",
            "Trivintor refina a escolha em acerto, onde foco e cálculo viram clareza.\n\n" +
            "Trivintor surge como um estrategista de mente luminosa. Sua presença traz uma nitidez rara: o excesso some, o essencial permanece. Em torno dele, " +
            "linhas discretas aparecem como rascunhos de possibilidades, e ele as percorre com o olhar até encontrar a melhor rota. Ele não se apressa; ele decide. " +
            "Onde Trivintor passa, a confusão se organiza e o erro perde espaço para a precisão.",
            "Trivintor refines choice into accuracy, where focus and calculation become clarity.\n\n" +
            "Trivintor appears like a strategist with a luminous mind. His presence brings rare sharpness: excess fades, essentials remain. Around him, subtle lines surface like " +
            "sketches of possibilities, and his gaze follows them until the best route reveals itself. He doesn’t rush; he decides. Where Trivintor passes, confusion organizes and " +
            "mistake yields to precision.");

        Add (
            24,
            "Quadrivela",
            "F",
            "Quadrivela sustenta o que cresce, onde estrutura e leveza mantêm tudo de pé.\n\n" +
            "Quadrivela manifesta-se como uma vela firme em ventos variáveis. Sua presença distribui peso e força com sabedoria, como se conhecesse a arte " +
            "de erguer sem forçar. Ao seu redor, planos invisíveis se alinham e criam apoio, permitindo expansão sem colapso. Ela é paciente com o tempo e " +
            "precisa com o espaço. Onde Quadrivela passa, projetos encontram sustentação e o mundo aprende a crescer com estabilidade.",
            "Quadrivela supports what grows, where structure and lightness keep everything standing.\n\n" +
            "Quadrivela manifests like a steady sail in shifting winds. Her presence distributes weight and strength with wisdom, as if she knew the art of lifting without strain. " +
            "Around her, unseen planes align and form support, allowing expansion without collapse. She is patient with time and precise with space. Where Quadrivela passes, projects " +
            "find footing and the world learns to grow with stability.");

        Add (
            25,
            "Quintovar",
            "M",
            "Quintovar desperta novas rotas, onde curiosidade e coragem se tornam impulso.\n\n" +
            "Quintovar surge com olhar inquieto e passos rápidos, como alguém que pressente portas escondidas. Sua presença traz um brilho de possibilidade: " +
            "o mundo parece maior do que era um instante antes. Em torno dele, sinais surgem como convites — pequenas pistas para quem tem vontade de explorar. " +
            "Ele não evita o desconhecido; ele o transforma em caminho. Onde Quintovar passa, a rotina se quebra e a aventura começa.",
            "Quintovar awakens new routes, where curiosity and courage become momentum.\n\n" +
            "Quintovar appears with a restless gaze and quick steps, like someone sensing hidden doors. His presence carries a glint of possibility: the world seems larger than it was a moment " +
            "before. Around him, signs emerge like invitations—small clues for those willing to explore. He doesn’t avoid the unknown; he turns it into a path. Where Quintovar passes, routine breaks " +
            "and adventure begins.");

        Add (
            26,
            "Hexavina",
            "F",
            "Hexavina harmoniza o coletivo, onde partes diferentes trabalham como uma só vontade.\n\n" +
            "Hexavina manifesta-se como uma coordenadora silenciosa da vida em conjunto. Sua presença cria um senso de encaixe: cada elemento encontra a posição " +
            "certa sem perder identidade. Padrões delicados surgem ao redor como redes de cooperação, e o que era ruído vira composição. Ela não manda; ela " +
            "sincroniza. Onde Hexavina passa, o esforço se distribui e a soma se torna mais forte do que qualquer parte isolada.",
            "Hexavina harmonizes the collective, where different parts work as a single will.\n\n" +
            "Hexavina manifests as a silent coordinator of shared life. Her presence creates a sense of fit: each element finds the right position without losing identity. Delicate patterns " +
            "appear around her like networks of cooperation, and what was noise becomes composition. She doesn’t command; she synchronizes. Where Hexavina passes, effort spreads out and the whole " +
            "becomes stronger than any isolated part.");

        Add (
            27,
            "Septavion",
            "M",
            "Septavion revela o invisível, onde intuição e profundidade guiam decisões.\n\n" +
            "Septavion caminha como quem escuta camadas que o mundo não mostra. Sua presença torna o silêncio mais nítido, e no silêncio surgem sinais. " +
            "Ele observa sem pressa e, quando escolhe, parece sempre ter visto algo a mais. Em torno dele, símbolos discretos aparecem e somem como " +
            "pistas de um mapa interior. Onde Septavion passa, dúvidas viram perguntas melhores — e as respostas chegam quando devem.",
            "Septavion reveals the unseen, where intuition and depth guide decisions.\n\n" +
            "Septavion walks like one who hears layers the world doesn’t show. His presence makes silence sharper, and within silence, signs appear. He observes without haste, and when he chooses, it " +
            "always feels as if he saw something extra. Around him, subtle symbols appear and fade like clues on an inner map. Where Septavion passes, doubts become better questions—and answers arrive " +
            "when they should.");

        Add (
            28,
            "Octalira",
            "F",
            "Octalira mantém a continuidade, onde ciclos suaves protegem o equilíbrio do mundo.\n\n" +
            "Octalira manifesta-se como uma dança de retorno. Sua presença repete sem cansar, como maré que sabe a hora de ir e vir. Ao redor dela, " +
            "linhas curvas de luz desenham trajetos que se cruzam sem conflito, e o ambiente encontra ritmo. Ela não busca o novo a qualquer custo; " +
            "busca o certo de novo. Onde Octalira passa, o caos perde pressa e a realidade aprende a respirar.",
            "Octalira preserves continuity, where gentle cycles protect the world’s balance.\n\n" +
            "Octalira manifests like a dance of return. Her presence repeats without wearing out, like a tide that knows when to come and go. Around her, curved lines of light trace paths that cross " +
            "without conflict, and the environment finds rhythm. She doesn’t chase novelty at any cost; she chases what is right—again. Where Octalira passes, chaos loses its rush and reality learns " +
            "to breathe.");

        Add (
            29,
            "Novarion",
            "M",
            "Novarion prepara a virada, onde o fim amadurece para dar lugar ao próximo passo.\n\n" +
            "Novarion surge como uma última faísca antes da mudança. Sua aura tem uma tensão controlada, como arco prestes a soltar a flecha. " +
            "Ele carrega a sabedoria de reconhecer quando insistir é apenas repetir e quando recuar é, na verdade, avançar. Ao seu redor, " +
            "o que estava preso começa a ceder, e caminhos novos aparecem sem ruído. Onde Novarion passa, transição deixa de ser medo e vira escolha.",
            "Novarion prepares the turn, where an ending matures to make room for the next step.\n\n" +
            "Novarion appears like a final spark before change. His aura holds controlled tension, like a bow about to release an arrow. He carries the wisdom to know when persistence is merely repetition and " +
            "when stepping back is, in truth, moving forward. Around him, what was stuck begins to loosen, and new paths appear without noise. Where Novarion passes, transition stops being fear and becomes choice.");

        Add (
            30,
            "Trizena",
            "F",
            "Trizena transforma esforço em resultado, onde constância e clareza sustentam o avanço.\n\n" +
            "Trizena caminha como quem conhece a arte de manter o rumo. Sua presença é firme, mas não rígida: ela ajusta o passo sem perder a direção. " +
            "Ao seu redor, o mundo parece aceitar a disciplina como conforto, e não como peso. Ela sabe dividir o grande em etapas possíveis, " +
            "e cada etapa vira promessa cumprida. Onde Trizena passa, o cansaço vira resistência e a resistência vira vitória tranquila.",
            "Trizena turns effort into results, where consistency and clarity sustain progress.\n\n" +
            "Trizena walks like one who knows the art of holding course. Her presence is steady but not rigid: she adjusts the pace without losing direction. Around her, the world seems to accept discipline " +
            "as comfort rather than burden. She knows how to break the large into achievable stages, and each stage becomes a kept promise. Where Trizena passes, fatigue becomes endurance—and endurance becomes a quiet win.");
        Add (
            31,
            "Trionyx",
            "M",
            "Trionyx transforma tensão em foco, onde controle e instinto viram precisão.\n\n" +
            "Trionyx surge como uma lâmina de calma no meio do ruído. Sua presença tem a firmeza de quem conhece o perigo, mas não se deixa dominar por ele. " +
            "Ao seu redor, o ar parece afiar-se: escolhas ficam nítidas, distrações perdem força. Ele se move com economia e intenção, como se cada gesto já " +
            "tivesse sido decidido antes de acontecer. Trionyx não busca confronto — busca domínio de si. Onde ele passa, o mundo aprende a ser exato.",
            "Trionyx turns tension into focus, where control and instinct become precision.\n\n" +
            "Trionyx appears like a blade of calm within the noise. His presence carries the steadiness of one who knows danger yet refuses to be ruled by it. Around him, the air seems to sharpen: " +
            "choices grow clear and distractions lose their pull. He moves with economy and intent, as if each gesture were decided before it happens. Trionyx doesn’t seek conflict—he seeks mastery of self. " +
            "Where he passes, the world learns to be exact.");

        Add (
            32,
            "Duotrina",
            "F",
            "Duotrina costura contrastes em continuidade, onde duas vozes viram um só caminho.\n\n" +
            "Duotrina manifesta-se como uma mediadora de camadas. Sua presença reúne o que parecia separado, não por forçar acordo, mas por revelar o ponto " +
            "em que as diferenças se completam. Em torno dela, brilhos surgem em pares e depois se multiplicam, como alianças que se espalham. Sua fala tem " +
            "ritmo de conciliação, e seu silêncio tem peso de escolha. Onde Duotrina caminha, o conflito perde importância e a cooperação ganha forma.",
            "Duotrina stitches contrasts into continuity, where two voices become a single path.\n\n" +
            "Duotrina manifests as a mediator of layers. Her presence gathers what seemed separate—not by forcing agreement, but by revealing the point where differences complete each other. Around her, " +
            "glints appear in pairs and then multiply, like alliances spreading outward. Her speech carries a conciliatory rhythm, and her silence holds the weight of choice. Where Duotrina walks, conflict " +
            "loses its grip and cooperation takes shape.");

        Add (
            33,
            "Triatrix",
            "M",
            "Triatrix organiza a criação em rotas claras, onde imaginação e método caminham juntos.\n\n" +
            "Triatrix surge como um desenhista do possível. Sua presença traz estrutura sem sufocar, e liberdade sem dispersar. Ao seu redor, linhas " +
            "invisíveis formam padrões que se ajustam conforme a necessidade, como esboços vivos. Ele enxerga alternativas em camadas e escolhe a " +
            "melhor com serenidade. Onde Triatrix passa, o que era ideia vira plano — e o plano vira mundo.",
            "Triatrix organizes creation into clear routes, where imagination and method walk together.\n\n" +
            "Triatrix appears like a drafter of possibility. His presence brings structure without suffocating, and freedom without scattering. Around him, unseen lines form patterns that adapt to need, " +
            "like living sketches. He sees alternatives in layers and chooses the best with calm. Where Triatrix passes, what was idea becomes plan—and plan becomes world.");

        Add (
            34,
            "Quadrya",
            "F",
            "Quadrya sustenta o avanço com serenidade, onde base firme permite altura sem medo.\n\n" +
            "Quadrya manifesta-se como um ponto de apoio constante. Sua presença distribui forças como quem conhece o peso das coisas e sabe onde colocá-lo. " +
            "Ao redor dela, o ambiente se alinha: direções ficam claras, passos ficam seguros. Ela não acelera; ela estabiliza. Onde Quadrya permanece, " +
            "o mundo encontra equilíbrio e a pressa perde comando.",
            "Quadrya sustains progress with serenity, where steady footing allows height without fear.\n\n" +
            "Quadrya manifests as a constant support point. Her presence distributes forces like one who understands weight and where to place it. Around her, the environment aligns: directions become clear and steps become sure. " +
            "She doesn’t speed things up; she stabilizes them. Where Quadrya remains, the world finds balance and haste loses its grip.");

        Add (
            35,
            "Quinzor",
            "M",
            "Quinzor vira o jogo com astúcia, onde decisão certa transforma chance em vantagem.\n\n" +
            "Quinzor surge com um brilho curioso no olhar e uma calma de quem já viu a mesma encruzilhada muitas vezes. Sua presença é leve, mas " +
            "estrategicamente pesada: cada palavra parece posicionar algo no tabuleiro do destino. Ao seu redor, pequenas oportunidades acendem como " +
            "faíscas, e ele as recolhe antes que desapareçam. Onde Quinzor passa, o improvável vira opção real — e a opção vira vitória.",
            "Quinzor flips the game with cunning, where the right decision turns chance into advantage.\n\n" +
            "Quinzor appears with a curious spark in his eyes and the calm of one who has seen the same crossroads many times. His presence is light, yet strategically heavy: every word seems to place something on fate’s board. Around him, " +
            "small opportunities flare like sparks, and he gathers them before they vanish. Where Quinzor passes, the unlikely becomes a real option—and the option becomes a win.");

        Add (
            36,
            "Hexatria",
            "F",
            "Hexatria tece harmonia duradoura, onde cuidado e estrutura protegem o que cresce.\n\n" +
            "Hexatria manifesta-se como uma guardiã de sistemas vivos. Sua presença lembra colmeias, redes e formas que se sustentam por cooperação. " +
            "Ao seu redor, partes dispersas parecem encontrar encaixe, e o que era excesso vira equilíbrio. Ela age com paciência: ajusta, reforça, " +
            "preserva. Onde Hexatria passa, o mundo aprende que força também é sustentar.",
            "Hexatria weaves lasting harmony, where care and structure protect what grows.\n\n" +
            "Hexatria manifests as a guardian of living systems. Her presence evokes honeycombs, networks, and forms held together by cooperation. Around her, scattered parts find their fit, and what was excess becomes balance. She acts with patience: " +
            "adjusting, reinforcing, preserving. Where Hexatria passes, the world learns that strength can also mean sustaining.");

        Add (
            37,
            "Septorin",
            "M",
            "Septorin revela sentido no silêncio, onde mistério e intuição guiam o passo certo.\n\n" +
            "Septorin surge como um viajante de olhos profundos. Sua presença diminui o barulho do mundo e faz emergir sinais discretos, como se a realidade " +
            "sussurrasse instruções escondidas. Ele não corre atrás de respostas; ele cria espaço para que elas apareçam. Onde Septorin caminha, a mente " +
            "desacelera e aprende a enxergar o que sempre esteve ali.",
            "Septorin reveals meaning in silence, where mystery and intuition guide the right step.\n\n" +
            "Septorin appears like a traveler with deep eyes. His presence lowers the world’s noise and brings forth subtle signs, as if reality whispered hidden directions. He doesn’t chase answers; he makes room for them to appear. Where Septorin walks, " +
            "the mind slows down and learns to see what was always there.");

        Add (
            38,
            "Octaryn",
            "F",
            "Octaryn mantém o ritmo do mundo, onde repetição vira refinamento e paz.\n\n" +
            "Octaryn manifesta-se como um compasso vivo. Sua presença dá cadência às ações e suaviza os extremos, como maré que regula o excesso. " +
            "Ao seu redor, movimentos se tornam mais limpos, e o que era impulso vira fluxo. Ela não impõe ciclos; ela os revela. Onde Octaryn " +
            "passa, o caos aprende a respirar em tempo.",
            "Octaryn keeps the world’s rhythm, where repetition becomes refinement and peace.\n\n" +
            "Octaryn manifests like a living tempo. Her presence gives cadence to actions and softens extremes, like a tide that regulates excess. Around her, movements become cleaner and what was impulse becomes flow. She doesn’t impose cycles; she reveals them. " +
            "Where Octaryn passes, chaos learns to breathe in time.");

        Add (
            39,
            "Novatrix",
            "M",
            "Novatrix guarda a fronteira da mudança, onde o fim amadurece para virar começo.\n\n" +
            "Novatrix surge como uma última curva antes da virada. Sua presença traz clareza sobre o que deve ficar e o que precisa ir. " +
            "Ao seu redor, estruturas antigas cedem sem colapsar, como se a realidade aprendesse a soltar com elegância. Ele não apressa " +
            "o novo; ele prepara o terreno. Onde Novatrix passa, transição vira passagem segura.",
            "Novatrix guards the border of change, where endings mature into beginnings.\n\n" +
            "Novatrix appears like the final bend before the turn. His presence brings clarity about what must remain and what must go. Around him, old structures loosen without collapsing, as if reality learned to let go with elegance. He doesn’t rush the new; he prepares the ground. " +
            "Where Novatrix passes, transition becomes a safe crossing.");

        Add (
            40,
            "Quadrinae",
            "F",
            "Quadrinae firma o que importa, onde ordem e direção sustentam o próximo passo.\n\n" +
            "Quadrinae manifesta-se como um mapa de estabilidade. Sua presença organiza o espaço em referências claras, como se cada coisa ganhasse um lugar " +
            "legítimo. Ela não reduz o mundo; ela o torna navegável. Ao seu redor, o caos perde arestas e a dúvida encontra norte. Onde Quadrinae " +
            "passa, o ambiente se alinha e a confiança volta a existir.",
            "Quadrinae anchors what matters, where order and direction sustain the next step.\n\n" +
            "Quadrinae manifests like a map of stability. Her presence organizes space into clear references, as if everything gained a rightful place. She doesn’t shrink the world; she makes it navigable. Around her, chaos loses its edges and doubt finds north. Where Quadrinae passes, the environment aligns and confidence returns.");
        Add (
            41,
            "Quadrion",
            "M",
            "Quadrion endireita o caos, onde ordem e propósito se tornam caminho seguro.\n\n" +
            "Quadrion surge como um eixo silencioso em torno do qual o mundo se organiza. Sua presença não é rígida — é firme. Ao seu redor, " +
            "tudo parece ganhar alinhamento: o que estava torto encontra direção, o que estava solto encontra base. Ele observa como quem mede " +
            "distâncias internas e, com poucos gestos, ajusta o essencial. Onde Quadrion passa, o ambiente perde ruído e ganha intenção.",
            "Quadrion straightens chaos, where order and purpose become a safe path.\n\n" +
            "Quadrion appears like a silent axis around which the world organizes itself. His presence isn’t rigid—it’s steady. Around him, " +
            "everything seems to align: what was crooked finds direction, what was loose finds footing. He watches as if measuring inner distances " +
            "and, with few gestures, adjusts what matters. Where Quadrion passes, the environment loses noise and gains intent.");

        Add (
            42,
            "Duoquadria",
            "F",
            "Duoquadria une estrutura e gentileza, onde firmeza e cuidado coexistem sem conflito.\n\n" +
            "Duoquadria manifesta-se como uma guardiã de equilíbrio aplicado. Sua presença cria apoio sem peso, como um abraço que também sustenta. " +
            "Ao seu redor, forças contrárias encontram um ponto comum, e o que era tensão vira colaboração. Ela não apaga diferenças; ela as " +
            "encaixa. Onde Duoquadria caminha, o mundo aprende que estabilidade também pode ser acolhimento.",
            "Duoquadria unites structure and kindness, where steadiness and care coexist without conflict.\n\n" +
            "Duoquadria manifests as a guardian of applied balance. Her presence provides support without heaviness, like an embrace that also holds. " +
            "Around her, opposing forces find common ground and what was tension becomes collaboration. She doesn’t erase differences; she fits them. " +
            "Where Duoquadria walks, the world learns that stability can also be warmth.");

        Add (
            44,
            "Quadrorix",
            "M",
            "Quadrorix governa limites com justiça, onde força vira proteção e direção.\n\n" +
            "Quadrorix surge como um sentinela de fronteiras invisíveis. Sua presença delimita sem sufocar, como muralhas feitas de critério e respeito. " +
            "Ele lê o mundo como um terreno que precisa de regras claras para permanecer livre. Ao seu redor, excessos recuam e intenções se revelam. " +
            "Onde Quadrorix permanece, o que é essencial fica — e o que é ruído se dissolve.",
            "Quadrorix rules boundaries with fairness, where strength becomes protection and direction.\n\n" +
            "Quadrorix appears as a sentinel of unseen borders. His presence defines without suffocating, like walls built from judgment and respect. " +
            "He reads the world as ground that needs clear rules to stay free. Around him, excess retreats and intentions reveal themselves. " +
            "Where Quadrorix remains, what is essential stays—and what is noise dissolves.");

        Add (
            45,
            "Quinquadra",
            "F",
            "Quinquadra sustenta mudança com estabilidade, onde adaptação não perde o chão.\n\n" +
            "Quinquadra manifesta-se como uma construtora de transições. Sua presença ensina que crescer não precisa ser queda e que mudar não precisa " +
            "ser caos. Ao seu redor, estruturas flexíveis surgem como pontes, permitindo atravessar sem quebrar. Ela caminha com serenidade prática, " +
            "ajustando o mundo para que o novo se encaixe. Onde Quinquadra passa, transformação vira arquitetura.",
            "Quinquadra sustains change with stability, where adaptation doesn’t lose its footing.\n\n" +
            "Quinquadra manifests as a builder of transitions. Her presence teaches that growth doesn’t have to be collapse and change doesn’t have to " +
            "be chaos. Around her, flexible structures appear like bridges, letting you cross without breaking. She walks with practical serenity, " +
            "tuning the world so the new can fit. Where Quinquadra passes, transformation becomes architecture.");

        Add (
            48,
            "Octaquadrix",
            "M",
            "Octaquadrix domina o ritmo do retorno, onde ciclos fortes mantêm o mundo em pé.\n\n" +
            "Octaquadrix surge como um guardião de cadências profundas. Sua presença é como uma engrenagem silenciosa que nunca falha: repetição sem " +
            "cansaço, movimento sem desperdício. Ao seu redor, o caos perde impulso e encontra padrão. Ele não impõe ordem à força; ele a revela " +
            "como uma lei natural. Onde Octaquadrix caminha, o mundo aprende a girar com equilíbrio.",
            "Octaquadrix commands the rhythm of return, where strong cycles keep the world standing.\n\n" +
            "Octaquadrix appears as a guardian of deep cadences. His presence is like a silent gear that never fails: repetition without fatigue, " +
            "motion without waste. Around him, chaos loses momentum and finds pattern. He doesn’t force order; he reveals it as a natural law. " +
            "Where Octaquadrix walks, the world learns to turn in balance.");

        Add (
            50,
            "Quintessara",
            "F",
            "Quintessara acende possibilidades, onde vontade e liberdade abrem novos horizontes.\n\n" +
            "Quintessara manifesta-se como um salto de energia criativa. Sua presença amplia o mundo sem quebrá-lo, como se abrisse janelas em paredes " +
            "antigas. Ao seu redor, caminhos inesperados surgem, e o que era limite vira escolha. Ela não se prende a um único rumo; ela coleciona " +
            "rotas. Onde Quintessara passa, o medo encolhe e a curiosidade cresce.",
            "Quintessara ignites possibility, where will and freedom open new horizons.\n\n" +
            "Quintessara manifests as a burst of creative energy. Her presence expands the world without breaking it, as if opening windows in old walls. " +
            "Around her, unexpected paths appear and what was a limit becomes a choice. She doesn’t bind herself to a single direction; she collects routes. " +
            "Where Quintessara passes, fear shrinks and curiosity grows.");

        Add (
            54,
            "Quinhexor",
            "M",
            "Quinhexor encontra encaixe no improvável, onde ordem e improviso viram solução.\n\n" +
            "Quinhexor surge como um engenheiro de atalhos. Sua presença não rejeita o erro; ela o recicla em caminho. Ao seu redor, peças que não " +
            "combinavam começam a se ajustar, e o mundo parece aceitar arranjos novos. Ele pensa rápido sem ser ansioso, e decide sem ser duro. " +
            "Onde Quinhexor passa, o impossível aprende a ter forma.",
            "Quinhexor finds fit in the unlikely, where order and improvisation become solution.\n\n" +
            "Quinhexor appears like an engineer of shortcuts. His presence doesn’t reject mistakes; it recycles them into paths. Around him, pieces that didn’t match begin to " +
            "align, and the world seems to accept new arrangements. He thinks fast without anxiety and decides without harshness. Where Quinhexor passes, the impossible " +
            "learns to take shape.");

        Add (
            55,
            "Quinquinara",
            "F",
            "Quinquinara duplica a coragem, onde insistência vira arte e avanço.\n\n" +
            "Quinquinara manifesta-se como uma força que retorna com mais clareza. Sua presença insiste sem repetir: ela aprende, ajusta e volta melhor. " +
            "Ao seu redor, tentativas falhas deixam de ser derrota e viram treino. Ela carrega uma alegria teimosa, capaz de manter o rumo mesmo quando " +
            "o mundo oscila. Onde Quinquinara passa, perseverança ganha brilho e propósito.",
            "Quinquinara doubles courage, where persistence becomes art and progress.\n\n" +
            "Quinquinara manifests as a force that returns with greater clarity. Her presence persists without repeating: it learns, adjusts, and comes back better. " +
            "Around her, failed attempts stop being defeat and become practice. She carries a stubborn joy that holds course even as the world wavers. Where Quinquinara " +
            "passes, perseverance gains shine and purpose.");

        Add (
            60,
            "Hexagor",
            "M",
            "Hexagor sustenta a disciplina do mundo, onde constância transforma esforço em resultado.\n\n" +
            "Hexagor surge como uma rotina viva: firme, repetível e necessária. Sua presença organiza o ambiente em etapas claras, e cada etapa parece " +
            "alcançável. Ele não oferece atalhos; oferece ritmo. Ao seu redor, ações simples ganham potência, porque são feitas na hora certa, do jeito " +
            "certo, de novo. Onde Hexagor passa, o cansaço se converte em resistência e a resistência em conquista.",
            "Hexagor sustains the world’s discipline, where consistency turns effort into results.\n\n" +
            "Hexagor appears like a living routine: steady, repeatable, necessary. His presence organizes the environment into clear steps, and each step feels " +
            "reachable. He doesn’t offer shortcuts; he offers rhythm. Around him, simple actions gain power because they are done at the right time, the right way, again. " +
            "Where Hexagor passes, fatigue turns into endurance—and endurance into achievement.");

        Add (
            64,
            "Hexaquara",
            "F",
            "Hexaquara protege o coletivo, onde cuidado e estrutura mantêm tudo inteiro.\n\n" +
            "Hexaquara manifesta-se como uma guardiã de comunidades. Sua presença distribui peso e responsabilidade com justiça, como quem sabe que " +
            "o todo depende do equilíbrio das partes. Ao seu redor, padrões surgem como redes, lembrando que união é engenharia delicada. Ela não " +
            "apressa; ela sustenta. Onde Hexaquara caminha, cooperação vira força e a força vira abrigo.",
            "Hexaquara protects the collective, where care and structure keep everything whole.\n\n" +
            "Hexaquara manifests as a guardian of communities. Her presence distributes weight and responsibility with fairness, as one who knows the whole depends on the balance of its parts. Around her, patterns appear like networks, reminding that unity is delicate engineering. She doesn’t rush; she sustains. Where Hexaquara walks, cooperation becomes strength—and strength becomes shelter.");

        Add (
            66,
            "Hexhexor",
            "M",
            "Hexhexor harmoniza engrenagens do destino, onde repetição vira perfeição.\n\n" +
            "Hexhexor surge como um mestre de mecanismos. Sua presença transforma movimentos brutos em ciclos elegantes, e o que era ruído vira " +
            "compasso. Ao seu redor, padrões se repetem com mínima perda, como se o mundo aprendesse a economizar esforço. Ele não busca brilho; " +
            "busca encaixe. Onde Hexhexor passa, a realidade se torna eficiente — e, por isso, mais leve.",
            "Hexhexor harmonizes fate’s gears, where repetition becomes perfection.\n\n" +
            "Hexhexor appears like a master of mechanisms. His presence turns rough motion into elegant cycles, and what was noise becomes tempo. Around him, patterns repeat with minimal loss, as if the world learned to waste less effort. He doesn’t chase shine; he chases fit. Where Hexhexor passes, reality becomes efficient—and therefore lighter.");
        Add (
            72,
            "Septadua",
            "F",
            "Septadua revela padrões no improvável, onde paciência e método transformam complexidade em caminho.\n\n" +
            "Septadua manifesta-se como uma tecelã de regularidades. Sua presença encontra ordem onde outros veem excesso, e, ao seu redor, " +
            "movimentos se repetem com intenção — como se a realidade aprendesse a encaixar peças grandes com delicadeza. Ela não se apressa: " +
            "observa, identifica, alinha. Onde Septadua caminha, o difícil deixa de ser um muro e vira sequência de passos possíveis.",
            "Septadua reveals patterns in the unlikely, where patience and method turn complexity into a path.\n\n" +
            "Septadua manifests as a weaver of regularities. Her presence finds order where others see excess, and around her, movements repeat with intent—as if reality learned to fit large pieces gently. She doesn’t rush: she observes, identifies, aligns. Where Septadua walks, difficulty stops being a wall and becomes a sequence of possible steps.");

        Add (
            75,
            "Septaquor",
            "M",
            "Septaquor abre trilhas na persistência, onde repetição vira domínio e calma.\n\n" +
            "Septaquor surge como um viajante de treino e constância. Sua presença é firme sem ser pesada, e o mundo ao redor parece aceitar " +
            "o trabalho contínuo como destino honroso. Ele não busca atalhos: busca refinamento. Onde Septaquor passa, o esforço ganha ritmo " +
            "e o ritmo vira vitória discreta.",
            "Septaquor carves trails through persistence, where repetition becomes mastery and calm.\n\n" +
            "Septaquor appears like a traveler of practice and constancy. His presence is steady without heaviness, and the world around seems to accept sustained work as an honorable fate. He doesn’t seek shortcuts; he seeks refinement. Where Septaquor passes, effort finds rhythm—and rhythm becomes a quiet win.");

        Add (
            80,
            "Octogena",
            "F",
            "Octogena sustenta a maturidade do mundo, onde ciclos longos viram força serena.\n\n" +
            "Octogena manifesta-se como a memória do que funciona. Sua presença traz uma sensação de tempo bem vivido: o que é frágil se fortalece, " +
            "o que é instável encontra ritmo. Ao seu redor, movimentos se repetem com suavidade, como maré que aprende a respeitar a margem. " +
            "Onde Octogena caminha, o mundo amadurece sem perder brilho.",
            "Octogena sustains the world’s maturity, where long cycles become serene strength.\n\n" +
            "Octogena manifests as the memory of what works. Her presence carries the feeling of time well lived: what is fragile strengthens, what is unstable finds rhythm. Around her, motions repeat softly, like a tide learning to respect the shore. Where Octogena walks, the world matures without losing its shine.");

        Add (
            90,
            "Novarix",
            "M",
            "Novarix guarda o fim antes da virada, onde conclusão e clareza preparam o próximo começo.\n\n" +
            "Novarix surge como um juiz de ciclos: observa, encerra, libera. Sua presença traz a calma de quem sabe finalizar sem arrependimento. " +
            "Ao seu redor, o que estava disperso se organiza, e o que era insistência vira escolha. Onde Novarix passa, o mundo aprende a fechar " +
            "portas com gentileza — e abrir outras com coragem.",
            "Novarix guards the end before the turn, where completion and clarity prepare the next beginning.\n\n" +
            "Novarix appears like a judge of cycles: he observes, concludes, releases. His presence brings the calm of one who can finish without regret. Around him, what was scattered organizes, and what was stubbornness becomes choice. Where Novarix passes, the world learns to close doors gently—and open others with courage.");

        Add (
            96,
            "Novexa",
            "F",
            "Novexa revela elegância na estrutura, onde harmonia e precisão sustentam o extraordinário.\n\n" +
            "Novexa manifesta-se como uma arquiteta de formas raras. Sua presença alinha partes grandes sem quebrar delicadeza, e o espaço " +
            "ao redor parece aceitar padrões complexos com naturalidade. Ela não exibe força; exibe encaixe. Onde Novexa caminha, o que " +
            "parecia impossível encontra método — e o método vira beleza.",
            "Novexa reveals elegance in structure, where harmony and precision sustain the extraordinary.\n\n" +
            "Novexa manifests as an architect of rare forms. Her presence aligns large parts without breaking delicacy, and space around seems to accept complex patterns naturally. She doesn’t display force; she displays fit. Where Novexa walks, what seemed impossible finds method—and method becomes beauty.");

        Add (
            100,
            "Centorion",
            "M",
            "Centorion lidera com firmeza, onde disciplina e honra constroem caminho duradouro.\n\n" +
            "Centorion surge como um comandante de passos largos. Sua presença inspira confiança: não por prometer vitória fácil, mas por " +
            "sustentar o rumo quando o mundo vacila. Ao seu redor, o ambiente parece organizar-se em ordem de marcha, e até o medo ganha " +
            "forma para ser enfrentado. Onde Centorion passa, coragem vira hábito e o hábito vira legado.",
            "Centorion leads with steadiness, where discipline and honor build a lasting path.\n\n" +
            "Centorion appears like a commander of long strides. His presence inspires trust—not by promising easy victory, but by holding course when the world wavers. Around him, the environment seems to fall into marching order, and even fear takes shape to be faced. Where Centorion passes, courage becomes habit—and habit becomes legacy.");

        Add (
            108,
            "Centavira",
            "F",
            "Centavira amplia a visão, onde equilíbrio e alcance transformam limites em horizonte.\n\n" +
            "Centavira manifesta-se como uma guia de grandes distâncias. Sua presença expande sem dispersar, como um mapa que cresce, mas " +
            "mantém o centro. Ao seu redor, caminhos se tornam legíveis e o mundo parece caber nos olhos. Onde Centavira caminha, " +
            "o impossível encolhe e o próximo passo se torna claro.",
            "Centavira expands vision, where balance and reach turn limits into horizon.\n\n" +
            "Centavira manifests as a guide of long distances. Her presence expands without scattering, like a map that grows yet keeps its center. Around her, paths become readable and the world seems to fit within sight. Where Centavira walks, the impossible shrinks and the next step becomes clear.");

        Add (
            120,
            "Centovintor",
            "M",
            "Centovintor sustenta a expansão, onde constância e estratégia mantêm o progresso vivo.\n\n" +
            "Centovintor surge como um navegador de etapas longas. Sua presença não acelera o mundo — ela o mantém avançando. Ao seu redor, " +
            "o esforço se distribui, e o caminho parece menos pesado. Ele conhece a arte de continuar quando a euforia passa. Onde Centovintor " +
            "caminha, o futuro deixa de ser desejo e vira plano.",
            "Centovintor sustains expansion, where consistency and strategy keep progress alive.\n\n" +
            "Centovintor appears like a navigator of long stages. His presence doesn’t speed the world up—it keeps it moving. Around him, effort spreads out and the path feels lighter. He knows how to continue when excitement fades. Where Centovintor walks, the future stops being a wish and becomes a plan.");

        Add (
            125,
            "Centoquinara",
            "F",
            "Centoquinara refina a intenção, onde disciplina paciente transforma desejo em forma.\n\n" +
            "Centoquinara manifesta-se como uma artesã de repetição consciente. Sua presença torna o trabalho contínuo quase sagrado: " +
            "cada retorno traz melhoria, cada ajuste traz clareza. Ao seu redor, padrões se fecham com perfeição tranquila. Onde " +
            "Centoquinara caminha, a constância deixa de ser peso e vira poder silencioso.",
            "Centoquinara refines intent, where patient discipline turns desire into form.\n\n" +
            "Centoquinara manifests as a craftswoman of mindful repetition. Her presence makes sustained work feel almost sacred: each return brings improvement, each adjustment brings clarity. Around her, patterns close with quiet perfection. Where Centoquinara walks, consistency stops being a burden and becomes quiet power.");

        Add (
            144,
            "Centoquadrix",
            "M",
            "Centoquadrix ergue uma fortaleza de ordem, onde perfeição e rigor viram proteção.\n\n" +
            "Centoquadrix surge como um guardião de estruturas impecáveis. Sua presença não tolera desperdício: tudo ao redor encontra " +
            "encaixe e propósito. Ele não é frio — é exato. Onde Centoquadrix passa, o caos perde acesso, e a mente encontra um chão " +
            "sólido para construir.",
            "Centoquadrix raises a fortress of order, where perfection and rigor become protection.\n\n" +
            "Centoquadrix appears as a guardian of impeccable structures. His presence tolerates no waste: everything around finds fit and purpose. He isn’t cold—he’s exact. Where Centoquadrix passes, chaos loses access and the mind finds solid ground to build upon.");

        Add (
            150,
            "Centoquinella",
            "F",
            "Centoquinella inspira conquista limpa, onde coragem e método caminham sem ruído.\n\n" +
            "Centoquinella manifesta-se como uma vencedora serena. Sua presença traz confiança sem arrogância e precisão sem dureza. " +
            "Ao seu redor, escolhas se alinham, e o caminho parece abrir-se por mérito, não por acaso. Onde Centoquinella caminha, " +
            "vitória vira consequência natural de um rumo bem mantido.",
            "Centoquinella inspires clean conquest, where courage and method move without noise.\n\n" +
            "Centoquinella manifests as a serene victor. Her presence brings confidence without arrogance and precision without harshness. Around her, choices align and the path seems to open by merit rather than chance. Where Centoquinella walks, victory becomes the natural consequence of a well-held course.");

        Add (
            180,
            "Centoctarion",
            "M",
            "Centoctarion governa o horizonte final, onde grandeza e disciplina transformam limites em legado.\n\n" +
            "Centoctarion surge como o peso nobre de uma meta distante. Sua presença amplia o mundo e exige responsabilidade: o que é " +
            "grande precisa ser sustentado. Ao seu redor, o esforço ganha dignidade e o caminho se torna épico. Ele não promete facilidade; " +
            "promete sentido. Onde Centoctarion passa, a ambição vira construção — e a construção vira memória.",
            "Centoctarion commands the far horizon, where greatness and discipline turn limits into legacy.\n\n" +
            "Centoctarion appears as the noble weight of a distant goal. His presence expands the world and demands responsibility: what is great must be sustained. Around him, effort gains dignity and the path turns epic. He doesn’t promise ease; he promises meaning. Where Centoctarion walks, ambition becomes construction—and construction becomes memory.");

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
