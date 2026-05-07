using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Cena de listagem de missões (UI gerada em runtime, alinhada ao nível select).</summary>
public class ContiGoMissionsSceneUI : MonoBehaviour
{
    const float HomeButtonFontSize = 60f;
    const float TitleFrameW = 550f;
    const float TitleFrameH = 200f;
    const int MissionListPadding = 24;
    /// <summary>Altura mínima da linha — acomodarte + ícone alto + margens do frame.</summary>
    const float MissionRowMinHeight = 220f;
    /// <summary>Largura da coluna do cartão/ícone à esquerda (o desenho escala em altura dentro da linha).</summary>
    const float MissionRowIconColumnWidth = 108f;
    const float MissionRowRightWidth = 230f;

    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Button / btn_rectangle_01_n_dark (fundo do botão Home)")]
    [SerializeField] Sprite _homeButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Button / btn_rectangle_01_n_dark (RANKING → Google)")]
    [SerializeField] Sprite _rankingButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Frame / frame_linetextframe_05_White2 (fundo do título)")]
    [SerializeField] Sprite _titleFrameSprite;

    [Header ("Missions list style (GUI Pro CasualGame)")]
    [Tooltip ("Assets/Layer Lab/GUI Pro-CasualGame/ResourcesData/Sprites/Components/Frame/Frame_ListFrame07.png")]
    [SerializeField] Sprite _missionRowFrameSprite;
    [Tooltip ("Assets/Layer Lab/GUI Pro-CasualGame/ResourcesData/Sprites/Components/Button/Button01_145_Green.Png")]
    [SerializeField] Sprite _claimButtonSprite;
    [Tooltip ("Imagem à esquerda no item da missão (ex.: carta com '?'). Se null, usa placeholder.")]
    [SerializeField] Sprite _missionLeftIconSprite;

    void Awake ()
    {
        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        ContiGoProgressRuntime.ReloadFromDisk ();

        TMP_FontAsset font = ContiGo2DSharedUi.GetBoardCellFont ();
        TMP_FontAsset missionListFont = font;
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

        AddBlueBackground (canvasRt);

        AddTitle (canvasRt, pt ? "DESBLOQUEIOS" : "UNLOCKS", font, 0.88f, 0.96f, _titleFrameSprite);

        GameObject scrollGo = new GameObject ("Scroll", typeof (RectTransform));
        scrollGo.transform.SetParent (canvasRt, false);
        RectTransform scrollRt = scrollGo.GetComponent<RectTransform> ();
        scrollRt.anchorMin = new Vector2 (0.04f, 0.2f);
        scrollRt.anchorMax = new Vector2 (0.96f, 0.86f);
        scrollRt.offsetMin = Vector2.zero;
        scrollRt.offsetMax = Vector2.zero;

        ScrollRect sr = scrollGo.AddComponent<ScrollRect> ();
        sr.horizontal = false;
        sr.vertical = true;
        sr.movementType = ScrollRect.MovementType.Clamped;
        sr.scrollSensitivity = 40f;

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
        cRt.sizeDelta = new Vector2 (0f, 0f);

        VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup> ();
        vlg.spacing = 16f;
        vlg.padding = new RectOffset (MissionListPadding, MissionListPadding, MissionListPadding, MissionListPadding);
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childForceExpandWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandHeight = false;

        ContentSizeFitter csf = content.AddComponent<ContentSizeFitter> ();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        sr.viewport = vRt;
        sr.content = cRt;

        foreach (ContiGoMissionDefinition def in ContiGoMissionsCatalog.All) {
            bool done = ContiGoProgressRuntime.IsMissionCompleted (def.Id);
            bool claimed = ContiGoProgressRuntime.IsCardUnlocked (def.TargetCardId);
            AddMissionRow (content.transform, def, done, claimed, pt, missionListFont, _missionRowFrameSprite, _claimButtonSprite, _missionLeftIconSprite);
        }

        Sprite actionBtnBg = ContiGo2DSharedUi.GetSceneActionButtonBackgroundSprite ()
            ?? (_rankingButtonSprite != null ? _rankingButtonSprite : _homeButtonSprite);
        AddRankingButton (canvasRt, pt, font, actionBtnBg);
        ContiGo2DSharedUi.AddHomeBackButtonTopLeft (canvasRt);
    }

    static void AddMissionRow (
        Transform parent,
        ContiGoMissionDefinition def,
        bool done,
        bool claimed,
        bool pt,
        TMP_FontAsset font,
        Sprite frameSprite,
        Sprite claimBtnSprite,
        Sprite leftIconSprite)
    {
        GameObject row = new GameObject ("Row_" + def.Id, typeof (RectTransform));
        row.transform.SetParent (parent, false);
        LayoutElement le = row.AddComponent<LayoutElement> ();
        le.minHeight = MissionRowMinHeight;
        le.preferredHeight = MissionRowMinHeight;

        Image bg = row.AddComponent<Image> ();
        if (frameSprite != null) {
            bg.sprite = frameSprite;
            bg.type = frameSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            bg.preserveAspect = false;
        }

        // Cores do exemplo:
        // - claim disponível: #1DB1FF
        // - pendente: #045E9D
        Color pendingCol = new Color (0.015686f, 0.368627f, 0.615686f, 1f);
        Color claimableCol = new Color (0.113725f, 0.694117f, 1f, 1f);
        bool claimable = done && !claimed;
        Color baseCol = claimable ? claimableCol : pendingCol;
        bg.color = claimable ? baseCol : new Color (baseCol.r, baseCol.g, baseCol.b, 0.92f);

        HorizontalLayoutGroup rowHlg = row.AddComponent<HorizontalLayoutGroup> ();
        // Mais espaço útil à esquerda: cartão colado ao lado interno do frame da linha.
        rowHlg.padding = new RectOffset (6, 16, 12, 12);
        rowHlg.spacing = 12;
        rowHlg.childAlignment = TextAnchor.MiddleLeft;
        rowHlg.childControlWidth = true;
        rowHlg.childForceExpandWidth = false;
        rowHlg.childControlHeight = true;
        rowHlg.childForceExpandHeight = true;

        // Ícone/cartão à esquerda: coluna fixa, altura = altura útil da linha (via HLG).
        GameObject left = new GameObject ("LeftIcon", typeof (RectTransform));
        left.transform.SetParent (row.transform, false);
        LayoutElement lle = left.AddComponent<LayoutElement> ();
        lle.preferredWidth = MissionRowIconColumnWidth;
        lle.minWidth = MissionRowIconColumnWidth;
        lle.flexibleWidth = 0f;

        RectTransform lrtRoot = left.GetComponent<RectTransform> ();
        lrtRoot.anchorMin = Vector2.zero;
        lrtRoot.anchorMax = Vector2.one;

        GameObject inner = new GameObject ("IconInner", typeof (RectTransform));
        inner.transform.SetParent (left.transform, false);
        RectTransform irt = inner.GetComponent<RectTransform> ();
        irt.anchorMin = Vector2.zero;
        irt.anchorMax = Vector2.one;
        irt.offsetMin = new Vector2 (0f, 6f);
        irt.offsetMax = new Vector2 (-2f, -6f);

        Image limg = inner.AddComponent<Image> ();
        limg.raycastTarget = false;
        if (leftIconSprite != null) {
            limg.sprite = leftIconSprite;
            limg.type = leftIconSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            limg.preserveAspect = true;
            limg.color = Color.white;
        } else {
            limg.sprite = null;
            limg.color = new Color (0f, 0f, 0f, 0.28f);
            GameObject q = new GameObject ("Q", typeof (RectTransform));
            q.transform.SetParent (inner.transform, false);
            RectTransform qrt = q.GetComponent<RectTransform> ();
            qrt.anchorMin = Vector2.zero;
            qrt.anchorMax = Vector2.one;
            qrt.offsetMin = Vector2.zero;
            qrt.offsetMax = Vector2.zero;
            TextMeshProUGUI qtxt = q.AddComponent<TextMeshProUGUI> ();
            qtxt.raycastTarget = false;
            qtxt.text = "?";
            qtxt.fontSize = 72f;
            qtxt.enableAutoSizing = true;
            qtxt.fontSizeMin = 40f;
            qtxt.fontSizeMax = 80f;
            qtxt.alignment = TextAlignmentOptions.Center;
            qtxt.color = Color.white;
            if (font != null)
                qtxt.font = font;
        }

        // Bloco central: Título (em cima) + descrição (em baixo)
        GameObject mid = new GameObject ("Mid", typeof (RectTransform));
        mid.transform.SetParent (row.transform, false);
        LayoutElement midLe = mid.AddComponent<LayoutElement> ();
        midLe.flexibleWidth = 1f;
        midLe.minWidth = 120f;
        RectTransform mrt = mid.GetComponent<RectTransform> ();
        mrt.anchorMin = Vector2.zero;
        mrt.anchorMax = Vector2.one;
        mrt.offsetMin = new Vector2 (2f, 8f);
        mrt.offsetMax = new Vector2 (-2f, -8f);

        string title = ContiGoFantasyNames.GetFantasyName (def.TargetCardId);
        if (string.IsNullOrEmpty (title))
            title = pt ? "Missão" : "Mission";

        GameObject titleGo = new GameObject ("Title", typeof (RectTransform));
        titleGo.transform.SetParent (mid.transform, false);
        RectTransform ttr = titleGo.GetComponent<RectTransform> ();
        ttr.anchorMin = new Vector2 (0f, 0.55f);
        ttr.anchorMax = new Vector2 (1f, 1f);
        ttr.offsetMin = Vector2.zero;
        ttr.offsetMax = Vector2.zero;
        TextMeshProUGUI titleTmp = titleGo.AddComponent<TextMeshProUGUI> ();
        titleTmp.raycastTarget = false;
        titleTmp.text = title;
        titleTmp.fontSize = 40f;
        titleTmp.enableAutoSizing = true;
        titleTmp.fontSizeMin = 28f;
        titleTmp.fontSizeMax = 44f;
        titleTmp.alignment = TextAlignmentOptions.TopLeft;
        titleTmp.color = Color.white;
        if (font != null)
            titleTmp.font = font;

        GameObject descGo = new GameObject ("Description", typeof (RectTransform));
        descGo.transform.SetParent (mid.transform, false);
        RectTransform dtr = descGo.GetComponent<RectTransform> ();
        dtr.anchorMin = new Vector2 (0f, 0f);
        dtr.anchorMax = new Vector2 (1f, 0.58f);
        dtr.offsetMin = Vector2.zero;
        dtr.offsetMax = Vector2.zero;
        TextMeshProUGUI descTmp = descGo.AddComponent<TextMeshProUGUI> ();
        descTmp.raycastTarget = false;
        descTmp.text = pt ? def.DescriptionPt : def.DescriptionEn;
        descTmp.fontSize = 30f;
        descTmp.enableAutoSizing = true;
        descTmp.fontSizeMin = 22f;
        descTmp.fontSizeMax = 34f;
        descTmp.alignment = TextAlignmentOptions.BottomLeft;
        descTmp.color = new Color (1f, 1f, 1f, 0.95f);
        if (font != null)
            descTmp.font = font;
        descTmp.enableWordWrapping = true;

        // Bloco da direita: Claim (se elegível) ou texto de estado (pendente/concluída)
        GameObject right = new GameObject ("Right", typeof (RectTransform));
        right.transform.SetParent (row.transform, false);
        LayoutElement rle = right.AddComponent<LayoutElement> ();
        rle.preferredWidth = MissionRowRightWidth;
        rle.minWidth = MissionRowRightWidth - 24f;
        rle.flexibleWidth = 0f;
        RectTransform rrt = right.GetComponent<RectTransform> ();
        rrt.anchorMin = Vector2.zero;
        rrt.anchorMax = Vector2.one;
        rrt.offsetMin = Vector2.zero;
        rrt.offsetMax = Vector2.zero;

        TextMeshProUGUI statusTmp = null;
        Button claimBtn = null;
        GameObject claimGo = null;

        if (claimable) {
            claimGo = new GameObject ("ClaimButton", typeof (RectTransform));
            claimGo.transform.SetParent (right.transform, false);
            RectTransform crt = claimGo.GetComponent<RectTransform> ();
            crt.anchorMin = new Vector2 (0f, 0.5f);
            crt.anchorMax = new Vector2 (1f, 0.5f);
            crt.pivot = new Vector2 (0.5f, 0.5f);
            crt.sizeDelta = new Vector2 (-20f, 92f);
            crt.anchoredPosition = Vector2.zero;

            Image cimg = claimGo.AddComponent<Image> ();
            if (claimBtnSprite != null) {
                cimg.sprite = claimBtnSprite;
                cimg.type = claimBtnSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
                cimg.preserveAspect = false;
                cimg.color = Color.white;
            } else {
                cimg.color = new Color (0.2f, 0.8f, 0.3f, 1f);
            }
            claimBtn = claimGo.AddComponent<Button> ();
            claimBtn.targetGraphic = cimg;

            GameObject ctextGo = new GameObject ("Text", typeof (RectTransform));
            ctextGo.transform.SetParent (claimGo.transform, false);
            RectTransform ctr = ctextGo.GetComponent<RectTransform> ();
            ctr.anchorMin = Vector2.zero;
            ctr.anchorMax = Vector2.one;
            ctr.offsetMin = new Vector2 (10f, 6f);
            ctr.offsetMax = new Vector2 (-10f, -6f);
            TextMeshProUGUI ctmp = ctextGo.AddComponent<TextMeshProUGUI> ();
            ctmp.raycastTarget = false;
            ctmp.text = "Claim";
            ctmp.fontSize = 34f;
            ctmp.enableAutoSizing = true;
            ctmp.fontSizeMin = 22f;
            ctmp.fontSizeMax = 36f;
            ctmp.alignment = TextAlignmentOptions.Center;
            ctmp.color = Color.white;
            if (font != null)
                ctmp.font = font;

            claimBtn.onClick.AddListener (() => {
                bool unlockedNow = ContiGoProgressRuntime.ClaimMissionReward (def);
                if (!unlockedNow)
                    return;
                // Atualiza UI do item sem reconstruir a lista.
                if (claimGo != null)
                    Object.Destroy (claimGo);
                if (statusTmp == null) {
                    GameObject stGo2 = new GameObject ("Status", typeof (RectTransform));
                    stGo2.transform.SetParent (right.transform, false);
                    RectTransform srt = stGo2.GetComponent<RectTransform> ();
                    srt.anchorMin = new Vector2 (0f, 0f);
                    srt.anchorMax = new Vector2 (1f, 1f);
                    srt.offsetMin = new Vector2 (0f, 18f);
                    srt.offsetMax = new Vector2 (-6f, -18f);
                    statusTmp = stGo2.AddComponent<TextMeshProUGUI> ();
                    statusTmp.raycastTarget = false;
                    statusTmp.fontSize = 28f;
                    statusTmp.enableAutoSizing = true;
                    statusTmp.fontSizeMin = 20f;
                    statusTmp.fontSizeMax = 32f;
                    statusTmp.alignment = TextAlignmentOptions.MidlineRight;
                    statusTmp.color = Color.white;
                    if (font != null)
                        statusTmp.font = font;
                }
                statusTmp.text = pt ? "Concluída" : "Done";
            });
        } else {
            GameObject stGo = new GameObject ("Status", typeof (RectTransform));
            stGo.transform.SetParent (right.transform, false);
            RectTransform sr = stGo.GetComponent<RectTransform> ();
            sr.anchorMin = new Vector2 (0f, 0f);
            sr.anchorMax = new Vector2 (1f, 1f);
            sr.offsetMin = new Vector2 (0f, 18f);
            sr.offsetMax = new Vector2 (-6f, -18f);
            statusTmp = stGo.AddComponent<TextMeshProUGUI> ();
            statusTmp.raycastTarget = false;
            statusTmp.text = done ? (pt ? "Concluída" : "Done") : (pt ? "Pendente" : "Pending");
            statusTmp.fontSize = 28f;
            statusTmp.enableAutoSizing = true;
            statusTmp.fontSizeMin = 20f;
            statusTmp.fontSizeMax = 32f;
            statusTmp.alignment = TextAlignmentOptions.MidlineRight;
            statusTmp.color = Color.white;
            if (font != null)
                statusTmp.font = font;
        }
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

    static void AddRankingButton (RectTransform canvasRt, bool pt, TMP_FontAsset font, Sprite backgroundSprite)
    {
        GameObject go = new GameObject ("BtnRanking", typeof (RectTransform));
        go.transform.SetParent (canvasRt, false);
        RectTransform rt = go.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.08f, 0.1f);
        rt.anchorMax = new Vector2 (0.92f, 0.18f);
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
        btn.onClick.AddListener (() => SceneManager.LoadScene ("ContiGoGpgsRanking"));

        GameObject txtGo = new GameObject ("Text", typeof (RectTransform));
        txtGo.transform.SetParent (go.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (8f, 4f);
        tr.offsetMax = new Vector2 (-8f, -4f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.raycastTarget = false;
        tmp.text = pt ? "RANKING" : "LEADERBOARD";
        tmp.fontSize = HomeButtonFontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }

}
