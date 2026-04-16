using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAudio : MonoBehaviour
{
    void Start()
    {
        BGSoundScript.instance.gameObject.GetComponent<AudioSource>().Pause();
    }
}
