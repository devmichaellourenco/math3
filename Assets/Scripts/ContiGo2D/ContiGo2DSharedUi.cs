using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Sprites e fontes partilhadas na UI 2D em runtime.</summary>
public static class ContiGo2DSharedUi
{
    static ContiGo2DUiSprites _spritesAsset;
    static Sprite _modeListButtonSprite;
    static Sprite _homeNavBlueButtonSprite;
    static Sprite _homeNavGreenButtonSprite;
    static TMP_FontAsset _boardCellFont;
    static Sprite _boardCellBackgroundSprite;
    static Sprite _sceneActionButtonBackgroundSprite;
    static Sprite _sceneHomeBackIconSprite;
    static Sprite _sceneHomeButtonFlushBackgroundSprite;
    static Sprite _gameplaySettingsIconSprite;
    static Sprite _gameplaySettingsFlushRightBackgroundSprite;
    static Sprite _gameplayHowToPlayIconSprite;

    static ContiGo2DUiSprites LoadSpritesAsset ()
    {
        return _spritesAsset != null
            ? _spritesAsset
            : (_spritesAsset = Resources.Load<ContiGo2DUiSprites> ("ContiGo2D/ContiGo2DUiSprites"));
    }

    /// <summary>
    /// Fundo dos botões da lista de modos (<c>btn_rectangle_01_n_dark</c> no asset), com fallbacks.
    /// </summary>
    public static Sprite GetModeListButtonSprite ()
    {
        if (_modeListButtonSprite != null)
            return _modeListButtonSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.ModeListButtonSprite != null)
            _modeListButtonSprite = cfg.ModeListButtonSprite;

        if (_modeListButtonSprite == null) {
            _modeListButtonSprite = Resources.Load<Sprite> ("Imagens/CONFIRMA-QUIT-HOME-PAINEL.fw")
                ?? Resources.Load<Sprite> ("Imagens/CONFIRMA-QUIT-HOME-PANEL.fw");
        }

        return _modeListButtonSprite;
    }

    /// <summary><c>Button01_225_Sky</c> — Desbloqueios e Ranking na barra da Home.</summary>
    public static Sprite GetHomeNavBlueButtonSprite ()
    {
        if (_homeNavBlueButtonSprite != null)
            return _homeNavBlueButtonSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.HomeNavBlueButtonSprite != null)
            _homeNavBlueButtonSprite = cfg.HomeNavBlueButtonSprite;

        if (_homeNavBlueButtonSprite == null)
            _homeNavBlueButtonSprite = GetModeListButtonSprite ();

        return _homeNavBlueButtonSprite;
    }

    /// <summary><c>Button01_225_Green</c> — Cartas na barra da Home.</summary>
    public static Sprite GetHomeNavGreenButtonSprite ()
    {
        if (_homeNavGreenButtonSprite != null)
            return _homeNavGreenButtonSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.HomeNavGreenButtonSprite != null)
            _homeNavGreenButtonSprite = cfg.HomeNavGreenButtonSprite;

        if (_homeNavGreenButtonSprite == null)
            _homeNavGreenButtonSprite = GetModeListButtonSprite ();

        return _homeNavGreenButtonSprite;
    }

    /// <summary>
    /// Fonte dos valores (e nomes fantasia) nas células do tabuleiro —
    /// <c>LilitaOne-Regular Outline 50 SDF</c> via <see cref="ContiGo2DUiSprites"/>.
    /// </summary>
    public static TMP_FontAsset GetBoardCellFont ()
    {
        if (_boardCellFont != null)
            return _boardCellFont;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.BoardCellFont != null)
            _boardCellFont = cfg.BoardCellFont;

        if (_boardCellFont == null)
            _boardCellFont = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");

        return _boardCellFont;
    }

    /// <summary>Fundo das células do tabuleiro (<c>Button01_145_BlueGray</c> no asset).</summary>
    public static Sprite GetBoardCellBackgroundSprite ()
    {
        if (_boardCellBackgroundSprite != null)
            return _boardCellBackgroundSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.BoardCellBackgroundSprite != null)
            _boardCellBackgroundSprite = cfg.BoardCellBackgroundSprite;

        return _boardCellBackgroundSprite;
    }

    /// <summary><c>Button01_195_BlueGray</c> — fundos dos botões em Desbloqueios, Cartas e Ranking GPGS.</summary>
    public static Sprite GetSceneActionButtonBackgroundSprite ()
    {
        if (_sceneActionButtonBackgroundSprite != null)
            return _sceneActionButtonBackgroundSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.SceneActionButtonBackgroundSprite != null)
            _sceneActionButtonBackgroundSprite = cfg.SceneActionButtonBackgroundSprite;

        return _sceneActionButtonBackgroundSprite;
    }

    /// <summary>Fundo da linha de cada modo na cena <c>ContiGo2DLevelSelect</c> (Layer Lab Button01_*).</summary>
    public static Sprite GetLevelSelectRowSprite (ContiGo2DLevelId levelId)
    {
        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null) {
            switch (levelId) {
            case ContiGo2DLevelId.Iniciante:
                if (cfg.LevelSelectInicianteRowSprite != null)
                    return cfg.LevelSelectInicianteRowSprite;
                break;
            case ContiGo2DLevelId.Profissional:
                if (cfg.LevelSelectProfissionalRowSprite != null)
                    return cfg.LevelSelectProfissionalRowSprite;
                break;
            case ContiGo2DLevelId.Sabio:
                if (cfg.LevelSelectSabioRowSprite != null)
                    return cfg.LevelSelectSabioRowSprite;
                break;
            case ContiGo2DLevelId.Mestre:
                if (cfg.LevelSelectMestreRowSprite != null)
                    return cfg.LevelSelectMestreRowSprite;
                break;
            }
        }
        return GetModeListButtonSprite ();
    }

    /// <summary><c>Icon_PictoIcon_Back</c> — botão Home (canto superior esquerdo).</summary>
    public static Sprite GetSceneHomeBackIconSprite ()
    {
        if (_sceneHomeBackIconSprite != null)
            return _sceneHomeBackIconSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.SceneHomeBackIconSprite != null)
            _sceneHomeBackIconSprite = cfg.SceneHomeBackIconSprite;

        return _sceneHomeBackIconSprite;
    }

    /// <summary><c>Button_FlushLeft_Gray</c> — fundo do botão Home (canto superior esquerdo).</summary>
    public static Sprite GetSceneHomeButtonFlushBackgroundSprite ()
    {
        if (_sceneHomeButtonFlushBackgroundSprite != null)
            return _sceneHomeButtonFlushBackgroundSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.SceneHomeButtonFlushBackgroundSprite != null)
            _sceneHomeButtonFlushBackgroundSprite = cfg.SceneHomeButtonFlushBackgroundSprite;

        return _sceneHomeButtonFlushBackgroundSprite;
    }

    /// <summary><c>Icon_PictoIcon_Setting02</c> — botão de menu no HUD do gameplay.</summary>
    public static Sprite GetGameplaySettingsIconSprite ()
    {
        if (_gameplaySettingsIconSprite != null)
            return _gameplaySettingsIconSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.GameplaySettingsIconSprite != null)
            _gameplaySettingsIconSprite = cfg.GameplaySettingsIconSprite;

        return _gameplaySettingsIconSprite;
    }

    /// <summary><c>Button_FlushRight_Gray</c> — fundo do botão de menu no gameplay.</summary>
    public static Sprite GetGameplaySettingsFlushRightBackgroundSprite ()
    {
        if (_gameplaySettingsFlushRightBackgroundSprite != null)
            return _gameplaySettingsFlushRightBackgroundSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.GameplaySettingsFlushRightBackgroundSprite != null)
            _gameplaySettingsFlushRightBackgroundSprite = cfg.GameplaySettingsFlushRightBackgroundSprite;

        return _gameplaySettingsFlushRightBackgroundSprite;
    }

    /// <summary><c>Icon_ImageIcon_Info</c> — botão de ajuda / «como jogar» no HUD do gameplay.</summary>
    public static Sprite GetGameplayHowToPlayIconSprite ()
    {
        if (_gameplayHowToPlayIconSprite != null)
            return _gameplayHowToPlayIconSprite;

        ContiGo2DUiSprites cfg = LoadSpritesAsset ();
        if (cfg != null && cfg.GameplayHowToPlayIconSprite != null)
            _gameplayHowToPlayIconSprite = cfg.GameplayHowToPlayIconSprite;

        return _gameplayHowToPlayIconSprite;
    }

    const float SceneHomeNavButtonWidth = 148f;
    const float SceneHomeNavButtonHeight = 132f;

    /// <summary>Botão para a cena <c>Home</c> (148×132, topo-esquerdo): <c>Button_FlushLeft_Gray</c> + ícone voltar.</summary>
    public static void AddHomeBackButtonTopLeft (RectTransform canvasRt, float insetLeft = 14f, float insetTop = 14f)
    {
        Sprite flushBg = GetSceneHomeButtonFlushBackgroundSprite ();
        Sprite backIcon = GetSceneHomeBackIconSprite ();

        GameObject go = new GameObject ("BtnBackHome", typeof (RectTransform));
        go.transform.SetParent (canvasRt, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0f, 1f);
        rt.anchorMax = new Vector2 (0f, 1f);
        rt.pivot = new Vector2 (0f, 1f);
        rt.sizeDelta = new Vector2 (SceneHomeNavButtonWidth, SceneHomeNavButtonHeight);
        rt.anchoredPosition = new Vector2 (insetLeft, -insetTop);

        Image main = go.AddComponent<Image> ();
        if (flushBg != null) {
            main.sprite = flushBg;
            main.color = Color.white;
            main.type = flushBg.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            main.preserveAspect = false;
        } else {
            main.color = new Color (0.32f, 0.33f, 0.36f, 1f);
        }
        main.raycastTarget = true;

        Button btn = go.AddComponent<Button> ();
        btn.targetGraphic = main;
        ColorBlock cb = ColorBlock.defaultColorBlock;
        cb.normalColor = Color.white;
        cb.highlightedColor = new Color (0.94f, 0.94f, 0.94f, 1f);
        cb.pressedColor = new Color (0.85f, 0.85f, 0.85f, 1f);
        cb.selectedColor = Color.white;
        cb.disabledColor = new Color (0.7f, 0.7f, 0.7f, 0.5f);
        cb.colorMultiplier = 1f;
        btn.colors = cb;
        btn.onClick.AddListener (() => SceneManager.LoadScene ("Home"));

        if (backIcon != null) {
            GameObject iconGo = new GameObject ("IconBack", typeof (RectTransform));
            iconGo.transform.SetParent (go.transform, false);
            RectTransform ir = iconGo.GetComponent<RectTransform> ();
            ir.anchorMin = Vector2.zero;
            ir.anchorMax = Vector2.one;
            ir.offsetMin = new Vector2 (28f, 22f);
            ir.offsetMax = new Vector2 (-20f, -22f);
            Image ico = iconGo.AddComponent<Image> ();
            ico.sprite = backIcon;
            ico.color = Color.white;
            ico.preserveAspect = true;
            ico.raycastTarget = false;
        }

        go.transform.SetAsLastSibling ();
    }

    /// <summary>Botão de menu / definições no gameplay (148×132, topo-direito): <c>Button_FlushRight_Gray</c> + ícone de settings.</summary>
    public static GameObject CreateGameplaySettingsMenuButton (RectTransform parent, UnityAction onClick, float insetRight = 10f, float insetTop = 10f)
    {
        return CreateGameplayFlushRightHudButton (parent, "Menu", onClick, GetGameplaySettingsIconSprite (), "IconSettings", insetRight, insetTop);
    }

    /// <summary>Botão «como jogar» / ajuda (148×132, topo-direito): mesmo fundo flush-direito + <c>Icon_ImageIcon_Info</c>.</summary>
    public static GameObject CreateGameplayHowToPlayHelpButton (RectTransform parent, UnityAction onClick, float insetRight, float insetTop = 10f)
    {
        return CreateGameplayFlushRightHudButton (parent, "Help", onClick, GetGameplayHowToPlayIconSprite (), "IconInfo", insetRight, insetTop);
    }

    static GameObject CreateGameplayFlushRightHudButton (
        RectTransform parent,
        string goName,
        UnityAction onClick,
        Sprite iconSprite,
        string iconChildName,
        float insetRight,
        float insetTop)
    {
        Sprite flushBg = GetGameplaySettingsFlushRightBackgroundSprite ();

        GameObject go = new GameObject (goName, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (1f, 1f);
        rt.anchorMax = new Vector2 (1f, 1f);
        rt.pivot = new Vector2 (1f, 1f);
        rt.sizeDelta = new Vector2 (SceneHomeNavButtonWidth, SceneHomeNavButtonHeight);
        rt.anchoredPosition = new Vector2 (-insetRight, -insetTop);

        Image main = go.AddComponent<Image> ();
        if (flushBg != null) {
            main.sprite = flushBg;
            main.color = Color.white;
            main.type = flushBg.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            main.preserveAspect = false;
        } else {
            main.color = new Color (0.32f, 0.33f, 0.36f, 1f);
        }
        main.raycastTarget = true;

        Button btn = go.AddComponent<Button> ();
        btn.targetGraphic = main;
        ColorBlock cb = ColorBlock.defaultColorBlock;
        cb.normalColor = Color.white;
        cb.highlightedColor = new Color (0.94f, 0.94f, 0.94f, 1f);
        cb.pressedColor = new Color (0.85f, 0.85f, 0.85f, 1f);
        cb.selectedColor = Color.white;
        cb.disabledColor = new Color (0.7f, 0.7f, 0.7f, 0.5f);
        cb.colorMultiplier = 1f;
        btn.colors = cb;
        if (onClick != null)
            btn.onClick.AddListener (onClick);

        if (iconSprite != null) {
            GameObject iconGo = new GameObject (iconChildName, typeof (RectTransform));
            iconGo.transform.SetParent (go.transform, false);
            RectTransform ir = iconGo.GetComponent<RectTransform> ();
            ir.anchorMin = Vector2.zero;
            ir.anchorMax = Vector2.one;
            ir.offsetMin = new Vector2 (20f, 22f);
            ir.offsetMax = new Vector2 (-28f, -22f);
            Image ico = iconGo.AddComponent<Image> ();
            ico.sprite = iconSprite;
            ico.color = Color.white;
            ico.preserveAspect = true;
            ico.raycastTarget = false;
        }

        return go;
    }
}
