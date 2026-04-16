using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Peca : MonoBehaviour, IPooledObject
{
    public int x;
    public int y;
    public int valor;
    public int status;
    public  GameController gc = GameController.Instance;

    public float health = 50f;
    public Material[] material;
    public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        status = 0;
    }

    public void OnObjectSpawn()//função da interface criada IPooledObject
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        status = 0;
    }
    
    public void VerificaResultado(bool resposta, Jogador j)
    {
        if (resposta == true)
        {
            AlteraStatus(j.getIdPlayer());
        }
    }

    public void AlteraStatus(int numero)
    {
        status = numero;
        rend.sharedMaterial = material[status];
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
        rend.sharedMaterial = material[status];
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void ChangeMaterial()
    {
    }
}
