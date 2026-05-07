using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Monta um tabuleiro N×N em um RectTransform com GridLayoutGroup.</summary>
public static class ContiGoMatch2D
{
    public static ContiGoBoardCell2D[,] BuildBoard (
        RectTransform gridRoot,
        TMP_FontAsset font,
        System.Action<ContiGoBoardCell2D> onCellClicked,
        List<ContiGoBoardCell2D> outAllCells,
        float cellSideFixed,
        int gridSide,
        IList<int> boardValuesShuffled)
    {
        if (gridSide < 2 || gridSide > 8)
            gridSide = 8;
        int need = gridSide * gridSide;
        if (boardValuesShuffled == null || boardValuesShuffled.Count != need) {
            Debug.LogError ($"ContiGoMatch2D.BuildBoard: esperados {need} valores, recebidos {boardValuesShuffled?.Count ?? 0}.");
            return new ContiGoBoardCell2D[gridSide, gridSide];
        }

        outAllCells.Clear ();
        foreach (Transform c in gridRoot)
            Object.Destroy (c.gameObject);

        GridLayoutGroup grid = gridRoot.GetComponent<GridLayoutGroup> ();
        if (grid == null)
            grid = gridRoot.gameObject.AddComponent<GridLayoutGroup> ();
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.UpperLeft;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = gridSide;
        grid.spacing = new Vector2 (6f, 6f);
        grid.padding = new RectOffset (8, 8, 8, 8);

        float side;
        if (cellSideFixed > 0.5f) {
            side = cellSideFixed;
        } else {
            float w = gridRoot.rect.width - grid.padding.horizontal;
            float h = gridRoot.rect.height - grid.padding.vertical;
            float gaps = gridSide - 1;
            float cellW = (w - grid.spacing.x * gaps) / gridSide;
            float cellH = (h - grid.spacing.y * gaps) / gridSide;
            side = Mathf.Max (32f, Mathf.Min (cellW, cellH));
        }
        grid.cellSize = new Vector2 (side, side);

        ContiGoBoardCell2D[,] pieces = new ContiGoBoardCell2D[gridSide, gridSide];
        int idx = 0;

        Sprite cellFaceSprite = ContiGo2DSharedUi.GetBoardCellBackgroundSprite ();
        Sprite rounded = RoundedRectSpriteFactory.Get (64, 10);
        Color borderCol = new Color (0f, 0f, 0f, 0.35f);
        float borderPx = 2f;

        for (int xx = 0; xx < gridSide; xx++) {
            for (int yy = 0; yy < gridSide; yy++) {
                GameObject go = new GameObject ($"Cell_{xx}_{yy}", typeof (RectTransform));
                go.transform.SetParent (gridRoot, false);

                Image fillImg;
                if (cellFaceSprite != null) {
                    fillImg = go.AddComponent<Image> ();
                    fillImg.sprite = cellFaceSprite;
                    fillImg.color = Color.white;
                    if (cellFaceSprite.border.sqrMagnitude > 0.0001f) {
                        fillImg.type = Image.Type.Sliced;
                        fillImg.preserveAspect = false;
                    } else {
                        fillImg.type = Image.Type.Simple;
                        fillImg.preserveAspect = true;
                    }
                } else {
                    Image borderImg = go.AddComponent<Image> ();
                    borderImg.sprite = rounded;
                    borderImg.type = Image.Type.Sliced;
                    borderImg.color = borderCol;

                    GameObject fillGo = new GameObject ("Fill", typeof (RectTransform));
                    fillGo.transform.SetParent (go.transform, false);
                    RectTransform fillRt = fillGo.GetComponent<RectTransform> ();
                    fillRt.anchorMin = Vector2.zero;
                    fillRt.anchorMax = Vector2.one;
                    fillRt.offsetMin = new Vector2 (borderPx, borderPx);
                    fillRt.offsetMax = new Vector2 (-borderPx, -borderPx);

                    fillImg = fillGo.AddComponent<Image> ();
                    fillImg.sprite = rounded;
                    fillImg.type = Image.Type.Sliced;
                    fillImg.color = Color.white;
                }

                Button btn = go.AddComponent<Button> ();
                btn.targetGraphic = fillImg;
                ColorBlock cb = ColorBlock.defaultColorBlock;
                cb.normalColor = Color.white;
                cb.highlightedColor = new Color (0.96f, 0.96f, 0.96f, 1f);
                cb.pressedColor = new Color (0.88f, 0.88f, 0.88f, 1f);
                cb.selectedColor = Color.white;
                cb.disabledColor = new Color (0.78f, 0.78f, 0.78f, 0.5f);
                cb.colorMultiplier = 1f;
                btn.colors = cb;

                ContiGoBoardCell2D cell = go.AddComponent<ContiGoBoardCell2D> ();
                cell.background = fillImg;
                cell.button = btn;

                Transform labelParent = cellFaceSprite != null ? go.transform : go.transform.GetChild (0);
                GameObject textGo = new GameObject ("Label", typeof (RectTransform));
                textGo.transform.SetParent (labelParent, false);
                RectTransform trt = textGo.GetComponent<RectTransform> ();
                trt.anchorMin = Vector2.zero;
                trt.anchorMax = Vector2.one;
                trt.offsetMin = Vector2.zero;
                trt.offsetMax = Vector2.zero;

                TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI> ();
                if (font != null) {
                    tmp.font = font;
                }
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.enableWordWrapping = ContiGoFantasyNames.USE_FANTASY_NAMES_ON_BOARD;
                tmp.fontSize = ContiGoFantasyNames.GetSuggestedCellFontSize (side, gridSide);
                tmp.color = ContiGoBoardCell2D.BoardCellLabelColor;
                tmp.fontWeight = TMPro.FontWeight.Bold;
                cell.label = tmp;

                int v = boardValuesShuffled[idx];
                idx++;
                cell.Configure (xx, yy, v, onCellClicked);
                pieces[xx, yy] = cell;
                outAllCells.Add (cell);
            }
        }
        return pieces;
    }
}
