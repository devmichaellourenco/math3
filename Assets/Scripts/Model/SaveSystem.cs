using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem: MonoBehaviour
{
    public GameObject boardScript;

    public void SaveBoard()
    {
        Board script = boardScript.GetComponent<Board>();
        ES2.Save(script.idBoard, "idBoard");
        ES2.Save(script.width, "width");
        ES2.Save(script.height, "height");
        ES2.Save(script.mainTime, "mainTime");
        ES2.Save(script.playerTime, "playerTime");
        ES2.Save(script.matchPoint, "matchPoint");

        Debug.Log("SALVO: "+script.mainTime.ToString());
    }

    public void LoadBoard()
    {
        Board script = boardScript.GetComponent<Board>();
        script.idBoard= ES2.Load<int>("idBoard");
        script.width= ES2.Load<int>("width");
        script.height= ES2.Load<int>("height");
        script.mainTime= ES2.Load<int>("mainTime");
        script.playerTime= ES2.Load<int>("playerTime");
        script.matchPoint= ES2.Load<int>("matchPoint");
        Debug.Log("CARREGADO: " + script.mainTime.ToString());
    }
}
