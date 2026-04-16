using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public GameObject btnSoundOn;
    public GameObject btnSoundOff;
    public Slider sliderVolume;

    void Awake ()
    {
        if (!ES2.Exists ("bgMusicVolume"))
            ES2.Save (0.6f, "bgMusicVolume");
        float v = ES2.Load<float> ("bgMusicVolume");
        if (sliderVolume != null)
            sliderVolume.SetValueWithoutNotify (v);
        VerifySoundOn ();
    }

    /// <summary> Chamado após <see cref="VolumeValueChange.SetBGMusicVolume"/> para alinhar slider e ícones ao ES2. </summary>
    public void RefreshFromStorage ()
    {
        if (sliderVolume != null && ES2.Exists ("bgMusicVolume"))
            sliderVolume.SetValueWithoutNotify (ES2.Load<float> ("bgMusicVolume"));
        VerifySoundOn ();
    }

    public void VerifySoundOn ()
    {
        if (btnSoundOn == null || btnSoundOff == null)
            return;
        float v = 0f;
        if (ES2.Exists ("bgMusicVolume"))
            v = ES2.Load<float> ("bgMusicVolume");
        else if (sliderVolume != null)
            v = sliderVolume.value;
        bool soundOn = v > 0.0001f;
        btnSoundOn.SetActive (soundOn);
        btnSoundOff.SetActive (!soundOn);
    }
}
