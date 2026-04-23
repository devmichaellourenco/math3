using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Cena leve: escolher Iniciante … Mestre antes do <see cref="ContiGoGameController2D"/>.</summary>
public class ContiGo2DLevelSelectUI : MonoBehaviour
{
    const float LevelSelectFontSize = 60f;
    const float TitleFrameW = 550f;
    const float TitleFrameH = 200f;

    [Tooltip ("GUI PRO Kit - Fantasy RPG / Sprites / Component / Button / btn_rectangle_01_n_mint")]
    [SerializeField] Sprite _navMissionsButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / Sprites / Component / Button / btn_rectangle_01_n_yellow")]
    [SerializeField] Sprite _navCollectionButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / Sprites / Component / Button / btn_rectangle_01_n_dark (Ranking Google)")]
    [SerializeField] Sprite _navRankingButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / Sprites / Component / Button / btn_rectangle_01_n_dark (botões Iniciante…Mestre)")]
    [SerializeField] Sprite _levelChallengeButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Frame / frame_linetextframe_05_White2 (fundo do título)")]
    [SerializeField] Sprite _titleFrameSprite;

    void Awake ()
    {
        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        TMP_FontAsset font = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");

        // Idioma segue o mesmo padrão do jogo (ES2 key "language").
        string language = "portuguese";
        try {
            if (ES2.Exists ("language"))
                language = ES2.Load<string> ("language");
        } catch {
            language = "portuguese";
        }
        bool pt = language == "portuguese";

        GameObject canvasGo = new GameObject ("Canvas", typeof (RectTransform));
        Canvas canvas = canvasGo.AddComponent<Canvas> ();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler> ();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2 (1080f, 1920f);
        canvasGo.AddComponent<GraphicRaycaster> ();

        RectTransform canvasRt = canvasGo.GetComponent<RectTransform> ();
        canvasRt.anchorMin = Vector2.zero;
        canvasRt.anchorMax = Vector2.one;
        canvasRt.offsetMin = Vector2.zero;
        canvasRt.offsetMax = Vector2.zero;

        AddBlueSceneBackground (canvasRt);

        Sprite levelRowSprite = _levelChallengeButtonSprite != null
            ? _levelChallengeButtonSprite
            : LoadLevelRowPanelSprite ();

        AddTitle (canvasRt, pt ? "ESCOLHA O DESAFIO" : "CHOOSE THE CHALLENGE", font, 0.86f, 0.94f, _titleFrameSprite);

        // Tenta login silencioso assim que o jogador entra no menu de modos.
        PlayGamesController.TrySignInSilent ();

        AddNavButton (canvasRt, pt ? "GOOGLE PLAY" : "GOOGLE PLAY", () => SceneManager.LoadScene ("GPGSAuth"), 0.26f, 0.34f, font, _navRankingButtonSprite != null ? _navRankingButtonSprite : _levelChallengeButtonSprite);
        AddNavButton (canvasRt, pt ? "MISSÕES" : "MISSIONS", () => SceneManager.LoadScene ("ContiGoMissions"), 0.18f, 0.26f, font, _navMissionsButtonSprite);
        AddNavButton (canvasRt, pt ? "RANKING" : "LEADERBOARD", () => SceneManager.LoadScene ("ContiGoGpgsRanking"), 0.10f, 0.18f, font, _navRankingButtonSprite != null ? _navRankingButtonSprite : _levelChallengeButtonSprite);
        AddNavButton (canvasRt, pt ? "COLEÇÃO" : "COLLECTION", () => SceneManager.LoadScene ("ContiGoCollection"), 0.02f, 0.10f, font, _navCollectionButtonSprite);

        AddLevelButton (canvasRt, ContiGo2DLevelId.Iniciante, pt ? "INICIANTE  (2×2)" : "BEGINNER  (2×2)", 0.68f, 0.78f, font, levelRowSprite);
        AddLevelButton (canvasRt, ContiGo2DLevelId.Profissional, pt ? "PROFISSIONAL  (4×4)" : "PRO  (4×4)", 0.54f, 0.64f, font, levelRowSprite);
        AddLevelButton (canvasRt, ContiGo2DLevelId.Sabio, pt ? "SÁBIO  (6×6)" : "SAGE  (6×6)", 0.40f, 0.50f, font, levelRowSprite);
        AddLevelButton (canvasRt, ContiGo2DLevelId.Mestre, pt ? "MESTRE  (8×8)" : "MASTER  (8×8)", 0.26f, 0.36f, font, levelRowSprite);

        AddCloseButtonTopRight (canvasRt, () => SceneManager.LoadScene ("Home"));
    }

    /// <summary>Igual ao fundo do <see cref="ContiGoGameController2D"/> (mosaico azul).</summary>
    static void AddBlueSceneBackground (RectTransform canvasRt)
    {
        GameObject bgGo = new GameObject ("Background", typeof (RectTransform));
        bgGo.transform.SetParent (canvasRt, false);
        RectTransform bgRt = bgGo.GetComponent<RectTransform> ();
        bgRt.SetAsFirstSibling ();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;
        Image bg = bgGo.AddComponent<Image> ();
        bg.color = Color.white;
        bg.type = Image.Type.Tiled;
        bg.preserveAspect = false;
        bg.raycastTarget = false;
        Sprite bgSp = Resources.Load<Sprite> ("Imagens/bg-square-blue-vertical.fw");
        if (bgSp == null)
            bgSp = Resources.Load<Sprite> ("Imagens/bg-square-blue");
        if (bgSp != null)
            bg.sprite = bgSp;
    }

    static Sprite LoadLevelRowPanelSprite ()
    {
        return Resources.Load<Sprite> ("Imagens/CONFIRMA-QUIT-HOME-PAINEL.fw")
            ?? Resources.Load<Sprite> ("Imagens/CONFIRMA-QUIT-HOME-PANEL.fw");
    }

    static void AddTitle (RectTransform parent, string text, TMP_FontAsset font, float yMin, float yMax, Sprite titleFrameSprite)
    {
        float yMid = (yMin + yMax) * 0.5f;

        GameObject go = new GameObject ("Title", typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.5f, yMid);
        rt.anchorMax = new Vector2 (0.5f, yMid);
        rt.pivot = new Vector2 (0.5f, 0.5f);
        rt.sizeDelta = new Vector2 (TitleFrameW, TitleFrameH);
        rt.anchoredPosition = Vector2.zero;

        if (titleFrameSprite != null) {
            GameObject frameGo = new GameObject ("TitleFrame", typeof (RectTransform));
            frameGo.transform.SetParent (go.transform, false);
            RectTransform fr = frameGo.GetComponent<RectTransform> ();
            fr.anchorMin = Vector2.zero;
            fr.anchorMax = Vector2.one;
            fr.offsetMin = Vector2.zero;
            fr.offsetMax = Vector2.zero;
            Image frameImg = frameGo.AddComponent<Image> ();
            frameImg.sprite = titleFrameSprite;
            frameImg.color = Color.white;
            frameImg.raycastTarget = false;
            if (titleFrameSprite.border.sqrMagnitude > 0.0001f) {
                frameImg.type = Image.Type.Sliced;
                frameImg.preserveAspect = false;
            } else {
                frameImg.type = Image.Type.Simple;
                frameImg.preserveAspect = false;
            }
        }

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (go.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (12f, 8f);
        tr.offsetMax = new Vector2 (-12f, -8f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.raycastTarget = false;
        tmp.text = text;
        tmp.fontSize = LevelSelectFontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }

    static void AddLevelButton (RectTransform parent, ContiGo2DLevelId id, string label, float yMin, float yMax, TMP_FontAsset font, Sprite rowPanelSprite)
    {
        string scene = ContiGo2DLevelCatalog.SceneFileName (id);
        // Raiz só define a faixa no ecrã; cada botão é independente. O sprite partilhado fica no filho Image (sem AspectRatioFitter no raiz — evita sobreposição total).
        GameObject root = new GameObject ("BtnLevel_" + id, typeof (RectTransform));
        root.transform.SetParent (parent, false);
        RectTransform rt = root.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.08f, yMin);
        rt.anchorMax = new Vector2 (0.92f, yMax);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        GameObject bgGo = new GameObject ("Background", typeof (RectTransform));
        bgGo.transform.SetParent (root.transform, false);
        RectTransform bgRt = bgGo.GetComponent<RectTransform> ();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;
        Image img = bgGo.AddComponent<Image> ();
        img.raycastTarget = true;
        if (rowPanelSprite != null) {
            img.sprite = rowPanelSprite;
            img.color = Color.white;
            if (rowPanelSprite.border.sqrMagnitude > 0.0001f) {
                img.type = Image.Type.Sliced;
                img.preserveAspect = false;
            } else {
                img.type = Image.Type.Simple;
                img.preserveAspect = true;
            }
        } else {
            img.color = new Color (0.2f, 0.45f, 0.75f, 1f);
        }

        Button btn = root.AddComponent<Button> ();
        btn.targetGraphic = img;
        btn.onClick.AddListener (() => SceneManager.LoadScene (scene));

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (root.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (8f, 4f);
        tr.offsetMax = new Vector2 (-8f, -4f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.raycastTarget = false;
        tmp.text = label;
        tmp.fontSize = LevelSelectFontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }

    static void AddNavButton (RectTransform parent, string label, UnityAction onClick, float yMin, float yMax, TMP_FontAsset font, Sprite backgroundSprite)
    {
        GameObject go = new GameObject ("Btn_" + label, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.08f, yMin);
        rt.anchorMax = new Vector2 (0.92f, yMax);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image img = go.AddComponent<Image> ();
        if (backgroundSprite != null) {
            img.sprite = backgroundSprite;
            img.type = Image.Type.Sliced;
            img.color = Color.white;
            img.preserveAspect = false;
        } else {
            img.color = new Color (0.2f, 0.45f, 0.75f, 1f);
        }
        Button btn = go.AddComponent<Button> ();
        btn.targetGraphic = img;
        btn.onClick.AddListener (onClick);

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (go.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (8f, 4f);
        tr.offsetMax = new Vector2 (-8f, -4f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.raycastTarget = false;
        tmp.text = label;
        tmp.fontSize = LevelSelectFontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }

    static void AddCloseButtonTopRight (RectTransform parent, UnityAction onClick)
    {
        // Igual estilo do "GameAbout" na Home: botão de fechar no topo direito.
        GameObject go = new GameObject ("BtnClose", typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (1f, 1f);
        rt.anchorMax = new Vector2 (1f, 1f);
        rt.pivot = new Vector2 (1f, 1f);
        rt.sizeDelta = new Vector2 (96f, 96f);
        rt.anchoredPosition = new Vector2 (-18f, -18f);

        Image img = go.AddComponent<Image> ();
        img.color = Color.white;
        Button b = go.AddComponent<Button> ();
        b.targetGraphic = img;
        b.onClick.AddListener (onClick);

        // SpriteSwap com hover/pressed se existirem.
        SetupSpriteSwapFromImagens (b, img, "btn-close-home.fw");
    }

    static void SetupSpriteSwapFromImagens (Button b, Image img, string normalSpriteFileName)
    {
        Sprite n = Resources.Load<Sprite> ("Imagens/" + normalSpriteFileName);
        if (n == null) {
            img.color = new Color (0.28f, 0.45f, 0.7f, 1f);
            return;
        }
        int dot = normalSpriteFileName.LastIndexOf ('.');
        string prefix = dot >= 0 ? normalSpriteFileName.Substring (0, dot) : normalSpriteFileName;
        string ext = dot >= 0 ? normalSpriteFileName.Substring (dot) : "";
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
    }
}
