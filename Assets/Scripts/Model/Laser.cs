using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Color corLaser = Color.green;
    public int DistanciaDoLaser = 50;
    public float LarguraInicial = 0.02f, LarguraFinal = 0.1f;
    private GameObject luzColisao;
    private Vector3 posicaoLuz;

    void Start()
    {
        luzColisao = new GameObject();
        luzColisao.AddComponent<Light>();
        luzColisao.GetComponent<Light>().intensity = 8;// no caso 8 é o valor maximo possivel para estes itens
        luzColisao.GetComponent<Light>().bounceIntensity = 8;
        luzColisao.GetComponent<Light>().range = LarguraFinal * 2;
        luzColisao.GetComponent<Light>().color = corLaser;
        posicaoLuz = new Vector3(0, 0, LarguraFinal);
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        lineRenderer.material = new Material(Shader.Find("Standard"));
        if (lineRenderer.material!=null)
        {
            Debug.Log(lineRenderer.material.ToString());
        }
        lineRenderer.SetColors(corLaser, corLaser);
        lineRenderer.SetWidth(LarguraInicial, LarguraFinal);
        lineRenderer.SetVertexCount(2);
    }

    void Update()
    {
        Vector3 PontoFinalLaser = transform.position + transform.forward * DistanciaDoLaser;
        RaycastHit PontoDeColisao;
        if (Physics.Raycast(transform.position, transform.forward, out PontoDeColisao, DistanciaDoLaser))
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, PontoDeColisao.point);
            luzColisao.transform.position = (PontoDeColisao.point - posicaoLuz);
        }
        else
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, PontoFinalLaser);
            luzColisao.transform.position = PontoFinalLaser;
        }
    }
}
