using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideObserver: MonoBehaviour
{
    private SideMatrixWithDepth m_ObservedColors;
    private SideMatrix m_Solution;
    private eSide m_Side;
    private int m_Differences;

    public SideObserver(string i_Level, eSide i_Side)
    {
        m_Solution = GameObject.Find("Level" + i_Level + "Side" + i_Side).GetComponent<SideMatrix>();
        m_Side = i_Side;
        m_Differences = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (m_Solution.At(i, j) != eColor.None)
                {
                    m_Differences++;
                }
                Debug.Log("Color at [" + i + ", " + j + "]: " + m_Solution.At(i, j));
            }
        }
    }

    public void BridgeEnter(int i_StartRow, int i_EndRow, int i_StartCol, int i_EndCol, int i_BridgeHeight, eColor i_BridgeColor)
    {
        switch (m_Side)
        {
            case eSide.A:

                break;
            case eSide.B:

                break;
        }
    }

    private void updateForFullBridge(int i_StartX, int i_EndX, int i_Y, int i_Depth, eColor i_BridgeColor)
    {
        updateForOnlyHeight(i_StartX, i_Y, i_Depth, i_BridgeColor);
        for (int xIterator = i_StartX + 1; xIterator < i_EndX; xIterator++)
        {
            m_ObservedColors.UpdateColor(xIterator, i_Y, i_Depth, i_BridgeColor);
        }
        updateForOnlyHeight(i_EndX, i_Y, i_Depth, i_BridgeColor);
    }

    private void updateForOnlyHeight(int i_X, int i_Y, int i_Depth, eColor i_BridgeColor)
    {
        for (int yIterator = 0; yIterator <= i_Y; yIterator++)
        {
            m_ObservedColors.UpdateColor(i_X, yIterator, i_Depth, i_BridgeColor);
        }
    }

    private void updateForSideBFullBridge(int i_StartRow, int i_EndRow, int i_StartCol, int i_EndCol, int i_BridgeHeight, eColor i_BridgeColor)
    {

    }
}
