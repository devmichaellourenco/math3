using UnityEngine;

public class VolumeValueChange : MonoBehaviour {

    public BGSoundScript bgSoundScript;

    void ResolveBgSound ()
    {
        if (bgSoundScript == null)
            bgSoundScript = BGSoundScript.instance;
    }

    bool TryGetMusicSource (out AudioSource src)
    {
        src = null;
        ResolveBgSound ();
        if (bgSoundScript == null || bgSoundScript.musicSource == null)
            return false;
        src = bgSoundScript.musicSource;
        return true;
    }

    // Music volume variable that will be modified
    // by dragging slider knob
    private float bgMusicVolume;
    private float musicVolume;

	// Use this for initialization
	void Start () {

        if (!ES2.Exists("bgMusicVolume"))
        {        
            ES2.Save(0.6f, "bgMusicVolume");
        }
        if (!ES2.Exists("musicVolume"))
        {        
            ES2.Save(0.6f, "musicVolume");
        }
        if (!ES2.Exists("bgMusicVolumeActive"))
        {        
            ES2.Save(true, "bgMusicVolumeActive");
        }        
        if (!ES2.Exists("musicVolumeActive"))
        {        
            ES2.Save(true, "musicVolumeActive");
        } 

        if(ES2.Load<float>("bgMusicVolume") != null){
            bgMusicVolume = ES2.Load<float>("bgMusicVolume");
        }
        else{
            ES2.Save(1f, "bgMusicVolume");
            bgMusicVolume = ES2.Load<float>("bgMusicVolume");
        }
        if(ES2.Load<float>("musicVolume") != null){
            musicVolume = ES2.Load<float>("musicVolume");
        }
        else{
            ES2.Save(1f, "musicVolume");
            musicVolume = ES2.Load<float>("musicVolume");
        }
        if (TryGetMusicSource (out AudioSource mus))
            mus.volume = bgMusicVolume;
        SyncSliderIconsFromStorage ();
	}
	
	// Update is called once per frame
	public void Update () {
        if (!TryGetMusicSource (out AudioSource mus))
            return;
        mus.volume = bgMusicVolume;
	}

    // Method that is called by slider game object
    // This method takes vol value passed by slider
    // and sets it as musicValue
    public void SetMusicVolume(float vol)
    {
        ES2.Save(vol, "musicVolume");  
        musicVolume =  vol;
        if (TryGetMusicSource (out AudioSource mus))
            mus.volume = vol;
    }
    public void SetBGMusicVolume(float vol)
    {       
        ES2.Save(vol, "bgMusicVolume");
        bgMusicVolume =  vol;
        if (TryGetMusicSource (out AudioSource mus))
            mus.volume = vol;
        SyncSliderIconsFromStorage ();
    }

    static void SyncSliderIconsFromStorage ()
    {
        SliderValue sv = Object.FindFirstObjectByType<SliderValue> ();
        if (sv != null)
            sv.RefreshFromStorage ();
    }
}

