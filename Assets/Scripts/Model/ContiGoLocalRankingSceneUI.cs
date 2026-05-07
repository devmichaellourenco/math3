using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Cena para exibir o ranking local (Easy Save).</summary>
public class ContiGoLocalRankingSceneUI : MonoBehaviour
{
    const float TitleFrameW = 550f;
    const float TitleFrameH = 200f;
    const float RowHeight = 152f;
    const float RankColWidth = 150f;
    const float ScoreColWidth = 320f;
    const float RowPaddingX = 22f;
    const float RowPaddingY = 10f;
    const float ScoreRightMargin = 34f;

    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Frame / frame_linetextframe_05_White2")]
    [SerializeField] Sprite _titleFrameSprite;

    [Tooltip ("Assets/Layer Lab/GUI Pro-CasualGame/ResourcesData/Sprites/Components/Frame/ListFrame03_Single_Bg_Blue.png")]
    [SerializeField] Sprite _rowFrameSprite;

    void Awake ()
    {
        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        TMP_FontAsset font = ContiGo2DSharedUi.GetBoardCellFont ();
        bool pt = true;
        try {
            if (ES2.Exists ("language"))
                pt = ES2.Load<string> ("language") == "portuguese";
        } catch { pt = true; }

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
        AddTitle (canvasRt, pt ? "RANKING LOCAL" : "LOCAL RANKING", font, 0.88f, 0.96f, _titleFrameSprite);

        // Scroll + content
        GameObject scrollGo = new GameObject ("Scroll", typeof (RectTransform));
        scrollGo.transform.SetParent (canvasRt, false);
        RectTransform scrollRt = scrollGo.GetComponent<RectTransform> ();
        scrollRt.anchorMin = new Vector2 (0.04f, 0.1f);
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
        vlg.spacing = 10f;
        vlg.padding = new RectOffset (8, 8, 8, 8);
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childForceExpandWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandHeight = false;
        ContentSizeFitter csf = content.AddComponent<ContentSizeFitter> ();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        sr.viewport = vRt;
        sr.content = cRt;

        List<RankingEntry> list = LoadLocalRanking ();
        if (list != null)
            list.Sort (RankingEntry.Compare);

        int count = Mathf.Min (50, list != null ? list.Count : 0);
        for (int i = 0; i < count; i++) {
            RankingEntry e = list[i];
            if (e == null)
                continue;
            AddRow (content.transform, i + 1, e, font);
        }

        ContiGo2DSharedUi.AddHomeBackButtonTopLeft (canvasRt);
    }

    static List<RankingEntry> LoadLocalRanking ()
    {
        RankingData rd = FindFirstObjectByType<RankingData> ();
        if (rd == null) {
            GameObject go = new GameObject ("RankingData");
            rd = go.AddComponent<RankingData> ();
        }
        return rd.LoadRankingData ();
    }

    void AddRow (Transform parent, int position, RankingEntry entry, TMP_FontAsset font)
    {
        GameObject row = new GameObject ("Row_" + position, typeof (RectTransform));
        row.transform.SetParent (parent, false);
        LayoutElement le = row.AddComponent<LayoutElement> ();
        le.minHeight = RowHeight;
        le.preferredHeight = RowHeight;

        Image bg = row.AddComponent<Image> ();
        if (_rowFrameSprite != null) {
            bg.sprite = _rowFrameSprite;
            bg.type = _rowFrameSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            bg.preserveAspect = false;
            bg.color = Color.white;
        } else {
            bg.color = new Color (0.15f, 0.2f, 0.28f, 0.45f);
        }

        TextMeshProUGUI colRank = CreateTmp (row.transform, "Rank", position.ToString (), 46f, TextAlignmentOptions.Center, font);
        RectTransform rrtRank = colRank.rectTransform;
        rrtRank.anchorMin = new Vector2 (0f, 0f);
        rrtRank.anchorMax = new Vector2 (0f, 1f);
        rrtRank.pivot = new Vector2 (0f, 0.5f);
        rrtRank.sizeDelta = new Vector2 (RankColWidth, 0f);
        rrtRank.anchoredPosition = Vector2.zero;
        rrtRank.offsetMin = new Vector2 (RowPaddingX, RowPaddingY);
        rrtRank.offsetMax = new Vector2 (0f, -RowPaddingY);
        colRank.enableWordWrapping = false;

        TextMeshProUGUI colScore = CreateTmp (row.transform, "Score", FormatLocalResult (entry), 38f, TextAlignmentOptions.MidlineRight, font);
        RectTransform rrtScore = colScore.rectTransform;
        rrtScore.anchorMin = new Vector2 (1f, 0f);
        rrtScore.anchorMax = new Vector2 (1f, 1f);
        rrtScore.pivot = new Vector2 (1f, 0.5f);
        rrtScore.sizeDelta = new Vector2 (ScoreColWidth, 0f);
        rrtScore.anchoredPosition = new Vector2 (-RowPaddingX, 0f);
        rrtScore.offsetMin = new Vector2 (0f, RowPaddingY);
        rrtScore.offsetMax = new Vector2 (-ScoreRightMargin, -RowPaddingY);
        colScore.enableWordWrapping = false;
    }

    static string FormatLocalResult (RankingEntry e)
    {
        if (e == null)
            return "0";
        // Mantém simples: "pontos" e (se houver) tempo gasto.
        if (e.timeSpentSeconds > 0)
            return e.points + "  (" + e.timeSpentSeconds + "s)";
        return e.points.ToString ();
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
            frameImg.type = titleFrameSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            frameImg.preserveAspect = false;
        }

        TextMeshProUGUI tmp = CreateTmp (go.transform, "Text", text, 44f, TextAlignmentOptions.Center, font);
        RectTransform tr = tmp.rectTransform;
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (12f, 8f);
        tr.offsetMax = new Vector2 (-12f, -8f);
        tmp.raycastTarget = false;
    }

    static TextMeshProUGUI CreateTmp (Transform parent, string name, string text, float size, TextAlignmentOptions align, TMP_FontAsset font)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI> ();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = align;
        tmp.enableWordWrapping = true;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
        return tmp;
    }
}

