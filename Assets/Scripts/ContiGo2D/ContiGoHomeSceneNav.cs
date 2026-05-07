using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Barra inferior na cena <c>Home</c>: Desbloqueios, Cartas, Ranking (Sky, Green, Sky).</summary>
public static class ContiGoHomeSceneNav
{
    const float NavBarHeightFraction = 0.085f;
    const float NavTabFontSizeMax = 100f;
    const float NavTabFontSizeMin = 60f;
    const string NavRootName = "ContiGoHomeNavStrip";

    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RegisterSceneHook ()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        TryBuildHomeNavIfNeeded ();
    }

    static void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        TryBuildHomeNavIfNeeded ();
    }

    static void TryBuildHomeNavIfNeeded ()
    {
        if (SceneManager.GetActiveScene ().name != "Home")
            return;
        if (GameObject.Find (NavRootName) != null)
            return;

        if (Object.FindObjectOfType<EventSystem> () == null) {
            GameObject es = new GameObject ("EventSystem");
            es.AddComponent<EventSystem> ();
            es.AddComponent<StandaloneInputModule> ();
        }

        Canvas canvas = Object.FindObjectOfType<Canvas> ();
        if (canvas == null)
            return;

        PlayGamesController.TrySignInSilent ();

        TMP_FontAsset font = ContiGo2DSharedUi.GetBoardCellFont ();
        string language = "portuguese";
        try {
            if (ES2.Exists ("language"))
                language = ES2.Load<string> ("language");
        } catch {
            language = "portuguese";
        }
        bool pt = language == "portuguese";

        RectTransform canvasRt = canvas.GetComponent<RectTransform> ();
        GameObject root = new GameObject (NavRootName, typeof (RectTransform));
        root.transform.SetParent (canvasRt, false);
        RectTransform rootRt = root.GetComponent<RectTransform> ();
        rootRt.anchorMin = Vector2.zero;
        rootRt.anchorMax = Vector2.one;
        rootRt.offsetMin = Vector2.zero;
        rootRt.offsetMax = Vector2.zero;
        rootRt.SetAsLastSibling ();

        GameObject barGo = new GameObject ("NavBar", typeof (RectTransform));
        barGo.transform.SetParent (rootRt, false);
        RectTransform barRt = barGo.GetComponent<RectTransform> ();
        barRt.anchorMin = new Vector2 (0f, 0f);
        barRt.anchorMax = new Vector2 (1f, NavBarHeightFraction);
        barRt.offsetMin = Vector2.zero;
        barRt.offsetMax = Vector2.zero;

        Image barBg = barGo.AddComponent<Image> ();
        barBg.raycastTarget = false;
        barBg.color = new Color (0.07f, 0.08f, 0.11f, 0.98f);

        HorizontalLayoutGroup hlg = barGo.AddComponent<HorizontalLayoutGroup> ();
        hlg.padding = new RectOffset (10, 10, 4, 6);
        hlg.spacing = 4;
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;

        Sprite blue = ContiGo2DSharedUi.GetHomeNavBlueButtonSprite ();
        Sprite green = ContiGo2DSharedUi.GetHomeNavGreenButtonSprite ();

        AddNavTab (barRt, pt ? "DESBLOQUEIOS" : "UNLOCKS", () => SceneManager.LoadScene ("ContiGoMissions"), font, blue);
        AddNavTab (barRt, pt ? "CARTAS" : "CARDS", () => SceneManager.LoadScene ("ContiGoCollection"), font, green);
        AddNavTab (barRt, pt ? "RANKING" : "RANK", () => SceneManager.LoadScene ("ContiGoGpgsRanking"), font, blue);
    }

    static void AddNavTab (RectTransform barParent, string label, UnityAction onClick, TMP_FontAsset font, Sprite backgroundSprite)
    {
        Sprite rowSprite = backgroundSprite;

        GameObject go = new GameObject ("Tab_" + label, typeof (RectTransform));
        go.transform.SetParent (barParent, false);

        LayoutElement le = go.AddComponent<LayoutElement> ();
        le.flexibleWidth = 1f;
        le.flexibleHeight = 1f;
        le.minWidth = 0f;
        le.minHeight = 0f;

        GameObject bgGo = new GameObject ("Background", typeof (RectTransform));
        bgGo.transform.SetParent (go.transform, false);
        RectTransform bgRt = bgGo.GetComponent<RectTransform> ();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.offsetMin = Vector2.zero;
        bgRt.offsetMax = Vector2.zero;

        Image img = bgGo.AddComponent<Image> ();
        img.raycastTarget = true;
        if (rowSprite != null) {
            img.sprite = rowSprite;
            img.color = Color.white;
            if (rowSprite.border.sqrMagnitude > 0.0001f) {
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
        tr.offsetMin = new Vector2 (8f, 2f);
        tr.offsetMax = new Vector2 (-8f, -2f);
        TextMeshProUGUI tmp = txtGo.AddComponent<TextMeshProUGUI> ();
        tmp.raycastTarget = false;
        tmp.text = label;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = NavTabFontSizeMin;
        tmp.fontSizeMax = NavTabFontSizeMax;
        tmp.fontSize = NavTabFontSizeMax;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;
        tmp.overflowMode = TextOverflowModes.Ellipsis;
        if (font != null)
            tmp.font = font;
    }
}
