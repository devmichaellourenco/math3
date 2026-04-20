using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{


    public enum Sound
    {
        PlayerMove,
        PlayerAttack,
        PieceHit,
        PieceTrue,
        PieceFalse,
        GainPoint,
        UIConfimation,
        UICancel,
        MusicBackgroundHome

    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    const string ContiGo2DMenuHelpOpenClipResource = "Sound/Effects/SELECT-dgsl9m_ui-132";
    static AudioClip _contiGo2DMenuHelpOpenClip;

    /// <summary>SFX ao abrir menu ou "como jogar" no ContiGo 2D (Resources).</summary>
    public static void PlayContiGo2DMenuOrHelpOpenSound ()
    {
        if (_contiGo2DMenuHelpOpenClip == null)
            _contiGo2DMenuHelpOpenClip = Resources.Load<AudioClip> (ContiGo2DMenuHelpOpenClipResource);
        if (_contiGo2DMenuHelpOpenClip == null) {
            Debug.LogWarning ("AudioClip em Resources não encontrado: " + ContiGo2DMenuHelpOpenClipResource + " — a usar UIConfimation.");
            PlaySound (Sound.UIConfimation);
            return;
        }
        if (oneShotGameObject == null) {
            oneShotGameObject = new GameObject ("One Shot Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource> ();
        }
        oneShotAudioSource.PlayOneShot (_contiGo2DMenuHelpOpenClip);
    }

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1f;
            //audioSource.volume = ES2.Load<float>("bgMusicVolume");
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound)) {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.PlayerMove:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .05f;
                    if(lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
                break;
        }
    }

  private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if(soundAudioClip.sound== sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
