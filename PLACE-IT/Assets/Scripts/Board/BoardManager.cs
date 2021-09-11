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
            m_IsWin = true;
        }
    }
	
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
    }

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
                endRow += i_BridgeAbove.SpacesOccupies - 1;
            }
            else if (!i_BridgeAbove.IsTilted && m_IsRotated)
            {
                endRow -= i_BridgeAbove.SpacesOccupies - 1;
            }
            else
            {
                endCol += i_BridgeAbove.SpacesOccupies - 1;
            }

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

        if (isInBoardRange(endRow, endCol))
        {
            rightSlot = m_SlotManagers[endRow - 1, endCol - 1];
            i_LeftSlot.TurnOff();
            rightSlot.TurnOff();
        }
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
                //Debug.Log("Side A");
                break;
            case "B":
                m_IsRotated = true;
                //Debug.Log("Side B");
                break;
        }
    }
}
