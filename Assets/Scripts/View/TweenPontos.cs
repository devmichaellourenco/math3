using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenPontos : MonoBehaviour
{
    public void OnClose()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(ShowMe);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
    
    void ShowMe()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);
    }
}
