using UnityEngine.Audio;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource[] m_AudioSource;

    public void PlayClickSound()
    {
        m_AudioSource[0].Play();
        Debug.Log("Play click");
    }

    public void PlayWinSound()
    {
        m_AudioSource[1].Play();
        Debug.Log("Play win");
    }
}
