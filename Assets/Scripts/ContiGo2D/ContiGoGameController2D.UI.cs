using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class ContiGoGameController2D
{
    /// <summary> Arte dos botões da Home em px (largura × altura); mantém proporção no HUD/modal. </summary>
    const float HomeButtonArtWidthPx = 425f;
    const float HomeButtonArtHeightPx = 390f;
    /// <summary> Altura dos ícones no modal do menu (proporção da arte; ~2.5× face à versão inicial de 44px). </summary>
    const float MenuModalIconHeightPx = 110f;
    const float MenuModalHlgSpacing = 28f;
    /// <summary>Tamanho do hexágono no modal de pause (altura em px da referência 1080×1920).</summary>
    const float HexPauseMenuButtonPx = 132f;
    const float HexPauseMenuSpacingPx = 26f;
    const float HexPauseMenuPopupPadPx = 40f;

    Image tutorialSlideImage;
    TextMeshProUGUI tutorialSlideCounter;
    Button tutorialPrevButton;
    Button tutorialNextButton;
    Button tutorialCloseButton;
    readonly string[] tutorialSlideResourceNames = {
        "Imagens/Tutorial/01-EXEMPLO-CONTA.fw",
        "Imagens/Tutorial/02-COMO-JOGAR.fw",
        "Imagens/Tutorial/03-BENEFICIOS-PARA-O-CEREBRO.fw",
        "Imagens/Tutorial/04-EXEMPLO-CONTA-2.fw",
        "Imagens/Tutorial/05-DIVERTIDO-DESAFIADOR.fw"
    };
    Sprite[] tutorialSlides;
    int tutorialSlideIndex;
    const float TutorialSlideArtWidth = 1602f;
    const float TutorialSlideArtHeight = 2562f;
    AspectRatioFitter tutorialSlideArf;

    void BuildUi ()
    {
        TMP_FontAsset font = ContiGo2DSharedUi.GetBoardCellFont ();
        _uiFont = font;

        GameObject canvasGo = new GameObject ("Canvas2D");
        rootCanvas = canvasGo.AddComponent<Canvas> ();
        rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler> ();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2 (1080f, 1920f);
        scaler.matchWidthOrHeight = 0.52f;
        canvasGo.AddComponent<GraphicRaycaster> ();

        canvasRt = canvasGo.GetComponent<RectTransform> ();
        canvasRt.anchorMin = Vector2.zero;
        canvasRt.anchorMax = Vector2.one;
        canvasRt.offsetMin = Vector2.zero;
        canvasRt.offsetMax = Vector2.zero;

        Image bg = CreatePanel (canvasRt, "Background", Color.white);
        bg.rectTransform.SetAsFirstSibling ();
        RectTransform bgRt = bg.rectTransform;
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;
        bg.type = Image.Type.Tiled;
        bg.preserveAspect = false;
        // Fundo do gameplay (Resources/Imagens/*)
        Sprite bgSp = Resources.Load<Sprite> ("Imagens/bg-square-blue-vertical.fw");
        if (bgSp == null)
            bgSp = Resources.Load<Sprite> ("Imagens/bg-square-blue");
        if (bgSp != null)
            bg.sprite = bgSp;

        GameObject safeGo = new GameObject ("SafeArea", typeof (RectTransform));
        safeGo.transform.SetParent (canvasRt, false);
        safeAreaRt = safeGo.GetComponent<RectTransform> ();
        safeAreaRt.anchorMin = Vector2.zero;
        safeAreaRt.anchorMax = Vector2.one;
        safeAreaRt.offsetMin = Vector2.zero;
        safeAreaRt.offsetMax = Vector2.zero;
        safeGo.AddComponent<ContiGoSafeArea> ();

        GameObject jogadorGo = new GameObject ("JogadorState");
        jogadorGo.transform.SetParent (safeAreaRt, false);
        jogador = jogadorGo.AddComponent<Jogador> ();

        // Modal do tutorial (?) em slides (imagens).
        helpScreen = new GameObject ("HelpOverlay");
        helpScreen.transform.SetParent (canvasRt, false);
        RectTransform helpRt = helpScreen.AddComponent<RectTransform> ();
        helpRt.anchorMin = Vector2.zero;
        helpRt.anchorMax = Vector2.one;
        helpRt.offsetMin = Vector2.zero;
        helpRt.offsetMax = Vector2.zero;
        Image helpBg = helpScreen.AddComponent<Image> ();
        helpBg.color = new Color (0f, 0f, 0f, 0.6f);
        Button helpBgBtn = helpScreen.AddComponent<Button> ();
        helpBgBtn.targetGraphic = helpBg;
        helpBgBtn.onClick.AddListener (HelpPressed);
        helpScreen.SetActive (false);

        GameObject helpPanelGo = new GameObject ("HelpPanel", typeof (RectTransform));
        helpPanelGo.transform.SetParent (helpScreen.transform, false);
        RectTransform helpPanelRt = helpPanelGo.GetComponent<RectTransform> ();
        // Painel responsivo dentro da safe area (evita ficar fora do ecrã em celulares).
        helpPanelRt.anchorMin = new Vector2 (0.06f, 0.08f);
        helpPanelRt.anchorMax = new Vector2 (0.94f, 0.92f);
        helpPanelRt.pivot = new Vector2 (0.5f, 0.5f);
        helpPanelRt.offsetMin = Vector2.zero;
        helpPanelRt.offsetMax = Vector2.zero;
        // Mantém o painel mais "vertical" para aproveitar melhor o ecrã com slides retrato.
        AspectRatioFitter panelArf = helpPanelGo.AddComponent<AspectRatioFitter> ();
        panelArf.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        panelArf.aspectRatio = TutorialSlideArtWidth / TutorialSlideArtHeight;
        Image helpPanelBg = helpPanelGo.AddComponent<Image> ();
        helpPanelBg.color = new Color (0.12f, 0.13f, 0.16f, 1f);
        helpPanelGo.AddComponent<RectMask2D> ();

        // Barra superior (separada da imagem): contador + fechar.
        GameObject topBarGo = new GameObject ("TutorialTopBar", typeof (RectTransform));
        topBarGo.transform.SetParent (helpPanelGo.transform, false);
        RectTransform topBarRt = topBarGo.GetComponent<RectTransform> ();
        // Barra no topo, em faixa separada (sem sobrepor a imagem).
        // Usa altura fixa para podermos "cortar" a área do slide com offsetMax.
        const float TopBarHeightPx = 96f;
        topBarRt.anchorMin = new Vector2 (0f, 1f);
        topBarRt.anchorMax = new Vector2 (1f, 1f);
        topBarRt.pivot = new Vector2 (0.5f, 1f);
        topBarRt.anchoredPosition = Vector2.zero;
        topBarRt.sizeDelta = new Vector2 (0f, TopBarHeightPx);
        Image topBarBg = topBarGo.AddComponent<Image> ();
        topBarBg.color = new Color (0f, 0f, 0f, 0.75f);
        topBarBg.raycastTarget = false;

        tutorialSlideCounter = CreateTmp (topBarRt, "SlideCounter", "1 / 5", 34f, TextAlignmentOptions.Center, font);
        RectTransform cntRt = tutorialSlideCounter.rectTransform;
        // Menos padding vertical: mais compacto.
        cntRt.anchorMin = new Vector2 (0.18f, 0.18f);
        cntRt.anchorMax = new Vector2 (0.82f, 0.82f);
        cntRt.offsetMin = Vector2.zero;
        cntRt.offsetMax = Vector2.zero;

        tutorialCloseButton = CreateImagensSpriteButton (topBarRt, "CloseHelp", "btn-close-home.fw", HelpPressed);
        RectTransform hcRt = tutorialCloseButton.GetComponent<RectTransform> ();
        hcRt.anchorMin = new Vector2 (1f, 0.5f);
        hcRt.anchorMax = new Vector2 (1f, 0.5f);
        hcRt.pivot = new Vector2 (1f, 0.5f);
        hcRt.sizeDelta = new Vector2 (68f, 68f);
        // Padding para nunca sair para fora à direita em ecrãs estreitos.
        hcRt.anchoredPosition = new Vector2 (-12f, 0f);

        // Imagem principal do slide.
        GameObject slideGo = new GameObject ("TutorialSlide", typeof (RectTransform));
        slideGo.transform.SetParent (helpPanelGo.transform, false);
        // Garante que o topo fica sempre por cima do slide na hierarquia/UI draw order.
        topBarGo.transform.SetAsLastSibling ();
        RectTransform slideRt = slideGo.GetComponent<RectTransform> ();
        // Área da imagem abaixo da barra superior (não sobrepõe).
        slideRt.anchorMin = Vector2.zero;
        slideRt.anchorMax = Vector2.one;
        slideRt.offsetMin = new Vector2 (18f, 18f);
        slideRt.offsetMax = new Vector2 (-18f, -(TopBarHeightPx + 18f));
        tutorialSlideImage = slideGo.AddComponent<Image> ();
        tutorialSlideImage.color = Color.white;
        tutorialSlideImage.preserveAspect = true;
        tutorialSlideArf = slideGo.AddComponent<AspectRatioFitter> ();
        tutorialSlideArf.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        tutorialSlideArf.aspectRatio = TutorialSlideArtWidth / TutorialSlideArtHeight;
        Button slideBtn = slideGo.AddComponent<Button> ();
        slideBtn.targetGraphic = tutorialSlideImage;
        slideBtn.onClick.AddListener (NextTutorialSlide);

        // Setas sobrepostas (centradas verticalmente) por cima da imagem.
        tutorialPrevButton = CreateImagensSpriteButton (helpPanelRt, "PrevSlide", "grey_sliderLeft", PrevTutorialSlide);
        ForceButtonSpriteOrFallback (tutorialPrevButton, "grey_sliderLeft");
        SetButtonAlpha (tutorialPrevButton, 0.60f);
        RectTransform prevRt = tutorialPrevButton.GetComponent<RectTransform> ();
        prevRt.anchorMin = new Vector2 (0f, 0.5f);
        prevRt.anchorMax = new Vector2 (0f, 0.5f);
        prevRt.pivot = new Vector2 (0.5f, 0.5f);
        prevRt.sizeDelta = new Vector2 (110f, 110f);
        prevRt.anchoredPosition = new Vector2 (56f, 0f);

        tutorialNextButton = CreateImagensSpriteButton (helpPanelRt, "NextSlide", "grey_sliderRight", NextTutorialSlide);
        ForceButtonSpriteOrFallback (tutorialNextButton, "grey_sliderRight");
        SetButtonAlpha (tutorialNextButton, 0.60f);
        RectTransform nextRt = tutorialNextButton.GetComponent<RectTransform> ();
        nextRt.anchorMin = new Vector2 (1f, 0.5f);
        nextRt.anchorMax = new Vector2 (1f, 0.5f);
        nextRt.pivot = new Vector2 (0.5f, 0.5f);
        nextRt.sizeDelta = new Vector2 (110f, 110f);
        nextRt.anchoredPosition = new Vector2 (-56f, 0f);

        boardGridRoot = CreatePanel (safeAreaRt, "BoardGrid", Color.clear).rectTransform;
        boardGridRoot.GetComponent<Image> ().raycastTarget = false;

        pecasPanelRt = CreatePanel (safeAreaRt, "PecasRestantesStrip", new Color (0f, 0f, 0f, 0.20f)).rectTransform;
        pecasPanelRt.gameObject.AddComponent<RectMask2D> ();
        Image pecasStripImg = pecasPanelRt.GetComponent<Image> ();
        pecasStripImg.raycastTarget = false;
        pecasStripImg.sprite = RoundedRectSpriteFactory.Get (64, 10);
        pecasStripImg.type = Image.Type.Sliced;

        txtPecasRestantesListaBoard = CreateTmp (pecasPanelRt, "PecasListaBoard", "", 24f, TextAlignmentOptions.TopLeft, font);
        txtPecasRestantesListaBoard.enableWordWrapping = true;
        txtPecasRestantesListaBoard.overflowMode = TextOverflowModes.Overflow;
        RectTransform listaBoardRt = txtPecasRestantesListaBoard.rectTransform;
        listaBoardRt.anchorMin = Vector2.zero;
        listaBoardRt.anchorMax = Vector2.one;
        listaBoardRt.offsetMin = new Vector2 (10f, 8f);
        listaBoardRt.offsetMax = new Vector2 (-10f, -8f);

        diceAboveBoardRt = CreatePanel (safeAreaRt, "DiceAboveBoard", Color.clear).rectTransform;
        diceAboveBoardRt.GetComponent<Image> ().raycastTarget = false;
        HorizontalLayoutGroup diceHlg = diceAboveBoardRt.gameObject.AddComponent<HorizontalLayoutGroup> ();
        diceHlg.childAlignment = TextAnchor.MiddleCenter;
        diceHlg.spacing = DiceHlgSpacing;
        diceHlg.padding = new RectOffset (8, 8, 8, 8);
        diceHlg.childControlWidth = false;
        diceHlg.childControlHeight = false;
        diceHlg.childForceExpandWidth = false;
        diceHlg.childForceExpandHeight = false;

        CreateDiceSlotSquare (diceAboveBoardRt, "DieA", font, out txtA);
        CreateDiceSlotSquare (diceAboveBoardRt, "DieB", font, out txtB);
        CreateDiceSlotSquare (diceAboveBoardRt, "DieC", font, out txtC);

        // Painel de notificações (acima de vidas|trocar|marcadas). Não bloqueia gameplay.
        notificationsRowRt = CreatePanel (safeAreaRt, "NotificationsRow", new Color (0f, 0f, 0f, 0.26f)).rectTransform;
        Image notifImg = notificationsRowRt.GetComponent<Image> ();
        notifImg.raycastTarget = false;
        notifImg.sprite = RoundedRectSpriteFactory.Get (64, 10);
        notifImg.type = Image.Type.Sliced;

        txtNotifications = CreateTmp (notificationsRowRt, "NotificationsText", "", 30f, TextAlignmentOptions.Center, font);
        txtNotifications.raycastTarget = false;
        txtNotifications.enableWordWrapping = false;
        txtNotifications.overflowMode = TextOverflowModes.Ellipsis;
        txtNotifications.alpha = 0f;
        RectTransform ntRt = txtNotifications.rectTransform;
        ntRt.anchorMin = new Vector2 (0.03f, 0.12f);
        ntRt.anchorMax = new Vector2 (0.97f, 0.88f);
        ntRt.offsetMin = Vector2.zero;
        ntRt.offsetMax = Vector2.zero;

        // Linha superior: vidas (esq) | trocar rodada (centro) | marcadas (dir)
        topHudRowAboveTimerRt = CreatePanel (safeAreaRt, "TopHudRow", new Color (0f, 0f, 0f, 0.22f)).rectTransform;
        Image topHudImg = topHudRowAboveTimerRt.GetComponent<Image> ();
        topHudImg.raycastTarget = false;
        topHudImg.sprite = RoundedRectSpriteFactory.Get (64, 10);
        topHudImg.type = Image.Type.Sliced;

        Image vidasIcon = CreateHudIcon (topHudRowAboveTimerRt, "VidasIcon", _hudLivesIconSprite, "Imagens/Jogador/ui-icon-chances.fw");
        RectTransform vidasIconRt = vidasIcon.rectTransform;
        vidasIconRt.anchorMin = new Vector2 (0.03f, 0.22f);
        vidasIconRt.anchorMax = new Vector2 (0.10f, 0.78f);
        vidasIconRt.offsetMin = Vector2.zero;
        vidasIconRt.offsetMax = Vector2.zero;

        txtVidas = CreateTmp (topHudRowAboveTimerRt, "Vidas", "3", 40f, TextAlignmentOptions.MidlineLeft, font);
        RectTransform vRt = txtVidas.rectTransform;
        vRt.anchorMin = new Vector2 (0.11f, 0.22f);
        vRt.anchorMax = new Vector2 (0.30f, 0.78f);
        vRt.offsetMin = Vector2.zero;
        vRt.offsetMax = Vector2.zero;

        string trocarLbl = language == "portuguese" ? "Trocar" : "Swap";
        btnPularRodada = CreateTextButton (topHudRowAboveTimerRt, "TrocarRodada", trocarLbl, font, PularRodadaPressed).gameObject;
        Image pularImg = btnPularRodada.GetComponent<Image> ();
        if (pularImg != null)
            pularImg.color = new Color (0.92f, 0.62f, 0.12f, 1f); // amarelo/laranja: trocar dados / rodada
        SetButtonTextSize (btnPularRodada.GetComponent<Button> (), 34f);
        RectTransform pularRt = btnPularRodada.GetComponent<RectTransform> ();
        pularRt.anchorMin = new Vector2 (0.36f, 0.20f);
        pularRt.anchorMax = new Vector2 (0.64f, 0.80f);
        pularRt.offsetMin = Vector2.zero;
        pularRt.offsetMax = Vector2.zero;

        Image pontosIcon = CreateHudIcon (topHudRowAboveTimerRt, "PontosIcon", _hudScoreTrophyIconSprite, "Imagens/Jogador/ui-icon-points.fw");
        RectTransform pontosIconRt = pontosIcon.rectTransform;
        pontosIconRt.anchorMin = new Vector2 (0.70f, 0.22f);
        pontosIconRt.anchorMax = new Vector2 (0.77f, 0.78f);
        pontosIconRt.offsetMin = Vector2.zero;
        pontosIconRt.offsetMax = Vector2.zero;

        txtPontos = CreateTmp (topHudRowAboveTimerRt, "Pontos", "0 / " + ContiGo2DLevelCatalog.CellCount (levelId), 32f, TextAlignmentOptions.MidlineLeft, font);
        RectTransform ptsRt = txtPontos.rectTransform;
        ptsRt.anchorMin = new Vector2 (0.78f, 0.22f);
        ptsRt.anchorMax = new Vector2 (0.98f, 0.78f);
        ptsRt.offsetMin = Vector2.zero;
        ptsRt.offsetMax = Vector2.zero;

        // Timer sozinho (acima dos dados)
        timerRowAboveDiceRt = CreatePanel (safeAreaRt, "TimerRow", new Color (0f, 0f, 0f, 0.18f)).rectTransform;
        Image timerImg = timerRowAboveDiceRt.GetComponent<Image> ();
        timerImg.raycastTarget = false;
        timerImg.sprite = RoundedRectSpriteFactory.Get (64, 10);
        timerImg.type = Image.Type.Sliced;

        txtMainTimer = CreateTmp (timerRowAboveDiceRt, "MainTimer", "5:00", DiceFontPreferred, TextAlignmentOptions.Center, font);
        RectTransform mtRt = txtMainTimer.rectTransform;
        mtRt.anchorMin = new Vector2 (0f, 0f);
        mtRt.anchorMax = new Vector2 (1f, 1f);
        mtRt.offsetMin = new Vector2 (0f, 10f);
        mtRt.offsetMax = new Vector2 (0f, -10f);

        txtResultadoApresentado = CreateTmp (timerRowAboveDiceRt, "ResultadoHud", "", 24f, TextAlignmentOptions.Bottom, font);
        RectTransform resRt = txtResultadoApresentado.rectTransform;
        resRt.anchorMin = new Vector2 (0.05f, 0.02f);
        resRt.anchorMax = new Vector2 (0.95f, 0.28f);
        resRt.offsetMin = Vector2.zero;
        resRt.offsetMax = Vector2.zero;
        txtResultadoApresentado.enableWordWrapping = false;
        txtResultadoApresentado.overflowMode = TextOverflowModes.Ellipsis;

        btnPause = ContiGo2DSharedUi.CreateGameplaySettingsMenuButton (safeAreaRt, MenuPressed);
        RectTransform menuBtnRt = btnPause.GetComponent<RectTransform> ();
        float menuBtnW = menuBtnRt.sizeDelta.x;
        btnPause.SetActive (false);

        btnHelp = ContiGo2DSharedUi.CreateGameplayHowToPlayHelpButton (safeAreaRt, HelpPressed, 10f + menuBtnW + 10f);
        btnHelp.SetActive (false);

        strategicTimerScreen = new GameObject ("StrategicOverlay");
        strategicTimerScreen.transform.SetParent (canvasRt, false);
        RectTransform stRt = strategicTimerScreen.AddComponent<RectTransform> ();
        stRt.anchorMin = Vector2.zero;
        stRt.anchorMax = Vector2.one;
        stRt.offsetMin = Vector2.zero;
        stRt.offsetMax = Vector2.zero;
        Image stBg = strategicTimerScreen.AddComponent<Image> ();
        stBg.color = new Color (0f, 0f, 0f, 0.72f);
        strategicTimerScreen.SetActive (false);

        GameObject stTextGo = new GameObject ("StrategicText");
        stTextGo.transform.SetParent (strategicTimerScreen.transform, false);
        RectTransform str = stTextGo.AddComponent<RectTransform> ();
        str.anchorMin = new Vector2 (0.15f, 0.42f);
        str.anchorMax = new Vector2 (0.85f, 0.58f);
        str.offsetMin = Vector2.zero;
        str.offsetMax = Vector2.zero;
        txtStrategicTimer = stTextGo.AddComponent<TextMeshProUGUI> ();
        if (font != null)
            txtStrategicTimer.font = font;
        txtStrategicTimer.fontSize = 156;
        txtStrategicTimer.alignment = TextAlignmentOptions.Center;
        txtStrategicTimer.color = Color.white;

        menuScreen = new GameObject ("MenuOverlay");
        menuScreen.transform.SetParent (canvasRt, false);
        RectTransform menuRt = menuScreen.AddComponent<RectTransform> ();
        menuRt.anchorMin = Vector2.zero;
        menuRt.anchorMax = Vector2.one;
        menuRt.offsetMin = Vector2.zero;
        menuRt.offsetMax = Vector2.zero;
        Image menuBg = menuScreen.AddComponent<Image> ();
        // Sem escurecimento: vê-se só o fundo do tabuleiro: bloqueia cliques no jogo por baixo.
        menuBg.color = new Color (0f, 0f, 0f, 0f);
        menuBg.raycastTarget = true;
        menuScreen.SetActive (false);

        GameObject menuPanel = new GameObject ("MenuPanel", typeof (RectTransform));
        menuPanel.transform.SetParent (menuScreen.transform, false);
        RectTransform menuPanelRt = menuPanel.GetComponent<RectTransform> ();
        menuPanelRt.anchorMin = Vector2.zero;
        menuPanelRt.anchorMax = Vector2.one;
        menuPanelRt.offsetMin = Vector2.zero;
        menuPanelRt.offsetMax = Vector2.zero;

        if (_menuPopupBgSprite != null && _menuHexButtonSprite != null) {
            BuildPauseMenuHexagonModal (menuPanelRt);
        } else {
            Vector2 menuIconSize = SizeDeltaForHomeButtonArt (MenuModalIconHeightPx);
            RectOffset menuStripPad = new RectOffset (16, 16, 16, 16);
            float stripInnerW = menuIconSize.x * 3f + MenuModalHlgSpacing * 2f;
            float stripInnerH = menuIconSize.y;
            float stripW = stripInnerW + menuStripPad.horizontal;
            float stripH = stripInnerH + menuStripPad.vertical;

            GameObject menuStripGo = new GameObject ("MenuStrip", typeof (RectTransform));
            menuStripGo.transform.SetParent (menuPanelRt, false);
            RectTransform menuStripRt = menuStripGo.GetComponent<RectTransform> ();
            menuStripRt.anchorMin = new Vector2 (0.5f, 0.5f);
            menuStripRt.anchorMax = new Vector2 (0.5f, 0.5f);
            menuStripRt.pivot = new Vector2 (0.5f, 0.5f);
            menuStripRt.sizeDelta = new Vector2 (stripW, stripH);
            menuStripRt.anchoredPosition = Vector2.zero;
            Image menuStripBg = menuStripGo.AddComponent<Image> ();
            menuStripBg.sprite = RoundedRectSpriteFactory.Get (64, 12);
            menuStripBg.type = Image.Type.Sliced;
            menuStripBg.color = new Color (0.18f, 0.19f, 0.22f, 0.82f);
            menuStripBg.raycastTarget = true;

            HorizontalLayoutGroup menuHlg = menuStripGo.AddComponent<HorizontalLayoutGroup> ();
            menuHlg.childAlignment = TextAnchor.MiddleCenter;
            menuHlg.spacing = MenuModalHlgSpacing;
            menuHlg.padding = menuStripPad;
            menuHlg.childControlWidth = false;
            menuHlg.childControlHeight = false;
            menuHlg.childForceExpandWidth = false;
            menuHlg.childForceExpandHeight = false;

            menuBackButton = CreateImagensSpriteButton (menuStripRt, "MenuBack", "btn-close-home.fw", MenuBackPressed);
            ConfigureMenuModalIconButton (menuBackButton.GetComponent<RectTransform> (), menuIconSize);

            menuRestartButton = CreateImagensSpriteButton (menuStripRt, "MenuRestart", "btn-reset.fw", MenuRestartPressed);
            ConfigureMenuModalIconButton (menuRestartButton.GetComponent<RectTransform> (), menuIconSize);

            menuHomeButton = CreateImagensSpriteButton (menuStripRt, "MenuHome", "btn-home.fw", MenuHomePressed);
            ConfigureMenuModalIconButton (menuHomeButton.GetComponent<RectTransform> (), menuIconSize);
        }

        gameOverScreen = new GameObject ("GameOver");
        gameOverScreen.transform.SetParent (canvasRt, false);
        RectTransform goRt = gameOverScreen.AddComponent<RectTransform> ();
        goRt.anchorMin = Vector2.zero;
        goRt.anchorMax = Vector2.one;
        goRt.offsetMin = Vector2.zero;
        goRt.offsetMax = Vector2.zero;
        Image goBg = gameOverScreen.AddComponent<Image> ();
        goBg.color = new Color (0f, 0f, 0f, 0.72f);
        goBg.raycastTarget = true;
        gameOverScreen.SetActive (false);

        GameObject goPanel = new GameObject ("Panel");
        goPanel.transform.SetParent (gameOverScreen.transform, false);
        RectTransform panelRt = goPanel.AddComponent<RectTransform> ();
        panelRt.anchorMin = Vector2.zero;
        panelRt.anchorMax = Vector2.one;
        panelRt.offsetMin = Vector2.zero;
        panelRt.offsetMax = Vector2.zero;

        bool useHexGameOver = _menuPopupBgSprite != null && _menuHexButtonSprite != null
            && _menuRestartIconSprite != null && _menuExitIconSprite != null;
        if (useHexGameOver) {
            BuildGameOverHexagonModal (panelRt, font);
        } else {
            panelRt.anchorMin = new Vector2 (0.15f, 0.3f);
            panelRt.anchorMax = new Vector2 (0.85f, 0.7f);

            GameObject titleFrameGo = new GameObject ("GOTextFrame", typeof (RectTransform));
            titleFrameGo.transform.SetParent (panelRt, false);
            RectTransform tfRt = titleFrameGo.GetComponent<RectTransform> ();
            tfRt.anchorMin = new Vector2 (0.5f, 1f);
            tfRt.anchorMax = new Vector2 (0.5f, 1f);
            tfRt.pivot = new Vector2 (0.5f, 1f);
            tfRt.sizeDelta = new Vector2 (400f, 120f);
            tfRt.anchoredPosition = Vector2.zero;
            Image tfImg = titleFrameGo.AddComponent<Image> ();
            Sprite tfSp = Resources.Load<Sprite> ("GUI PRO Kit - Fantasy RPG/ResourcesData/Sprites/Component/Frame/frame_linetextframe_04_White2");
            if (tfSp != null) {
                tfImg.sprite = tfSp;
                tfImg.color = Color.white;
                tfImg.type = Image.Type.Sliced;
                tfImg.preserveAspect = false;
            } else {
                tfImg.sprite = RoundedRectSpriteFactory.Get (64, 12);
                tfImg.type = Image.Type.Sliced;
                tfImg.color = new Color (0.18f, 0.19f, 0.22f, 0.92f);
            }
            tfImg.raycastTarget = false;

            gameOverMainText = CreateTmp (tfRt, "GOText", "", 34f, TextAlignmentOptions.Center, font);
            RectTransform gotr = gameOverMainText.rectTransform;
            gotr.anchorMin = Vector2.zero;
            gotr.anchorMax = Vector2.one;
            gotr.offsetMin = new Vector2 (12f, 8f);
            gotr.offsetMax = new Vector2 (-12f, -8f);
            gotr.anchoredPosition = new Vector2 (0f, -30f);

            float hudBtnH2 = 110f;
            Vector2 iconSize2 = SizeDeltaForHomeButtonArt (hudBtnH2);

            gameOverRestartButton = CreateImagensSpriteButton (panelRt, "MenuRestart", "btn-reset.fw", RestartPressed);
            RectTransform rr = gameOverRestartButton.GetComponent<RectTransform> ();
            rr.anchorMin = new Vector2 (0.32f, 0.06f);
            rr.anchorMax = new Vector2 (0.32f, 0.06f);
            rr.pivot = new Vector2 (0.5f, 0.5f);
            rr.sizeDelta = iconSize2;
            rr.anchoredPosition = Vector2.zero;

            gameOverHomeButton = CreateImagensSpriteButton (panelRt, "MenuHome", "btn-home.fw", HomePressed);
            RectTransform hr = gameOverHomeButton.GetComponent<RectTransform> ();
            hr.anchorMin = new Vector2 (0.68f, 0.06f);
            hr.anchorMax = new Vector2 (0.68f, 0.06f);
            hr.pivot = new Vector2 (0.5f, 0.5f);
            hr.sizeDelta = iconSize2;
            hr.anchoredPosition = Vector2.zero;
        }
    }

    /// <summary>Game Over / Vitória: mesmo padrão do demo Component_1 — Popup01_Single_Navy + Group_Menu_Hexagon (ReStart + Exit).</summary>
    void BuildGameOverHexagonModal (RectTransform parentPanelRt, TMP_FontAsset font)
    {
        float hex = HexPauseMenuButtonPx;
        float sp = HexPauseMenuSpacingPx;
        float pad = HexPauseMenuPopupPadPx;
        float innerW = hex * 2f + sp;
        float innerH = hex;
        float titleH = 108f;
        float gap = 22f;
        // Popup mais largo para caber “valores” + “sua escolha” sem truncar demais.
        float extraW = 240f;
        float popW = innerW + pad * 2f + extraW;
        // + área do relatório de erros (até 3 itens). Altura extra fixa para não “esmagar” tudo.
        // Mais alto para ficar legível e não truncar em telas menores.
        float errorsH = 460f;
        float popH = pad * 2f + titleH + gap + innerH + gap + errorsH;

        GameObject popGo = new GameObject ("GameOverPopupCasual", typeof (RectTransform));
        popGo.transform.SetParent (parentPanelRt, false);
        RectTransform popRt = popGo.GetComponent<RectTransform> ();
        popRt.anchorMin = new Vector2 (0.5f, 0.5f);
        popRt.anchorMax = new Vector2 (0.5f, 0.5f);
        popRt.pivot = new Vector2 (0.5f, 0.5f);
        popRt.sizeDelta = new Vector2 (popW, popH);
        popRt.anchoredPosition = Vector2.zero;

        Image bg = popGo.AddComponent<Image> ();
        bg.sprite = _menuPopupBgSprite;
        bg.type = _menuPopupBgSprite != null && _menuPopupBgSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
        bg.preserveAspect = false;
        bg.color = Color.white;
        bg.raycastTarget = true;

        GameObject content = new GameObject ("Content", typeof (RectTransform));
        content.transform.SetParent (popGo.transform, false);
        RectTransform cRt = content.GetComponent<RectTransform> ();
        cRt.anchorMin = Vector2.zero;
        cRt.anchorMax = Vector2.one;
        cRt.offsetMin = new Vector2 (pad, pad);
        cRt.offsetMax = new Vector2 (-pad, -pad);

        VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup> ();
        vlg.spacing = Mathf.RoundToInt (gap);
        vlg.padding = new RectOffset (0, 0, 0, 0);
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childForceExpandWidth = true;
        // Importante: precisa controlar a altura dos filhos (senão o ErrorsBlock não reserva espaço e pode sobrepor).
        vlg.childControlHeight = true;
        vlg.childForceExpandHeight = false;

        GameObject titleHolder = new GameObject ("GOTextArea", typeof (RectTransform));
        titleHolder.transform.SetParent (content.transform, false);
        LayoutElement titleLe = titleHolder.AddComponent<LayoutElement> ();
        titleLe.preferredHeight = titleH;
        titleLe.flexibleWidth = 1f;

        gameOverMainText = CreateTmp (titleHolder.transform, "GOText", "", 36f, TextAlignmentOptions.Center, font);
        RectTransform gotr = gameOverMainText.rectTransform;
        gotr.anchorMin = Vector2.zero;
        gotr.anchorMax = Vector2.one;
        gotr.offsetMin = new Vector2 (8f, 4f);
        gotr.offsetMax = new Vector2 (-8f, -4f);
        gameOverMainText.enableWordWrapping = true;

        GameObject rowGo = new GameObject ("Group_Menu_Hexagon", typeof (RectTransform));
        rowGo.transform.SetParent (content.transform, false);
        LayoutElement rowLe = rowGo.AddComponent<LayoutElement> ();
        rowLe.preferredHeight = innerH;
        rowLe.flexibleWidth = 1f;

        HorizontalLayoutGroup hlg = rowGo.AddComponent<HorizontalLayoutGroup> ();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = sp;
        hlg.padding = new RectOffset (0, 0, 0, 0);
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        gameOverRestartButton = CreateHexPauseMenuButton (
            rowGo.transform, "Button_Hexagon_ReStart", _menuHexButtonSprite, _menuRestartIconSprite, RestartPressed, hex);
        gameOverHomeButton = CreateHexPauseMenuButton (
            rowGo.transform, "Button_Hexagon_Exit", _menuHexButtonSprite, _menuExitIconSprite, HomePressed, hex);

        // Espaço extra entre botões e erros (separação visual clara).
        GameObject spacer = new GameObject ("Spacer_BetweenButtonsAndErrors", typeof (RectTransform));
        spacer.transform.SetParent (content.transform, false);
        LayoutElement spacerLe = spacer.AddComponent<LayoutElement> ();
        spacerLe.preferredHeight = 10f;
        spacerLe.flexibleWidth = 1f;

        // Bloco de erros (fundo azul + até 3 linhas roxas).
        GameObject errorsGo = new GameObject ("ErrorsBlock", typeof (RectTransform));
        errorsGo.transform.SetParent (content.transform, false);
        LayoutElement errorsLe = errorsGo.AddComponent<LayoutElement> ();
        errorsLe.preferredHeight = errorsH;
        errorsLe.flexibleWidth = 1f;

        Image errorsBg = errorsGo.AddComponent<Image> ();
        // Fundo do bloco de erros deve ser roxo (mesmo frame dos itens), não o azul.
        Sprite blockBgSp = _gameOverErrorItemFrameSprite != null ? _gameOverErrorItemFrameSprite : _gameOverErrorsListBgSprite;
        if (blockBgSp != null) {
            errorsBg.sprite = blockBgSp;
            errorsBg.type = blockBgSp.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            errorsBg.preserveAspect = false;
        } else {
            errorsBg.sprite = RoundedRectSpriteFactory.Get (64, 12);
            errorsBg.type = Image.Type.Sliced;
        }
        errorsBg.color = Color.white;
        errorsBg.raycastTarget = false;

        gameOverErrorsBlock = errorsGo;

        GameObject inner = new GameObject ("ErrorsContent", typeof (RectTransform));
        inner.transform.SetParent (errorsGo.transform, false);
        RectTransform innerRt = inner.GetComponent<RectTransform> ();
        innerRt.anchorMin = Vector2.zero;
        innerRt.anchorMax = Vector2.one;
        innerRt.offsetMin = new Vector2 (20f, 20f);
        innerRt.offsetMax = new Vector2 (-20f, -20f);

        // Importante: NÃO usar VerticalLayoutGroup aqui. Vamos “fatiar” o retângulo:
        // header fixo em cima + lista preenchendo o resto. Isso elimina sobreposição.
        const float ErrorsHeaderHeight = 104f;
        const float ErrorsGapBelowHeader = 14f;

        // Componente 1: header (ERROS + colunas) — topo com altura fixa.
        GameObject headerGo = new GameObject ("ErrorsHeader", typeof (RectTransform));
        headerGo.transform.SetParent (inner.transform, false);
        RectTransform headerRt = headerGo.GetComponent<RectTransform> ();
        headerRt.anchorMin = new Vector2 (0f, 1f);
        headerRt.anchorMax = new Vector2 (1f, 1f);
        headerRt.pivot = new Vector2 (0.5f, 1f);
        headerRt.anchoredPosition = Vector2.zero;
        headerRt.sizeDelta = new Vector2 (0f, ErrorsHeaderHeight);

        Image headerBg = headerGo.AddComponent<Image> ();
        Sprite headerSp = _gameOverErrorItemFrameSprite != null ? _gameOverErrorItemFrameSprite : null;
        if (headerSp != null) {
            headerBg.sprite = headerSp;
            headerBg.type = headerSp.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            headerBg.preserveAspect = false;
        } else {
            headerBg.sprite = RoundedRectSpriteFactory.Get (64, 12);
            headerBg.type = Image.Type.Sliced;
        }
        headerBg.color = Color.white;
        headerBg.raycastTarget = false;

        GameObject headerInner = new GameObject ("Inner", typeof (RectTransform));
        headerInner.transform.SetParent (headerGo.transform, false);
        RectTransform hiRt = headerInner.GetComponent<RectTransform> ();
        hiRt.anchorMin = Vector2.zero;
        hiRt.anchorMax = Vector2.one;
        hiRt.offsetMin = new Vector2 (16f, 12f);
        hiRt.offsetMax = new Vector2 (-16f, -12f);

        // Linha do título “ERROS”
        TextMeshProUGUI title = CreateTmp (headerInner.transform, "Title", "ERROS", 26f, TextAlignmentOptions.Center, font);
        title.raycastTarget = false;
        RectTransform titleRt = title.rectTransform;
        titleRt.anchorMin = new Vector2 (0f, 0.52f);
        titleRt.anchorMax = new Vector2 (1f, 1f);
        titleRt.offsetMin = Vector2.zero;
        titleRt.offsetMax = Vector2.zero;

        // Linha de colunas “valores | sua escolha”
        GameObject cols = new GameObject ("Cols", typeof (RectTransform));
        cols.transform.SetParent (headerInner.transform, false);
        RectTransform colsRt = cols.GetComponent<RectTransform> ();
        colsRt.anchorMin = new Vector2 (0f, 0f);
        colsRt.anchorMax = new Vector2 (1f, 0.52f);
        colsRt.offsetMin = Vector2.zero;
        colsRt.offsetMax = Vector2.zero;

        TextMeshProUGUI l = CreateTmp (cols.transform, "ColValores", "valores", 20f, TextAlignmentOptions.MidlineLeft, font);
        l.fontStyle = FontStyles.Bold;
        l.raycastTarget = false;
        RectTransform lrt = l.rectTransform;
        lrt.anchorMin = new Vector2 (0f, 0f);
        lrt.anchorMax = new Vector2 (0.70f, 1f);
        lrt.offsetMin = new Vector2 (6f, 0f);
        lrt.offsetMax = Vector2.zero;

        TextMeshProUGUI r = CreateTmp (cols.transform, "ColEscolha", "sua escolha", 20f, TextAlignmentOptions.MidlineRight, font);
        r.fontStyle = FontStyles.Bold;
        r.raycastTarget = false;
        RectTransform rrt = r.rectTransform;
        rrt.anchorMin = new Vector2 (0.70f, 0f);
        rrt.anchorMax = new Vector2 (1f, 1f);
        rrt.offsetMin = Vector2.zero;
        rrt.offsetMax = new Vector2 (-6f, 0f);

        // Componente 2: lista (somente linhas de dados) — ocupa o resto abaixo do header.
        GameObject listGo = new GameObject ("ErrorsList", typeof (RectTransform));
        listGo.transform.SetParent (inner.transform, false);
        RectTransform listRt = listGo.GetComponent<RectTransform> ();
        listRt.anchorMin = new Vector2 (0f, 0f);
        listRt.anchorMax = new Vector2 (1f, 1f);
        listRt.offsetMin = Vector2.zero;
        listRt.offsetMax = new Vector2 (0f, -(ErrorsHeaderHeight + ErrorsGapBelowHeader));

        VerticalLayoutGroup listVlg = listGo.AddComponent<VerticalLayoutGroup> ();
        listVlg.spacing = 14f;
        listVlg.padding = new RectOffset (0, 0, 0, 0);
        listVlg.childAlignment = TextAnchor.UpperCenter;
        listVlg.childControlWidth = true;
        listVlg.childForceExpandWidth = true;
        listVlg.childControlHeight = true;
        listVlg.childForceExpandHeight = false;
        gameOverErrorsRoot = listRt;
        if (gameOverErrorsBlock != null)
            gameOverErrorsBlock.SetActive (false);
    }

    static Vector2 SizeDeltaForHomeButtonArt (float height)
    {
        float w = height * (HomeButtonArtWidthPx / HomeButtonArtHeightPx);
        return new Vector2 (w, height);
    }

    static void ConfigureMenuModalIconButton (RectTransform rt, Vector2 sizeDelta)
    {
        if (rt == null)
            return;
        rt.anchorMin = new Vector2 (0.5f, 0.5f);
        rt.anchorMax = new Vector2 (0.5f, 0.5f);
        rt.pivot = new Vector2 (0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = Vector2.zero;
        LayoutElement le = rt.GetComponent<LayoutElement> ();
        if (le == null)
            le = rt.gameObject.AddComponent<LayoutElement> ();
        le.preferredWidth = sizeDelta.x;
        le.preferredHeight = sizeDelta.y;
    }

    void BuildPauseMenuHexagonModal (RectTransform menuPanelRt)
    {
        float hex = HexPauseMenuButtonPx;
        float sp = HexPauseMenuSpacingPx;
        float pad = HexPauseMenuPopupPadPx;
        float innerW = hex * 3f + sp * 2f;
        float innerH = hex;
        float popW = innerW + pad * 2f;
        float popH = innerH + pad * 2f;

        GameObject popGo = new GameObject ("MenuPopupCasual", typeof (RectTransform));
        popGo.transform.SetParent (menuPanelRt, false);
        RectTransform popRt = popGo.GetComponent<RectTransform> ();
        popRt.anchorMin = new Vector2 (0.5f, 0.5f);
        popRt.anchorMax = new Vector2 (0.5f, 0.5f);
        popRt.pivot = new Vector2 (0.5f, 0.5f);
        popRt.sizeDelta = new Vector2 (popW, popH);
        popRt.anchoredPosition = Vector2.zero;

        Image bg = popGo.AddComponent<Image> ();
        if (_menuPopupBgSprite != null) {
            bg.sprite = _menuPopupBgSprite;
            bg.type = _menuPopupBgSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            bg.preserveAspect = false;
        }
        bg.color = Color.white;
        bg.raycastTarget = true;

        GameObject rowGo = new GameObject ("Group_Menu_Hexagon", typeof (RectTransform));
        rowGo.transform.SetParent (popGo.transform, false);
        RectTransform rowRt = rowGo.GetComponent<RectTransform> ();
        rowRt.anchorMin = Vector2.zero;
        rowRt.anchorMax = Vector2.one;
        rowRt.offsetMin = new Vector2 (pad, pad);
        rowRt.offsetMax = new Vector2 (-pad, -pad);

        HorizontalLayoutGroup hlg = rowGo.AddComponent<HorizontalLayoutGroup> ();
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.spacing = sp;
        hlg.padding = new RectOffset (0, 0, 0, 0);
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        menuBackButton = CreateHexPauseMenuButton (
            rowGo.transform, "Button_Hexagon_Continue", _menuHexButtonSprite, _menuContinueIconSprite, MenuBackPressed, hex);
        menuRestartButton = CreateHexPauseMenuButton (
            rowGo.transform, "Button_Hexagon_ReStart", _menuHexButtonSprite, _menuRestartIconSprite, MenuRestartPressed, hex);
        menuHomeButton = CreateHexPauseMenuButton (
            rowGo.transform, "Button_Hexagon_Exit", _menuHexButtonSprite, _menuExitIconSprite, MenuHomePressed, hex);
    }

    static Button CreateHexPauseMenuButton (
        Transform parent, string objectName, Sprite hexSprite, Sprite iconSprite, UnityAction onClick, float sizePx)
    {
        GameObject go = new GameObject (objectName, typeof (RectTransform));
        go.transform.SetParent (parent, false);

        LayoutElement le = go.AddComponent<LayoutElement> ();
        le.preferredWidth = sizePx;
        le.preferredHeight = sizePx;

        Image hexImg = go.AddComponent<Image> ();
        hexImg.sprite = hexSprite;
        if (hexSprite != null && hexSprite.border.sqrMagnitude > 0.0001f) {
            hexImg.type = Image.Type.Sliced;
            hexImg.preserveAspect = false;
        } else {
            hexImg.type = Image.Type.Simple;
            hexImg.preserveAspect = true;
        }
        hexImg.color = Color.white;

        Button b = go.AddComponent<Button> ();
        b.targetGraphic = hexImg;
        b.transition = Selectable.Transition.ColorTint;
        ColorBlock cb = ColorBlock.defaultColorBlock;
        cb.highlightedColor = new Color (0.95f, 0.95f, 1f, 1f);
        cb.pressedColor = new Color (0.78f, 0.82f, 0.95f, 1f);
        b.colors = cb;
        if (onClick != null)
            b.onClick.AddListener (onClick);

        if (iconSprite != null) {
            GameObject iconGo = new GameObject ("Icon", typeof (RectTransform));
            iconGo.transform.SetParent (go.transform, false);
            RectTransform ir = iconGo.GetComponent<RectTransform> ();
            ir.anchorMin = new Vector2 (0.2f, 0.18f);
            ir.anchorMax = new Vector2 (0.8f, 0.82f);
            ir.offsetMin = Vector2.zero;
            ir.offsetMax = Vector2.zero;
            Image iimg = iconGo.AddComponent<Image> ();
            iimg.sprite = iconSprite;
            iimg.color = Color.white;
            iimg.preserveAspect = true;
            iimg.raycastTarget = false;
        }

        return b;
    }

    /// <param name="normalSpriteFileName">Ficheiro em Resources/Imagens (ex.: btn-home.fw). Hover/pressed: prefixo + "-hover"/"-pressed" + extensão. </param>
    static Button CreateImagensSpriteButton (Transform parent, string name, string normalSpriteFileName, UnityAction onClick)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image img = go.AddComponent<Image> ();
        img.color = Color.white;
        Button b = go.AddComponent<Button> ();
        b.targetGraphic = img;
        b.onClick.AddListener (onClick);
        if (!TrySetupImagensSpriteSwap (b, img, normalSpriteFileName))
            img.color = new Color (0.28f, 0.45f, 0.7f, 1f);
        return b;
    }

    static bool TrySetupImagensSpriteSwap (Button b, Image img, string normalName)
    {
        Sprite n = Resources.Load<Sprite> ("Imagens/" + normalName);
        if (n == null)
            return false;
        int dot = normalName.LastIndexOf ('.');
        string prefix = dot >= 0 ? normalName.Substring (0, dot) : normalName;
        string ext = dot >= 0 ? normalName.Substring (dot) : "";
        Sprite hi = Resources.Load<Sprite> ("Imagens/" + prefix + "-hover" + ext);
        Sprite pr = Resources.Load<Sprite> ("Imagens/" + prefix + "-pressed" + ext);
        img.sprite = n;
        img.type = Image.Type.Simple;
        img.preserveAspect = true;
        b.transition = Selectable.Transition.SpriteSwap;
        SpriteState st = b.spriteState;
        st.highlightedSprite = hi != null ? hi : n;
        st.pressedSprite = pr != null ? pr : n;
        st.selectedSprite = st.highlightedSprite;
        st.disabledSprite = n;
        b.spriteState = st;
        return true;
    }

    static void ForceButtonSpriteOrFallback (Button b, string imagensResourceName)
    {
        if (b == null)
            return;
        Image img = b.GetComponent<Image> ();
        if (img == null)
            return;
        if (img.sprite != null)
            return;
        Sprite sp = Resources.Load<Sprite> ("Imagens/" + imagensResourceName);
        if (sp != null) {
            img.sprite = sp;
            img.type = Image.Type.Simple;
            img.preserveAspect = true;
            img.color = Color.white;
        } else {
            // fallback visível
            img.color = new Color (1f, 0.2f, 0.2f, 0.85f);
        }
    }

    static void SetButtonAlpha (Button b, float alpha01)
    {
        if (b == null)
            return;
        Image img = b.GetComponent<Image> ();
        if (img == null)
            return;
        Color c = img.color;
        c.a = Mathf.Clamp01 (alpha01);
        img.color = c;
    }

    static void LayoutHudSegment (RectTransform rt, float xmin, float xmax)
    {
        rt.anchorMin = new Vector2 (xmin, 0.08f);
        rt.anchorMax = new Vector2 (xmax, 0.92f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    static void CreateDiceSlotSquare (Transform parent, string name, TMP_FontAsset font, out TextMeshProUGUI tmp)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        LayoutElement le = go.AddComponent<LayoutElement> ();
        le.preferredWidth = DiceSidePreferred;
        le.preferredHeight = DiceSidePreferred;

        Sprite rounded = RoundedRectSpriteFactory.Get (64, 12);
        Color borderCol = new Color (1f, 1f, 1f, 0.22f);
        float borderPx = 2f;

        Image borderImg = go.AddComponent<Image> ();
        borderImg.sprite = rounded;
        borderImg.type = Image.Type.Sliced;
        borderImg.color = borderCol;

        GameObject fillGo = new GameObject ("Fill", typeof (RectTransform));
        fillGo.transform.SetParent (go.transform, false);
        RectTransform fillRt = fillGo.GetComponent<RectTransform> ();
        fillRt.anchorMin = Vector2.zero;
        fillRt.anchorMax = Vector2.one;
        fillRt.offsetMin = new Vector2 (borderPx, borderPx);
        fillRt.offsetMax = new Vector2 (-borderPx, -borderPx);

        Image fillImg = fillGo.AddComponent<Image> ();
        fillImg.sprite = rounded;
        fillImg.type = Image.Type.Sliced;
        fillImg.color = new Color (1f, 1f, 1f, 0.12f);

        RectTransform rt = fillGo.GetComponent<RectTransform> ();
        tmp = CreateTmp (rt, "Val", "0", DiceFontPreferred, TextAlignmentOptions.Center, font);
        RectTransform tr = tmp.rectTransform;
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (4f, 4f);
        tr.offsetMax = new Vector2 (-4f, -4f);
    }

    static Image CreatePanel (Transform parent, string name, Color c)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image i = go.AddComponent<Image> ();
        i.color = c;
        return i;
    }

    static TextMeshProUGUI CreateTmp (Transform parent, string name, string text, float size, TextAlignmentOptions align, TMP_FontAsset font)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        TextMeshProUGUI t = go.AddComponent<TextMeshProUGUI> ();
        if (font != null)
            t.font = font;
        t.text = text;
        t.fontSize = size;
        t.alignment = align;
        t.color = Color.white;
        return t;
    }

    static Button CreateTextButton (Transform parent, string name, string label, TMP_FontAsset font, UnityAction onClick)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image img = go.AddComponent<Image> ();
        img.color = new Color (0.28f, 0.45f, 0.7f);
        Button b = go.AddComponent<Button> ();
        b.targetGraphic = img;
        b.onClick.AddListener (onClick);

        GameObject t = new GameObject ("Txt", typeof (RectTransform));
        t.transform.SetParent (go.transform, false);
        RectTransform tr = t.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = Vector2.zero;
        tr.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = t.AddComponent<TextMeshProUGUI> ();
        if (font != null)
            tmp.font = font;
        tmp.text = label;
        tmp.fontSize = 22;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        return b;
    }

    static void SetButtonTextSize (Button b, float size)
    {
        if (b == null)
            return;
        TextMeshProUGUI tmp = b.GetComponentInChildren<TextMeshProUGUI> (true);
        if (tmp != null)
            tmp.fontSize = size;
    }

    Image CreateHudIcon (Transform parent, string name, Sprite preferred, string resourcesFallback)
    {
        if (preferred != null)
            return CreateIconFromSprite (parent, name, preferred);
        return CreateIcon (parent, name, resourcesFallback);
    }

    static Image CreateIconFromSprite (Transform parent, string name, Sprite sp)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image img = go.AddComponent<Image> ();
        img.sprite = sp;
        img.type = Image.Type.Simple;
        img.preserveAspect = true;
        img.color = Color.white;
        img.raycastTarget = false;
        return img;
    }

    static Image CreateIcon (Transform parent, string name, string resourcesPath)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image img = go.AddComponent<Image> ();
        Sprite sp = Resources.Load<Sprite> (resourcesPath);
        img.sprite = sp;
        img.preserveAspect = true;
        img.color = Color.white;
        img.raycastTarget = false;
        return img;
    }

    static void CreateCornerCover (RectTransform parent, string name, Color color, bool coverBottom, float heightPx)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = coverBottom ? new Vector2 (0f, 0f) : new Vector2 (0f, 1f);
        rt.anchorMax = coverBottom ? new Vector2 (1f, 0f) : new Vector2 (1f, 1f);
        rt.pivot = coverBottom ? new Vector2 (0.5f, 0f) : new Vector2 (0.5f, 1f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2 (0f, heightPx);
        Image img = go.AddComponent<Image> ();
        img.color = color;
        img.raycastTarget = false;
    }

    static RectTransform CreateHelpBlock (Transform parent, string name, out RectTransform contentRt, float preferredHeight, float flexible)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image bg = go.AddComponent<Image> ();
        bg.color = new Color (1f, 1f, 1f, 0.06f);

        RectTransform rt = go.GetComponent<RectTransform> ();
        LayoutElement le = go.AddComponent<LayoutElement> ();
        if (preferredHeight > 1f)
            le.preferredHeight = preferredHeight;
        le.flexibleHeight = flexible;

        GameObject contentGo = new GameObject ("Content", typeof (RectTransform));
        contentGo.transform.SetParent (go.transform, false);
        contentRt = contentGo.GetComponent<RectTransform> ();
        contentRt.anchorMin = Vector2.zero;
        contentRt.anchorMax = Vector2.one;
        contentRt.offsetMin = new Vector2 (14f, 12f);
        contentRt.offsetMax = new Vector2 (-14f, -12f);

        return rt;
    }

    static void StretchInside (RectTransform rt, float pad)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2 (pad, pad);
        rt.offsetMax = new Vector2 (-pad, -pad);
    }

    void EnsureTutorialSlidesLoaded ()
    {
        if (tutorialSlides != null && tutorialSlides.Length == tutorialSlideResourceNames.Length)
            return;
        tutorialSlides = new Sprite[tutorialSlideResourceNames.Length];
        for (int i = 0; i < tutorialSlideResourceNames.Length; i++)
            tutorialSlides[i] = Resources.Load<Sprite> (tutorialSlideResourceNames[i]);
        tutorialSlideIndex = 0;
        ApplyTutorialSlide ();
    }

    void ApplyTutorialSlide ()
    {
        if (tutorialSlideImage == null)
            return;
        if (tutorialSlides == null || tutorialSlides.Length == 0) {
            tutorialSlideImage.sprite = null;
            if (tutorialSlideCounter != null)
                tutorialSlideCounter.text = "";
            return;
        }
        tutorialSlideIndex = Mathf.Clamp (tutorialSlideIndex, 0, tutorialSlides.Length - 1);
        Sprite sp = tutorialSlides[tutorialSlideIndex];
        tutorialSlideImage.sprite = sp;
        // Proporção dinâmica por slide (só o 02 é diferente, mas funciona para todos).
        if (tutorialSlideArf != null && sp != null) {
            Rect r = sp.rect;
            if (r.height > 0.5f)
                tutorialSlideArf.aspectRatio = r.width / r.height;
        }
        if (tutorialSlideCounter != null)
            tutorialSlideCounter.text = (tutorialSlideIndex + 1) + " / " + tutorialSlides.Length;
        if (tutorialPrevButton != null)
            tutorialPrevButton.gameObject.SetActive (tutorialSlideIndex > 0);
        if (tutorialNextButton != null)
            tutorialNextButton.gameObject.SetActive (tutorialSlideIndex < tutorialSlides.Length - 1);
    }

    void NextTutorialSlide ()
    {
        EnsureTutorialSlidesLoaded ();
        if (tutorialSlides == null || tutorialSlides.Length == 0)
            return;
        tutorialSlideIndex = Mathf.Min (tutorialSlideIndex + 1, tutorialSlides.Length - 1);
        ApplyTutorialSlide ();
    }

    void PrevTutorialSlide ()
    {
        EnsureTutorialSlidesLoaded ();
        if (tutorialSlides == null || tutorialSlides.Length == 0)
            return;
        tutorialSlideIndex = Mathf.Max (tutorialSlideIndex - 1, 0);
        ApplyTutorialSlide ();
    }

    void ResetTutorialSlides ()
    {
        EnsureTutorialSlidesLoaded ();
        tutorialSlideIndex = 0;
        ApplyTutorialSlide ();
    }
}
