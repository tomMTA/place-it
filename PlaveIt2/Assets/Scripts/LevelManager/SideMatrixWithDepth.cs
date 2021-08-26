using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMatrixWithDepth
{
    private SideMatrix m_SideMatrix;
    private int[,] m_Depths = new int[9, 8];

    public eColor GetColor(int i_X, int i_Y)
    {
        return m_SideMatrix.At(i_X, i_Y);
    }

    public int GetDepth(int i_X, int i_Y)
    {
        return m_Depths[i_X, i_Y];
    }

    public void UpdateColor(int i_X, int i_Y, int i_Depth, eColor i_Color)
    {
        if (!anotherBridgeIsHidingThisOne(i_X, i_Y, i_Depth))
        {
            m_SideMatrix.Set(i_X, i_Y, i_Color);
            m_Depths[i_X, i_Y] = i_Depth;
        }
    }

    private bool anotherBridgeIsHidingThisOne(int i_X, int i_Y, int i_Depth)
    {
        return m_Depths[i_X, i_Y] > 0 && m_Depths[i_X, i_Y] < i_Depth;
    }
}
