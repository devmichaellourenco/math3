using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Persistência local (Easy Save 2, alinhado ao resto do projeto).</summary>
public static class ContiGoProgressPersistence
{
    const string KeyUnlockedCards = "contigo_cards_unlocked_v1";
    const string KeyMissionsDone = "contigo_missions_done_v1";

    public static HashSet<int> LoadUnlockedCards ()
    {
        var set = new HashSet<int> ();
        try {
            if (!ES2.Exists (KeyUnlockedCards))
                return set;
            List<int> list = ES2.LoadList<int> (KeyUnlockedCards);
            if (list == null)
                return set;
            foreach (int id in list)
                set.Add (id);
        } catch (Exception ex) {
            Debug.LogWarning ("ContiGoProgressPersistence: cartas — " + ex.Message);
        }
        return set;
    }

    public static HashSet<string> LoadCompletedMissions ()
    {
        var set = new HashSet<string> ();
        try {
            if (!ES2.Exists (KeyMissionsDone))
                return set;
            List<string> list = ES2.LoadList<string> (KeyMissionsDone);
            if (list == null)
                return set;
            foreach (string id in list) {
                if (!string.IsNullOrEmpty (id))
                    set.Add (id);
            }
        } catch (Exception ex) {
            Debug.LogWarning ("ContiGoProgressPersistence: missões — " + ex.Message);
        }
        return set;
    }

    public static void Save (HashSet<int> unlockedCards, HashSet<string> missionsDone)
    {
        try {
            var c = new List<int> ();
            if (unlockedCards != null) {
                foreach (int i in unlockedCards)
                    c.Add (i);
            }
            ES2.Save (c, KeyUnlockedCards);

            var m = new List<string> ();
            if (missionsDone != null) {
                foreach (string s in missionsDone) {
                    if (!string.IsNullOrEmpty (s))
                        m.Add (s);
                }
            }
            ES2.Save (m, KeyMissionsDone);
        } catch (Exception ex) {
            Debug.LogWarning ("ContiGoProgressPersistence: Save — " + ex.Message);
        }
    }
}
