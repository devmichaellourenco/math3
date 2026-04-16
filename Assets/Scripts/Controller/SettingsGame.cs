using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsGame : MonoBehaviour
{
    public GameObject boardScript;

    public TMP_InputField width;

    public TMP_InputField height;

    public TMP_InputField mainTime;

    public TMP_InputField playerTime;

    public TMP_InputField strategicTime;

    public TMP_InputField matchPoint;

    Board boardData;

    private void Start()
    {
        LoadBoardSettings();
    }
    
    public void SaveBoardSettings()
    {
       // ES2.Save(int.Parse(width.text), "width");
       // ES2.Save(int.Parse(height.text), "height");
        ES2.Save(int.Parse(mainTime.text), "mainTime");
        ES2.Save(int.Parse(playerTime.text), "playerTime");
        ES2.Save(int.Parse(strategicTime.text), "strategicTime");
        ES2.Save(int.Parse(matchPoint.text), "matchPoint");  
    }

    public void LoadBoardSettings()
    {      
       // width.text= ES2.Load<int>("width").ToString();
       // height.text = ES2.Load<int>("height").ToString();
        mainTime.text = ES2.Load<int>("mainTime").ToString();
        playerTime.text = ES2.Load<int>("playerTime").ToString();
        strategicTime.text = ES2.Load<int>("strategicTime").ToString();
        matchPoint.text = ES2.Load<int>("matchPoint").ToString();     
    }

    public void SetWidthBoard(int width)
    {

    }

    public void SetHeightBoard(int height)
    {

    }
    
    public void SetMainTime(int mainTimeIndex)
    {

    }

    public void SetPlayerTime(int playerTimeIndex)
    {
        
    }

    public void SetPlayerTimeOn(bool isPlayerTime)
    {

    }
}
