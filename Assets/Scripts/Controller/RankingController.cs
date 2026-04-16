using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankingController : MonoBehaviour
{
    public List<GameObject> itemsRankingView;
    public GameObject ranking;
    public RankingData novoRanking;

    void Start ()
    {
        ResolveRankingItemRows ();
        novoRanking = new RankingData ();
        List<RankingEntry> rankingPego = novoRanking.LoadRankingData ();
        List<RankingEntry> listaOrdenada = rankingPego
            .OrderBy (e => e, Comparer<RankingEntry>.Create (RankingEntry.Compare))
            .Take (10)
            .ToList ();

        for (int i = 0; i < 10; i++) {
            GameObject rowGo = (itemsRankingView != null && i < itemsRankingView.Count)
                ? itemsRankingView[i]
                : null;
            if (rowGo == null)
                continue;
            if (i < listaOrdenada.Count) {
                ItemRankingView item = rowGo.GetComponent<ItemRankingView> ()
                    ?? rowGo.GetComponentInChildren<ItemRankingView> (true);
                if (item == null || item.itemIndex == null || item.itemValue == null)
                    continue;
                item.itemIndex.text = (i + 1).ToString ();
                item.itemValue.text = FormatRankingValue (listaOrdenada[i]);
                rowGo.SetActive (true);
            } else {
                rowGo.SetActive (false);
            }
        }
    }

    /// <summary>
    /// Garante 10 entradas e preenche slots null com linhas existentes como filhos deste objeto (VerticalLayoutGroup).
    /// </summary>
    void ResolveRankingItemRows ()
    {
        if (itemsRankingView == null)
            itemsRankingView = new List<GameObject> ();
        while (itemsRankingView.Count < 10)
            itemsRankingView.Add (null);

        var fromLayout = new List<GameObject> ();
        for (int c = 0; c < transform.childCount; c++) {
            ItemRankingView irv = transform.GetChild (c).GetComponentInChildren<ItemRankingView> (true);
            if (irv != null)
                fromLayout.Add (irv.gameObject);
        }

        bool anyAssigned = false;
        foreach (GameObject g in itemsRankingView) {
            if (g != null) {
                anyAssigned = true;
                break;
            }
        }

        if (!anyAssigned && fromLayout.Count > 0) {
            itemsRankingView.Clear ();
            itemsRankingView.AddRange (fromLayout);
            while (itemsRankingView.Count < 10)
                itemsRankingView.Add (null);
            return;
        }

        int fi = 0;
        for (int i = 0; i < itemsRankingView.Count && fi < fromLayout.Count; i++) {
            if (itemsRankingView[i] != null)
                continue;
            while (fi < fromLayout.Count && itemsRankingView.Contains (fromLayout[fi]))
                fi++;
            if (fi >= fromLayout.Count)
                break;
            itemsRankingView[i] = fromLayout[fi++];
        }
    }

    static string FormatRankingValue (RankingEntry e)
    {
        if (e == null)
            return "-";
        int s = Mathf.Max (0, e.timeSpentSeconds);
        int m = s / 60;
        int ss = s % 60;
        return e.points + "  |  " + m + ":" + ss.ToString ("00");
    }
}
