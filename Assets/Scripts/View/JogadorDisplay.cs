using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JogadorDisplay : MonoBehaviour
{
    public Jogador jogador;
    public TextMeshProUGUI txIdPlayer;
    public TextMeshProUGUI txtA;
    public TextMeshProUGUI txtB;
    public TextMeshProUGUI txtC;
    public TextMeshProUGUI txtVidas;
    public TextMeshProUGUI txtResultadoApresentado;
    public TextMeshProUGUI txtRodada;
    public TextMeshProUGUI txtPontos;
    public TextMeshProUGUI txtTempoRodada;

    public GameObject panelPlayerHelp;

    private void Awake()
    {
        jogador.AtualizarDadosJogador(jogador.getIdPlayer(), 
            jogador.vidas, 
            jogador.valorSelecionado, 
            jogador.a, 
            jogador.b, 
            jogador.c,
            jogador.rodada,
            jogador.tempoRodada,
            jogador.pontos,
            jogador.ResultadoApresentado);
        AtualizarDisplayJogador(jogador.getIdPlayer(), 
            jogador.vidas, 
            jogador.a, 
            jogador.b, 
            jogador.c, 
            jogador.rodada, 
            jogador.tempoRodada, 
            jogador.pontos, 
            "" );
    }

    public void AtualizarDisplayJogador(int idJogador, int vidasAtuais, int valor1, int valor2, int valor3, int rodada, float tempoRodada, float pontos, string resultadoApresentado)
    {
        if (jogador.getIdPlayer() == idJogador) { 
            txtA.text = valor1.ToString();
            txtB.text = valor2.ToString();
            txtC.text = valor3.ToString();
            txtVidas.text = vidasAtuais.ToString();
            txtRodada.text = rodada.ToString();
            txtPontos.text = pontos.ToString();
            txtTempoRodada.text = tempoRodada.ToString();
            txtResultadoApresentado.text = resultadoApresentado;
        }
    }

    public bool podeSelecionar()
    {
        var status = false;
        return status;
    }
}
