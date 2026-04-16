using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dado 
{
    public int valor;

    public int RolarDado(int rangeInicial, int rangeFinal)
    {
        int valor = Random.Range(rangeInicial, rangeFinal);
        return valor;
    }
}
