using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ContiGoGameController2D
{
    readonly ContiGoMatchSessionState _missionSession = new ContiGoMatchSessionState ();
    readonly Queue<string> _notificationQueue = new Queue<string> ();
    Coroutine _coNotifications;

    void ResetMissionSessionForNewMatch ()
    {
        _missionSession.ResetForNewBoard (levelId);
    }

    void OnMatchPlayStarted ()
    {
        _missionSession.OnPlayStarted ();
        List<ContiGoUnlockEvent> unlocks = ContiGoProgressRuntime.ProcessAfterMatchStart (_missionSession);
        if (unlocks != null && unlocks.Count > 0)
            EnqueueUnlockNotifications (unlocks);
    }

    void OnMissionCorrectHit (int pieceValue)
    {
        int errsBefore = _missionSession.Errors;
        _missionSession.RegisterCorrectHit (pieceValue);
        List<ContiGoUnlockEvent> unlocks = ContiGoProgressRuntime.ProcessAfterHit (_missionSession, pieceValue, errsBefore);
        if (unlocks != null && unlocks.Count > 0)
            EnqueueUnlockNotifications (unlocks);
    }

    void OnMissionWrongChoice ()
    {
        _missionSession.RegisterError ();
    }

    void OnMissionVictory ()
    {
        _missionSession.MarkVictory ();
        List<ContiGoUnlockEvent> unlocks = ContiGoProgressRuntime.ProcessAfterVictory (_missionSession, timer, jogador.vidas);
        if (unlocks != null && unlocks.Count > 0)
            EnqueueUnlockNotifications (unlocks);
    }

    void EnqueueUnlockNotifications (List<ContiGoUnlockEvent> events)
    {
        bool pt = language == "portuguese";
        foreach (ContiGoUnlockEvent ev in events) {
            string msg = pt
                ? ("Carta desbloqueada: " + ev.FantasyName + " (" + ev.CardId + ")")
                : ("Card unlocked: " + ev.FantasyName + " (" + ev.CardId + ")");
            _notificationQueue.Enqueue (msg);
        }
        if (_coNotifications == null)
            _coNotifications = StartCoroutine (CoRunNotifications ());
    }

    IEnumerator CoRunNotifications ()
    {
        if (txtNotifications == null) {
            _coNotifications = null;
            yield break;
        }
        while (_notificationQueue.Count > 0) {
            SoundManager.PlaySound (SoundManager.Sound.UIConfimation);
            txtNotifications.text = _notificationQueue.Dequeue ();

            // fade in
            for (float t = 0f; t < 0.18f; t += Time.unscaledDeltaTime) {
                txtNotifications.alpha = Mathf.Clamp01 (t / 0.18f);
                yield return null;
            }
            txtNotifications.alpha = 1f;

            float hold = 2.2f;
            for (float t = 0f; t < hold; t += Time.unscaledDeltaTime)
                yield return null;

            // fade out
            for (float t = 0f; t < 0.25f; t += Time.unscaledDeltaTime) {
                txtNotifications.alpha = 1f - Mathf.Clamp01 (t / 0.25f);
                yield return null;
            }
            txtNotifications.alpha = 0f;
        }
        txtNotifications.text = "";
        _coNotifications = null;
    }
}
