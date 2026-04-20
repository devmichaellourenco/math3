using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class ContiGoGameController2D
{
    /// <summary> Arte dos botões da Home em px (largura × altura); mantém proporção no HUD/modal. </summary>
    const float HomeButtonArtWidthPx = 425f;
    const float HomeButtonArtHeightPx = 390f;

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
        TMP_FontAsset font = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");

        GameObject canvasGo = new GameObject ("Canvas2D");
        rootCanvas = canvasGo.AddComponent<Canvas> ();
        rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler> ().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
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
        // Arredondamento leve em todas as bordas.

        Image vidasIcon = CreateIcon (topHudRowAboveTimerRt, "VidasIcon", "Imagens/Jogador/ui-icon-chances.fw");
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

        Image pontosIcon = CreateIcon (topHudRowAboveTimerRt, "PontosIcon", "Imagens/Jogador/ui-icon-points.fw");
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
        // Arredondamento leve em todas as bordas.

        txtMainTimer = CreateTmp (timerRowAboveDiceRt, "MainTimer", "5:00", DiceFontPreferred, TextAlignmentOptions.Center, font);
        RectTransform mtRt = txtMainTimer.rectTransform;
        mtRt.anchorMin = new Vector2 (0f, 0f);
        mtRt.anchorMax = new Vector2 (1f, 1f);
        mtRt.offsetMin = new Vector2 (0f, 10f);
        mtRt.offsetMax = new Vector2 (0f, -10f);

        // Resultado/feedback (sempre visível; antes ficava no modal antigo do ?).
        txtResultadoApresentado = CreateTmp (timerRowAboveDiceRt, "ResultadoHud", "", 24f, TextAlignmentOptions.Bottom, font);
        RectTransform resRt = txtResultadoApresentado.rectTransform;
        resRt.anchorMin = new Vector2 (0.05f, 0.02f);
        resRt.anchorMax = new Vector2 (0.95f, 0.28f);
        resRt.offsetMin = Vector2.zero;
        resRt.offsetMax = Vector2.zero;
        txtResultadoApresentado.enableWordWrapping = false;
        txtResultadoApresentado.overflowMode = TextOverflowModes.Ellipsis;

        float hudBtnH = 56f;
        Vector2 hudBtnSize = SizeDeltaForHomeButtonArt (hudBtnH);

        btnPause = CreateImagensSpriteButton (safeAreaRt, "Menu", "btn-settings-home.fw", MenuPressed).gameObject;
        RectTransform menuBtnRt = btnPause.GetComponent<RectTransform> ();
        menuBtnRt.anchorMin = new Vector2 (1f, 1f);
        menuBtnRt.anchorMax = new Vector2 (1f, 1f);
        menuBtnRt.pivot = new Vector2 (1f, 1f);
        menuBtnRt.sizeDelta = hudBtnSize;
        menuBtnRt.anchoredPosition = new Vector2 (-10f, -10f);
        btnPause.SetActive (false);

        btnHelp = CreateImagensSpriteButton (safeAreaRt, "Help", "btn-how-to-play.fw", HelpPressed).gameObject;
        RectTransform helpBtnRt = btnHelp.GetComponent<RectTransform> ();
        helpBtnRt.anchorMin = new Vector2 (1f, 1f);
        helpBtnRt.anchorMax = new Vector2 (1f, 1f);
        helpBtnRt.pivot = new Vector2 (1f, 1f);
        helpBtnRt.sizeDelta = hudBtnSize;
        helpBtnRt.anchoredPosition = new Vector2 (-10f - hudBtnSize.x - 10f, -10f);
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
        menuBg.color = new Color (0f, 0f, 0f, 0.55f);
        menuScreen.SetActive (false);

        GameObject menuPanel = new GameObject ("MenuPanel", typeof (RectTransform));
        menuPanel.transform.SetParent (menuScreen.transform, false);
        RectTransform menuPanelRt = menuPanel.GetComponent<RectTransform> ();
        menuPanelRt.anchorMin = new Vector2 (0.12f, 0.28f);
        menuPanelRt.anchorMax = new Vector2 (0.88f, 0.72f);
        menuPanelRt.offsetMin = Vector2.zero;
        menuPanelRt.offsetMax = Vector2.zero;
        Image menuPanelBg = menuPanel.AddComponent<Image> ();
        menuPanelBg.color = new Color (0.18f, 0.19f, 0.22f);

        menuBackButton = CreateImagensSpriteButton (menuPanelRt, "MenuBack", "btn-close-home.fw", MenuBackPressed);
        RectTransform br0 = menuBackButton.GetComponent<RectTransform> ();
        br0.anchorMin = new Vector2 (0.08f, 0.62f);
        br0.anchorMax = new Vector2 (0.92f, 0.9f);
        br0.offsetMin = Vector2.zero;
        br0.offsetMax = Vector2.zero;

        menuRestartButton = CreateImagensSpriteButton (menuPanelRt, "MenuRestart", "btn-reset.fw", MenuRestartPressed);
        RectTransform br1 = menuRestartButton.GetComponent<RectTransform> ();
        br1.anchorMin = new Vector2 (0.08f, 0.34f);
        br1.anchorMax = new Vector2 (0.92f, 0.62f);
        br1.offsetMin = Vector2.zero;
        br1.offsetMax = Vector2.zero;

        menuHomeButton = CreateImagensSpriteButton (menuPanelRt, "MenuHome", "btn-home.fw", MenuHomePressed);
        RectTransform br2 = menuHomeButton.GetComponent<RectTransform> ();
        br2.anchorMin = new Vector2 (0.08f, 0.08f);
        br2.anchorMax = new Vector2 (0.92f, 0.34f);
        br2.offsetMin = Vector2.zero;
        br2.offsetMax = Vector2.zero;

        gameOverScreen = new GameObject ("GameOver");
        gameOverScreen.transform.SetParent (canvasRt, false);
        RectTransform goRt = gameOverScreen.AddComponent<RectTransform> ();
        goRt.anchorMin = Vector2.zero;
        goRt.anchorMax = Vector2.one;
        goRt.offsetMin = Vector2.zero;
        goRt.offsetMax = Vector2.zero;
        Image goBg = gameOverScreen.AddComponent<Image> ();
        goBg.color = new Color (0f, 0f, 0f, 0.82f);
        gameOverScreen.SetActive (false);

        GameObject goPanel = new GameObject ("Panel");
        goPanel.transform.SetParent (gameOverScreen.transform, false);
        RectTransform panelRt = goPanel.AddComponent<RectTransform> ();
        panelRt.anchorMin = new Vector2 (0.15f, 0.3f);
        panelRt.anchorMax = new Vector2 (0.85f, 0.7f);
        panelRt.offsetMin = Vector2.zero;
        panelRt.offsetMax = Vector2.zero;
        Image panelBg = goPanel.AddComponent<Image> ();
        panelBg.color = new Color (0.18f, 0.19f, 0.22f);

        gameOverMainText = CreateTmp (panelRt, "GOText", "", 34f, TextAlignmentOptions.Center, font);
        RectTransform gotr = gameOverMainText.rectTransform;
        gotr.anchorMin = new Vector2 (0.05f, 0.45f);
        gotr.anchorMax = new Vector2 (0.95f, 0.92f);
        gotr.offsetMin = Vector2.zero;
        gotr.offsetMax = Vector2.zero;

        string againLbl = language == "portuguese" ? "Jogar de novo" : "Play again";
        string menuLbl = language == "portuguese" ? "Menu" : "Menu";

        gameOverRestartButton = CreateTextButton (panelRt, "Reiniciar", againLbl, font, RestartPressed);
        RectTransform rr = gameOverRestartButton.GetComponent<RectTransform> ();
        rr.anchorMin = new Vector2 (0.08f, 0.12f);
        rr.anchorMax = new Vector2 (0.48f, 0.38f);
        rr.offsetMin = Vector2.zero;
        rr.offsetMax = Vector2.zero;

        gameOverHomeButton = CreateTextButton (panelRt, "Home", menuLbl, font, HomePressed);
        RectTransform hr = gameOverHomeButton.GetComponent<RectTransform> ();
        hr.anchorMin = new Vector2 (0.52f, 0.12f);
        hr.anchorMax = new Vector2 (0.92f, 0.38f);
        hr.offsetMin = Vector2.zero;
        hr.offsetMax = Vector2.zero;
    }

    static Vector2 SizeDeltaForHomeButtonArt (float height)
    {
        float w = height * (HomeButtonArtWidthPx / HomeButtonArtHeightPx);
        return new Vector2 (w, height);
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

    static Image CreateIcon (Transform parent, string name, string resourcesPath)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image img = go.AddComponent<Image> ();
        Sprite sp = Resources.Load<Sprite> (resourcesPath);
        img.sprite = sp;
        img.preserveAspect = true;
        img.color = Color.white;
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
