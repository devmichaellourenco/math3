using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NOVO JOGADOR", menuName = "JOGADOR")]
public class JogadorData: ScriptableObject
{
    public int idPlayer;
    public int vidas;
    public int valorSelecionado;
    public int a;
    public int b;
    public int c;
    public string resultadoApresentado;

}
