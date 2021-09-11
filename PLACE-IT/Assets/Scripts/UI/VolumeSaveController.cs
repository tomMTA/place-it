using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//todo - make seperate scripts like this for music & effects. Worked for explicitly hardcoding "ThemeSound" for finding the AudioSource object

public class VolumeSaveController : MonoBehaviour
{
    [SerializeField] Slider m_VolumeSlider;
    //[SerializeField] AudioSource[] m_Sources;
    [SerializeField] string[] m_SoundNames;
    const float DEFAULT_VOLUME = 0.7f;

    void Start()
    {
        LoadValues();
    }

    public void VolumeSlider(Slider i_Slider)
    {
        Debug.Log("in VolumeSlider, Slider: " + i_Slider.name);
        m_VolumeSlider = i_Slider;
        for (int i = 0; i < m_SoundNames.Length; i++)
        {
            //Debug.Log("Sound name" + m_Sources[i].clip.name);
            //m_Sources[i] = GameObject.Find(m_SoundNames[i]).GetComponent<AudioSource>();
            PlayerPrefs.SetFloat(m_SoundNames[i] + " VolumeValue", m_VolumeSlider.value);
        }
        LoadValues();
    }

    void LoadValues()
    {
        float volumeValue;

        for (int i = 0; i < m_SoundNames.Length; i++)
        {
            volumeValue = PlayerPrefs.GetFloat(m_SoundNames[i] + " VolumeValue", -1);
            if (volumeValue != -1)
            {
                m_VolumeSlider.value = volumeValue;
                GameObject.Find(m_SoundNames[i]).GetComponent<AudioSource>().volume = volumeValue;
                //m_Sources[i].volume = volumeValue;
            }
        }
    }
}
