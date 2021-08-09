using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePlacement : MonoBehaviour
{
    [SerializeField] public int m_Height;
    [SerializeField] public int m_SpacesOccupies;
    [SerializeField] public int m_Id;
    BoardManager m_BoardManager;
    Vector3 m_Target;
    Vector3 m_OriginalPosition;
    Transform m_OriginalParent;
    const float k_Speed = 100;
    bool m_IsInSlot;
    bool m_IsInMotion;

    // Start is called before the first frame update
    void Start()
    {
        m_BoardManager = GameObject.Find("GameBoard").GetComponent<BoardManager>();
        m_OriginalParent = transform.parent;
        m_IsInSlot = false;
        m_IsInMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsInMotion)
        {
            moveToSlot();
        }
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

    public int Height
    {
        get { return m_Height; }
    }

    public int SpacesOccupies
    {
        get { return m_SpacesOccupies; }
    }

    private void moveToSlot()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_Target, Time.deltaTime * k_Speed);
        if (transform.position.y <= m_Target.y)
        {
            m_IsInMotion = false;
            m_IsInSlot = true;
        }
    }

    private void setForPlacementIfPossible()
    {
        SlotsHighlighter slotsHighlighter1 = transform.GetChild(0).GetComponent<SlotsHighlighter>();
        //SlotsHighlighter slotsHighlighter2 = transform.GetChild(1).GetComponent<SlotsHighlighter>();
        //bool isAboveFreeSlot = slotsHighlighter1.IsAboveFreeSlot() && slotsHighlighter2.IsAboveFreeSlot();
        //if (isAboveFreeSlot)
        try
        {
            m_BoardManager.PlaceBridge(slotsHighlighter1.CurrentSlotManager, GetComponent<Bridge>());
            Vector3 differenceVector = slotsHighlighter1.GetDifferenceVector();
            Vector3 plainDifference = new Vector3(differenceVector.x, 0, differenceVector.z);

            transform.SetParent(GameObject.Find("GameBoard").transform);
            /*slotsHighlighter1.EnterSlot(this.gameObject);
            slotsHighlighter2.EnterSlot(this.gameObject);*/
            slotsHighlighter1.enabled = false;
            //slotsHighlighter2.enabled = false;
            m_OriginalPosition = transform.position;
            transform.position += plainDifference;
            transform.GetComponent<DragBridge>().enabled = false;
            m_Target = new Vector3(transform.position.x, transform.position.y - differenceVector.y - 2f, transform.position.z);
            m_IsInMotion = true;
        }
        catch (System.Exception e)
        {
            /*MessageBoxManager mbm = GameObject.Find("MessageBox").GetComponent<MessageBoxManager>();
            mbm.ShowText("hi");
            mbm.ShowText(e.ToString);*/
        }
    }

    private void returnUp()
    {
        SlotsHighlighter slotsHighlighter1 = transform.GetChild(0).GetComponent<SlotsHighlighter>();
        SlotManager currentSlotManager = slotsHighlighter1.CurrentSlotManager;
        //SlotsHighlighter slotsHighlighter2 = transform.GetChild(1).GetComponent<SlotsHighlighter>();

        transform.parent = m_OriginalParent;
        transform.position = m_OriginalPosition;
        transform.GetComponent<DragBridge>().enabled = true;
        slotsHighlighter1.enabled = true;
        //m_BoardManager.PullOutBridge(currentSlotManager.Row, currentSlotManager.Col, gameObject);
        //slotsHighlighter2.enabled = true;
        //slotsHighlighter1.LeaveSlot();
        //slotsHighlighter2.LeaveSlot();
        m_IsInSlot = false;
    }

    //old way
    /*void OnCollisionEnter(Collision collision)
    {
        Vector3 differenceVector = transform.GetComponentInChildren<SlotsHighlighter>().GetDifferenceVector();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        transform.position += differenceVector + new Vector3(0, -1f, 0);
        isInSlot = true;
        isInMotion = false;
        //rigidbody.velocity = Vector3.zero;
    }*/
}
