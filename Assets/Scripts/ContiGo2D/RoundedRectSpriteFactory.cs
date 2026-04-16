using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fabrica sprites de retângulo arredondado gerados em runtime (com 9-slice).
/// Evita dependência de assets (PNGs) e permite cantos levemente arredondados em uGUI.
/// </summary>
public static class RoundedRectSpriteFactory
{
    struct Key
    {
        public int size;
        public int radius;
    }

    static readonly Dictionary<long, Sprite> Cache = new Dictionary<long, Sprite> ();

    static long PackKey (Key k)
    {
        return ((long)k.size << 32) | (uint)k.radius;
    }

    public static Sprite Get (int size = 64, int radius = 10)
    {
        size = Mathf.Clamp (size, 16, 256);
        radius = Mathf.Clamp (radius, 2, size / 2 - 1);

        Key k = new Key { size = size, radius = radius };
        long packed = PackKey (k);
        if (Cache.TryGetValue (packed, out Sprite existing) && existing != null)
            return existing;

        Texture2D tex = new Texture2D (size, size, TextureFormat.RGBA32, false);
        tex.name = $"RoundedRect_{size}_r{radius}";
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        Color32[] px = new Color32[size * size];
        byte aFull = 255;
        byte aZero = 0;

        float r = radius;
        float r2 = r * r;
        float innerMin = r - 0.5f;
        float innerMax = (size - 1) - (r - 0.5f);

        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float xf = x;
                float yf = y;

                float cx = Mathf.Clamp (xf, innerMin, innerMax);
                float cy = Mathf.Clamp (yf, innerMin, innerMax);
                float dx = xf - cx;
                float dy = yf - cy;
                float d2 = dx * dx + dy * dy;

                byte a = d2 <= r2 ? aFull : aZero;
                px[y * size + x] = new Color32 (255, 255, 255, a);
            }
        }

        tex.SetPixels32 (px);
        tex.Apply (false, false);

        Sprite sp = Sprite.Create (tex, new Rect (0, 0, size, size), new Vector2 (0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect,
            new Vector4 (radius, radius, radius, radius));
        sp.name = tex.name;

        Cache[packed] = sp;
        return sp;
    }
}

