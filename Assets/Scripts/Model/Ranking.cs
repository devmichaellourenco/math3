using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public RankingData rankingData;
    // Start is called before the first frame update

    public void Start(){
        rankingData = new RankingData();
    }

    public void SaveRanking(List<RankingEntry> listItemsRanking){
        rankingData.SaveRankingData(listItemsRanking);
    }

    public List<RankingEntry> LoadRanking(){
        return rankingData.LoadRankingData();
    }
}
