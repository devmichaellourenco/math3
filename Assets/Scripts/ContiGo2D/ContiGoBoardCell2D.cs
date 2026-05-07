using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Célula do tabuleiro 8x8 em UI (Canvas). Não substitui <see cref="Peca"/> 3D.</summary>
public class ContiGoBoardCell2D : MonoBehaviour
{
    public int x;
    public int y;
    public int valor;
    public int status;

    public Image background;
    public TextMeshProUGUI label;
    public Button button;

    /// <summary>Cor do texto nas células do tabuleiro.</summary>
    public static readonly Color BoardCellLabelColor = Color.white;

    public Color unmarkedColor = Color.white;
    /// <summary>Verde claro da célula marcada; texto mantém <see cref="BoardCellLabelColor"/>.</summary>
    public Color markedColor = new Color (0.45f, 0.82f, 0.52f);
    /// <summary>Feedback visual de resposta errada (fundo + estados do botão).</summary>
    public static readonly Color WrongAnswerFlashColor = new Color (0.9f, 0.22f, 0.2f, 1f);

    public void Configure (int row, int col, int value, Action<ContiGoBoardCell2D> onClick)
    {
        x = row;
        y = col;
        valor = value;
        status = 0;
        label.text = ContiGoFantasyNames.FormatBoardCell (value);
        label.color = BoardCellLabelColor;
        label.fontWeight = TMPro.FontWeight.Bold;
        if (background != null)
            background.color = unmarkedColor;
        ApplyDefaultUnmarkedButtonColors ();
        button.onClick.RemoveAllListeners ();
        if (onClick != null)
            button.onClick.AddListener (() => onClick (this));
    }

    void ApplyDefaultUnmarkedButtonColors ()
    {
        if (button == null)
            return;
        ColorBlock cb = ColorBlock.defaultColorBlock;
        cb.normalColor = Color.white;
        cb.highlightedColor = new Color (0.96f, 0.96f, 0.96f, 1f);
        cb.pressedColor = new Color (0.88f, 0.88f, 0.88f, 1f);
        cb.selectedColor = Color.white;
        cb.disabledColor = unmarkedColor;
        cb.colorMultiplier = 1f;
        button.colors = cb;
    }

    /// <summary>Vermelho no fill e nos tints do <see cref="Button"/> (inclui estado desativado durante o lock de input).</summary>
    public void ApplyWrongAnswerVisual ()
    {
        if (background != null)
            background.color = WrongAnswerFlashColor;
        if (button == null)
            return;
        ColorBlock cb = button.colors;
        Color r = WrongAnswerFlashColor;
        cb.normalColor = r;
        cb.highlightedColor = new Color (1f, 0.42f, 0.38f, 1f);
        cb.pressedColor = new Color (0.68f, 0.14f, 0.12f, 1f);
        cb.selectedColor = r;
        cb.disabledColor = r;
        cb.colorMultiplier = 1f;
        button.colors = cb;
    }

    /// <summary>Volta ao aspeto normal da célula por marcar (só se ainda não estiver marcada).</summary>
    public void RestoreUnmarkedAppearance ()
    {
        if (status != 0)
            return;
        if (background != null)
            background.color = unmarkedColor;
        ApplyDefaultUnmarkedButtonColors ();
    }

    public void SetMarked (int playerId)
    {
        status = playerId;
        if (background != null)
            background.color = markedColor;
        label.color = BoardCellLabelColor;
        if (button != null) {
            ColorBlock cb = button.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = new Color (0.96f, 0.96f, 0.96f, 1f);
            cb.pressedColor = new Color (0.88f, 0.88f, 0.88f, 1f);
            cb.selectedColor = Color.white;
            cb.disabledColor = markedColor;
            cb.colorMultiplier = 1f;
            button.colors = cb;
            button.interactable = false;
        }
    }
}
