using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] public int m_Height;
    [SerializeField] public int m_SpacesOccupies;
    [SerializeField] public int m_Id;

    private SlotsHighlighter m_SlotsHighlighter;
    private SlotManager m_LeftSlot = null;
    private float bridgeAngle = 5f;
    private BoardManager m_Board;
    public Transform m_Bridge;
    public float bridgeRotationSpeed = 10f;

    Vector3 m_SlotsTarget;
    Vector3 m_OriginalPosition;
    Transform m_OriginalParent;
    const float k_PlacementSpeed = 100f;
    bool m_IsInSlot;
    bool m_IsInPlacement;
    bool m_IsTilted;

    // Start is called before the first frame update
    void Start()
    {
        //initBridge();
        m_IsTilted = false;
        m_Board = GameObject.Find("GameBoard").GetComponent<BoardManager>();
        m_SlotsHighlighter = transform.GetChild(0).GetComponent<SlotsHighlighter>();
        m_SlotsHighlighter.HoveredAboveSlot += HighlightSlots;
        m_SlotsHighlighter.LeftAboveSlot += UnHighlightSlots;
        GetComponent<DragBridge>().Tilted += OnTilted;
    }

    // Update is called once per frame
    void Update()
    {
        //RotateOnClickAndDrag();
        if (m_IsInPlacement)
        {
            moveToSlot();
        }
    }

    private void initBridge()
    {
    }

    private void RotateOnClickAndDrag()
    {
        bridgeAngle += Input.GetAxis("Mouse X") * bridgeRotationSpeed * - Time.deltaTime;
        bridgeAngle = Mathf.Clamp(bridgeAngle, 0, 180);
        //if (Input.GetMouseButtonDown(0));
        //{
        //    this.transform.Rotate(Time.deltaTime * 10);
        //}
    }

    protected void HighlightSlots(SlotManager i_LeftSlow)
    {
        m_Board.TurnOnSlots(i_LeftSlow, this);
        m_LeftSlot = i_LeftSlow;
    }

    protected void UnHighlightSlots(SlotManager i_LeftSlow)
    {
        m_Board.TurnOffSlots(i_LeftSlow, this);
        m_LeftSlot = null;
    }

    public int Height
    {
        get { return m_Height; }
    }

    public int SpacesOccupies
    {
        get { return m_SpacesOccupies; }
    }

    public int Id
    {
        get { return m_Id; }
    }

    public bool IsTilted
    {
        get { return m_IsTilted; }
        set { m_IsTilted = value; }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (m_IsInSlot)
            {
                returnUp();
            }
            else
            {
                setForPlacementIfPossible();
            }
        }
    }

    protected void OnTilted()
    {
        if (m_LeftSlot)
        {
            UnHighlightSlots(m_LeftSlot);
        }
        m_IsTilted = !m_IsTilted;
    }

    private void moveToSlot()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_SlotsTarget, Time.deltaTime * k_PlacementSpeed);
        if (transform.position.y <= m_SlotsTarget.y)
        {
            m_IsInPlacement = false;
            m_IsInSlot = true;
        }
    }

    private void setForPlacementIfPossible()
    {
        //try
        {
            m_Board.PlaceBridge(m_SlotsHighlighter.CurrentSlotManager, this);
            Vector3 differenceVector = m_SlotsHighlighter.GetDifferenceVector();
            Vector3 plainDifference = new Vector3(differenceVector.x, 0, differenceVector.z);

            transform.SetParent(m_Board.transform);
            m_SlotsHighlighter.enabled = false;
            m_OriginalPosition = transform.position;
            transform.position += plainDifference;
            transform.GetComponent<DragBridge>().enabled = false;
            m_SlotsTarget = new Vector3(transform.position.x, transform.position.y - differenceVector.y - 2f, transform.position.z);
            m_IsInPlacement = true;
        }
        //catch (System.Exception e)
        {
            /*MessageBoxManager mbm = GameObject.Find("MessageBox").GetComponent<MessageBoxManager>();
            mbm.ShowText("hi");
            mbm.ShowText(e.ToString);*/
        }
    }

    private void returnUp()
    {
        SlotManager currentSlotManager = m_SlotsHighlighter.CurrentSlotManager;

        transform.parent = m_OriginalParent;
        transform.position = m_OriginalPosition;
        transform.GetComponent<DragBridge>().enabled = true;
        m_SlotsHighlighter.enabled = true;
        m_Board.PullOutBridge(currentSlotManager, this);
        m_IsInSlot = false;
    }
}
