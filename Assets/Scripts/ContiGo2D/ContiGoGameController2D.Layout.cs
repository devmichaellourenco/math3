using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ContiGoGameController2D
{
    IEnumerator CoBuildBoardAfterLayout (TMP_FontAsset font)
    {
        bool laidOut = false;
        for (int i = 0; i < 16; i++) {
            Canvas.ForceUpdateCanvases ();
            if (TryComputePortraitBoardLayout ()) {
                laidOut = true;
                break;
            }
            yield return null;
        }

               if (!laidOut) {
            Canvas.ForceUpdateCanvases ();
            if (!TryComputePortraitBoardLayout ()) {
                boardGridRoot.anchorMin = new Vector2 (0.05f, 0.22f);
                boardGridRoot.anchorMax = new Vector2 (0.95f, 0.78f);
                boardGridRoot.offsetMin = Vector2.zero;
                boardGridRoot.offsetMax = Vector2.zero;
                LayoutPecasRestantesStrip (BoardPad);
            }
        }

        int g = ContiGo2DLevelCatalog.GridSide (levelId);
        if (_pendingBoardValues == null || _pendingBoardValues.Count != g * g) {
            Debug.LogError ("ContiGoGameController2D: lista de valores do tabuleiro inválida para este nível.");
            yield break;
        }
        piecesInBoard = ContiGoMatch2D.BuildBoard (boardGridRoot, font, OnCellClicked, tabuleiroPecas, 0f, g, _pendingBoardValues);
        LayoutDiceStripFromBoardRoot ();
        ApplyDiceSlotSizesForStripWidth ();
        Canvas.ForceUpdateCanvases ();

        // GamePlay2D não tem GameController nem botões a chamar PlayStrategicTimer — igual ao fluxo antigo do script 2D.
        PlayStrategicTimer ();
    }

    /// <summary>
    /// De baixo para o topo do ecrã: faixa de valores por marcar → tabuleiro → dados → timer → barra (vidas | pular | marcadas).
    /// Anchors com pivot em baixo no eixo Y dentro da safe area.
    /// </summary>
    bool TryComputePortraitBoardLayout ()
    {
        if (safeAreaRt == null || boardGridRoot == null
            || diceAboveBoardRt == null || timerRowAboveDiceRt == null || topHudRowAboveTimerRt == null)
            return false;

        float w = safeAreaRt.rect.width;
        float h = safeAreaRt.rect.height;
        if (w < 2f || h < 2f)
            return false;

        float pad = BoardPad;
        float boardW = w - 2f * pad;

        float boardBottom = pad + PecasRestantesStripHeight + GapBelowPecas;
        float fixedStack = boardBottom + GapAboveBoardDice + DiceStripHeight
            + GapTimerRowAboveDice + TimerOnlyRowHeight + GapTopHudAboveTimer + TopHudRowHeight
            + GapNotificationsAboveTopHud + NotificationsRowHeight + pad;
        float availForBoard = h - fixedStack;
        float side = Mathf.Max (32f, Mathf.Min (boardW, availForBoard));
        for (int fit = 0; fit < 60; fit++) {
            float diceB = boardBottom + side + GapAboveBoardDice;
            float timerB = diceB + DiceStripHeight + GapTimerRowAboveDice;
            float topB = timerB + TimerOnlyRowHeight + GapTopHudAboveTimer;
            float topTop = topB + TopHudRowHeight + GapNotificationsAboveTopHud + NotificationsRowHeight;
            if (topTop <= h - pad + 0.5f)
                break;
            side -= 2f;
            if (side < 32f)
                return false;
        }

        boardGridRoot.anchorMin = new Vector2 (0.5f, 0f);
        boardGridRoot.anchorMax = new Vector2 (0.5f, 0f);
        boardGridRoot.pivot = new Vector2 (0.5f, 0f);
        boardGridRoot.anchoredPosition = new Vector2 (0f, boardBottom);
        boardGridRoot.sizeDelta = new Vector2 (side, side);

        float diceBottom = boardBottom + side + GapAboveBoardDice;
        diceAboveBoardRt.anchorMin = new Vector2 (0f, 0f);
        diceAboveBoardRt.anchorMax = new Vector2 (1f, 0f);
        diceAboveBoardRt.pivot = new Vector2 (0.5f, 0f);
        diceAboveBoardRt.anchoredPosition = new Vector2 (0f, diceBottom);
        diceAboveBoardRt.sizeDelta = new Vector2 (-2f * pad, DiceStripHeight);

        float timerBottom = diceBottom + DiceStripHeight + GapTimerRowAboveDice;
        timerRowAboveDiceRt.anchorMin = new Vector2 (0f, 0f);
        timerRowAboveDiceRt.anchorMax = new Vector2 (1f, 0f);
        timerRowAboveDiceRt.pivot = new Vector2 (0.5f, 0f);
        timerRowAboveDiceRt.anchoredPosition = new Vector2 (0f, timerBottom);
        timerRowAboveDiceRt.sizeDelta = new Vector2 (-2f * pad, TimerOnlyRowHeight);

        float topBottom = timerBottom + TimerOnlyRowHeight + GapTopHudAboveTimer;
        topHudRowAboveTimerRt.anchorMin = new Vector2 (0f, 0f);
        topHudRowAboveTimerRt.anchorMax = new Vector2 (1f, 0f);
        topHudRowAboveTimerRt.pivot = new Vector2 (0.5f, 0f);
        topHudRowAboveTimerRt.anchoredPosition = new Vector2 (0f, topBottom);
        topHudRowAboveTimerRt.sizeDelta = new Vector2 (-2f * pad, TopHudRowHeight);

        float notifBottom = topBottom + TopHudRowHeight + GapNotificationsAboveTopHud;
        if (notificationsRowRt != null) {
            notificationsRowRt.anchorMin = new Vector2 (0f, 0f);
            notificationsRowRt.anchorMax = new Vector2 (1f, 0f);
            notificationsRowRt.pivot = new Vector2 (0.5f, 0f);
            notificationsRowRt.anchoredPosition = new Vector2 (0f, notifBottom);
            notificationsRowRt.sizeDelta = new Vector2 (-2f * pad, NotificationsRowHeight);
        }

        LayoutPecasRestantesStrip (pad);
        return true;
    }

    void LayoutPecasRestantesStrip (float bottomInset)
    {
        if (pecasPanelRt == null)
            return;
        float pad = BoardPad;
        pecasPanelRt.anchorMin = new Vector2 (0f, 0f);
        pecasPanelRt.anchorMax = new Vector2 (1f, 0f);
        pecasPanelRt.pivot = new Vector2 (0.5f, 0f);
        pecasPanelRt.anchoredPosition = new Vector2 (0f, bottomInset);
        pecasPanelRt.sizeDelta = new Vector2 (-2f * pad, PecasRestantesStripHeight);
    }

    void LayoutDiceStripFromBoardRoot ()
    {
        if (diceAboveBoardRt == null)
            return;
        HorizontalLayoutGroup hlg = diceAboveBoardRt.GetComponent<HorizontalLayoutGroup> ();
        if (hlg != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate (diceAboveBoardRt);
    }

    void ApplyDiceSlotSizesForStripWidth ()
    {
        if (diceAboveBoardRt == null)
            return;
        float stripW = diceAboveBoardRt.rect.width;
        if (stripW < 8f)
            return;

        HorizontalLayoutGroup hlg = diceAboveBoardRt.GetComponent<HorizontalLayoutGroup> ();
        int padH = hlg != null ? hlg.padding.horizontal : 16;
        float spacing = hlg != null ? hlg.spacing : DiceHlgSpacing;
        float inner = stripW - padH - 2f * spacing;
        float side = inner / 3f;
        side = Mathf.Clamp (side, 40f, DiceSidePreferred);
        float t = side / DiceSidePreferred;
        float fontSize = Mathf.Lerp (40f, DiceFontPreferred, t);

        for (int i = 0; i < diceAboveBoardRt.childCount; i++) {
            Transform ch = diceAboveBoardRt.GetChild (i);
            LayoutElement le = ch.GetComponent<LayoutElement> ();
            if (le != null) {
                le.preferredWidth = side;
                le.preferredHeight = side;
            }
            TextMeshProUGUI tmp = ch.GetComponentInChildren<TextMeshProUGUI> (true);
            if (tmp != null)
                tmp.fontSize = fontSize;
        }

        if (txtMainTimer != null)
            txtMainTimer.fontSize = fontSize;

        if (hlg != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate (diceAboveBoardRt);
    }
}
