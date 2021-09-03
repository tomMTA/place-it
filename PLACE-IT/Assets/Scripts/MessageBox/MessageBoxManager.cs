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
        /*while (m_Enabled)
        {
            GUI.Label(new Rect(16, -103, 115, 19), i_Text);
        }*/
    }

    void DisableText()
    {
        m_Enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Enabled = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "CONGRATULATIONS!\nLEVEL COMPLETE");
    }
}
