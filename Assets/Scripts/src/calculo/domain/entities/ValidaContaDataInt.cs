using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidaContaDataInt 
{
    public bool confere;
    public int valorComparado;
    public List<int> resultadosValidados = new List<int>();

    public void ContaValidada(bool confere, List<int> resultadosPassados, int valorcomparado)
    { 
        valorComparado = valorcomparado;
        this.confere = confere;
        foreach (int resposta in resultadosPassados)
        {
            if( valorComparado == resposta)
            {
            this.resultadosValidados.Add(resposta);
            break;
            }
        }
        if(this.resultadosValidados.Count >0){
            this.confere = true;
        }else{
            this.confere = false;
        }
        //Debug.Log("VALIDACONTA: CONFERE: "+ this.confere +" RESULTADOS "+resultadosValidados.Count+" VALOR COMPARADO: "+valorComparado);
    }
}
