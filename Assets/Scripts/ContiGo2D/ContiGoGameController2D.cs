using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Modo Conti Go em Canvas 2D: tabuleiro N×N conforme <see cref="ContiGo2DLevelId"/> (2…8), 3 dados 1–6, 3 vidas, tempo por nível.
/// Não altera <see cref="GameController"/> nem o fluxo 3D.
/// </summary>
public partial class ContiGoGameController2D : MonoBehaviour
{
    [SerializeField] ContiGo2DLevelId levelId = ContiGo2DLevelId.Mestre;

    [Header ("Menu pausa / Game Over (Layer Lab GUI Pro-CasualGame)")]
    [Tooltip ("Popup01_Single_Navy — fundo do modal de pause e do Game Over.")]
    [SerializeField] Sprite _menuPopupBgSprite;
    [Tooltip ("Button_Hexagon199_Blue — arte do botão hexagonal.")]
    [SerializeField] Sprite _menuHexButtonSprite;
    [Tooltip ("Icon_PictoIcon_Play — continuar (fecha o menu).")]
    [SerializeField] Sprite _menuContinueIconSprite;
    [Tooltip ("Icon_PictoIcon_Resume — reiniciar partida.")]
    [SerializeField] Sprite _menuRestartIconSprite;
    [Tooltip ("Icon_PictoIcon_Exit_l — voltar à Home.")]
    [SerializeField] Sprite _menuExitIconSprite;

    [Header ("Game Over — relatório de erros (Layer Lab GUI Pro-CasualGame)")]
    [Tooltip ("ListFrame03_Single_Bg_Blue — fundo do bloco de erros.")]
    [SerializeField] Sprite _gameOverErrorsListBgSprite;
    [Tooltip ("ItemFrame03_Single_Purple — fundo de cada item de erro.")]
    [SerializeField] Sprite _gameOverErrorItemFrameSprite;

    [Header ("HUD — ícones vidas / pontos (Layer Lab GUI Pro-CasualGame)")]
    [Tooltip ("ResourcesData/Sprites/Components/IconMisc/Icon_StatsIcon_Hp02.png")]
    [SerializeField] Sprite _hudLivesIconSprite;
    [Tooltip ("ResourcesData/Sprites/Components/IconMisc/Icon_MenuIcon04_Trophy.Png")]
    [SerializeField] Sprite _hudScoreTrophyIconSprite;

    public bool gameOn = false;
    public bool gameStrategicTimer = false;

    public string language = "portuguese";

    public TextMeshProUGUI txtStrategicTimer;
    public GameObject strategicTimerScreen;
    public float strategicTimer;
    bool canCountStrategic = true;

    public GameObject btnPause;
    public GameObject btnPularRodada;
    GameObject btnHelp;

    bool menuOpen;
    bool gameOnBeforeMenu;

    public TextMeshProUGUI txtMainTimer;
    public float mainTimer;
    public long currentMainTimer;

    public float timer;
    bool canCount = true;
    bool doOnce = false;

    public GameObject gameOverScreen;
    TextMeshProUGUI gameOverMainText;
    Button gameOverRestartButton;
    Button gameOverHomeButton;
    RectTransform gameOverErrorsRoot;
    GameObject gameOverErrorsBlock;
    TMP_FontAsset _uiFont;

    struct WrongAttempt
    {
        public int a;
        public int b;
        public int c;
        public int choice;

        public WrongAttempt (int a, int b, int c, int choice)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.choice = choice;
        }
    }

    // Só existem 3 vidas/erros possíveis, então mantemos um buffer pequeno.
    readonly List<WrongAttempt> _wrongAttempts = new List<WrongAttempt> (3);

    GameObject menuScreen;
    Button menuRestartButton;
    Button menuHomeButton;
    Button menuBackButton;

    GameObject helpScreen;
    bool helpOpen;

    public List<int> pecasRestantes = new List<int> () {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 44, 45, 48, 50, 54, 55, 60, 64, 66, 72, 75, 80, 90, 96, 100, 108, 120, 125, 144, 150, 180
    };

    public bool gameHasEnded = false;

    public Jogador jogador;
    public TextMeshProUGUI txtA;
    public TextMeshProUGUI txtB;
    public TextMeshProUGUI txtC;
    public TextMeshProUGUI txtVidas;
    public TextMeshProUGUI txtResultadoApresentado;
    public TextMeshProUGUI txtPontos;
    TextMeshProUGUI txtNotifications;

    TextMeshProUGUI txtDiceRowHint;
    TextMeshProUGUI txtPecasRestantesTitulo;
    TextMeshProUGUI txtPecasRestantesLista;
    TextMeshProUGUI txtPecasRestantesListaBoard;

    Canvas rootCanvas;
    RectTransform canvasRt;
    RectTransform safeAreaRt;
    RectTransform pecasPanelRt;

    const float BoardPad = 8f;
    const float BoardSpacing = 6f;
    const float GapBelowPecas = 8f;
    /// <summary>Altura da faixa sob o tabuleiro com os valores das casas brancas por marcar (lista completa com wrap).</summary>
    const float PecasRestantesStripHeight = 189f;
    const float GapAboveBoardDice = 8f;
    /// <summary>Espaço entre a faixa dos dados e o bloco do timer.</summary>
    const float GapTimerRowAboveDice = 22f;
    /// <summary>Espaço entre o timer e a linha (vidas | trocar | marcadas).</summary>
    const float GapTopHudAboveTimer = 18f;
    /// <summary>Espaço entre a linha de vidas/trocar e o painel de notificações.</summary>
    const float GapNotificationsAboveTopHud = 10f;
    /// <summary>Altura da faixa com (vidas | trocar | marcadas).</summary>
    const float TopHudRowHeight = 72f;
    /// <summary>Altura do painel de notificações (desbloqueios, dicas), sem bloquear gameplay.</summary>
    const float NotificationsRowHeight = 74f;
    /// <summary>Altura do bloco do timer (sozinho, maior).</summary>
    const float TimerOnlyRowHeight = 84f;
    /// <summary>Altura reservada acima do tabuleiro (dados ~116px + margens do layout).</summary>
    const float DiceStripHeight = 152f;
    const float DiceSidePreferred = 116f;
    const float DiceFontPreferred = 72f;
    const float DiceHlgSpacing = 28f;
    RectTransform boardGridRoot;
    RectTransform topHudRowAboveTimerRt;
    RectTransform notificationsRowRt;
    RectTransform timerRowAboveDiceRt;
    RectTransform diceAboveBoardRt;
    ContiGoBoardCell2D[,] piecesInBoard;
    readonly List<ContiGoBoardCell2D> tabuleiroPecas = new List<ContiGoBoardCell2D> ();
    /// <summary>Valores embaralhados desta partida (mesma ordem do tabuleiro).</summary>
    List<int> _sessionBoardValues = new List<int> ();
    List<int> _pendingBoardValues = new List<int> ();
    int _cellGoalCount = 64;

    float nextInputTime;
    const float MinTapInterval = 0.22f;
    /// <summary>Após tocar numa casa branca, bloqueia todas as casas brancas (incluindo a tocada) e o próximo toque global durante este tempo.</summary>
    const float BoardInputLockSeconds = 0.5f;

    void Awake ()
    {
        SoundManager.Initialize ();
        EnsureEventSystem ();
        LoadLanguage ();
        BuildUi ();
        ApplyLocalizedStaticStrings ();
    }

    void Start ()
    {
        InitializeGame ();
    }

    void LoadLanguage ()
    {
        if (!ES2.Exists ("language")) {
            ES2.Save ("portuguese", "language");
        }
        language = ES2.Load<string> ("language");
    }

    void EnsureEventSystem ()
    {
        if (UnityEngine.Object.FindFirstObjectByType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }
    }

    void ApplyLocalizedStaticStrings ()
    {
        if (txtDiceRowHint != null) {
            txtDiceRowHint.text = language == "portuguese"
                ? "Valores sorteados — use +, -, x, / e parênteses para obter o valor de uma casa branca no tabuleiro."
                : "Drawn values — use +, -, x, / and parentheses to match a white cell on the board.";
        }
        string tituloPecas = language == "portuguese"
            ? "Valores que ainda estão no tabuleiro (casas brancas por marcar). Toque na casa cujo valor é o resultado da sua conta:"
            : "Values still on the board (unmarked white cells). Tap the cell whose value equals your calculation:";
        if (txtPecasRestantesTitulo != null)
            txtPecasRestantesTitulo.text = tituloPecas;
        AtualizarPecasRestantesTexto ();
        if (btnPularRodada != null) {
            Transform tr = btnPularRodada.transform.Find ("Txt");
            if (tr != null) {
                TextMeshProUGUI tmp = tr.GetComponent<TextMeshProUGUI> ();
                if (tmp != null)
                    tmp.text = language == "portuguese" ? "Trocar" : "Swap";
            }
        }
    }

    void AtualizarPecasRestantesTexto ()
    {
        if (txtPecasRestantesLista == null && txtPecasRestantesListaBoard == null)
            return;
        if (pecasRestantes == null || pecasRestantes.Count == 0) {
            string empty = language == "portuguese"
                ? "(nenhum — tabuleiro completo!)"
                : "(none — board complete!)";
            if (txtPecasRestantesLista != null)
                txtPecasRestantesLista.text = empty;
            if (txtPecasRestantesListaBoard != null)
                txtPecasRestantesListaBoard.text = empty;
            return;
        }
        var sorted = pecasRestantes.OrderBy (x => x).ToList ();
        // Gameplay: manter só valores numéricos para ocupar menos espaço (nomes ficam para desbloqueios/cartas/feedback).
        string listaVirgula = string.Join (", ", sorted);
        string listaHifen = string.Join (" - ", sorted.ConvertAll (x => x.ToString ()));
        if (txtPecasRestantesLista != null) {
            txtPecasRestantesLista.text = language == "portuguese"
                ? "(" + pecasRestantes.Count + " valores) " + listaVirgula
                : "(" + pecasRestantes.Count + " values) " + listaVirgula;
        }
        if (txtPecasRestantesListaBoard != null) {
            txtPecasRestantesListaBoard.text = language == "portuguese"
                ? "(" + pecasRestantes.Count + " valores) " + listaHifen
                : "(" + pecasRestantes.Count + " values) " + listaHifen;
        }
    }

    void RefreshMainTimerLabel ()
    {
        if (txtMainTimer == null)
            return;
        txtMainTimer.text = TimeConverter (Mathf.Max (0f, timer));
    }

    void MenuPressed ()
    {
        if (gameHasEnded)
            return;
        if (helpOpen)
            CloseHelp ();
        if (menuOpen) {
            CloseMenu ();
        } else {
            OpenMenu ();
        }
    }

    void HelpPressed ()
    {
        if (gameHasEnded)
            return;
        if (helpOpen) {
            CloseHelp ();
        } else {
            OpenHelp ();
        }
    }

    void OpenHelp ()
    {
        if (helpOpen)
            return;
        helpOpen = true;
        gameOnBeforeMenu = gameOn;
        gameOn = false;
        if (helpScreen != null)
            helpScreen.SetActive (true);
        if (helpScreen != null)
            helpScreen.transform.SetAsLastSibling ();
        ResetTutorialSlides ();
        SoundManager.PlayContiGo2DMenuOrHelpOpenSound ();
    }

    void CloseHelp ()
    {
        if (!helpOpen)
            return;
        helpOpen = false;
        if (helpScreen != null)
            helpScreen.SetActive (false);
        if (!gameHasEnded && !gameStrategicTimer && !menuOpen) {
            gameOn = gameOnBeforeMenu;
        }
        SoundManager.PlaySound (SoundManager.Sound.UICancel);
    }

    void OpenMenu ()
    {
        if (menuOpen)
            return;
        menuOpen = true;
        gameOnBeforeMenu = gameOn;
        gameOn = false;
        if (menuScreen != null)
            menuScreen.SetActive (true);
        SoundManager.PlayContiGo2DMenuOrHelpOpenSound ();
    }

    void CloseMenu ()
    {
        if (!menuOpen)
            return;
        menuOpen = false;
        if (menuScreen != null)
            menuScreen.SetActive (false);
        if (!gameHasEnded && !gameStrategicTimer) {
            gameOn = gameOnBeforeMenu;
        }
        SoundManager.PlaySound (SoundManager.Sound.UICancel);
    }

    void MenuRestartPressed ()
    {
        CloseMenu ();
        RestartPressed ();
    }

    void MenuHomePressed ()
    {
        CloseMenu ();
        HomePressed ();
    }

    void MenuBackPressed ()
    {
        CloseMenu ();
    }

    void PularRodadaPressed ()
    {
        if (!gameOn || gameHasEnded)
            return;
        if (Time.unscaledTime < nextInputTime)
            return;
        nextInputTime = Time.unscaledTime + BoardInputLockSeconds;
        SetAllUnmarkedCellsInteractable (false);
        if (btnPularRodada != null) {
            Button b = btnPularRodada.GetComponent<Button> ();
            if (b != null)
                b.interactable = false;
        }
        StartCoroutine (CoReenableUnmarkedBoardCells ());
        bool resposta = VerificaResultadosPossiveis (pecasRestantes, jogador.a, jogador.b, jogador.c);
        SoundManager.PlaySound (resposta ? SoundManager.Sound.PieceFalse : SoundManager.Sound.PieceTrue);
    }

    void RestartPressed ()
    {
        gameOverScreen.SetActive (false);
        ResetGame ();
        InitializeGame ();
    }

    void HomePressed ()
    {
        SceneManager.LoadScene ("Home");
    }

    public void InitializeGame ()
    {
        StopAllCoroutines ();
        _wrongAttempts.Clear ();

        int[] baseVals = ContiGo2DLevelCatalog.GetBoardValues (levelId);
        _sessionBoardValues = ContiGo2DLevelCatalog.ShuffleCopy (baseVals);
        _pendingBoardValues = new List<int> (_sessionBoardValues);
        _cellGoalCount = _sessionBoardValues.Count;
        pecasRestantes = new List<int> (_sessionBoardValues);
        ResetMissionSessionForNewMatch ();

        EnsureTimerSettingsExist ();
        mainTimer = ContiGo2DLevelCatalog.MainTimeSeconds (levelId);
        strategicTimer = ES2.Load<int> ("strategicTime");
        gameHasEnded = false;

        timer = mainTimer;
        canCount = true;
        doOnce = false;

        Dado dado1 = new Dado ();
        int v1 = dado1.RolarDado (1, 7);
        int v2 = dado1.RolarDado (1, 7);
        int v3 = dado1.RolarDado (1, 7);

        jogador.setIdPlayer (1);
        jogador.AtualizarDadosJogador (1, 3, 0, v1, v2, v3, 0, 0f, 0, jogador.ResultadoApresentado);
        AtualizarHud ("");
        RefreshMainTimerLabel ();
        canCountStrategic = false;
        gameOn = false;

        TMP_FontAsset boardFont = ContiGo2DSharedUi.GetBoardCellFont ();
        StartCoroutine (CoBuildBoardAfterLayout (boardFont));
    }

    void EnsureTimerSettingsExist ()
    {
        if (!ES2.Exists ("mainTime")) {
            ES2.Save (300, "mainTime");
        }
        if (!ES2.Exists ("playerTime")) {
            ES2.Save (30, "playerTime");
        }
        if (!ES2.Exists ("strategicTime")) {
            ES2.Save (5, "strategicTime");
        }
    }

    void AtualizarHud (string resultadoApresentado)
    {
        txtA.text = jogador.a.ToString ();
        txtB.text = jogador.b.ToString ();
        txtC.text = jogador.c.ToString ();

        if (language == "portuguese") {
            txtVidas.text = jogador.vidas.ToString ();
            txtPontos.text = (int)jogador.pontos + " / " + _cellGoalCount;
            txtResultadoApresentado.text = string.IsNullOrEmpty (resultadoApresentado)
                ? ""
                : "Resultado: " + resultadoApresentado;
        } else {
            txtVidas.text = jogador.vidas.ToString ();
            txtPontos.text = (int)jogador.pontos + " / " + _cellGoalCount;
            txtResultadoApresentado.text = string.IsNullOrEmpty (resultadoApresentado)
                ? ""
                : "Result: " + resultadoApresentado;
        }

        AtualizarPecasRestantesTexto ();
    }

    void SetAllUnmarkedCellsInteractable (bool interactable)
    {
        foreach (ContiGoBoardCell2D c in tabuleiroPecas) {
            if (c == null || c.status != 0)
                continue;
            if (c.button != null)
                c.button.interactable = interactable;
        }
    }

    IEnumerator CoReenableUnmarkedBoardCells ()
    {
        yield return new WaitForSecondsRealtime (BoardInputLockSeconds);
        foreach (ContiGoBoardCell2D c in tabuleiroPecas) {
            if (c == null || c.status != 0)
                continue;
            if (c.button != null)
                c.button.interactable = true;
        }
        if (btnPularRodada != null) {
            Button b = btnPularRodada.GetComponent<Button> ();
            if (b != null)
                b.interactable = true;
        }
    }

    void OnCellClicked (ContiGoBoardCell2D cell)
    {
        if (!gameOn || gameHasEnded || gameStrategicTimer)
            return;
        if (Time.unscaledTime < nextInputTime)
            return;
        if (cell.status != 0)
            return;

        nextInputTime = Time.unscaledTime + BoardInputLockSeconds;
        SetAllUnmarkedCellsInteractable (false);
        StartCoroutine (CoReenableUnmarkedBoardCells ());

        bool ok = VerificaEscolha (cell, jogador.a, jogador.b, jogador.c);
        if (ok) {
            cell.SetMarked (jogador.getIdPlayer ());
            SoundManager.PlaySound (SoundManager.Sound.PieceTrue);
            if (pecasRestantes.Count == 0) {
                Victory ();
            }
        } else {
            SoundManager.PlaySound (SoundManager.Sound.PieceFalse);
            StartCoroutine (CoWrongCellFlash (cell));
        }
    }

    IEnumerator CoWrongCellFlash (ContiGoBoardCell2D cell)
    {
        if (cell == null)
            yield break;
        cell.ApplyWrongAnswerVisual ();
        yield return new WaitForSecondsRealtime (0.38f);
        if (cell != null)
            cell.RestoreUnmarkedAppearance ();
    }

    bool VerificaEscolha (ContiGoBoardCell2D peca, int a, int b, int c)
    {
        CalculoData calculo = new CalculoData ();
        List<float> resultados = calculo.ExecutaFormulas (a, b, c);
        ValidaContaData calculoResultado = new ValidaContaData ();
        calculoResultado.ContaValidada (false, resultados, peca.valor);

        if (calculoResultado.confere) {
            Dado dado = new Dado ();
            int v1 = dado.RolarDado (1, 7);
            int v2 = dado.RolarDado (1, 7);
            int v3 = dado.RolarDado (1, 7);
            string msg = ContiGoFantasyNames.FormatHitFeedback (peca.valor, language == "portuguese");
            jogador.AtualizarDadosJogador (jogador.getIdPlayer (), jogador.vidas, 0, v1, v2, v3, 0, 0f, jogador.pontos + 1, msg);
            TiraItemDaLista (pecasRestantes, peca.valor);
            OnMissionCorrectHit (peca.valor);
            AtualizarHud (msg);
            return true;
        }

        RegisterWrongAttempt (a, b, c, peca != null ? peca.valor : 0);
        OnMissionWrongChoice ();
        jogador.vidas -= 1;
        jogador.ResultadoApresentado = language == "portuguese" ? "errou!" : "fail!";
        AtualizarHud (jogador.ResultadoApresentado);
        NextTurn ();
        return false;
    }

    void RegisterWrongAttempt (int a, int b, int c, int choice)
    {
        if (_wrongAttempts.Count >= 3)
            return;
        _wrongAttempts.Add (new WrongAttempt (a, b, c, choice));
    }

    string BuildWrongAttemptsReport (bool pt)
    {
        if (_wrongAttempts == null || _wrongAttempts.Count == 0)
            return "";

        System.Text.StringBuilder sb = new System.Text.StringBuilder (128);
        sb.Append ("\n\n");
        sb.Append (pt ? "ERROS" : "MISTAKES");

        for (int i = 0; i < _wrongAttempts.Count; i++) {
            WrongAttempt wa = _wrongAttempts[i];
            sb.Append ("\n");
            sb.Append (i + 1);
            sb.Append (":\n");
            sb.Append (pt ? "valores: " : "values: ");
            sb.Append (wa.a);
            sb.Append (" - ");
            sb.Append (wa.b);
            sb.Append (" - ");
            sb.Append (wa.c);
            sb.Append ("\n");
            sb.Append (pt ? "sua escolha: " : "your choice: ");
            sb.Append (wa.choice);
            if (i < _wrongAttempts.Count - 1)
                sb.Append ("\n");
        }

        return sb.ToString ();
    }

    void RefreshGameOverErrorsUi (bool pt)
    {
        if (gameOverErrorsBlock == null || gameOverErrorsRoot == null) {
            return;
        }

        // Limpa itens antigos
        for (int i = gameOverErrorsRoot.childCount - 1; i >= 0; i--) {
            Transform ch = gameOverErrorsRoot.GetChild (i);
            if (ch != null)
                Destroy (ch.gameObject);
        }

        if (_wrongAttempts == null || _wrongAttempts.Count == 0) {
            gameOverErrorsBlock.SetActive (false);
            return;
        }

        gameOverErrorsBlock.SetActive (true);

        TMP_FontAsset font = _uiFont != null ? _uiFont : ContiGo2DSharedUi.GetBoardCellFont ();

        for (int i = 0; i < _wrongAttempts.Count; i++) {
            WrongAttempt wa = _wrongAttempts[i];
            CreateGameOverErrorRow (gameOverErrorsRoot, i + 1, wa, pt, font);
        }
    }

    void CreateGameOverErrorRow (RectTransform parent, int index, WrongAttempt wa, bool pt, TMP_FontAsset font)
    {
        GameObject row = new GameObject ("Error_" + index, typeof (RectTransform));
        row.transform.SetParent (parent, false);
        RectTransform rt = row.GetComponent<RectTransform> ();
        rt.sizeDelta = new Vector2 (0f, 0f);

        LayoutElement le = row.AddComponent<LayoutElement> ();
        // Linha única de tabela: “a-b-c | escolha”
        le.preferredHeight = 66f;
        le.flexibleWidth = 1f;

        Image bg = row.AddComponent<Image> ();
        if (_gameOverErrorItemFrameSprite != null) {
            bg.sprite = _gameOverErrorItemFrameSprite;
            bg.type = _gameOverErrorItemFrameSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            bg.preserveAspect = false;
        } else {
            bg.sprite = RoundedRectSpriteFactory.Get (64, 12);
            bg.type = Image.Type.Sliced;
        }
        bg.color = Color.white;
        bg.raycastTarget = false;

        GameObject inner = new GameObject ("Inner", typeof (RectTransform));
        inner.transform.SetParent (row.transform, false);
        RectTransform irt = inner.GetComponent<RectTransform> ();
        irt.anchorMin = Vector2.zero;
        irt.anchorMax = Vector2.one;
        // Mais respiro vertical para não cortar a fonte em alguns devices.
        irt.offsetMin = new Vector2 (16f, 12f);
        irt.offsetMax = new Vector2 (-16f, -12f);

        string valores = wa.a + "-" + wa.b + "-" + wa.c;
        TextMeshProUGUI left = CreateTmp (inner.transform, "Valores", valores, 24f, TextAlignmentOptions.MidlineLeft, font);
        left.raycastTarget = false;
        RectTransform lrt = left.rectTransform;
        lrt.anchorMin = new Vector2 (0f, 0f);
        lrt.anchorMax = new Vector2 (0.70f, 1f);
        lrt.offsetMin = new Vector2 (6f, 0f);
        lrt.offsetMax = Vector2.zero;

        TextMeshProUGUI right = CreateTmp (inner.transform, "Escolha", wa.choice.ToString (), 24f, TextAlignmentOptions.MidlineRight, font);
        right.raycastTarget = false;
        RectTransform rrt = right.rectTransform;
        rrt.anchorMin = new Vector2 (0.70f, 0f);
        rrt.anchorMax = new Vector2 (1f, 1f);
        rrt.offsetMin = Vector2.zero;
        rrt.offsetMax = new Vector2 (-6f, 0f);
    }

    public bool VerificaResultadosPossiveis (List<int> pecasRest, float a, float b, float c)
    {
        CalculoData contas = new CalculoData ();
        List<ValidaContaData> valeounaoData = contas.ContasPossiveis (pecasRest, a, b, c);
        bool existe = contas.VerificaValidade (valeounaoData);

        if (existe) {
            OnMissionWrongChoice ();
            jogador.ResultadoApresentado = language == "portuguese" ? "errou!" : "fail!";
            Dado dado = new Dado ();
            int v1 = dado.RolarDado (1, 7);
            int v2 = dado.RolarDado (1, 7);
            int v3 = dado.RolarDado (1, 7);
            jogador.vidas -= 1;
            jogador.AtualizarDadosJogador (jogador.getIdPlayer (), jogador.vidas, 0, v1, v2, v3, 0, 0f, jogador.pontos, jogador.ResultadoApresentado);
            AtualizarHud ("");
            NextTurn ();
            return existe;
        }

        jogador.ResultadoApresentado = language == "portuguese" ? "acertou" : "correct!";
        SoundManager.PlaySound (SoundManager.Sound.PieceTrue);
        Dado dado2 = new Dado ();
        int w1 = dado2.RolarDado (1, 7);
        int w2 = dado2.RolarDado (1, 7);
        int w3 = dado2.RolarDado (1, 7);
        jogador.AtualizarDadosJogador (jogador.getIdPlayer (), jogador.vidas, 0, w1, w2, w3, 0, 0f, jogador.pontos, jogador.ResultadoApresentado);
        AtualizarHud (jogador.ResultadoApresentado);
        NextTurn ();
        return existe;
    }

    void TiraItemDaLista (List<int> pecas, int peca)
    {
        for (int i = 0; i < pecas.Count; i++) {
            if (pecas[i] == peca) {
                pecas.RemoveAt (i);
                break;
            }
        }
    }

    void NextTurn ()
    {
        Dado dado = new Dado ();
        int v1 = dado.RolarDado (1, 7);
        int v2 = dado.RolarDado (1, 7);
        int v3 = dado.RolarDado (1, 7);
        jogador.AtualizarDadosJogador (jogador.getIdPlayer (), jogador.vidas, 0, v1, v2, v3, 0, 0f, jogador.pontos, jogador.ResultadoApresentado);
        AtualizarHud (jogador.ResultadoApresentado);

        if (jogador.vidas <= 0) {
            GameOver ();
        }
    }

    void SaveRanking ()
    {
        // Ranking partilhado só faz sentido no 8×8 (mesmo teto de pontos e tempo de referência).
        if (levelId != ContiGo2DLevelId.Mestre)
            return;
        RankingData rd = new RankingData ();
        List<RankingEntry> rankingPego = rd.LoadRankingData ();
        int pts = (int)jogador.pontos;
        int timeRemaining = Mathf.CeilToInt (Mathf.Max (0f, timer));
        int timeSpent = Mathf.CeilToInt (Mathf.Max (0f, mainTimer - Mathf.Max (0f, timer)));
        RankingEntry entry = new RankingEntry (pts, timeSpent);
        entry.timeRemainingSeconds = timeRemaining;
        rankingPego.Add (entry);
        List<RankingEntry> rankingAtualizado = rankingPego
            .OrderBy (e => e, Comparer<RankingEntry>.Create (RankingEntry.Compare))
            .Take (10)
            .ToList ();
        rd.SaveRankingData (rankingAtualizado);
        // Tenta reportar esta partida imediatamente (se autenticado) e, em seguida,
        // garante que o melhor local fica sincronizado quando necessário.
        PlayGamesController.PostToLeaderboard (pts);
        PlayGamesController.FlushBestLocalIfNeeded ();
    }

    void GameOver ()
    {
        gameOn = false;
        if (gameHasEnded)
            return;
        gameHasEnded = true;
        if (btnPause != null)
            btnPause.SetActive (false);
        if (btnPularRodada != null)
            btnPularRodada.SetActive (false);
        if (btnHelp != null)
            btnHelp.SetActive (false);
        gameOverScreen.SetActive (true);
        SaveRanking ();
        bool pt = language == "portuguese";
        gameOverMainText.text = pt ? (jogador.pontos + " pontos") : (jogador.pontos + " points");
        RefreshGameOverErrorsUi (pt);
    }

    void Victory ()
    {
        if (gameHasEnded)
            return;
        OnMissionVictory ();
        gameOn = false;
        gameHasEnded = true;
        if (btnPause != null)
            btnPause.SetActive (false);
        if (btnPularRodada != null)
            btnPularRodada.SetActive (false);
        if (btnHelp != null)
            btnHelp.SetActive (false);
        gameOverScreen.SetActive (true);
        SaveRanking ();

        // Leaderboard de tempo: só quando vence o 8×8 (Mestre).
        if (levelId == ContiGo2DLevelId.Mestre) {
            int timeSpentSeconds = Mathf.CeilToInt (Mathf.Max (0f, mainTimer - Mathf.Max (0f, timer)));
            long timeMs = (long)timeSpentSeconds * 1000L;
            PlayGamesController.PostTimeToLeaderboardMs (timeMs);
        }
        gameOverMainText.text = language == "portuguese"
            ? "Venceu! " + jogador.pontos + " pontos"
            : "You win! " + jogador.pontos + " points";
        RefreshGameOverErrorsUi (language == "portuguese");
        SoundManager.PlaySound (SoundManager.Sound.UIConfimation);
    }

    void GameoverForMainTime ()
    {
        gameOn = false;
        if (gameHasEnded)
            return;
        gameHasEnded = true;
        if (btnPause != null)
            btnPause.SetActive (false);
        if (btnPularRodada != null)
            btnPularRodada.SetActive (false);
        if (btnHelp != null)
            btnHelp.SetActive (false);
        gameOverScreen.SetActive (true);
        SaveRanking ();
        bool pt = language == "portuguese";
        gameOverMainText.text = pt ? ("Tempo esgotado — " + jogador.pontos + " pontos") : ("Time up — " + jogador.pontos + " points");
        RefreshGameOverErrorsUi (pt);
    }

    void Update ()
    {
        if (gameOn) {
            if (timer >= 0f && canCount) {
                timer -= Time.deltaTime;
                RefreshMainTimerLabel ();
            } else if (timer <= 0f && !doOnce) {
                canCount = false;
                doOnce = true;
                timer = 0f;
                RefreshMainTimerLabel ();
                GameoverForMainTime ();
            }
        } else if (gameStrategicTimer) {
            if (strategicTimer >= 0f && canCountStrategic) {
                strategicTimer -= Time.deltaTime;
                txtStrategicTimer.text = TimeConverterSeconds (strategicTimer);
            } else if (strategicTimer <= 0f) {
                canCountStrategic = false;
                txtStrategicTimer.text = "";
                strategicTimer = 0f;
                strategicTimerScreen.SetActive (false);
                gameStrategicTimer = false;
                if (btnPause != null)
                    btnPause.SetActive (true);
                if (btnPularRodada != null)
                    btnPularRodada.SetActive (true);
                if (btnHelp != null)
                    btnHelp.SetActive (true);
                PlayGame ();
            }
        }
    }

    public string TimeConverter (float num)
    {
        TimeSpan spanTime = TimeSpan.FromSeconds (num);
        int minutes = Mathf.Max (0, spanTime.Minutes);
        int seconds = Mathf.Clamp (spanTime.Seconds, 0, 59);
        return minutes.ToString () + ":" + seconds.ToString ("00");
    }

    public string TimeConverterSeconds (float num)
    {
        TimeSpan spanTime = TimeSpan.FromSeconds (num);
        return spanTime.Seconds.ToString ();
    }

    public void PlayGame ()
    {
        gameOn = true;
        OnMatchPlayStarted ();
        SoundManager.PlaySound (SoundManager.Sound.UIConfimation);
    }

    public void PlayStrategicTimer ()
    {
        gameOn = false;
        strategicTimerScreen.SetActive (true);
        gameStrategicTimer = true;
        canCountStrategic = true;
    }

    public void ResetGame ()
    {
        foreach (ContiGoBoardCell2D c in tabuleiroPecas) {
            if (c != null)
                Destroy (c.gameObject);
        }
        tabuleiroPecas.Clear ();
        _wrongAttempts.Clear ();
        if (_sessionBoardValues != null && _sessionBoardValues.Count > 0)
            pecasRestantes = new List<int> (_sessionBoardValues);
        EnsureTimerSettingsExist ();
        mainTimer = ContiGo2DLevelCatalog.MainTimeSeconds (levelId);
        strategicTimer = ES2.Load<int> ("strategicTime");
        gameHasEnded = false;
        timer = mainTimer;
        canCount = true;
        doOnce = false;
        canCountStrategic = false;
    }

    public void OnApplicationFocus (bool focusStatus)
    {
        if (!gameOn)
            return;
        if (!focusStatus) {
            DateTime horarioQuePerdeuFoco = DateTime.UtcNow;
            ES2.Save (horarioQuePerdeuFoco.ToBinary (), "currentMainTime");
        } else {
            if (!ES2.Exists ("currentMainTime"))
                return;
            currentMainTimer = ES2.Load<long> ("currentMainTime");
            DateTime recebe = DateTime.FromBinary (currentMainTimer);
            DateTime agora = DateTime.UtcNow;
            TimeSpan elapsed = agora.Subtract (recebe);
            float timeLapsed = timer - (float)elapsed.TotalSeconds;
            if (timeLapsed <= 0) {
                GameoverForMainTime ();
            } else {
                timer = timeLapsed;
            }
        }
    }
}
