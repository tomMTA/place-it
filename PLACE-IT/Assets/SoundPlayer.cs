using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource[] m_AudioSource;
    [SerializeField] private Slider m_MusicSlider;
    [SerializeField] private Slider m_EffectsSlider;

    void Start()
    {
        //SetEffectsVolume();
        //SetMusicVolume();
        //PlayThemeSound();
    }

    public void PlayClickSound()
    {
        //m_AudioSource[0].Play();
        GameObject.Find("ClickSound").GetComponent<AudioSource>().Play();
    }

    public void PlayWinSound()
    {
        //m_AudioSource[1].Play();
        GameObject.Find("ApplauseSound").GetComponent<AudioSource>().Play();
    }

    /*public void PlayThemeSound()
    {
        m_AudioSource[2].Play();
    }*/

    public void ToggleEffects()
    {
        m_AudioSource[0].mute = !m_AudioSource[0].mute;
        m_AudioSource[1].mute = !m_AudioSource[1].mute;
    }

    /*public void ToggleMusic()
    {
        m_AudioSource[2].mute = !m_AudioSource[2].mute;
    }*/

    public void SetEffectsVolume(float i_Value)
    {
        m_AudioSource[0].volume = i_Value;
        m_AudioSource[1].volume = i_Value;
    }

    /*public void SetEffectsVolume()
    {
        m_AudioSource[0].volume = m_EffectsSlider.value;
        m_AudioSource[1].volume = m_EffectsSlider.value;
    }*/

    public void SetMusicVolume(Slider i_Slider)
    {
        GameObject.FindWithTag("ThemeSound").GetComponent<AudioSource>().volume = i_Slider.value;
    }
}
