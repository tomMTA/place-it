using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO - adjust accuracy of rays. debug.raycast works fine but is a bit off
//       find out why raycast isnt working if so

//Bugs -
//1. sometimes transition between slots allows not accurate pairs to be highlighted. Maybe create a dependency between a dominant slot and a subordiant one.
//2. this script doesnt work for some bridges - for example Yellow 2,3,4,5,6

public class SlotsHighlighter : MonoBehaviour
{
    private const string k_ExceptionMessage = "Move the bridge until you see the marked slots!";
    public delegate void HoveredAboveSlotHandler(SlotManager leftSlot);
    public event HoveredAboveSlotHandler HoveredAboveSlot;
    public delegate void LeftAboveSlotHandler(SlotManager leftSlot);
    public event LeftAboveSlotHandler LeftAboveSlot;
    [SerializeField] private LayerMask mask;
    private GameObject currentSlot;
    private BoardManager m_BoardManager;
    private readonly int k_HighlightSlotLayer = 10;
    private bool m_IsTilted;
    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        m_BoardManager = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<BoardManager>();
        m_IsTilted = false;
        ray = new Ray(transform.position, Vector3.down);
    }

    public bool IsTilted
    {
        set
        {
            turnOffSlots();
            m_IsTilted = value; 
        }
    }

    void debugRays()
    {
        //for debug--------------------------------------
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        }
    }

    void highlightSlots()
    {
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider != null)
            {
                GameObject hitReceiver = hitInfo.collider.gameObject;
                if (hitReceiver.layer == k_HighlightSlotLayer && hitReceiver != currentSlot)
                {
                    if (currentSlot != null)
                    {
                        LeftAboveSlot.Invoke(CurrentSlotManager);
                    }
                    currentSlot = hitReceiver;
                    HoveredAboveSlot.Invoke(CurrentSlotManager);
                }
            }
        }
        else
        {
            if (currentSlot != null)
            {
                LeftAboveSlot.Invoke(CurrentSlotManager);
            }
            currentSlot = null;
        }
    }

    private void turnOffSlots()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ray.origin = transform.position;
        //debugRays();
        highlightSlots();
    }

    public SlotManager CurrentSlotManager
    {
        get 
        {
            if (!currentSlot)
            {
                throw new System.Exception(k_ExceptionMessage);
            }
            return currentSlot.GetComponent<SlotManager>(); 
        }
    }

    //Maybe here also check if no bridge of same height is over this slot
    public bool IsAboveFreeSlot()
    {
        return currentSlot != null && currentSlot.GetComponent<SlotManager>().IsFree();
    }

    public Vector3 GetDifferenceVector()
    {
        if (currentSlot)
        {
            return new Vector3(currentSlot.transform.position.x - transform.position.x, transform.position.y - currentSlot.transform.position.y, currentSlot.transform.position.z - transform.position.z);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void TurnOffCurrentSlot()
    {
        if (currentSlot)
        {
            currentSlot.GetComponent<SlotManager>().TurnOff();
        }
    }

    public void TurnOnCurrentSlot()
    {
        if (currentSlot)
        {
            currentSlot.GetComponent<SlotManager>().TurnOn();
        }
    }

    public void EnterSlot(Bridge i_Bridge)
    {
        if(currentSlot)
        {
            SlotManager slotManager = currentSlot.GetComponent<SlotManager>();
            slotManager.BridgeEnter(i_Bridge);
            slotManager.TurnOff();
        }
    }

    public void LeaveSlot()
    {
        if (currentSlot)
        {
            SlotManager slotManager = currentSlot.GetComponent<SlotManager>();
            slotManager.BridgeLeave();
            slotManager.TurnOn();
        }
    }
}