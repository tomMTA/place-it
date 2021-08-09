using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private readonly SlotManager[,] m_SlotManagers = new SlotManager[9, 9];
    private readonly Dictionary<int, Bridge> m_BridgesInside = new Dictionary<int, Bridge>();
    private const string m_ExceptionMessage = "You can't place it here!";
    private bool m_IsRotated;
    private bool m_IsRotating;

    // Start is called before the first frame update
    void Start()
    {
        m_IsRotated = false;
        m_IsRotating = false;
        AlertPivot alertPivot = GetComponent<AlertPivot>();
        alertPivot.RotationStarted += OnRotationStart;
        alertPivot.RotationStopped += OnRotationStop;
        Transform highlightRowsTranform = transform.GetChild(0).transform;
        foreach (Transform slotsRow in highlightRowsTranform)
        {
            foreach (Transform slot in slotsRow.transform)
            {
                SlotManager temp = slot.gameObject.GetComponent<SlotManager>();
                m_SlotManagers[temp.Row - 1, temp.Col - 1] = temp;
            }
        }
    }

    private bool isInSameRange(Bridge i_Bridge1, Bridge i_Bridge2)
    {
        DragBridge dragBridge1 = i_Bridge1.transform.GetComponent<DragBridge>();
        DragBridge dragBridge2 = i_Bridge2.transform.GetComponent<DragBridge>();

        if (i_Bridge1.IsTilted != i_Bridge2.IsTilted)
        {
            return false;
        }

        SlotsHighlighter slotsHighlighter1 = i_Bridge1.transform.GetChild(0).GetComponent<SlotsHighlighter>();
        SlotsHighlighter slotsHighlighter2 = i_Bridge2.transform.GetChild(0).GetComponent<SlotsHighlighter>();
        int width1 = i_Bridge1.SpacesOccupies;
        int width2 = i_Bridge2.SpacesOccupies;
        int start1, start2, end1, end2;

        if (i_Bridge1.IsTilted)
        {
            start1 = slotsHighlighter1.CurrentSlotManager.Row;
            start2 = slotsHighlighter2.CurrentSlotManager.Row;
        }
        else
        {
            start1 = slotsHighlighter1.CurrentSlotManager.Col;
            start2 = slotsHighlighter2.CurrentSlotManager.Col;
        }

        end1 = start1 + width1 - 1;
        end2 = start2 + width2 - 1;

        return (start1 < start2 && end1 > start2 && end1 < end2) || (start1 < end2 && end1 > end2);
    }

    public void PlaceBridge(SlotManager i_LeftSlot, Bridge i_Bridge)
    {
        int startRow = i_LeftSlot.Row;
        int startCol = i_LeftSlot.Col;
        int bridgeWidth = i_Bridge.SpacesOccupies;
        int bridgeHeight = i_Bridge.Height;
        TurnOffSlots(i_LeftSlot, i_Bridge);
        placeBridge(startRow, startCol, i_Bridge, bridgeHeight, bridgeWidth, i_Bridge.IsTilted);
        m_BridgesInside.Add(i_Bridge.Id, i_Bridge);
    }

    //A different way:
    //1. in TurnOnSlots, check that their free to place in. if yes, SlotManager.BridgeAbove = i_Bridge (or += for multiple bridge handling)
    //2. in placeBridge, simply make sure that SlotManager.BridgeAbove == i_Bridge (or contains)

    //TODO: fix the direction of the bridge in place/pull bridge (tilted,rotated wise)
    private void placeBridge(int i_StartRow, int i_StartCol, Bridge i_Bridge, int i_BridgeHeight, int i_BridgeWidth, bool i_IsTilted)
    {
        SlotManager currentSlotManager = m_SlotManagers[i_StartRow - 1, i_StartCol - 1];
        Bridge bridgeInside = currentSlotManager.BridgeInside;
        //DragBridge bridgeToPlace = i_Bridge.GetComponent<DragBridge>();
        bool isSameRange = bridgeInside && isInSameRange(i_Bridge, bridgeInside);
        bool isHeightCollision = currentSlotManager.HeightInside >= i_BridgeHeight;

        //check that no other bridges collides in or above this slot
        if (isSameRange || isHeightCollision)
        {
            throw new System.Exception(m_ExceptionMessage);
        }

        foreach (int height in currentSlotManager.HeightsAbove)
        {
            Debug.Log(height);
            if (height == i_BridgeHeight)
            {
                throw new System.Exception(m_ExceptionMessage);
            }
        }

        //check recursively
        if (i_BridgeWidth != 1)
        {
            if (i_IsTilted)
            {
                placeBridge(i_StartRow + 1, i_StartCol, i_Bridge, i_BridgeHeight, i_BridgeWidth - 1, i_IsTilted);
            }
            else
            {
                placeBridge(i_StartRow, i_StartCol + 1, i_Bridge, i_BridgeHeight, i_BridgeWidth - 1, i_IsTilted);
            }
        }

        //update slots
        currentSlotManager.HeightsAbove.Add(i_BridgeHeight);
        if (i_BridgeWidth == 1 || i_BridgeWidth == i_Bridge.SpacesOccupies)
        {
            if (!currentSlotManager.IsFree())
            {
                throw new System.Exception(m_ExceptionMessage);
            }
            currentSlotManager.BridgeEnter(i_Bridge);
        }
    }

    public void PullOutBridge(SlotManager i_LeftSlot, Bridge i_Bridge)
    {
        int bridgeWidth = i_Bridge.SpacesOccupies;
        int leftRow = i_LeftSlot.Row;
        int leftCol = i_LeftSlot.Col;
        int firstIterationSlot, lastIterationSlot;
        bool isTilted = i_Bridge.IsTilted;
        SlotManager currentSlotManager;
        
        if (isTilted)
        {
            firstIterationSlot = i_LeftSlot.Row;
        }
        else
        {
            firstIterationSlot = i_LeftSlot.Col;
        }

        lastIterationSlot = firstIterationSlot + bridgeWidth - 1;
        for (int i = firstIterationSlot; i <= lastIterationSlot; i++)
        {
            currentSlotManager = isTilted ? m_SlotManagers[i - 1, leftCol - 1] : m_SlotManagers[leftRow - 1, i - 1];
            if (i == firstIterationSlot || i == lastIterationSlot)
            {
                currentSlotManager.BridgeLeave();
            }

            currentSlotManager.HeightsAbove.Remove(i_Bridge.Height);
        }

        TurnOnSlots(i_LeftSlot, i_Bridge);
        m_BridgesInside.Remove(i_Bridge.Id);
    }

    private bool isInBoardRange(int endRow, int endCol)
    {
        return endRow >= 1 && endRow <= 9
            && endCol >= 1 && endCol <= 9;
    }

    public void TurnOnSlots(SlotManager i_LeftSlot, Bridge i_BridgeAbove)
    {
        if (!m_IsRotating)
        {
            int endRow = i_LeftSlot.Row;
            int endCol = i_LeftSlot.Col;
            SlotManager rightSlot;

            if (i_BridgeAbove.IsTilted && !m_IsRotated)
            {
                Debug.Log("Bridge Tilted, Board not");
                endRow += i_BridgeAbove.SpacesOccupies - 1;
            }
            else if (!i_BridgeAbove.IsTilted && m_IsRotated)
            {
                Debug.Log("Board Tilted, Bridge not");
                endRow -= i_BridgeAbove.SpacesOccupies - 1;
            }
            else
            {
                endCol += i_BridgeAbove.SpacesOccupies - 1;
            }

            //Debug.Log("Turn on (" + i_LeftSlot.Row + ", " + i_LeftSlot.Col + "), (" + endRow + ", " + endCol + ")");

            if (isInBoardRange(endRow, endCol))
            {
                rightSlot = m_SlotManagers[endRow - 1, endCol - 1];
                i_LeftSlot.TurnOn();
                rightSlot.TurnOn();
            }
        }
    }

    public void TurnOffSlots(SlotManager i_LeftSlot, Bridge i_BridgeAbove)
    {
        int endRow = i_LeftSlot.Row;
        int endCol = i_LeftSlot.Col;
        SlotManager rightSlot;

        if (i_BridgeAbove.IsTilted && !m_IsRotated)
        {
            Debug.Log("Bridge Tilted, Board not");
            endRow += i_BridgeAbove.SpacesOccupies - 1;
        }
        else if (!i_BridgeAbove.IsTilted && m_IsRotated)
        {
            Debug.Log("Board Tilted, Bridge not");
            endRow -= i_BridgeAbove.SpacesOccupies - 1;
        }
        else
        {
            endCol += i_BridgeAbove.SpacesOccupies - 1;
        }

        //Debug.Log("Turn off (" + i_LeftSlot.Row + ", " + i_LeftSlot.Col + "), (" + endRow + ", " + endCol + ")");

        if (isInBoardRange(endRow, endCol))
        {
            rightSlot = m_SlotManagers[endRow - 1, endCol - 1];
            i_LeftSlot.TurnOff();
            rightSlot.TurnOff();
        }
    }

    public void TurnOnSlot(int i_Row, int i_Col)
    {
        m_SlotManagers[i_Row - 1, i_Col - 1].TurnOn();
    }

    public void TurnOffSlot(int i_Row, int i_Col)
    {
        m_SlotManagers[i_Row - 1, i_Col - 1].TurnOff();
    }

    protected void OnRotationStart()
    {
        m_IsRotating = true;
    }

    protected void OnRotationStop(string i_Side)
    {
        m_IsRotating = false;
        switch (i_Side)
        {
            case "A":
                m_IsRotated = false;
                Debug.Log("Side A");
                break;
            case "B":
                m_IsRotated = true;
                Debug.Log("Side B");
                break;
        }

        foreach (Bridge bridge in m_BridgesInside.Values)
        {
            bridge.IsTilted = !bridge.IsTilted;
        }
    }

   /* private bool isRangeClear(int i_StartRow, int i_StartCol, int i_BridgeHeight, int i_BridgeWidth, bool i_IsTilted)
    {
        bool isClear = true;
        SlotManager currentSlot;
        int rowIndex = i_StartRow - 1;
        int colIndex = i_StartCol - 1;

        for (int i = 0; i < i_BridgeWidth && isClear; i++)
        {
            currentSlot = m_SlotManagers[rowIndex, colIndex];
            if (currentSlot.HeightInside >= i_BridgeHeight)
            {
                isClear = false;
                break;
            }

            foreach (int height in currentSlot.HeightsAbove)
            {
                if (height == i_BridgeHeight)
                {
                    isClear = false;
                    break;
                }
            }

            if (i_IsTilted)
            {
                rowIndex--;
            }
            else
            {
                colIndex++;
            }
        }

        return isClear;
    }*/

    /*private bool isLengthRangeClear(int i_StartRow, int i_EndRow, int i_Col, int i_Height)
    {
        bool isClear = true;
        SlotManager slotManager;

        for (int i = i_StartRow - 1; i < i_EndRow && isClear; i++)
        {
            slotManager = m_SlotManagers[i, i_Col];
            if (slotManager.HeightInside >= i_Height)
            {
                isClear = false;
                break;
            }

            foreach (int height in slotManager.HeightsAbove)
            {
                if (height == i_Height)
                {
                    isClear = false;
                    break;
                }
            }
        }

        return isClear;
    }

    private bool isWidthRangeClear(int i_StartCol, int i_EndCol, int i_Row)
    {

    }


    public bool IsSlotsRangeClear(SlotManager i_LeftSlot, GameObject i_Bridge, bool i_IsTilted)
    {
        bool isRangeClear = true;
        int startRow = i_LeftSlot.Row;
        int startCol = i_LeftSlot.Col;
        int endRow, endCol;
        int bridgeWidth = i_Bridge.GetComponent<BridgePlacement>().SpacesOccupies;
        int bridgeHeight = i_Bridge.GetComponent<BridgePlacement>().Height;

        if (i_IsTilted)
        {
            endRow = startRow - bridgeWidth + 1;
            endCol = startCol;
        }
        else
        {
            endRow = startRow;
            endCol = startCol - bridgeWidth + 1;
        }

        return isRangeClear;
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
