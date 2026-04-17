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

    public Color unmarkedColor = Color.white;
    /// <summary>Verde claro: contraste com fundo preto do tabuleiro e número ainda legível em preto.</summary>
    public Color markedColor = new Color (0.45f, 0.82f, 0.52f);

    public void Configure (int row, int col, int value, Action<ContiGoBoardCell2D> onClick)
    {
        x = row;
        y = col;
        valor = value;
        status = 0;
        label.text = ContiGoFantasyNames.FormatBoardCell (value);
        label.color = Color.black;
        label.fontWeight = TMPro.FontWeight.Bold;
        background.color = unmarkedColor;
        if (button != null) {
            // Garante que "desativado" não escurece a cor base (evita verde ficar apagado).
            ColorBlock cb = button.colors;
            cb.disabledColor = unmarkedColor;
            cb.colorMultiplier = 1f;
            button.colors = cb;
        }
        button.onClick.RemoveAllListeners ();
        if (onClick != null)
            button.onClick.AddListener (() => onClick (this));
    }

    public void SetMarked (int playerId)
    {
        status = playerId;
        background.color = markedColor;
        label.color = Color.black;
        if (button != null) {
            // Ao desativar o botão, o Unity usa disabledColor; igualamos ao verde marcado.
            ColorBlock cb = button.colors;
            cb.disabledColor = markedColor;
            cb.colorMultiplier = 1f;
            button.colors = cb;
            button.interactable = false;
        }
    }
}
