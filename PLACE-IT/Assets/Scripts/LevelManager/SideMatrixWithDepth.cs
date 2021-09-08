using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//depth = 0 means the 1st row from our view
public class SideMatrixWithDepth
{
    /*private SideMatrix m_SideMatrix;
    private int[,] m_Depths = new int[9, 8];*/

    private readonly SortedDictionary<int, eColor>[,] m_SideMatrixWithDepths;

    public SideMatrixWithDepth()
    {
        m_SideMatrixWithDepths = new SortedDictionary<int, eColor>[9, 8];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                m_SideMatrixWithDepths[i, j] = new SortedDictionary<int, eColor>();
            }
        }
    }

    public eColor GetColor(int i_X, int i_Y)
    {
        if (m_SideMatrixWithDepths[i_X, i_Y].Count == 0)
        {
            return eColor.None;
        }

        return m_SideMatrixWithDepths[i_X, i_Y].First().Value;
    }

    public int GetDepth(int i_X, int i_Y)
    {
        if (m_SideMatrixWithDepths[i_X, i_Y].Count == 0)
        {
            return int.MaxValue;
        }

        return m_SideMatrixWithDepths[i_X, i_Y].First().Key;
    }

    public void UpdateColor(int i_X, int i_Y, int i_Depth, eColor i_Color)
    {
        m_SideMatrixWithDepths[i_X, i_Y].Add(i_Depth, i_Color);
    }

    public void RemoveColor(int i_X, int i_Y, int i_Depth)
    {
        m_SideMatrixWithDepths[i_X, i_Y].Remove(i_Depth);
    }

    /*private bool anotherBridgeIsHidingThisOne(int i_X, int i_Y, int i_Depth)
    {
        return m_SideMatrix.At(i_X, i_Y) != eColor.None && m_Depths[i_X, i_Y] < i_Depth;
    }*/
}
