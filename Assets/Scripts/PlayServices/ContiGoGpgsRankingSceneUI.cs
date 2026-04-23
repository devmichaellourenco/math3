using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

/// <summary>Cena simples para exibir Top 10 do Google Play Games Leaderboard.</summary>
public class ContiGoGpgsRankingSceneUI : MonoBehaviour
{
    const float HomeButtonFontSize = 60f;
    const float TitleFrameW = 550f;
    const float TitleFrameH = 200f;

    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Button / btn_rectangle_01_n_dark")]
    [SerializeField] Sprite _homeButtonSprite;
    [Tooltip ("GUI PRO Kit - Fantasy RPG / ResourcesData / Sprites / Component / Frame / frame_linetextframe_05_White2")]
    [SerializeField] Sprite _titleFrameSprite;

    TextMeshProUGUI _status;
    Transform _rowsParent;
    readonly List<GameObject> _rowGos = new List<GameObject> ();

    void Awake ()
    {
        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        TMP_FontAsset font = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");
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
        AddTitle (canvasRt, pt ? "RANKING" : "LEADERBOARD", font, 0.88f, 0.96f, _titleFrameSprite);

        // Status
        _status = CreateTmp (canvasRt, "Status", pt ? "A ligar ao Play Games…" : "Connecting to Play Games…", 30f, TextAlignmentOptions.Center, font);
        RectTransform stRt = _status.rectTransform;
        stRt.anchorMin = new Vector2 (0.06f, 0.82f);
        stRt.anchorMax = new Vector2 (0.94f, 0.88f);
        stRt.offsetMin = Vector2.zero;
        stRt.offsetMax = Vector2.zero;
        _status.color = new Color (0.9f, 0.92f, 1f, 0.95f);

        // Scroll + content
        GameObject scrollGo = new GameObject ("Scroll", typeof (RectTransform));
        scrollGo.transform.SetParent (canvasRt, false);
        RectTransform scrollRt = scrollGo.GetComponent<RectTransform> ();
        scrollRt.anchorMin = new Vector2 (0.04f, 0.1f);
        scrollRt.anchorMax = new Vector2 (0.96f, 0.80f);
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

        _rowsParent = content.transform;

        // Botões: Atualizar + Abrir Google + Home
        AddActionButtons (canvasRt, font, pt);
        AddBackButton (canvasRt, font, _homeButtonSprite);

        PlayGamesController.TrySignInSilent ();
        StartCoroutine (LoadAfterAuth (pt));
    }

    IEnumerator LoadAfterAuth (bool pt)
    {
        float t0 = Time.unscaledTime;
        while (Time.unscaledTime - t0 < 5.0f) {
            if (Social.localUser != null && Social.localUser.authenticated)
                break;
            yield return new WaitForSecondsRealtime (0.15f);
        }
        Refresh (pt);
    }

    void Refresh (bool pt)
    {
        string lid = PlayGamesController.GetLeaderboardId ();
        if (string.IsNullOrEmpty (lid)) {
            SetStatus (pt ? "LeaderboardId não configurado." : "LeaderboardId not configured.");
            return;
        }

        if (Social.localUser == null || !Social.localUser.authenticated) {
            SetStatus (pt ? "Play Games offline. Tente abrir o Google ou reconectar." : "Play Games offline. Try opening Google or reconnecting.");
            return;
        }

        SetStatus (pt ? "A carregar ranking…" : "Loading leaderboard…");

        ILeaderboard lb = Social.CreateLeaderboard ();
        lb.id = lid;
        lb.timeScope = TimeScope.AllTime;
        lb.userScope = UserScope.Global;
        lb.range = new Range (1, 10);

        lb.LoadScores (success => {
            if (!success) {
                SetStatus (pt ? "Falha ao carregar ranking." : "Failed to load leaderboard.");
                return;
            }
            IScore[] scores = lb.scores;
            if (scores == null || scores.Length == 0) {
                ClearRows ();
                SetStatus (pt ? "Sem dados no ranking ainda." : "No leaderboard data yet.");
                return;
            }

            // Carregar nomes de utilizador (se possível)
            var userIds = new List<string> ();
            foreach (IScore s in scores) {
                if (s != null && !string.IsNullOrEmpty (s.userID) && !userIds.Contains (s.userID))
                    userIds.Add (s.userID);
            }

            Social.LoadUsers (userIds.ToArray (), profiles => {
                var map = new Dictionary<string, string> ();
                if (profiles != null) {
                    foreach (var p in profiles) {
                        if (p == null || string.IsNullOrEmpty (p.id))
                            continue;
                        string name = !string.IsNullOrEmpty (p.userName) ? p.userName : p.id;
                        map[p.id] = name;
                    }
                }
                RenderRows (scores, map);
                SetStatus (pt ? "Top 10 global (Play Games)" : "Global Top 10 (Play Games)");
            });
        });
    }

    void RenderRows (IScore[] scores, Dictionary<string, string> userNameById)
    {
        ClearRows ();
        int count = Mathf.Min (10, scores != null ? scores.Length : 0);
        for (int i = 0; i < count; i++) {
            IScore s = scores[i];
            if (s == null)
                continue;
            string uname = s.userID;
            if (userNameById != null && !string.IsNullOrEmpty (s.userID) && userNameById.TryGetValue (s.userID, out string n))
                uname = n;

            AddRow (_rowsParent, s, uname);
        }
    }

    void AddRow (Transform parent, IScore s, string userName)
    {
        GameObject row = new GameObject ("Row", typeof (RectTransform));
        row.transform.SetParent (parent, false);
        LayoutElement le = row.AddComponent<LayoutElement> ();
        le.minHeight = 110f;
        le.preferredHeight = 110f;

        Image bg = row.AddComponent<Image> ();
        bg.color = new Color (0.15f, 0.2f, 0.28f, 0.45f);

        TMP_FontAsset font = Resources.Load<TMP_FontAsset> ("Fonts & Materials/Anton SDF");

        TextMeshProUGUI left = CreateTmp (row.transform, "Left", "", 34f, TextAlignmentOptions.MidlineLeft, font);
        RectTransform lrt = left.rectTransform;
        lrt.anchorMin = new Vector2 (0f, 0f);
        lrt.anchorMax = new Vector2 (0.70f, 1f);
        lrt.offsetMin = new Vector2 (12f, 6f);
        lrt.offsetMax = new Vector2 (-8f, -6f);
        left.enableWordWrapping = false;
        left.overflowMode = TextOverflowModes.Ellipsis;
        left.color = Color.white;

        TextMeshProUGUI right = CreateTmp (row.transform, "Right", "", 32f, TextAlignmentOptions.MidlineRight, font);
        RectTransform rrt = right.rectTransform;
        rrt.anchorMin = new Vector2 (0.70f, 0f);
        rrt.anchorMax = new Vector2 (1f, 1f);
        rrt.offsetMin = new Vector2 (4f, 6f);
        rrt.offsetMax = new Vector2 (-12f, -6f);
        right.enableWordWrapping = false;
        right.color = new Color (0.92f, 0.94f, 1f, 0.95f);

        string rank = s.rank > 0 ? s.rank.ToString () : (s.rank >= 0 ? (s.rank + 1).ToString () : "-");
        string name = string.IsNullOrEmpty (userName) ? "Jogador" : userName;
        left.text = rank + ". " + name;
        right.text = !string.IsNullOrEmpty (s.formattedValue) ? s.formattedValue : s.value.ToString ();

        _rowGos.Add (row);
    }

    void ClearRows ()
    {
        foreach (GameObject go in _rowGos) {
            if (go != null)
                Destroy (go);
        }
        _rowGos.Clear ();
    }

    void SetStatus (string msg)
    {
        if (_status != null)
            _status.text = msg;
    }

    void AddActionButtons (RectTransform canvasRt, TMP_FontAsset font, bool pt)
    {
        GameObject bar = new GameObject ("TopButtons", typeof (RectTransform));
        bar.transform.SetParent (canvasRt, false);
        RectTransform rt = bar.GetComponent<RectTransform> ();
        rt.anchorMin = new Vector2 (0.08f, 0.02f);
        rt.anchorMax = new Vector2 (0.92f, 0.09f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        HorizontalLayoutGroup hlg = bar.AddComponent<HorizontalLayoutGroup> ();
        hlg.spacing = 12f;
        hlg.padding = new RectOffset (0, 0, 0, 0);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = true;
        hlg.childForceExpandWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandHeight = true;

        CreateBarButton (rt, pt ? "ATUALIZAR" : "REFRESH", font, () => Refresh (pt));
        CreateBarButton (rt, pt ? "ABRIR GOOGLE" : "OPEN GOOGLE", font, PlayGamesController.ShowLeaderboardUI);
    }

    static void CreateBarButton (RectTransform parent, string text, TMP_FontAsset font, UnityEngine.Events.UnityAction onClick)
    {
        Button b = CreateTextButton (parent, text, text, font, onClick);
        Image img = b.GetComponent<Image> ();
        if (img != null)
            img.color = new Color (0.2f, 0.45f, 0.75f, 1f);
        SetButtonTextSize (b, 34f);
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
            frameImg.type = titleFrameSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            frameImg.preserveAspect = false;
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
            img.type = homeBackgroundSprite.border.sqrMagnitude > 0.0001f ? Image.Type.Sliced : Image.Type.Simple;
            img.preserveAspect = false;
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
        tmp.text = "HOME";
        tmp.fontSize = HomeButtonFontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
    }

    // Helpers (copiados do padrão do projeto)
    static Button CreateTextButton (Transform parent, string name, string label, TMP_FontAsset font, UnityEngine.Events.UnityAction onClick)
    {
        GameObject go = new GameObject (name, typeof (RectTransform));
        go.transform.SetParent (parent, false);
        Image img = go.AddComponent<Image> ();
        img.color = new Color (0.25f, 0.45f, 0.75f, 1f);
        Button b = go.AddComponent<Button> ();
        b.targetGraphic = img;
        b.onClick.AddListener (onClick);

        GameObject txtGo = new GameObject ("Txt", typeof (RectTransform));
        txtGo.transform.SetParent (go.transform, false);
        RectTransform tr = txtGo.GetComponent<RectTransform> ();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2 (8f, 4f);
        tr.offsetMax = new Vector2 (-8f, -4f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.raycastTarget = false;
        tmp.text = label;
        tmp.fontSize = 34f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        if (font != null)
            tmp.font = font;
        return b;
    }

    static void SetButtonTextSize (Button b, float size)
    {
        if (b == null)
            return;
        Transform tr = b.transform.Find ("Txt");
        if (tr == null)
            return;
        TextMeshProUGUI tmp = tr.GetComponent<TextMeshProUGUI> ();
        if (tmp != null)
            tmp.fontSize = size;
    }
}

