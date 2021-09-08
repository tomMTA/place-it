using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [SerializeField] public int m_Level;
    [SerializeField] public int[] m_Moves_Bars = new int[2];
    [SerializeField] public int[] m_Time_Bars = new int[2];
    [SerializeField] public GameObject m_StarsCanvas;
    [SerializeField] private SoundPlayer m_SoundPlayer;

    private AudioSource m_BridgePlacementSound;
    private AudioSource m_BridgePullOutSound;
    private AudioSource m_WinSound;
    private int m_Stars;
    private readonly SlotManager[,] m_SlotManagers = new SlotManager[9, 9];
    private readonly Dictionary<int, Bridge> m_BridgesInside = new Dictionary<int, Bridge>();
    private const string m_ExceptionMessage = "You can't place it here!";
    private bool m_IsRotated;
    private bool m_IsRotating;
    private bool m_IsWin;
    private SideObserver m_SideAObserver;
    private SideObserver m_SideBObserver;
    private int m_Moves;
    public delegate void TiltedHandler();
    public TiltedHandler Tilted;

    // Start is called before the first frame update
    void Start()
    {
        m_BridgePullOutSound = m_BridgePlacementSound = GetComponent<AudioSource>();
        m_IsRotated = false;
        m_IsRotating = false;
        m_IsWin = false;
        m_SideAObserver = new SideObserver(m_Level.ToString(), eSide.A);
        m_SideBObserver = new SideObserver(m_Level.ToString(), eSide.B);
        m_Moves = 0;
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
    public bool IsRotated
    {
        get { return m_IsRotated; }
    }

    private bool isInSameRange(Bridge i_Bridge1, Bridge i_Bridge2)
    {
        if (!i_Bridge1 || !i_Bridge2)
        {
            return false;
        }

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

        return (start1 < start2 && end1 > start2 && end1 < end2) || (start1 > start2 && start1 < end2 && end1 > end2);
    }

    private void incRowAndCol(bool i_IsBridgeTilted, int i_AmountToInc, ref int io_currentToNextRow, ref int io_CurrentToNextCol)
    {
        if (i_IsBridgeTilted && !m_IsRotated)
        {
            //Debug.Log("Bridge Tilted, Board not");
            io_currentToNextRow += i_AmountToInc;
        }
        else if (!i_IsBridgeTilted && m_IsRotated)
        {
            //Debug.Log("Board Tilted, Bridge not");
            io_currentToNextRow -= i_AmountToInc;
        }
        else
        {
            io_CurrentToNextCol += i_AmountToInc;
        }
    }

    /*public SlotManager PlaceBridge(SlotManager i_LeftSlot, Bridge i_Bridge)
    {
        int startRow = i_LeftSlot.Row;
        int startCol = i_LeftSlot.Col;
        int bridgeWidth = i_Bridge.SpacesOccupies;
        int bridgeHeight = i_Bridge.Height;
        SlotManager rightSlot;

        TurnOffSlots(i_LeftSlot, i_Bridge);
        //Debug.Log("inserted bridge " + i_Bridge.Id + ", there are " + m_BridgesInside.Count + " bridges in the board");
        rightSlot = placeBridge(startRow, startCol, i_Bridge, bridgeHeight, bridgeWidth, i_Bridge.IsTilted);
        m_BridgesInside.Add(i_Bridge.Id, i_Bridge);

        return rightSlot;
    }*/

    public void OnBridgeEnter()
    {
        m_SoundPlayer.PlayClickSound();
        if (m_IsWin)
        {
            Timer timer = GameObject.FindWithTag("Timer").GetComponent<Timer>();
            setStars(timer.Seconds);
            timer.enabled = false;
            m_StarsCanvas.SetActive(true);
            GameObject.Find("StarsPanel").GetComponent<StarsFiller>().FillStars(m_Stars);
            //Debug.Log("Stars: " + m_Stars);
            m_SoundPlayer.PlayWinSound();
            foreach (Bridge bridge in m_BridgesInside.Values)
            {
                bridge.Enabled = false;
            }
        }
    }

    public void PlaceBridge(Bridge i_Bridge)
    {
        SlotManager leftSlot = i_Bridge.LeftSlot;
        SlotManager rightSlot = i_Bridge.RightSlot;
        SlotManager currentSlot = leftSlot;

        if (leftSlot && rightSlot)
        {
            i_Bridge.EnteredSlot += OnBridgeEnter;
            leftSlot.TurnOff();
            rightSlot.TurnOff();
            leftSlot.BridgeEnter(i_Bridge);
            rightSlot.BridgeEnter(i_Bridge);
            //Debug.Log("LeftSlot: (" + leftSlot.Row + ", " + leftSlot.Col + "), RightSlot: (" + rightSlot.Row + ", " + rightSlot.Col + ")");
            m_SideAObserver.BridgeEnter(leftSlot.Row - 1, rightSlot.Row - 1, leftSlot.Col - 1, rightSlot.Col - 1, i_Bridge.Height - 1, i_Bridge.Color);
            m_SideBObserver.BridgeEnter(leftSlot.Row - 1, rightSlot.Row - 1, leftSlot.Col - 1, rightSlot.Col - 1, i_Bridge.Height - 1, i_Bridge.Color);
            m_BridgesInside.Add(i_Bridge.Id, i_Bridge);
            m_Moves++;
            while (currentSlot != null)
            {
                currentSlot.HeightsAbove.Add(i_Bridge.Height);
                currentSlot = getNextSlot(currentSlot, rightSlot);
            }
        }

        if (m_SideAObserver.Differences == 0 && m_SideBObserver.Differences == 0)
        {
            //i_Bridge.EnteredSlot += disableBridges;
            //GameObject.Find("Congratulations").GetComponent<Image>().enabled = true;
            //Debug.Log("WIN");
            m_IsWin = true;
        }
    }

    //A different way:
    //1. in TurnOnSlots, check that their free to place in. if yes, SlotManager.BridgeAbove = i_Bridge (or += for multiple bridge handling)
    //2. in placeBridge, simply make sure that SlotManager.BridgeAbove == i_Bridge (or contains)

    //TODO: fix the direction of the bridge in place/pull bridge (tilted,rotated wise)
    /*private SlotManager placeBridge(int i_StartRow, int i_StartCol, Bridge i_Bridge, int i_BridgeHeight, int i_BridgeWidth, bool i_IsTilted)
    {
        SlotManager rightSlot;
        SlotManager currentSlotManager = m_SlotManagers[i_StartRow - 1, i_StartCol - 1];
        Bridge bridgeInside = currentSlotManager.BridgeInside;
        //DragBridge bridgeToPlace = i_Bridge.GetComponent<DragBridge>();
        bool isSameRange = isInSameRange(i_Bridge, bridgeInside);
        bool isHeightCollision = currentSlotManager.HeightInside >= i_BridgeHeight;

        //Debug.Log("(" + currentSlotManager.Row + ", " + currentSlotManager.Col + ")");


        //check that no other bridges collides in or above this slot
        if (isSameRange || isHeightCollision)
        {
            throw new System.Exception(m_ExceptionMessage);
        }

        foreach (int height in currentSlotManager.HeightsAbove)
        {
            //Debug.Log(height);
            if (height == i_BridgeHeight)
            {
                throw new System.Exception(m_ExceptionMessage);
            }
        }

        //check recursively
        if (i_BridgeWidth != 1)
        {//respect board's state
            incRowAndCol(i_Bridge.IsTilted, 1, ref i_StartRow, ref i_StartCol);
            rightSlot = placeBridge(i_StartRow, i_StartCol, i_Bridge, i_BridgeHeight, i_BridgeWidth - 1, i_IsTilted);
            *//*if (i_IsTilted)
            {
                placeBridge(i_StartRow + 1, i_StartCol, i_Bridge, i_BridgeHeight, i_BridgeWidth - 1, i_IsTilted);
            }
            else
            {
                placeBridge(i_StartRow, i_StartCol + 1, i_Bridge, i_BridgeHeight, i_BridgeWidth - 1, i_IsTilted);
            }*//*
        }
        else
        {
            rightSlot = currentSlotManager;
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

        return rightSlot;
    }*/

    private int getStarsAccordingToBars(int[] i_Bars, int i_Value)
    {
        int stars;

        if (i_Value <= i_Bars[0])
        {
            stars = 3;
        }
        else if (i_Value <= i_Bars[1])
        {
            stars = 2;
        }
        else
        {
            stars = 1;
        }

        return stars;
    }

    private void setStars(int i_Seconds)
    {
        int timeStars = getStarsAccordingToBars(m_Time_Bars, i_Seconds);
        int movesStars = getStarsAccordingToBars(m_Moves_Bars, m_Moves);
        float avgStars = ((float)timeStars + (float)movesStars) / 2;

        m_Stars = (int)avgStars;
        /*if (m_Stars >= 2)
        {
            m_StarsCanvas.transform.Find("FilledStar2").gameObject.SetActive(true);
        }

        if (m_Stars >= 3)
        {
            m_StarsCanvas.transform.Find("FilledStar3").gameObject.SetActive(true);
        }*/
    }

    private SlotManager getNextSlot(SlotManager i_Current, SlotManager i_Last)
    {
        if (i_Last.Row > i_Current.Row)
        {
            return m_SlotManagers[i_Current.Row, i_Current.Col - 1];
        }
        else if (i_Last.Row < i_Current.Row)
        {
            return m_SlotManagers[i_Current.Row - 2, i_Current.Col - 1];
        }
        else if (i_Last.Col > i_Current.Col)
        {
            return m_SlotManagers[i_Current.Row - 1, i_Current.Col];
        }
        else if (i_Last.Col < i_Current.Col)
        {
            return m_SlotManagers[i_Current.Row - 1, i_Current.Col - 2];
        }
        else
        {
            return null;
        }
    }

    public void PullOutBridge(Bridge i_Bridge)
    {
        SlotManager leftSlot = i_Bridge.LeftSlot;
        SlotManager rightSlot = i_Bridge.RightSlot;
        SlotManager currentSlotManager = leftSlot;
        SlotManager lastSlotManager = rightSlot;

        m_SoundPlayer.PlayClickSound();
        //Debug.Log("(" + currentSlotManager.Row + ", " + currentSlotManager.Col + ")");

        currentSlotManager.BridgeLeave();
        do
        {
            currentSlotManager.HeightsAbove.Remove(i_Bridge.Height);
            currentSlotManager = getNextSlot(currentSlotManager, lastSlotManager);
        }
        while (currentSlotManager != null);

        lastSlotManager.BridgeLeave();
        m_BridgesInside.Remove(i_Bridge.Id);
        m_SideAObserver.BridgeLeave(leftSlot.Row - 1, rightSlot.Row - 1, leftSlot.Col - 1, rightSlot.Col - 1, i_Bridge.Height - 1);
        m_SideBObserver.BridgeLeave(leftSlot.Row - 1, rightSlot.Row - 1, leftSlot.Col - 1, rightSlot.Col - 1, i_Bridge.Height - 1);
        //i_Bridge.LeftSlot.TurnOn();
        //i_Bridge.RightSlot.TurnOn();
    }

    /*public void PullOutBridge(SlotManager i_StartSlot, Bridge i_Bridge)
    {
        int bridgeWidth = i_Bridge.SpacesOccupies;
        int currentRow = i_StartSlot.Row;
        int currentCol = i_StartSlot.Col;
        int endRow = currentRow, endCol = currentCol;
        int firstIterationSlot, lastIterationSlot;
        bool isTilted = i_Bridge.IsTilted;
        SlotManager currentSlotManager;

        m_SlotManagers[currentRow - 1, currentCol - 1].BridgeLeave();
        incRowAndCol(isTilted, bridgeWidth - 1, ref endRow, ref endCol);
        while (currentRow != endRow || currentCol != endCol)
        {
            Debug.Log("Taking care of slot (" + currentRow + ", " + currentCol + ")");
            currentSlotManager = m_SlotManagers[currentRow - 1, currentCol - 1];
            currentSlotManager.HeightsAbove.Remove(i_Bridge.Height);
            incRowAndCol(isTilted, 1, ref currentRow, ref currentCol);
        }

        m_SlotManagers[currentRow - 1, currentCol - 1].BridgeLeave();

        *//*if (isTilted)
        {
            firstIterationSlot = i_StartSlot.Row;
        }
        else
        {
            firstIterationSlot = i_StartSlot.Col;
        }

        lastIterationSlot = firstIterationSlot + bridgeWidth - 1;
        for (int i = firstIterationSlot; i <= lastIterationSlot; i++)
        {
            currentSlotManager = isTilted ? m_SlotManagers[i - 1, currentCol - 1] : m_SlotManagers[currentRow - 1, i - 1];
            if (i == firstIterationSlot || i == lastIterationSlot)
            {
                currentSlotManager.BridgeLeave();
            }

            currentSlotManager.HeightsAbove.Remove(i_Bridge.Height);
        }*//*

        TurnOnSlots(i_StartSlot, i_Bridge);
        m_BridgesInside.Remove(i_Bridge.Id);
        Debug.Log("removed bridge " + i_Bridge.Id + ", there are " + m_BridgesInside.Count + " bridges in the board");
    }*/

    private bool isInBoardRange(int endRow, int endCol)
    {
        return endRow >= 1 && endRow <= 9
            && endCol >= 1 && endCol <= 9;
    }

    private bool isHeightsCollision(int i_BridgeHeight, SlotManager i_Slot)
    {
        bool isCollision = false;
        Bridge bridgeInside = i_Slot.BridgeInside;

        if (bridgeInside && i_BridgeHeight <= bridgeInside.Height)
        {
            isCollision = true;
        }
        else
        {
            foreach (int height in i_Slot.HeightsAbove)
            {
                if (height == i_BridgeHeight)
                {
                    isCollision = true;
                    break;
                }
            }
        }

        return isCollision;
    }

    private bool isLowerBridgeAboveSlot(int i_BridgeHeight, SlotManager i_Slot)
    {
        bool isThere = false;

        foreach (int height in i_Slot.HeightsAbove)
        {
            if (height <= i_BridgeHeight)
            {
                isThere = true;
                break;
            }
        }
        return isThere;
    }

    private bool canBridgeBeAboveSlot(Bridge i_Bridge, SlotManager i_Slot)
    {
        Bridge bridgeInside = i_Slot.BridgeInside;

        return !isInSameRange(i_Bridge, bridgeInside) && !isHeightsCollision(i_Bridge.Height, i_Slot);
    }

    private bool isPossibleToPlace(SlotManager i_LeftSlot, SlotManager i_RightSlot, Bridge i_Bridge)
    {
        bool isPossibleToPlace = true;
        int bridgeHeight = i_Bridge.Height;

        if (!i_LeftSlot.IsFree() || !i_RightSlot.IsFree() || isLowerBridgeAboveSlot(bridgeHeight, i_LeftSlot) || isLowerBridgeAboveSlot(bridgeHeight, i_RightSlot))
        {
            isPossibleToPlace = false;
        }
        else
        {
            SlotManager currentSlotManager = i_LeftSlot;
            Bridge bridgeInside;

            do
            {
                bridgeInside = currentSlotManager.BridgeInside;
                if (!canBridgeBeAboveSlot(i_Bridge, currentSlotManager))
                {
                    isPossibleToPlace = false;
                    break;
                }

                currentSlotManager = getNextSlot(currentSlotManager, i_RightSlot);
            }
            while (currentSlotManager);
        }

        return isPossibleToPlace;
    }

    public SlotManager TurnOnSlots(SlotManager i_LeftSlot, Bridge i_BridgeAbove)
    {
        SlotManager rightSlot = null;

        if (!m_IsRotating)
        {
            int endRow = i_LeftSlot.Row;
            int endCol = i_LeftSlot.Col;

            if (i_BridgeAbove.IsTilted && !m_IsRotated)
            {
                //Debug.Log("Bridge Tilted, Board not");
                endRow += i_BridgeAbove.SpacesOccupies - 1;
            }
            else if (!i_BridgeAbove.IsTilted && m_IsRotated)
            {
                //Debug.Log("Board Tilted, Bridge not");
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
                if (isPossibleToPlace(i_LeftSlot, rightSlot, i_BridgeAbove))
                {
                    i_LeftSlot.TurnOn();
                    rightSlot.TurnOn();
                }
                else
                {
                    rightSlot = null;
                }
            }
        }

        return rightSlot;
    }

    public void TurnOffSlots(SlotManager i_LeftSlot, Bridge i_BridgeAbove)
    {
        int endRow = i_LeftSlot.Row;
        int endCol = i_LeftSlot.Col;
        SlotManager rightSlot;

        if (i_BridgeAbove.IsTilted && !m_IsRotated)
        {
            //Debug.Log("Bridge Tilted, Board not");
            endRow += i_BridgeAbove.SpacesOccupies - 1;
        }
        else if (!i_BridgeAbove.IsTilted && m_IsRotated)
        {
            //Debug.Log("Board Tilted, Bridge not");
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

    /*public void TurnOnSlot(int i_Row, int i_Col)
    {
        m_SlotManagers[i_Row - 1, i_Col - 1].TurnOn();
    }

    public void TurnOffSlot(int i_Row, int i_Col)
    {
        m_SlotManagers[i_Row - 1, i_Col - 1].TurnOff();
    }*/

    /*private void tiltBridges()
    {
        foreach (Bridge bridge in m_BridgesInside.Values)
        {
            bridge.Tilt*
            Debug.Log("Tilted bridge " + bridge.Id);
        }
    }*/

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
                //Debug.Log("Side A");
                break;
            case "B":
                m_IsRotated = true;
                //Debug.Log("Side B");
                break;
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
