using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System;
using System.Reflection;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

/// <summary>Integração simples com Google Play Games (via API Social) para leaderboards.</summary>
public class PlayGamesController : MonoBehaviour
{
    const string KeyLeaderboardIdOverride = "gpgs_leaderboard_id";
    const string KeyBestReported = "gpgs_leaderboard_best_reported_v1";

    static PlayGamesController _instance;

    [Header ("Google Play Games / Leaderboards")]
    [Tooltip ("ID do leaderboard. Se vazio, usa GPGSIds (gerado pelo plugin) ou ES2 key gpgs_leaderboard_id.")]
    [SerializeField] string leaderboardId = "";

    [Header ("Opcional (cena GPGSAuth)")]
    public Text mainText;
    string _statusLine = "Play Games: a autenticar…";

    // Estado simples para UI/Debug (GameOver / GPGSAuth).
    static string _lastLeaderboardPostMsg = "";
    static bool _lastLeaderboardPostSuccess = false;
    static bool _lastLeaderboardPostHadAttempt = false;
    static bool _lastLeaderboardPostInFlight = false;
    static float _lastLeaderboardPostStartedAt = -1f;
    static float _lastLeaderboardPostCompletedAt = -1f;

    public static string GetLastLeaderboardPostMessage ()
    {
        return _lastLeaderboardPostMsg ?? "";
    }

    public static bool GetLastLeaderboardPostInFlight ()
    {
        return _lastLeaderboardPostInFlight;
    }

    public static float GetLastLeaderboardPostSecondsSinceStart ()
    {
        if (_lastLeaderboardPostStartedAt < 0f)
            return -1f;
        return Time.realtimeSinceStartup - _lastLeaderboardPostStartedAt;
    }

    void Awake ()
    {
        if (_instance != null && _instance != this) {
            Destroy (gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad (gameObject);
        TryActivatePlayGamesPlatform ();
        TrySignInSilent ();

        if (mainText != null) {
            CancelInvoke (nameof (RefreshDebugText));
            InvokeRepeating (nameof (RefreshDebugText), 0.05f, 0.5f);
        }
    }

    static void TryActivatePlayGamesPlatform ()
    {
#if UNITY_ANDROID
        // Este plugin (v2.1.0 do GPGS) compila PlayGamesPlatform apenas em UNITY_ANDROID.
        // Por isso, aqui ativamos direto no Android.
        try {
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate ();
            Debug.Log ("PlayGamesController: GPGS ativado.");
        } catch (Exception e) {
            Debug.LogWarning ("PlayGamesController: falha a ativar GPGS: " + e);
        }
#else
        // Em Editor / outras plataformas, não faz nada. (GPGS real só existe no Android.)
#endif
    }

    void Start ()
    {
        if (mainText != null) {
            mainText.text = _statusLine;
            mainText.color = Color.white;
        }
    }

    static PlayGamesController EnsureExists ()
    {
        if (_instance != null)
            return _instance;
        PlayGamesController found = FindFirstObjectByType<PlayGamesController> ();
        if (found != null) {
            _instance = found;
            return _instance;
        }
        GameObject go = new GameObject ("PlayGamesController");
        _instance = go.AddComponent<PlayGamesController> ();
        return _instance;
    }

    string ResolveLeaderboardId ()
    {
        if (!string.IsNullOrEmpty (leaderboardId))
            return leaderboardId;
        try {
            if (ES2.Exists (KeyLeaderboardIdOverride)) {
                string v = ES2.Load<string> (KeyLeaderboardIdOverride);
                if (!string.IsNullOrEmpty (v))
                    return v;
            }
        } catch {
            // ignore
        }
        return GPGSIds.leaderboard_ranking;
    }

    public static string GetLeaderboardId ()
    {
        return EnsureExists ().ResolveLeaderboardId ();
    }

    public static void TrySignInSilent ()
    {
        EnsureExists ().TrySignInSilentInstance ();
    }

    void TrySignInSilentInstance ()
    {
        if (Social.localUser != null && Social.localUser.authenticated) {
            UpdateStatusText ("Play Games: conectado");
            FlushBestLocalIfNeededInstance ();
            return;
        }

        if (Social.localUser == null) {
            UpdateStatusText ("Play Games: indisponível");
            return;
        }

        Social.localUser.Authenticate (success => {
            UpdateStatusText (success ? "Play Games: conectado" : "Play Games: offline");
            if (success)
                FlushBestLocalIfNeededInstance ();
        });
    }

    void UpdateStatusText (string msg)
    {
        _statusLine = msg ?? "";
        if (mainText == null)
            return;
        mainText.text = BuildDebugText (msg);
        mainText.color = (msg != null && msg.Contains ("conectado")) ? new Color (0.75f, 1f, 0.78f) : Color.gray;
    }

    void RefreshDebugText ()
    {
        if (mainText == null)
            return;
        mainText.text = BuildDebugText (_statusLine);
    }

    string BuildDebugText (string headline)
    {
        string lid = null;
        try { lid = ResolveLeaderboardId (); } catch { lid = null; }

        bool hasUser = Social.localUser != null;
        bool authed = hasUser && Social.localUser.authenticated;
        string userName = hasUser ? Social.localUser.userName : "(null)";
        string userId = hasUser ? Social.localUser.id : "(null)";

        return
            (string.IsNullOrEmpty (headline) ? "Play Games" : headline) + "\n\n" +
            "authenticated: " + authed + "\n" +
            "userName: " + userName + "\n" +
            "userId: " + userId + "\n" +
            "leaderboardId: " + (string.IsNullOrEmpty (lid) ? "(vazio)" : lid);
    }

    /// <summary>Envia uma pontuação para o leaderboard (se autenticado).</summary>
    public static void PostToLeaderboard (long newScore)
    {
        EnsureExists ().PostToLeaderboardInstance (newScore);
    }

    void PostToLeaderboardInstance (long newScore)
    {
        _lastLeaderboardPostHadAttempt = true;
        _lastLeaderboardPostSuccess = false;
        _lastLeaderboardPostInFlight = false;
        _lastLeaderboardPostStartedAt = -1f;
        _lastLeaderboardPostCompletedAt = -1f;
        string lid = ResolveLeaderboardId ();
        if (string.IsNullOrEmpty (lid)) {
            _lastLeaderboardPostMsg = "Falha: leaderboardId vazio.";
            Debug.LogWarning ("PlayGamesController: leaderboardId vazio. Defina no Inspector ou via ES2 key " + KeyLeaderboardIdOverride + ".");
            return;
        }

        if (Social.localUser == null || !Social.localUser.authenticated) {
            _lastLeaderboardPostMsg = "Falha: Play Games offline (não autenticado).";
            Debug.Log ("PlayGamesController: não autenticado; mantendo ranking local. Score pendente: " + newScore);
            return;
        }

        _lastLeaderboardPostMsg = "A enviar score para o Google… (" + newScore + ")";
        _lastLeaderboardPostInFlight = true;
        _lastLeaderboardPostStartedAt = Time.realtimeSinceStartup;
        Social.ReportScore (newScore, lid, success => {
            Debug.Log ("PlayGamesController: ReportScore(" + newScore + ") => " + success);
            _lastLeaderboardPostSuccess = success;
            _lastLeaderboardPostInFlight = false;
            _lastLeaderboardPostCompletedAt = Time.realtimeSinceStartup;
            _lastLeaderboardPostMsg = success
                ? "Enviado para o Google com sucesso. (" + newScore + ")"
                : "Falha ao enviar para o Google. (" + newScore + ")";
            if (success) {
                try { ES2.Save ((int)Mathf.Clamp (newScore, 0, int.MaxValue), KeyBestReported); } catch { }
            }
        });
    }

    /// <summary>Abre a UI nativa do leaderboard (Play Games).</summary>
    public static void ShowLeaderboardUI ()
    {
        EnsureExists ().ShowLeaderboardUIInstance ();
    }

    void ShowLeaderboardUIInstance ()
    {
        if (Social.localUser == null || !Social.localUser.authenticated) {
            TrySignInSilentInstance ();
            Debug.Log ("PlayGamesController: precisa autenticar para mostrar leaderboard.");
            return;
        }
        Social.ShowLeaderboardUI ();
    }

    /// <summary>Sincroniza o melhor score local com o Google (se ainda não foi reportado).</summary>
    public static void FlushBestLocalIfNeeded ()
    {
        EnsureExists ().FlushBestLocalIfNeededInstance ();
    }

    void FlushBestLocalIfNeededInstance ()
    {
        string lid = ResolveLeaderboardId ();
        if (string.IsNullOrEmpty (lid))
            return;

        RankingData rd = new RankingData ();
        var list = rd.LoadRankingData ();
        if (list == null || list.Count == 0)
            return;

        RankingEntry best = null;
        foreach (var e in list) {
            if (e == null)
                continue;
            if (best == null || RankingEntry.Compare (e, best) < 0)
                best = e;
        }
        if (best == null)
            return;

        int bestLocal = Mathf.Max (0, best.points);
        int bestReported = 0;
        try { if (ES2.Exists (KeyBestReported)) bestReported = ES2.Load<int> (KeyBestReported); } catch { bestReported = 0; }

        if (bestLocal <= bestReported)
            return;

        PostToLeaderboardInstance (bestLocal);
    }
}
