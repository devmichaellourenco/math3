using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidaContaData 
{
    public bool confere;
    public int valorComparado;
    public List<float> resultadosValidados = new List<float>();

    public void ContaValidada(bool confere, List<float> resultadosPassados, int valorcomparado)
    { 
        valorComparado = valorcomparado;
        this.confere = confere;
        foreach (float resposta in resultadosPassados)
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
