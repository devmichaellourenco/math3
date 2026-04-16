using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public RankingData novoRanking;
    public Text txtRanking;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        VerificaSeDadosDeConfiguracaoExistemEInsereCasoNaoExista();
    }

    public void Start(){
       /* novoRanking = new RankingData();
        List<int> rankingPego = novoRanking.LoadRankingData();
        List<int> listaOrdenada = (from c in rankingPego
            orderby c descending
            select c).ToList();
        txtRanking.text="";
        foreach (int item in listaOrdenada)
        {
            txtRanking.text += item+",";
        }*/
    }

    public void VerificaSeDadosDeConfiguracaoExistemEInsereCasoNaoExista()
    {
        if (!ES2.Exists("mainTime"))
        {
            ES2.Save(180, "mainTime");
        }
        if (!ES2.Exists("currentMainTime"))
        {
            DateTime dateTime = DateTime.UtcNow;
            ES2.Save(dateTime.ToBinary(), "currentMainTime");
        }
        if (!ES2.Exists("playerTime"))
        {
            ES2.Save(30,"playerTime");
        }
        if (!ES2.Exists("strategicTime"))
        {
            ES2.Save(5, "strategicTime");
        }
        if (!ES2.Exists("matchPoint"))
        {
            ES2.Save(5,"matchPoint");
        }
        if (!ES2.Exists("ranking_points") && !ES2.Exists("ranking"))
        {
            var ranking = new List<RankingEntry> { new RankingEntry(0, 0) };
            new RankingData().SaveRankingData(ranking);
        }
        if (!ES2.Exists("bgMusicVolume"))
        {        
            ES2.Save(0.6f, "bgMusicVolume");
        }
        if (!ES2.Exists("musicVolume"))
        {        
            ES2.Save(0.6f, "musicVolume");
        }
        if (!ES2.Exists("bgMusicVolumeActive"))
        {        
            ES2.Save(true, "bgMusicVolumeActive");
        }        
        if (!ES2.Exists("language"))
        {        
            ES2.Save("portuguese", "language");
        } 

    }
}
