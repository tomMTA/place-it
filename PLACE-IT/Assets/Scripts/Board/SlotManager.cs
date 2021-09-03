using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO - add height of bridge inside or over. Might need to use indexed slots.
public class SlotManager : MonoBehaviour
{
    [SerializeField] int m_Row;
    [SerializeField] int m_Col;
    Bridge m_BridgeInside = null;
    private readonly List<int> m_HeightsAbove = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        m_BridgeInside = null;
    }

    public List<int> HeightsAbove
    {
        get { return m_HeightsAbove; }
    }

    /*public void AddHeightAbove(int i_Height)
    {
        m_HeightsAbove.Add(i_Height);
    }

    public void RemoveHeightAbove(int i_Height)
    {
        m_HeightsAbove.Remove(i_Height);
    }*/

    public int Row
    {
        get { return m_Row; }
    }

    public int Col
    {
        get { return m_Col; }
    }

    public int HeightInside
    {
        get { return m_BridgeInside == null ? 0 : m_BridgeInside.Height; }
    }

    public Bridge BridgeInside
    {
        get { return m_BridgeInside; }
    }

    public void TurnOn()
    {
        if (m_BridgeInside == null)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void TurnOff()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void BridgeEnter(Bridge i_Bridge)
    {
        if (m_BridgeInside == null)
        {
            m_BridgeInside = i_Bridge;
        }
    }

    public void BridgeLeave()
    {
        m_BridgeInside = null;
    }

    public bool IsFree()
    {
        return m_BridgeInside == null;
    }
}
