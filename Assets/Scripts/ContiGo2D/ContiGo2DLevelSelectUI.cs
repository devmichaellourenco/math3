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

    [Tooltip ("GUI PRO Kit - Fantasy RPG / Sprites / Component / Button / btn_rectangle_01_n_dark (Google Play)")]
    [SerializeField] Sprite _navRankingButtonSprite;
    [Tooltip ("Opcional: fallback se ContiGo2DUiSprites não tiver sprites de modos (ex.: Google Play).")]
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

        TMP_FontAsset font = ContiGo2DSharedUi.GetBoardCellFont ();

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

        AddTitle (canvasRt, pt ? "ESCOLHA O DESAFIO" : "CHOOSE THE CHALLENGE", font, 0.86f, 0.94f, _titleFrameSprite);

        // Tenta login silencioso assim que o jogador entra no menu de modos.
        PlayGamesController.TrySignInSilent ();

        Sprite googleBg = _navRankingButtonSprite != null ? _navRankingButtonSprite : _levelChallengeButtonSprite;
        if (googleBg == null)
            googleBg = ContiGo2DSharedUi.GetModeListButtonSprite ();
        AddNavButton (canvasRt, pt ? "GOOGLE PLAY" : "GOOGLE PLAY", () => SceneManager.LoadScene ("GPGSAuth"), 0.26f, 0.34f, font, googleBg);

        AddLevelButton (canvasRt, ContiGo2DLevelId.Iniciante, pt ? "INICIANTE  (2×2)" : "BEGINNER  (2×2)", 0.68f, 0.78f, font, ContiGo2DSharedUi.GetLevelSelectRowSprite (ContiGo2DLevelId.Iniciante));
        AddLevelButton (canvasRt, ContiGo2DLevelId.Profissional, pt ? "PROFISSIONAL  (4×4)" : "PRO  (4×4)", 0.54f, 0.64f, font, ContiGo2DSharedUi.GetLevelSelectRowSprite (ContiGo2DLevelId.Profissional));
        AddLevelButton (canvasRt, ContiGo2DLevelId.Sabio, pt ? "ERUDITO  (6×6)" : "ERUDITE  (6×6)", 0.40f, 0.50f, font, ContiGo2DSharedUi.GetLevelSelectRowSprite (ContiGo2DLevelId.Sabio));
        AddLevelButton (canvasRt, ContiGo2DLevelId.Mestre, pt ? "MESTRE  (8×8)" : "MASTER  (8×8)", 0.26f, 0.36f, font, ContiGo2DSharedUi.GetLevelSelectRowSprite (ContiGo2DLevelId.Mestre));

        ContiGo2DSharedUi.AddHomeBackButtonTopLeft (canvasRt);
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
            img.color = Color.white;
            if (backgroundSprite.border.sqrMagnitude > 0.0001f) {
                img.type = Image.Type.Sliced;
                img.preserveAspect = false;
            } else {
                img.type = Image.Type.Simple;
                img.preserveAspect = true;
            }
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

}
