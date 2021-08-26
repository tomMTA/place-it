using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class SideMatrix : MonoBehaviour
{
    //[SerializeField]
    public eColor[] colorArray = new eColor[72];

    public eColor At(int i_X, int i_Y)
    {
        return colorArray[i_X * 8 + i_Y];
    }

    public void Set(int i_X, int i_Y, eColor i_NewColor)
    {
        colorArray[i_X * 8 + i_Y] = i_NewColor;
    }
}
