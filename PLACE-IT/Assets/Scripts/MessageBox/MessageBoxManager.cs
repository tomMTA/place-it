using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxManager : MonoBehaviour
{
    private bool m_Enabled;

    public void ShowText(string i_Text)
    {
        m_Enabled = true;
        Invoke("DisableText", 3f);
    }

    void DisableText()
    {
        m_Enabled = false;
    }

    void Start()
    {
        m_Enabled = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "CONGRATULATIONS!\nLEVEL COMPLETE");
    }
}
