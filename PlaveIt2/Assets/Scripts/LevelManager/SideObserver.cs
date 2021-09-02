using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideObserver: MonoBehaviour
{
    private SideMatrixWithDepth m_ObservedColors = new SideMatrixWithDepth();
    private SideMatrix m_Solution;
    private eSide m_Side;
    private int m_Differences;

    public SideObserver(string i_Level, eSide i_Side)
    {
        //Debug.Log("Level" + i_Level + "Side" + i_Side);
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
                //Debug.Log("Color at [" + i + ", " + j + "]: " + m_Solution.At(i, j));
            }
        }
        //Debug.Log("Side " + m_Side + " initial differences: " + m_Differences);
    }

    public int Differences
    {
        get { return m_Differences; }
    }

    public void BridgeEnter(int i_StartRow, int i_EndRow, int i_StartCol, int i_EndCol, int i_BridgeHeight, eColor i_BridgeColor)
    {
        switch (m_Side)
        {
            case eSide.A:
                if (i_StartRow == i_EndRow)
                {
                    updateForFullBridge(i_StartCol, i_EndCol, i_BridgeHeight, 8 - i_StartRow, i_BridgeColor);
                    //Debug.Log("Side " + m_Side + " Depth: " + (8 - i_StartRow));
                }
                else
                {
                    updateForOnlyHeight(i_StartCol, i_BridgeHeight, 8 - i_EndRow, i_BridgeColor);
                    //Debug.Log("Side " + m_Side + " Depth: " + (8 - i_EndRow));
                }
                break;
            case eSide.B:
                if (i_StartCol == i_EndCol)
                {
                    updateForFullBridge(8 - i_EndRow, 8 - i_StartRow, i_BridgeHeight, 8 - i_StartCol, i_BridgeColor);
                    //Debug.Log("Side " + m_Side + ", startRow: " + i_StartRow + ", endRow: " + i_EndRow);
                    //Debug.Log("Side " + m_Side + " Depth: " + (8 - i_StartCol));
                }
                else
                {
                    updateForOnlyHeight(8 - i_StartRow, i_BridgeHeight, 8 - i_EndCol, i_BridgeColor);
                    //Debug.Log("Side " + m_Side + " Depth: " + (8 - i_EndCol));
                }
                break;
        }

        //printAllDifferences();
        //Debug.Log("Side " + m_Side + " differences: " + m_Differences);
        //printSideView();
    }

    private void updateForFullBridge(int i_StartX, int i_EndX, int i_Y, int i_Depth, eColor i_BridgeColor)
    {
        //Debug.Log("Side " + m_Side + "Full Bridge");
        if (m_Side == eSide.B)
        {
            //Debug.Log(" updating from " + i_StartX + " to " + i_EndX + " at height " + i_Y);
            if (i_StartX > i_EndX)
            {
                int temp = i_StartX;
                i_StartX = i_EndX;
                i_EndX = temp;
            }
            //Debug.Log(" updating from " + i_StartX + " to " + i_EndX + " at height " + i_Y);
        }
        //makeRightBigger(ref i_StartX, ref i_EndX);
       updateForOnlyHeight(i_StartX, i_Y, i_Depth, i_BridgeColor);
        for (int xIterator = i_StartX + 1; xIterator < i_EndX; xIterator++)
        {
            updateColorAndDifferences(xIterator, i_Y, i_Depth, i_BridgeColor);
        }
        updateForOnlyHeight(i_EndX, i_Y, i_Depth, i_BridgeColor);
    }

    private void printAllDifferences()
    {
        //Debug.Log("----------------Side" + m_Side + "Differences---------------");
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (!m_ObservedColors.GetColor(i, j).IsEqual(m_Solution.At(i, j)))
                {
                    //Debug.Log("(" + i + ", " + j + ") " + m_ObservedColors.GetColor(i, j) + ", " + m_Solution.At(i, j));
                }
            }
        }
    }

    private void updateForOnlyHeight(int i_X, int i_Y, int i_Depth, eColor i_BridgeColor)
    {
        for (int yIterator = 0; yIterator <= i_Y; yIterator++)
        {
            updateColorAndDifferences(i_X, yIterator, i_Depth, i_BridgeColor);
        }
    }

    public void BridgeLeave(int i_StartRow, int i_EndRow, int i_StartCol, int i_EndCol, int i_BridgeHeight)
    {
        switch (m_Side)
        {
            case eSide.A:
                if (i_StartRow == i_EndRow)
                {
                    removeForFullBridge(i_StartCol, i_EndCol, i_BridgeHeight, 8 - i_StartRow);
                }
                else
                {
                    removeForOnlyHeight(i_StartCol, i_BridgeHeight, 8 - i_EndRow);
                }
                break;
            case eSide.B:
                if (i_StartCol == i_EndCol)
                {
                    removeForFullBridge(8 - i_EndRow, 8 - i_StartRow, i_BridgeHeight, 8 - i_StartCol);
                }
                else
                {
                    removeForOnlyHeight(8 - i_StartRow, i_BridgeHeight, 8 - i_EndCol);
                }
                break;
        }

        //Debug.Log("Side " + m_Side + " differences: " + m_Differences);
        //printSideView();
    }

    private void removeForFullBridge(int i_StartX, int i_EndX, int i_Y, int i_Depth)
    {
        if (i_StartX > i_EndX)
        {
            int temp = i_StartX;
            i_StartX = i_EndX;
            i_EndX = temp;
        }
        removeForOnlyHeight(i_StartX, i_Y, i_Depth);
        for (int xIterator = i_StartX + 1; xIterator < i_EndX; xIterator++)
        {
            removeColorAndUpdateDifferences(xIterator, i_Y, i_Depth);
        }
        removeForOnlyHeight(i_EndX, i_Y, i_Depth);
    }

    private void removeForOnlyHeight(int i_X, int i_Y, int i_Depth)
    {
        for (int yIterator = 0; yIterator <= i_Y; yIterator++)
        {
            removeColorAndUpdateDifferences(i_X, yIterator, i_Depth);
        }
    }

    private void printSideView()
    {
        Debug.Log("Side " + m_Side);
        for (int i = 0; i < 9; i++)
        {
            string colors = "";
            for (int j = 0; j < 8; j++)
            {
                colors += m_ObservedColors.GetColor(i, j) + ", ";
            }
            Debug.Log(colors);
        }
    }

    private void updateColorAndDifferences(int i_X, int i_Y, int i_Depth, eColor i_Color)
    {
        eColor prevColor = m_ObservedColors.GetColor(i_X, i_Y);
        eColor solutionColor = m_Solution.At(i_X, i_Y);
        eColor newColor;

        m_ObservedColors.UpdateColor(i_X, i_Y, i_Depth, i_Color);
        newColor = m_ObservedColors.GetColor(i_X, i_Y);
        updateDifferences(prevColor, newColor, solutionColor);
        //Debug.Log("Side " + m_Side + " updated at (" + i_X + ", " + i_Y + ")");
    }

    private void removeColorAndUpdateDifferences(int i_X, int i_Y, int i_Depth)
    {
        eColor prevColor = m_ObservedColors.GetColor(i_X, i_Y);
        eColor solutionColor = m_Solution.At(i_X, i_Y);
        eColor newColor;

        m_ObservedColors.RemoveColor(i_X, i_Y, i_Depth);
        newColor = m_ObservedColors.GetColor(i_X, i_Y);
        updateDifferences(prevColor, newColor, solutionColor);
    }

    private void updateDifferences(eColor i_PrevColor, eColor i_NewColor, eColor i_SolutionColor)
    {
        if (!i_PrevColor.IsEqual(i_SolutionColor) && i_NewColor.IsEqual(i_SolutionColor))
        {
            //Debug.Log("Decreasing differences");
            m_Differences--;
        }

        if (i_PrevColor.IsEqual(i_SolutionColor) && !i_NewColor.IsEqual(i_SolutionColor))
        {
            //Debug.Log("Increasing differences");
            m_Differences++;
        }
        //Debug.Log("Side " + m_Side + ", Previous: " + i_PrevColor + ", New: " + i_NewColor + ", Solution: " + i_SolutionColor);
    }

    private void makeRightBigger(ref int io_Left, ref int io_Right)
    {
        if (io_Left > io_Right)
        {
            int temp = io_Left;
            io_Left = io_Right;
            io_Right = temp;
        }
    }
}
