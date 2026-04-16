using UnityEngine;

/// <summary>
/// Ajusta o RectTransform ao <see cref="Screen.safeArea"/> (notch, barra de gestos, etc.).
/// Coloque como pai do conteúdo que deve permanecer visível; fundo em tela cheia pode ficar fora.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent (typeof (RectTransform))]
public class ContiGoSafeArea : MonoBehaviour
{
    RectTransform _rt;
    Rect _lastSafe = new Rect (-1f, -1f, -1f, -1f);
    int _lastW = -1;
    int _lastH = -1;

    void Awake ()
    {
        _rt = GetComponent<RectTransform> ();
        Apply ();
    }

    void OnEnable ()
    {
        Apply ();
    }

    void Update ()
    {
        Rect sa = Screen.safeArea;
        if (Screen.width != _lastW || Screen.height != _lastH            || sa.x != _lastSafe.x || sa.y != _lastSafe.y
            || sa.width != _lastSafe.width || sa.height != _lastSafe.height) {
            Apply ();
        }
    }

    public void Apply ()
    {
        if (_rt == null)
            _rt = GetComponent<RectTransform> ();

        int sw = Screen.width;
        int sh = Screen.height;
        if (sw <= 0 || sh <= 0)
            return;

        Rect sa = Screen.safeArea;
        _lastSafe = sa;
        _lastW = sw;
        _lastH = sh;

        Vector2 min = new Vector2 (sa.x / sw, sa.y / sh);
        Vector2 max = new Vector2 ((sa.x + sa.width) / sw, (sa.y + sa.height) / sh);

        min.x = Mathf.Clamp01 (min.x);
        min.y = Mathf.Clamp01 (min.y);
        max.x = Mathf.Clamp01 (max.x);
        max.y = Mathf.Clamp01 (max.y);

        _rt.anchorMin = min;
        _rt.anchorMax = max;
        _rt.offsetMin = Vector2.zero;
        _rt.offsetMax = Vector2.zero;
        _rt.pivot = new Vector2 (0.5f, 0.5f);
    }
}
