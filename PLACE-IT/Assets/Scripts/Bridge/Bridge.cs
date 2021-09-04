using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] public int m_Height;
    [SerializeField] public int m_SpacesOccupies;
    [SerializeField] public eColor m_Color;
    [SerializeField] public int m_Id;

    private SlotsHighlighter m_SlotsHighlighter;
    private SlotManager m_LeftSlot = null;
    private SlotManager m_RightSlot = null;
    private float bridgeAngle = 5f;
    private BoardManager m_Board;
    public Transform m_Bridge;
    public float bridgeRotationSpeed = 10f;
    public delegate void EnteredSlotHandler();
    public event EnteredSlotHandler EnteredSlot;
    public delegate void ExitedSlotHandler(GameObject bridge);
    public event ExitedSlotHandler ExitedSlot;

    Vector3 m_SlotsTarget;
    Vector3 m_OriginalPosition;
    Transform m_OriginalParent;
    const float k_PlacementSpeed = 200f;
    private bool m_Enabled;
    private bool m_IsInSlot;
    private bool m_IsInPlacement;
    private bool m_IsTilted;
    private bool m_IsBoardRotated;

    // Start is called before the first frame update
    void Start()
    {
        //initBridge();
        m_Enabled = true;
        m_IsTilted = false;
        m_Board = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<BoardManager>();
        m_SlotsHighlighter = transform.GetChild(0).GetComponent<SlotsHighlighter>();
        m_SlotsHighlighter.HoveredAboveSlot += HighlightSlots;
        m_SlotsHighlighter.LeftAboveSlot += UnHighlightSlots;
        GetComponent<DragBridge>().Tilted += OnTilted;
        m_Board.Tilted += OnTilted;
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

    public void HighlightSlots(SlotManager i_LeftSlot)
    {
        m_RightSlot = m_Board.TurnOnSlots(i_LeftSlot, this);
        m_LeftSlot = i_LeftSlot;
    }

    public void UnHighlightSlots(SlotManager i_LeftSlot)
    {
        if (i_LeftSlot)
        {
            m_Board.TurnOffSlots(i_LeftSlot, this);
            m_LeftSlot = null;
        }
    }

    public int Height
    {
        get { return m_Height; }
    }

    public int SpacesOccupies
    {
        get { return m_SpacesOccupies; }
    }

    public eColor Color
    {
        get { return m_Color; }
    }

    public int Id
    {
        get { return m_Id; }
    }

    public bool Enabled
    {
        get { return m_Enabled; }
        set { m_Enabled = value; }
    }

    public bool IsTilted
    {
        get { return m_IsTilted; }
        set { m_IsTilted = value; }
    }

    public SlotManager LeftSlot
    {
        get { return m_LeftSlot; }
    }

    public SlotManager RightSlot
    {
        get { return m_RightSlot; }
    }

    public bool IsInSlot
    {
        get { return m_IsInSlot; }
    }

    private void rotate90Degrees()
    {
        int newYAngle = transform.eulerAngles.y == 180 ? 270 : 180;

        m_IsTilted = !m_IsTilted;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYAngle, transform.eulerAngles.z);
    }

    void OnMouseOver()
    {
        if (m_Enabled)
        {
            if (Input.GetMouseButtonDown(2))
            {
                //Debug.Log((m_IsInSlot ? "inside. " : "not inside. ") + (m_IsInPlacement ? "in placement." : "not in placement"));
                if (m_IsInSlot)
                {
                    returnUp();
                }
                else if (m_LeftSlot && m_RightSlot)
                {
                    setForPlacementIfPossible();
                }
            }

            if (!m_IsInSlot && Input.GetMouseButtonDown(1))
            {
                Tilt();
            }
        }
    }

    public void Tilt()
    {
        if (m_LeftSlot)
        {
            UnHighlightSlots(m_LeftSlot);
        }
        rotate90Degrees();
    }

    protected void OnTilted()
    {
        if (m_LeftSlot)
        {
            UnHighlightSlots(m_LeftSlot);
            rotate90Degrees();
        }
    }

    private void moveToSlot()
    {
        const float deviation = 0.00001f;

        transform.position = Vector3.MoveTowards(transform.position, m_SlotsTarget, Time.deltaTime * k_PlacementSpeed);
        if (transform.position.y <= m_SlotsTarget.y + deviation)
        {
            m_IsInPlacement = false;
            m_IsInSlot = true;
            EnteredSlot?.Invoke();
        }
    }

    private void setForPlacementIfPossible()
    {
        //try
        {
            m_IsBoardRotated = m_Board.IsRotated;
            //m_LeftSlot = m_SlotsHighlighter.CurrentSlotManager;
            m_Board.PlaceBridge(this);
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

        if (m_Board.IsRotated != m_IsBoardRotated)
        {
            m_IsTilted = !m_IsTilted;
            if (m_Board.IsRotated ^ m_IsTilted)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
            }
        }

        transform.parent = m_OriginalParent;
        //transform.position = m_OriginalPosition;
        transform.GetComponent<DragBridge>().enabled = true;
        m_SlotsHighlighter.enabled = true;
        m_Board.PullOutBridge(this);
        //Debug.Log("Curr IsRotated = " + m_Board.IsRotated + " prev IsRotated = " + m_IsBoardRotated);
        m_IsInSlot = false;
        ExitedSlot?.Invoke(gameObject);
    }
}
