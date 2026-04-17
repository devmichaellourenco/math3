using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Cena de coleção de cartas (valores + nomes fantasia + modal de detalhes para cartas desbloqueadas).</summary>
public class ContiGoCollectionSceneUI : MonoBehaviour
{
    TMP_FontAsset _font;
    bool _pt;
    GameObject _detailModal;
    TextMeshProUGUI _detailName;
    TextMeshProUGUI _detailNumber;
    TextMeshProUGUI _detailLore;
    Image _detailPortrait;
    TextMeshProUGUI _detailPortraitFallback;
    RectTransform _loreContentRt;
    RectTransform _loreViewportRt;

    void Awake ()
    {
        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        ContiGoProgressRuntime.ReloadFromDisk ();

        _font = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");
        string language = "portuguese";
        try {
            if (ES2.Exists ("language"))
                language = ES2.Load<string> ("language");
        } catch {
            language = "portuguese";
        }
        _pt = language == "portuguese";

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

        AddBlueBackground (canvasRt);
        AddTitle (canvasRt, _pt ? "COLEÇÃO" : "COLLECTION", _font, 0.88f, 0.96f);

        GameObject scrollGo = new GameObject ("Scroll", typeof (RectTransform));
        scrollGo.transform.SetParent (canvasRt, false);
        RectTransform scrollRt = scrollGo.GetComponent<RectTransform> ();
        scrollRt.anchorMin = new Vector2 (0.03f, 0.1f);
        scrollRt.anchorMax = new Vector2 (0.97f, 0.86f);
        scrollRt.offsetMin = Vector2.zero;
        scrollRt.offsetMax = Vector2.zero;

        ScrollRect sr = scrollGo.AddComponent<ScrollRect> ();
        sr.horizontal = false;
        sr.vertical = true;

        GameObject viewport = new GameObject ("Viewport", typeof (RectTransform));
        viewport.transform.SetParent (scrollGo.transform, false);
        RectTransform vRt = viewport.GetComponent<RectTransform> ();
        vRt.anchorMin = Vector2.zero;
        vRt.anchorMax = Vector2.one;
        vRt.offsetMin = Vector2.zero;
        vRt.offsetMax = Vector2.zero;
        Image vMask = viewport.AddComponent<Image> ();
        vMask.color = new Color (1f, 1f, 1f, 0.01f);
        viewport.AddComponent<Mask> ().showMaskGraphic = false;

        GameObject content = new GameObject ("Content", typeof (RectTransform));
        content.transform.SetParent (viewport.transform, false);
        RectTransform cRt = content.GetComponent<RectTransform> ();
        cRt.anchorMin = new Vector2 (0f, 1f);
        cRt.anchorMax = new Vector2 (1f, 1f);
        cRt.pivot = new Vector2 (0.5f, 1f);
        cRt.anchoredPosition = Vector2.zero;

        GridLayoutGroup grid = content.AddComponent<GridLayoutGroup> ();
        grid.cellSize = new Vector2 (200f, 120f);
        grid.spacing = new Vector2 (10f, 10f);
        grid.padding = new RectOffset (12, 12, 12, 12);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.UpperCenter;

        ContentSizeFitter csf = content.AddComponent<ContentSizeFitter> ();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        sr.viewport = vRt;
        sr.content = cRt;

        foreach (int id in ContiGoFantasyNames.AllRegisteredValueIdsSorted) {
            bool unlocked = ContiGoProgressRuntime.IsCardUnlocked (id);
            AddCardCell (content.transform, id, unlocked);
        }

        AddBackButton (canvasRt, _font);
        BuildDetailModal (canvasRt);
    }

    void AddCardCell (Transform parent, int id, bool unlocked)
    {
        GameObject cell = new GameObject ("Card_" + id, typeof (RectTransform));
        cell.transform.SetParent (parent, false);

        Image bg = cell.AddComponent<Image> ();
        bg.color = unlocked ? new Color (0.92f, 0.95f, 1f, 1f) : new Color (0.12f, 0.14f, 0.18f, 1f);

        if (unlocked) {
            Button btn = cell.AddComponent<Button> ();
            btn.targetGraphic = bg;
            int captured = id;
            btn.onClick.AddListener (() => TryOpenCardDetail (captured));
        }

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (cell.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (6f, 6f);
        tr.offsetMax = new Vector2 (-6f, -6f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        if (unlocked) {
            string name = ContiGoFantasyNames.GetFantasyName (id);
            tmp.text = name + "\n<size=80%>(" + id + ")</size>";
            if (_pt)
                tmp.text += "\n<size=68%><#4a5f8a>ver detalhes</size></size>";
            else
                tmp.text += "\n<size=68%><#4a5f8a>tap for details</size></size>";
            tmp.richText = true;
            tmp.color = new Color (0.1f, 0.12f, 0.18f, 1f);
        } else {
            tmp.text = "?\n<size=70%>—</size>";
            tmp.richText = true;
            tmp.color = new Color (0.55f, 0.58f, 0.65f, 1f);
        }
        tmp.fontSize = 22f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = true;
        if (_font != null)
            tmp.font = _font;
    }

    void TryOpenCardDetail (int id)
    {
        if (!ContiGoProgressRuntime.IsCardUnlocked (id))
            return;
        string name = ContiGoFantasyNames.GetFantasyName (id);
        _detailName.text = name;
        _detailNumber.text = _pt ? ("Valor: " + id) : ("Value: " + id);
        _detailLore.text = ContiGoFantasyNames.GetCardLore (id, _pt);
        RefreshLoreLayout ();

        Sprite sp = ContiGoFantasyNames.TryLoadCardPortrait (id);
        if (sp != null) {
            _detailPortrait.sprite = sp;
            _detailPortrait.color = Color.white;
            _detailPortrait.enabled = true;
            _detailPortraitFallback.gameObject.SetActive (false);
        } else {
            _detailPortrait.sprite = null;
            _detailPortrait.color = new Color (0.18f, 0.22f, 0.32f, 1f);
            _detailPortrait.enabled = true;
            _detailPortraitFallback.gameObject.SetActive (true);
            string initial = string.IsNullOrEmpty (name) ? "?" : name.Substring (0, 1).ToUpperInvariant ();
            _detailPortraitFallback.text = initial;
        }

        _detailModal.SetActive (true);
        _detailModal.transform.SetAsLastSibling ();
    }

    void RefreshLoreLayout ()
    {
        if (_detailLore == null || _loreContentRt == null || _loreViewportRt == null)
            return;
        Canvas.ForceUpdateCanvases ();
        float innerW = _loreViewportRt.rect.width - 16f;
        if (innerW < 40f)
            innerW = 520f;
        float h = _detailLore.GetPreferredValues (_detailLore.text, innerW, 0f).y;
        h = Mathf.Max (48f, h);
        _loreContentRt.offsetMin = new Vector2 (8f, -h);
        _loreContentRt.offsetMax = new Vector2 (-8f, 0f);
    }

    void CloseDetailModal ()
    {
        if (_detailModal != null)
            _detailModal.SetActive (false);
    }

    void BuildDetailModal (RectTransform canvasRt)
    {
        _detailModal = new GameObject ("CardDetailModal");
        _detailModal.transform.SetParent (canvasRt, false);
        RectTransform rootRt = _detailModal.AddComponent<RectTransform> ();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;

        Image dim = _detailModal.AddComponent<Image> ();
        dim.color = new Color (0f, 0f, 0f, 0.62f);
        Button dimBtn = _detailModal.AddComponent<Button> ();
        dimBtn.targetGraphic = dim;
        dimBtn.onClick.AddListener (CloseDetailModal);

        GameObject panel = new GameObject ("Panel", typeof (RectTransform));
        panel.transform.SetParent (_detailModal.transform, false);
        RectTransform prt = panel.GetComponent<RectTransform> ();
        prt.anchorMin = new Vector2 (0.5f, 0.5f);
        prt.anchorMax = new Vector2 (0.5f, 0.5f);
        prt.pivot = new Vector2 (0.5f, 0.5f);
        prt.sizeDelta = new Vector2 (Mathf.Min (980f, canvasRt.rect.width > 10f ? canvasRt.rect.width - 48f : 980f), 1180f);

        Image pbg = panel.AddComponent<Image> ();
        pbg.color = new Color (0.96f, 0.97f, 1f, 1f);
        pbg.sprite = RoundedRectSpriteFactory.Get (64, 12);
        pbg.type = Image.Type.Sliced;

        GameObject closeGo = new GameObject ("BtnClose", typeof (RectTransform));
        closeGo.transform.SetParent (panel.transform, false);
        RectTransform cr = closeGo.GetComponent<RectTransform> ();
        cr.anchorMin = new Vector2 (1f, 1f);
        cr.anchorMax = new Vector2 (1f, 1f);
        cr.pivot = new Vector2 (1f, 1f);
        cr.sizeDelta = new Vector2 (72f, 72f);
        cr.anchoredPosition = new Vector2 (-12f, -12f);
        Image cimg = closeGo.AddComponent<Image> ();
        cimg.color = new Color (0.85f, 0.88f, 0.94f, 1f);
        Button cbtn = closeGo.AddComponent<Button> ();
        cbtn.targetGraphic = cimg;
        cbtn.onClick.AddListener (CloseDetailModal);
        GameObject ctx = new GameObject ("X", typeof (RectTransform));
        ctx.transform.SetParent (closeGo.transform, false);
        RectTransform cx = ctx.GetComponent<RectTransform> ();
        cx.anchorMin = Vector2.zero;
        cx.anchorMax = Vector2.one;
        cx.offsetMin = Vector2.zero;
        cx.offsetMax = Vector2.zero;
        TextMeshProUGUI xtmp = ctx.AddComponent<TextMeshProUGUI> ();
        xtmp.text = "\u00D7";
        xtmp.fontSize = 40f;
        xtmp.alignment = TextAlignmentOptions.Center;
        xtmp.color = new Color (0.15f, 0.17f, 0.22f, 1f);
        if (_font != null)
            xtmp.font = _font;

        GameObject portraitGo = new GameObject ("Portrait", typeof (RectTransform));
        portraitGo.transform.SetParent (panel.transform, false);
        RectTransform portRt = portraitGo.GetComponent<RectTransform> ();
        portRt.anchorMin = new Vector2 (0.5f, 1f);
        portRt.anchorMax = new Vector2 (0.5f, 1f);
        portRt.pivot = new Vector2 (0.5f, 1f);
        portRt.sizeDelta = new Vector2 (300f, 300f);
        portRt.anchoredPosition = new Vector2 (0f, -88f);
        _detailPortrait = portraitGo.AddComponent<Image> ();
        _detailPortrait.preserveAspect = true;
        _detailPortrait.color = new Color (0.18f, 0.22f, 0.32f, 1f);

        GameObject fbGo = new GameObject ("PortraitFallback", typeof (RectTransform));
        fbGo.transform.SetParent (portraitGo.transform, false);
        RectTransform fbRt = fbGo.GetComponent<RectTransform> ();
        fbRt.anchorMin = Vector2.zero;
        fbRt.anchorMax = Vector2.one;
        fbRt.offsetMin = Vector2.zero;
        fbRt.offsetMax = Vector2.zero;
        _detailPortraitFallback = fbGo.AddComponent<TextMeshProUGUI> ();
        _detailPortraitFallback.alignment = TextAlignmentOptions.Center;
        _detailPortraitFallback.fontSize = 120f;
        _detailPortraitFallback.color = new Color (0.75f, 0.82f, 0.95f, 1f);
        if (_font != null)
            _detailPortraitFallback.font = _font;
        _detailPortraitFallback.raycastTarget = false;

        _detailName = CreateTmpInParent (panel.transform, "Name", "", 40f, TextAlignmentOptions.Center, new Vector2 (0.06f, 0.52f), new Vector2 (0.94f, 0.62f), _font);
        _detailNumber = CreateTmpInParent (panel.transform, "NumberLine", "", 30f, TextAlignmentOptions.Center, new Vector2 (0.06f, 0.46f), new Vector2 (0.94f, 0.52f), _font);
        _detailName.color = new Color (0.08f, 0.1f, 0.14f, 1f);
        _detailNumber.color = new Color (0.2f, 0.24f, 0.32f, 1f);

        GameObject loreScroll = new GameObject ("LoreScroll", typeof (RectTransform));
        loreScroll.transform.SetParent (panel.transform, false);
        RectTransform lsRt = loreScroll.GetComponent<RectTransform> ();
        lsRt.anchorMin = new Vector2 (0.06f, 0.08f);
        lsRt.anchorMax = new Vector2 (0.94f, 0.44f);
        lsRt.offsetMin = Vector2.zero;
        lsRt.offsetMax = Vector2.zero;

        ScrollRect lsr = loreScroll.AddComponent<ScrollRect> ();
        lsr.horizontal = false;
        lsr.vertical = true;
        lsr.movementType = ScrollRect.MovementType.Clamped;

        GameObject lvp = new GameObject ("Viewport", typeof (RectTransform));
        lvp.transform.SetParent (loreScroll.transform, false);
        RectTransform lvRt = lvp.GetComponent<RectTransform> ();
        lvRt.anchorMin = Vector2.zero;
        lvRt.anchorMax = Vector2.one;
        lvRt.offsetMin = Vector2.zero;
        lvRt.offsetMax = Vector2.zero;
        Image lvImg = lvp.AddComponent<Image> ();
        lvImg.color = new Color (1f, 1f, 1f, 0.02f);
        lvp.AddComponent<Mask> ().showMaskGraphic = false;

        GameObject lcontent = new GameObject ("Content", typeof (RectTransform));
        lcontent.transform.SetParent (lvp.transform, false);
        RectTransform lcRt = lcontent.GetComponent<RectTransform> ();
        lcRt.anchorMin = new Vector2 (0f, 1f);
        lcRt.anchorMax = new Vector2 (1f, 1f);
        lcRt.pivot = new Vector2 (0.5f, 1f);
        lcRt.anchoredPosition = Vector2.zero;
        lcRt.offsetMin = new Vector2 (8f, -120f);
        lcRt.offsetMax = new Vector2 (-8f, 0f);

        _loreContentRt = lcRt;
        _loreViewportRt = lvRt;

        _detailLore = lcontent.AddComponent<TextMeshProUGUI> ();
        _detailLore.text = "";
        _detailLore.fontSize = 28f;
        _detailLore.alignment = TextAlignmentOptions.TopJustified;
        _detailLore.color = new Color (0.12f, 0.14f, 0.2f, 1f);
        _detailLore.enableWordWrapping = true;
        if (_font != null)
            _detailLore.font = _font;

        lsr.viewport = lvRt;
        lsr.content = lcRt;

        _detailModal.SetActive (false);
    }

    static TextMeshProUGUI CreateTmpInParent (
        Transform parent,
        string name,
        string text,
        float size,
        TextAlignmentOptions align,
        Vector2 anchorMin,
        Vector2 anchorMax,
        TMP_FontAsset font)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI> ();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = align;
        tmp.enableWordWrapping = true;
        if (font != null)
            tmp.font = font;
        return tmp;
    }

    static void AddTitle (RectTransform parent, string text, TMP_FontAsset font, float yMin, float yMax)
    {
        GameObject go = new GameObject ("Title", typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.06f, yMin);
        rt.anchorMax = new Vector2 (0.94f, yMax);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI> ();
        tmp.text = text;
        tmp.fontSize = 44f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }

    static void AddBlueBackground (RectTransform canvasRt)
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
        bg.raycastTarget = false;
        Sprite bgSp = Resources.Load<Sprite> ("Imagens/bg-square-blue-vertical.fw")
            ?? Resources.Load<Sprite> ("Imagens/bg-square-blue");
        if (bgSp != null)
            bg.sprite = bgSp;
    }

    static void AddBackButton (RectTransform canvasRt, TMP_FontAsset font)
    {
        GameObject go = new GameObject ("BtnBack", typeof (RectTransform));
        go.transform.SetParent (canvasRt, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.08f, 0.02f);
        rt.anchorMax = new Vector2 (0.92f, 0.09f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        Image img = go.AddComponent<Image> ();
        img.color = new Color (0.2f, 0.45f, 0.75f, 1f);
        Button btn = go.AddComponent<Button> ();
        btn.targetGraphic = img;
        btn.onClick.AddListener (() => SceneManager.LoadScene ("Home"));

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (go.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (8f, 4f);
        tr.offsetMax = new Vector2 (-8f, -4f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.text = "← Home";
        tmp.fontSize = 36f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }
}
