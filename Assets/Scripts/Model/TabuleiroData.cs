using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class TabuleiroData
{
    public int idBoard;
    public int width;
    public int height;
    public int mainTime;
    public int playerTime;
    public int matchPoint;

    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;

    public TabuleiroData(Board board)
    {
        idBoard = board.idBoard;
        width = board.width;
        height = board.height;
        mainTime = board.mainTime;
        playerTime = board.playerTime;
        matchPoint = board.matchPoint;
    }
    
    public List<int> pecas= new List<int>() { 0, 1, 2, 3, 4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,44,45,48,50,54,55,60,64,66,72,75,80,90,96,100,108,120,125,144,150,180 };

    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        Debug.Log(pecas.Count.ToString());
        ShuffleList<int>(pecas);
    }

    private void Setup()
    {
        for (int i= 0; i < width; i++){
            for(int j=0;j<height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
            }
        }
    }

    public List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

    // esta função gera um tabuleiro de dimensões definidas pelo desenvolvedor, sendo os eixos x e y;
    // baseado no vídeo do link https://www.youtube.com/watch?v=qsRVu-EUedk 
}
