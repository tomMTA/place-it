using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNavigator : MonoBehaviour
{
    [SerializeField] public GameObject[] m_Texts;
    private int m_CurrentIndex = 0;

    public void Next()
    {
        m_Texts[m_CurrentIndex].SetActive(false);
        m_CurrentIndex++;
        m_Texts[m_CurrentIndex].SetActive(true);
        if (m_CurrentIndex == m_Texts.Length - 1)
        {
            GameObject.Find("NextButton").SetActive(false);
        }
    }

    public void Done()
    {
        m_Texts[m_CurrentIndex].SetActive(false);
        m_CurrentIndex = 0;
        m_Texts[m_CurrentIndex].SetActive(true);
    }
}
