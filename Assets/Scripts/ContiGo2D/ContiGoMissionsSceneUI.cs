using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Cena de listagem de missões (UI gerada em runtime, alinhada ao nível select).</summary>
public class ContiGoMissionsSceneUI : MonoBehaviour
{
    void Awake ()
    {
        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        ContiGoProgressRuntime.ReloadFromDisk ();

        TMP_FontAsset font = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");
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

        AddTitle (canvasRt, pt ? "MISSÕES" : "MISSIONS", font, 0.88f, 0.96f);

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

        foreach (ContiGoMissionDefinition def in ContiGoMissionsCatalog.All) {
            bool done = ContiGoProgressRuntime.IsMissionCompleted (def.Id);
            AddMissionRow (content.transform, def, done, pt, font);
        }

        AddBackButton (canvasRt, font);
    }

    static void AddMissionRow (Transform parent, ContiGoMissionDefinition def, bool done, bool pt, TMP_FontAsset font)
    {
        GameObject row = new GameObject ("Row_" + def.Id, typeof (RectTransform));
        row.transform.SetParent (parent, false);
        LayoutElement le = row.AddComponent<LayoutElement> ();
        le.minHeight = 140f;
        le.preferredHeight = 140f;

        Image bg = row.AddComponent<Image> ();
        bg.color = done ? new Color (0.35f, 0.62f, 0.42f, 0.35f) : new Color (0.15f, 0.2f, 0.28f, 0.45f);

        GameObject txtGo = new GameObject ("Desc", typeof (RectTransform));
        txtGo.transform.SetParent (row.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = new Vector2 (0f, 0f);
        tr.anchorMax = new Vector2 (0.78f, 1f);
        tr.offsetMin = new Vector2 (12f, 6f);
        tr.offsetMax = new Vector2 (-8f, -6f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.text = pt ? def.DescriptionPt : def.DescriptionEn;
        tmp.fontSize = 32f;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 24f;
        tmp.fontSizeMax = 36f;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
        tmp.enableWordWrapping = true;

        GameObject stGo = new GameObject ("Status", typeof (RectTransform));
        stGo.transform.SetParent (row.transform, false);
        RectTransform sr = stGo.GetComponent<RectTransform> ();
        sr.anchorMin = new Vector2 (0.78f, 0f);
        sr.anchorMax = new Vector2 (1f, 1f);
        sr.offsetMin = new Vector2 (4f, 6f);
        sr.offsetMax = new Vector2 (-12f, -6f);
        TextMeshProUGUI st = stGo.AddComponent<TextMeshProUGUI> ();
        st.text = done ? (pt ? "Concluída" : "Done") : (pt ? "Pendente" : "Pending");
        st.fontSize = 28f;
        st.enableAutoSizing = true;
        st.fontSizeMin = 20f;
        st.fontSizeMax = 32f;
        st.alignment = TextAlignmentOptions.MidlineRight;
        st.color = done ? new Color (0.85f, 1f, 0.88f) : new Color (0.85f, 0.85f, 0.9f);
        if (font != null)
            st.font = font;
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
