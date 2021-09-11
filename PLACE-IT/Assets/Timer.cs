using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public Text timerText;
    private float m_StartTime;
    private int m_CurrentMinutes;
    private int m_CurrentSeconds;

    // Start is called before the first frame update
    void Start()
    {
        m_StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            float t = Time.time - m_StartTime;
            m_CurrentMinutes = (int)t / 60;
            m_CurrentSeconds = (int)t % 60;

            timerText.text = "";
            if (m_CurrentMinutes < 10)
            {
                timerText.text += "0";
            }

            timerText.text += m_CurrentMinutes.ToString() + ":";
            if (m_CurrentSeconds < 10)
            {
                timerText.text += "0";
            }

            timerText.text += m_CurrentSeconds.ToString();
        }
    }

    public int Seconds
    {
        get { return m_CurrentMinutes * 60 + m_CurrentSeconds; }
    }

}
