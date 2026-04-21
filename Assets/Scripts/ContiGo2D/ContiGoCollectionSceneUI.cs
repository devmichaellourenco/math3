using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Cena de coleção de cartas (valores + nomes fantasia + modal de detalhes para cartas desbloqueadas).</summary>
public class ContiGoCollectionSceneUI : MonoBehaviour
{
    TMP_FontAsset _font;
    /// <summary>Fonte de corpo para a lore no modal (mais legível que display).</summary>
    TMP_FontAsset _loreModalFont;
    bool _pt;
    GameObject _detailModal;
    TextMeshProUGUI _detailName;
    TextMeshProUGUI _modalRankTl;
    TextMeshProUGUI _detailLore;
    Image _detailPortrait;
    TextMeshProUGUI _detailPortraitFallback;
    RectTransform _loreContentRt;
    RectTransform _loreViewportRt;
    float _loreContentPadH = 8f;

    /// <summary>Largura/altura da célula da grelha — mesma proporção no modal de detalhe.</summary>
    const float CollectionCardW = 156f;
    const float CollectionCardH = 218f;

    /// <summary>Escala do retângulo do modal de detalhe e tetos de UI interna (relativo ao layout base).</summary>
    const float DetailModalSizeMultiplier = 2f;

    const float HomeButtonFontSize = 60f;
    const float TitleFrameW = 550f;
    const float TitleFrameH = 200f;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Button / btn_rectangle_01_n_dark (fundo do botão Home)")]
    [SerializeField] Sprite _homeButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Frame / frame_linetextframe_05_White2 (fundo do título)")]
    [SerializeField] Sprite _titleFrameSprite;

    void Awake ()
    {
        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        ContiGoProgressRuntime.ReloadFromDisk ();

        _font = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");
        _loreModalFont = Resources.Load<TMP_FontAsset> ("Fonts & Materials/LiberationSans SDF")
            ?? Resources.Load<TMP_FontAsset> ("Fonts & Materials/LiberationSans SDF - Fallback");
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
        AddTitle (canvasRt, _pt ? "COLEÇÃO" : "COLLECTION", _font, 0.88f, 0.96f, _titleFrameSprite);

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
        // Proporção ~5:7 (carta de baralho clássica).
        grid.cellSize = new Vector2 (CollectionCardW, CollectionCardH);
        grid.spacing = new Vector2 (12f, 14f);
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

        AddBackButton (canvasRt, _font, _homeButtonSprite);
        BuildDetailModal (canvasRt);
    }

    void AddCardCell (Transform parent, int id, bool unlocked)
    {
        GameObject cell = new GameObject ("Card_" + id, typeof (RectTransform));
        cell.transform.SetParent (parent, false);

        Sprite round = RoundedRectSpriteFactory.Get (64, 12);

        GameObject borderGo = new GameObject ("Border", typeof (RectTransform));
        borderGo.transform.SetParent (cell.transform, false);
        RectTransform borderRt = borderGo.GetComponent<RectTransform> ();
        borderRt.anchorMin = Vector2.zero;
        borderRt.anchorMax = Vector2.one;
        borderRt.offsetMin = Vector2.zero;
        borderRt.offsetMax = Vector2.zero;
        Image borderImg = borderGo.AddComponent<Image> ();
        borderImg.sprite = round;
        borderImg.type = Image.Type.Sliced;
        borderImg.color = unlocked
            ? new Color (0.14f, 0.16f, 0.24f, 1f)
            : new Color (0.07f, 0.08f, 0.1f, 1f);

        if (unlocked) {
            Button btn = borderGo.AddComponent<Button> ();
            btn.targetGraphic = borderImg;
            ColorBlock cb = btn.colors;
            cb.highlightedColor = new Color (0.92f, 0.94f, 1f, 1f);
            cb.pressedColor = new Color (0.82f, 0.86f, 0.95f, 1f);
            btn.colors = cb;
            int captured = id;
            btn.onClick.AddListener (() => TryOpenCardDetail (captured));
        }

        const float inset = 7f;
        GameObject faceGo = new GameObject ("Face", typeof (RectTransform));
        faceGo.transform.SetParent (cell.transform, false);
        RectTransform faceRt = faceGo.GetComponent<RectTransform> ();
        faceRt.anchorMin = Vector2.zero;
        faceRt.anchorMax = Vector2.one;
        faceRt.offsetMin = new Vector2 (inset, inset);
        faceRt.offsetMax = new Vector2 (-inset, -inset);
        Image faceImg = faceGo.AddComponent<Image> ();
        faceImg.sprite = round;
        faceImg.type = Image.Type.Sliced;
        faceImg.raycastTarget = false;
        faceImg.color = unlocked
            ? new Color (0.99f, 0.97f, 0.94f, 1f)
            : new Color (0.16f, 0.17f, 0.2f, 1f);

        bool cellHasPortrait = false;
        if (unlocked) {
            GameObject artGo = new GameObject ("CardArt", typeof (RectTransform));
            artGo.transform.SetParent (faceGo.transform, false);
            artGo.transform.SetAsFirstSibling ();
            RectTransform artRt = artGo.GetComponent<RectTransform> ();
            artRt.anchorMin = Vector2.zero;
            artRt.anchorMax = Vector2.one;
            artRt.offsetMin = Vector2.zero;
            artRt.offsetMax = Vector2.zero;
            Image artImg = artGo.AddComponent<Image> ();
            artImg.type = Image.Type.Simple;
            artImg.preserveAspect = false;
            artImg.raycastTarget = false;
            Sprite sp = ContiGoFantasyNames.TryLoadCardPortrait (id);
            if (sp != null) {
                artImg.sprite = sp;
                artImg.color = Color.white;
                faceImg.color = new Color (1f, 1f, 1f, 0f);
                cellHasPortrait = true;
            } else {
                artImg.sprite = null;
                artImg.color = new Color (0.18f, 0.2f, 0.28f, 1f);
            }
        }

        Color ink = unlocked
            ? new Color (0.12f, 0.14f, 0.22f, 1f)
            : new Color (0.45f, 0.48f, 0.55f, 1f);

        string rank = unlocked ? id.ToString () : "?";
        string name = unlocked ? ContiGoFantasyNames.GetFantasyName (id) : "";

        TextMeshProUGUI rankTl = AddCardCornerLabel (faceGo.transform, "RankTL", rank, ink, 26f, TextAlignmentOptions.TopLeft,
            new Vector2 (0f, 1f), new Vector2 (0f, 1f), new Vector2 (0f, 1f), new Vector2 (10f, -8f), new Vector2 (52f, 40f), 0f);
        TextMeshProUGUI rankBr = AddCardCornerLabel (faceGo.transform, "RankBR", rank, ink, 26f, TextAlignmentOptions.TopLeft,
            new Vector2 (1f, 0f), new Vector2 (1f, 0f), new Vector2 (1f, 0f), new Vector2 (-10f, 8f), new Vector2 (52f, 40f), 180f);

        GameObject centerGo = new GameObject ("Name", typeof (RectTransform));
        centerGo.transform.SetParent (faceGo.transform, false);
        RectTransform cRt = centerGo.GetComponent<RectTransform> ();
        cRt.anchorMin = new Vector2 (0.08f, 0.22f);
        cRt.anchorMax = new Vector2 (0.92f, 0.78f);
        cRt.offsetMin = Vector2.zero;
        cRt.offsetMax = Vector2.zero;
        TextMeshProUGUI centerTmp = centerGo.AddComponent<TextMeshProUGUI> ();
        centerTmp.text = unlocked ? name : "?";
        centerTmp.fontSize = unlocked ? 21f : 56f;
        centerTmp.fontWeight = FontWeight.Bold;
        centerTmp.alignment = TextAlignmentOptions.Center;
        centerTmp.color = ink;
        centerTmp.enableWordWrapping = true;
        centerTmp.raycastTarget = false;
        if (_font != null)
            centerTmp.font = _font;

        TextMeshProUGUI hint = null;
        if (unlocked) {
            GameObject hintGo = new GameObject ("Hint", typeof (RectTransform));
            hintGo.transform.SetParent (faceGo.transform, false);
            RectTransform hRt = hintGo.GetComponent<RectTransform> ();
            hRt.anchorMin = new Vector2 (0.06f, 0f);
            hRt.anchorMax = new Vector2 (0.94f, 0.18f);
            hRt.offsetMin = new Vector2 (4f, 4f);
            hRt.offsetMax = new Vector2 (-4f, -2f);
            hint = hintGo.AddComponent<TextMeshProUGUI> ();
            hint.text = _pt ? "ver detalhes" : "details";
            hint.fontSize = 15f;
            hint.alignment = TextAlignmentOptions.Bottom;
            hint.color = new Color (0.35f, 0.42f, 0.55f, 1f);
            hint.raycastTarget = false;
            if (_font != null)
                hint.font = _font;
        }

        if (cellHasPortrait) {
            const float ow = 0.22f;
            Color oc = new Color (0f, 0f, 0f, 0.82f);
            rankTl.outlineWidth = ow;
            rankTl.outlineColor = oc;
            rankBr.outlineWidth = ow;
            rankBr.outlineColor = oc;
            centerTmp.outlineWidth = ow;
            centerTmp.outlineColor = oc;
            if (hint != null) {
                hint.outlineWidth = ow;
                hint.outlineColor = oc;
                hint.color = new Color (0.92f, 0.94f, 1f, 1f);
            }
        }
    }

    TextMeshProUGUI AddCardCornerLabel (
        Transform parent,
        string objName,
        string text,
        Color color,
        float fontSize,
        TextAlignmentOptions align,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 pivot,
        Vector2 anchoredPos,
        Vector2 sizeDelta,
        float zRotation = 0f)
    {
        GameObject go = new GameObject (objName, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
        rt.localEulerAngles = new Vector3 (0f, 0f, zRotation);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI> ();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontWeight = FontWeight.Bold;
        tmp.alignment = align;
        tmp.color = color;
        tmp.raycastTarget = false;
        if (_font != null)
            tmp.font = _font;
        return tmp;
    }

    /// <summary>Canto do rank no modal com painel semitransparente por baixo do texto.</summary>
    TextMeshProUGUI AddModalCornerRank (
        Transform parent,
        string objName,
        string text,
        float fontSize,
        Color textColor,
        Color glassColor,
        Sprite roundSprite,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 pivot,
        Vector2 anchoredPos,
        Vector2 sizeDelta,
        float zRotation)
    {
        GameObject root = new GameObject (objName, typeof (RectTransform));
        root.transform.SetParent (parent, false);
        RectTransform rt = root.GetComponent<RectTransform> ();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
        rt.localEulerAngles = new Vector3 (0f, 0f, zRotation);

        GameObject glassGo = new GameObject ("Glass", typeof (RectTransform));
        glassGo.transform.SetParent (root.transform, false);
        RectTransform gRt = glassGo.GetComponent<RectTransform> ();
        gRt.anchorMin = Vector2.zero;
        gRt.anchorMax = Vector2.one;
        gRt.offsetMin = new Vector2 (-5f, -4f);
        gRt.offsetMax = new Vector2 (5f, 4f);
        Image glass = glassGo.AddComponent<Image> ();
        glass.sprite = roundSprite;
        glass.type = Image.Type.Sliced;
        glass.color = glassColor;
        glass.raycastTarget = false;

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (root.transform, false);
        RectTransform tRt = txtGo.GetComponent<RectTransform> ();
        tRt.anchorMin = Vector2.zero;
        tRt.anchorMax = Vector2.one;
        tRt.offsetMin = new Vector2 (3f, 2f);
        tRt.offsetMax = new Vector2 (-3f, -2f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontWeight = FontWeight.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = textColor;
        tmp.raycastTarget = false;
        if (_font != null)
            tmp.font = _font;
        return tmp;
    }

    void TryOpenCardDetail (int id)
    {
        if (!ContiGoProgressRuntime.IsCardUnlocked (id))
            return;
        string name = ContiGoFantasyNames.GetFantasyName (id);
        string rank = id.ToString ();
        if (_modalRankTl != null)
            _modalRankTl.text = rank;
        _detailName.text = name;
        _detailLore.text = ContiGoFantasyNames.GetCardLore (id, _pt);

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
        // Com o modal inativo, o viewport tem largura 0 e o Mask transparente falha com TMP — recalcular já visível.
        Canvas.ForceUpdateCanvases ();
        RefreshLoreLayout ();
    }

    void RefreshLoreLayout ()
    {
        if (_detailLore == null || _loreContentRt == null || _loreViewportRt == null)
            return;
        Canvas.ForceUpdateCanvases ();
        float pad = _loreContentPadH;
        float innerW = _loreViewportRt.rect.width - 2f * pad;
        if (innerW < 40f)
            innerW = 520f;
        float h = _detailLore.GetPreferredValues (_detailLore.text, innerW, 0f).y;
        h = Mathf.Max (48f, h);
        _loreContentRt.offsetMin = new Vector2 (pad, -h);
        _loreContentRt.offsetMax = new Vector2 (-pad, 0f);
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

        float safeW = canvasRt.rect.width > 10f ? canvasRt.rect.width - 56f : 400f * DetailModalSizeMultiplier;
        float safeH = canvasRt.rect.height > 10f ? canvasRt.rect.height - 72f : 880f * DetailModalSizeMultiplier;
        float maxW = Mathf.Clamp (safeW, 260f, 520f * DetailModalSizeMultiplier);
        float maxH = Mathf.Clamp (safeH, 360f, 920f * DetailModalSizeMultiplier);
        float cardAspect = CollectionCardW / CollectionCardH;
        float cardW = maxW;
        float cardH = cardW / cardAspect;
        if (cardH > maxH) {
            cardH = maxH;
            cardW = cardH * cardAspect;
        }

        // Conteúdo interno proporcional à largura da borda (igual à grelha 156px), com limites em px.
        float w = cardW;
        float cap = DetailModalSizeMultiplier;
        float modalInset = Mathf.Clamp (w * (7f / CollectionCardW), 6f, 14f * cap);
        float modalCorner = Mathf.Clamp (w * (26f / CollectionCardW), 20f, 34f * cap);
        float cornerPadX = Mathf.Clamp (w * (10f / CollectionCardW), 8f, 14f * cap);
        float cornerPadY = Mathf.Clamp (w * (8f / CollectionCardW), 6f, 12f * cap);
        Vector2 cornerSz = new Vector2 (
            Mathf.Clamp (w * (52f / CollectionCardW), 42f, 70f * cap),
            Mathf.Clamp (w * (40f / CollectionCardW), 34f, 54f * cap));
        float nameFont = Mathf.Clamp (w * (21f / CollectionCardW), 18f, 32f * cap);
        // Descrição: tipo de letra mais comum e um pouco menor que o título/nome.
        float loreFont = Mathf.Clamp (w * (12.5f / CollectionCardW), 14f, 20f * cap);
        float portraitFallbackFont = Mathf.Clamp (w * (56f / CollectionCardW), 40f, 72f * cap);
        float lorePadH = Mathf.Clamp (w * (8f / CollectionCardW), 6f, 14f * cap);
        float closeSide = Mathf.Clamp (w * 0.115f, 44f, 56f * cap);
        float closeInset = Mathf.Clamp (w * 0.065f, 8f, 12f * cap);
        float closeXFont = Mathf.Clamp (closeSide * 0.52f, 22f, 32f * cap);

        _loreContentPadH = lorePadH;

        // O modal é só o overlay escuro + esta carta (sem painel à volta).
        Sprite roundD = RoundedRectSpriteFactory.Get (64, 12);
        Color borderCol = new Color (0.14f, 0.16f, 0.24f, 1f);
        Color textOnGlass = new Color (0.96f, 0.97f, 1f, 1f);
        Color loreOnGlass = new Color (0.93f, 0.95f, 0.99f, 1f);
        Color glassCorner = new Color (0.04f, 0.06f, 0.1f, 0.52f);
        Color glassName = new Color (0.04f, 0.06f, 0.1f, 0.48f);
        Color glassLore = new Color (0.05f, 0.07f, 0.11f, 0.42f);

        GameObject cardRoot = new GameObject ("DetailPlayingCard", typeof (RectTransform));
        cardRoot.transform.SetParent (_detailModal.transform, false);
        RectTransform cardRt = cardRoot.GetComponent<RectTransform> ();
        cardRt.anchorMin = new Vector2 (0.5f, 0.5f);
        cardRt.anchorMax = new Vector2 (0.5f, 0.5f);
        cardRt.pivot = new Vector2 (0.5f, 0.5f);
        cardRt.anchoredPosition = Vector2.zero;
        cardRt.sizeDelta = new Vector2 (cardW, cardH);

        GameObject borderGo = new GameObject ("Border", typeof (RectTransform));
        borderGo.transform.SetParent (cardRoot.transform, false);
        RectTransform borderRt = borderGo.GetComponent<RectTransform> ();
        borderRt.anchorMin = Vector2.zero;
        borderRt.anchorMax = Vector2.one;
        borderRt.offsetMin = Vector2.zero;
        borderRt.offsetMax = Vector2.zero;
        Image borderImg = borderGo.AddComponent<Image> ();
        borderImg.sprite = roundD;
        borderImg.type = Image.Type.Sliced;
        borderImg.color = borderCol;
        borderImg.raycastTarget = true;

        GameObject faceGo = new GameObject ("Face", typeof (RectTransform));
        faceGo.transform.SetParent (cardRoot.transform, false);
        RectTransform faceRt = faceGo.GetComponent<RectTransform> ();
        faceRt.anchorMin = Vector2.zero;
        faceRt.anchorMax = Vector2.one;
        faceRt.offsetMin = new Vector2 (modalInset, modalInset);
        faceRt.offsetMax = new Vector2 (-modalInset, -modalInset);
        Image faceImg = faceGo.AddComponent<Image> ();
        faceImg.sprite = roundD;
        faceImg.type = Image.Type.Sliced;
        faceImg.raycastTarget = true;
        faceImg.color = new Color (1f, 1f, 1f, 0f);

        // Arte da carta em ecrã completo na face (832×1248 ou outro); por cima: texto com “vidro” semitransparente.
        GameObject portraitRoot = new GameObject ("CardPortraitBg", typeof (RectTransform));
        portraitRoot.transform.SetParent (faceGo.transform, false);
        RectTransform portraitRt = portraitRoot.GetComponent<RectTransform> ();
        portraitRt.anchorMin = Vector2.zero;
        portraitRt.anchorMax = Vector2.one;
        portraitRt.offsetMin = Vector2.zero;
        portraitRt.offsetMax = Vector2.zero;
        _detailPortrait = portraitRoot.AddComponent<Image> ();
        _detailPortrait.preserveAspect = false;
        _detailPortrait.type = Image.Type.Simple;
        _detailPortrait.color = new Color (0.18f, 0.22f, 0.32f, 1f);
        _detailPortrait.raycastTarget = false;

        GameObject fbGo = new GameObject ("PortraitFallback", typeof (RectTransform));
        fbGo.transform.SetParent (portraitRoot.transform, false);
        RectTransform fbRt = fbGo.GetComponent<RectTransform> ();
        fbRt.anchorMin = Vector2.zero;
        fbRt.anchorMax = Vector2.one;
        fbRt.offsetMin = Vector2.zero;
        fbRt.offsetMax = Vector2.zero;
        _detailPortraitFallback = fbGo.AddComponent<TextMeshProUGUI> ();
        _detailPortraitFallback.alignment = TextAlignmentOptions.Center;
        _detailPortraitFallback.fontSize = portraitFallbackFont;
        _detailPortraitFallback.color = new Color (0.78f, 0.84f, 0.96f, 1f);
        if (_font != null)
            _detailPortraitFallback.font = _font;
        _detailPortraitFallback.raycastTarget = false;

        _modalRankTl = AddModalCornerRank (faceGo.transform, "RankTL", "0", modalCorner, textOnGlass, glassCorner, roundD,
            new Vector2 (0f, 1f), new Vector2 (0f, 1f), new Vector2 (0f, 1f), new Vector2 (cornerPadX, -cornerPadY), cornerSz, 0f);

        // Nome numa faixa acima; descrição (lore) logo abaixo, com scroll na zona intermédia-inferior.
        GameObject nameWrap = new GameObject ("NameBlock", typeof (RectTransform));
        nameWrap.transform.SetParent (faceGo.transform, false);
        RectTransform nameWrapRt = nameWrap.GetComponent<RectTransform> ();
        nameWrapRt.anchorMin = new Vector2 (0.06f, 0.52f);
        nameWrapRt.anchorMax = new Vector2 (0.94f, 0.60f);
        nameWrapRt.offsetMin = Vector2.zero;
        nameWrapRt.offsetMax = Vector2.zero;

        GameObject nameGlass = new GameObject ("NameGlass", typeof (RectTransform));
        nameGlass.transform.SetParent (nameWrap.transform, false);
        RectTransform nameGlassRt = nameGlass.GetComponent<RectTransform> ();
        nameGlassRt.anchorMin = Vector2.zero;
        nameGlassRt.anchorMax = Vector2.one;
        nameGlassRt.offsetMin = new Vector2 (4f, 3f);
        nameGlassRt.offsetMax = new Vector2 (-4f, -3f);
        Image nameGlassImg = nameGlass.AddComponent<Image> ();
        nameGlassImg.sprite = roundD;
        nameGlassImg.type = Image.Type.Sliced;
        nameGlassImg.color = glassName;
        nameGlassImg.raycastTarget = false;

        GameObject nameGo = new GameObject ("Name", typeof (RectTransform));
        nameGo.transform.SetParent (nameWrap.transform, false);
        RectTransform nameRt = nameGo.GetComponent<RectTransform> ();
        nameRt.anchorMin = Vector2.zero;
        nameRt.anchorMax = Vector2.one;
        nameRt.offsetMin = new Vector2 (10f, 4f);
        nameRt.offsetMax = new Vector2 (-10f, -4f);
        _detailName = nameGo.AddComponent<TextMeshProUGUI> ();
        _detailName.text = "";
        _detailName.fontSize = nameFont;
        _detailName.fontWeight = FontWeight.Bold;
        _detailName.alignment = TextAlignmentOptions.Center;
        _detailName.color = textOnGlass;
        _detailName.enableWordWrapping = true;
        _detailName.raycastTarget = false;
        if (_font != null)
            _detailName.font = _font;

        GameObject loreScroll = new GameObject ("LoreScroll", typeof (RectTransform));
        loreScroll.transform.SetParent (faceGo.transform, false);
        RectTransform lsRt = loreScroll.GetComponent<RectTransform> ();
        lsRt.anchorMin = new Vector2 (0.06f, 0.04f);
        lsRt.anchorMax = new Vector2 (0.94f, 0.50f);
        lsRt.offsetMin = Vector2.zero;
        lsRt.offsetMax = Vector2.zero;

        GameObject loreGlass = new GameObject ("LoreGlass", typeof (RectTransform));
        loreGlass.transform.SetParent (loreScroll.transform, false);
        RectTransform loreGlassRt = loreGlass.GetComponent<RectTransform> ();
        loreGlassRt.anchorMin = Vector2.zero;
        loreGlassRt.anchorMax = Vector2.one;
        loreGlassRt.offsetMin = new Vector2 (3f, 3f);
        loreGlassRt.offsetMax = new Vector2 (-3f, -3f);
        Image loreGlassImg = loreGlass.AddComponent<Image> ();
        loreGlassImg.sprite = roundD;
        loreGlassImg.type = Image.Type.Sliced;
        loreGlassImg.color = glassLore;
        loreGlassImg.raycastTarget = false;

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
        lvImg.raycastTarget = true;
        lvp.AddComponent<RectMask2D> ();

        GameObject lcontent = new GameObject ("Content", typeof (RectTransform));
        lcontent.transform.SetParent (lvp.transform, false);
        RectTransform lcRt = lcontent.GetComponent<RectTransform> ();
        lcRt.anchorMin = new Vector2 (0f, 1f);
        lcRt.anchorMax = new Vector2 (1f, 1f);
        lcRt.pivot = new Vector2 (0.5f, 1f);
        lcRt.anchoredPosition = Vector2.zero;
        lcRt.offsetMin = new Vector2 (lorePadH, -120f);
        lcRt.offsetMax = new Vector2 (-lorePadH, 0f);

        _loreContentRt = lcRt;
        _loreViewportRt = lvRt;

        _detailLore = lcontent.AddComponent<TextMeshProUGUI> ();
        _detailLore.text = "";
        _detailLore.fontSize = loreFont;
        _detailLore.alignment = TextAlignmentOptions.TopJustified;
        _detailLore.color = loreOnGlass;
        _detailLore.enableWordWrapping = true;
        if (_loreModalFont != null)
            _detailLore.font = _loreModalFont;
        else if (_font != null)
            _detailLore.font = _font;

        lsr.viewport = lvRt;
        lsr.content = lcRt;

        GameObject closeGo = new GameObject ("BtnClose", typeof (RectTransform));
        closeGo.transform.SetParent (cardRoot.transform, false);
        RectTransform cr = closeGo.GetComponent<RectTransform> ();
        cr.anchorMin = new Vector2 (1f, 1f);
        cr.anchorMax = new Vector2 (1f, 1f);
        cr.pivot = new Vector2 (1f, 1f);
        cr.sizeDelta = new Vector2 (closeSide, closeSide);
        cr.anchoredPosition = new Vector2 (-closeInset, -closeInset);
        Image cimg = closeGo.AddComponent<Image> ();
        cimg.color = new Color (0.22f, 0.24f, 0.34f, 0.95f);
        cimg.sprite = roundD;
        cimg.type = Image.Type.Sliced;
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
        xtmp.fontSize = closeXFont;
        xtmp.alignment = TextAlignmentOptions.Center;
        xtmp.color = new Color (0.95f, 0.96f, 1f, 1f);
        if (_font != null)
            xtmp.font = _font;

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

    static void AddBackButton (RectTransform canvasRt, TMP_FontAsset font, Sprite homeBackgroundSprite)
    {
        GameObject go = new GameObject ("BtnBack", typeof (RectTransform));
        go.transform.SetParent (canvasRt, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.08f, 0.02f);
        rt.anchorMax = new Vector2 (0.92f, 0.09f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        Image img = go.AddComponent<Image> ();
        if (homeBackgroundSprite != null) {
            img.sprite = homeBackgroundSprite;
            img.color = Color.white;
            if (homeBackgroundSprite.border.sqrMagnitude > 0.0001f) {
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
        btn.onClick.AddListener (() => SceneManager.LoadScene ("Home"));

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (go.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (8f, 4f);
        tr.offsetMax = new Vector2 (-8f, -4f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.raycastTarget = false;
        tmp.text = "← Home";
        tmp.fontSize = HomeButtonFontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }
}
