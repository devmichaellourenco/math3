using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundScript : MonoBehaviour
{
    // fonte: https://www.youtube.com/watch?v=82Mn8v55nr0
    public static BGSoundScript instance;
    public AudioSource musicSource;
    // PlayGlobal



    public void Awake()
    {
        if(instance!=null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        // Este projeto usa SFX via SoundManager; a música de fundo foi desativada por design.
        if (musicSource != null) {
            musicSource.playOnAwake = false;
            musicSource.loop = false;
            musicSource.Stop ();
            musicSource.mute = true;
        }
    }

    void Start(){
        // Mantido vazio: música de fundo desativada.
    }
    void Update(){
        // Não ajustar volume (música desativada).
    }


    public void PlayMusic(AudioClip clip)//We don't need Play for your problem.
    {
        // Música de fundo desativada. Mantém API para não quebrar chamadas legadas.
        if (musicSource != null) {
            musicSource.Stop ();
            musicSource.mute = true;
        }
        return;
        musicSource.clip = clip;
        musicSource.volume = ES2.Load<float>("bgMusicVolume");
        musicSource.Play ();
    }

    public void StopMusic(AudioClip clip)
    {
        if (musicSource == null)
            return;
        musicSource.clip = clip;
        musicSource.Stop ();
    }
    //Play Global End
}
