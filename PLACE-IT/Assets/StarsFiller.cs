using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StarsFiller : MonoBehaviour
{
    [SerializeField] private Sprite m_FilledStar;

    public void FillStars(int i_NumOfStars)
    {
        if (i_NumOfStars > 1)
        {
            gameObject.transform.Find("Star2").GetComponent<Image>().sprite = m_FilledStar;
        }

        if (i_NumOfStars > 2)
        {
            gameObject.transform.Find("Star3").GetComponent<Image>().sprite = m_FilledStar;
        }
    }
}
