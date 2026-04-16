using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRankingView : MonoBehaviour
{
    public TextMeshProUGUI itemIndex;
    public TextMeshProUGUI itemValue;

    public void DisplayItemRanking(int itemIndex, int points){
        this.itemValue.text = points.ToString();
        this.itemIndex.text = itemIndex.ToString();
    } 

}
