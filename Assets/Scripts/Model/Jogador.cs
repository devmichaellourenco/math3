using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador:MonoBehaviour
{
    private int idPlayer;
    public int vidas;
    public int valorSelecionado;
    public int a;
    public int b;
    public int c;
    public int rodada;
    public float tempoRodada;
    public float pontos;
    public string ResultadoApresentado;

    public int getIdPlayer(){
        return idPlayer;
    }
    public void setIdPlayer(int valor){
        this.idPlayer  = valor;
    }
    public void AtualizarDadosJogador(int id, int vidasAtuais, int valorSelecionadoAtual, int aAtual, int bAtual, int cAtual,int rodadaAtual, float tempoRodadaAtual, float pontosAtual, string resultadoAtual )
    {
        this.idPlayer = id;
        this.vidas = vidasAtuais;
        this.valorSelecionado = valorSelecionadoAtual;
        this.a = aAtual;
        this.b = bAtual;
        this.c = cAtual;
        this.rodada = rodadaAtual;
        this.tempoRodada = tempoRodadaAtual;
        this.pontos = pontosAtual;
        this.ResultadoApresentado = resultadoAtual;
    }
}
