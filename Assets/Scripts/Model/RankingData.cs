using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persistência do ranking via Easy Save 2. O ES2 não serializa tipos próprios como RankingEntry;
/// usamos duas <see cref="List{Int32}"/> paralelas (pontos e tempo gasto em segundos).
/// </summary>
public class RankingData : MonoBehaviour
{
    const string KeyPoints = "ranking_points";
    const string KeyTimeSpent = "ranking_timeSpent";
    const string KeyLegacy = "ranking";

    public void SaveRankingData (List<RankingEntry> ranking)
    {
        WriteRankingV2 (ranking);
    }

    public List<RankingEntry> LoadRankingData ()
    {
        if (!ES2.Exists (KeyPoints) && !ES2.Exists (KeyLegacy))
            return new List<RankingEntry> { new RankingEntry (0, 0) };

        if (ES2.Exists (KeyPoints)) {
            try {
                List<int> points = ES2.LoadList<int> (KeyPoints);
                List<int> times = ES2.Exists (KeyTimeSpent)
                    ? ES2.LoadList<int> (KeyTimeSpent)
                    : new List<int> ();
                List<RankingEntry> list = MergePointsAndTimes (points, times);
                list.RemoveAll (e => e == null);
                if (list.Count == 0)
                    list.Add (new RankingEntry (0, 0));
                ApplyTimeRemainingMigration (list);
                return list;
            } catch (Exception ex) {
                Debug.LogWarning ("RankingData: erro ao ler " + KeyPoints + " — " + ex.Message);
            }
        }

        if (ES2.Exists (KeyLegacy)) {
            try {
                List<int> old = ES2.LoadList<int> (KeyLegacy);
                var rankingPego = new List<RankingEntry> ();
                if (old != null) {
                    foreach (int p in old)
                        rankingPego.Add (new RankingEntry (p, 0));
                }
                if (rankingPego.Count == 0)
                    rankingPego.Add (new RankingEntry (0, 0));
                ApplyTimeRemainingMigration (rankingPego);
                WriteRankingV2 (rankingPego);
                TryDeleteLegacy ();
                return rankingPego;
            } catch (Exception ex) {
                Debug.LogWarning ("RankingData: ranking legado inválido — " + ex.Message);
            }
        }

        Debug.LogWarning ("RankingData: dados de ranking ilegíveis; a repor.");
        TryDeleteAllRankingKeys ();
        var def = new List<RankingEntry> { new RankingEntry (0, 0) };
        WriteRankingV2 (def);
        return def;
    }

    static void ApplyTimeRemainingMigration (List<RankingEntry> rankingPego)
    {
        if (rankingPego == null)
            return;
        int mainTime = ES2.Exists ("mainTime") ? ES2.Load<int> ("mainTime") : 300;
        bool changed = false;
        foreach (RankingEntry e in rankingPego) {
            if (e == null)
                continue;
            if (e.timeSpentSeconds <= 0 && e.timeRemainingSeconds > 0) {
                e.timeSpentSeconds = Mathf.Max (0, mainTime - e.timeRemainingSeconds);
                changed = true;
            }
        }
        if (changed)
            WriteRankingV2 (rankingPego);
    }

    static List<RankingEntry> MergePointsAndTimes (List<int> points, List<int> times)
    {
        var result = new List<RankingEntry> ();
        if (points == null || points.Count == 0)
            return result;
        for (int i = 0; i < points.Count; i++) {
            int t = (times != null && i < times.Count) ? times[i] : 0;
            result.Add (new RankingEntry (points[i], t));
        }
        return result;
    }

    static void WriteRankingV2 (List<RankingEntry> ranking)
    {
        if (ranking == null)
            ranking = new List<RankingEntry> ();
        var points = new List<int> ();
        var times = new List<int> ();
        foreach (RankingEntry e in ranking) {
            if (e == null)
                continue;
            points.Add (e.points);
            times.Add (e.timeSpentSeconds);
        }
        if (points.Count == 0) {
            points.Add (0);
            times.Add (0);
        }
        ES2.Save (points, KeyPoints);
        ES2.Save (times, KeyTimeSpent);
        TryDeleteLegacy ();
    }

    static void TryDeleteLegacy ()
    {
        if (!ES2.Exists (KeyLegacy))
            return;
        try {
            ES2.Delete (KeyLegacy);
        } catch {
            // ignorar
        }
    }

    static void TryDeleteAllRankingKeys ()
    {
        foreach (string key in new[] { KeyPoints, KeyTimeSpent, KeyLegacy }) {
            if (!ES2.Exists (key))
                continue;
            try {
                ES2.Delete (key);
            } catch {
                // ignorar
            }
        }
    }
}
