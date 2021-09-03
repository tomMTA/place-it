using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BridgesSpawner : MonoBehaviour
{
    public GameObject[] m_BridgeObjects;
    public Bridge[] m_BridgeScripts;

    void Awake()
    {
        //serialized way
        /*int numOfBridges = m_BridgeObjects.Length;

        for (int i = 0; i < numOfBridges; i++)
        {
            m_BridgeObjects[i] = (GameObject)Instantiate(m_BridgeObjects[i], transform.position, transform.rotation);
            m_BridgeObjects[i].SetActive(true);
        }*/


        //all bridges way
        const int verticalDistance = 5;
        const int horizontalDistance = 5;
        //m_BridgeScripts = new GameObject[24];
        m_BridgeScripts = Resources.FindObjectsOfTypeAll<Bridge>();
        m_BridgeObjects = new GameObject[m_BridgeScripts.Length];

        for (int i = 1; i < m_BridgeScripts.Length; i++)
        {
            m_BridgeObjects[i] = (GameObject)Instantiate(m_BridgeScripts[i].gameObject,
                transform.position + new Vector3(i - horizontalDistance, i * verticalDistance, 0), transform.rotation);
            //m_BridgeObjects[i].SetActive(true);

            Debug.Log("Instansiated " + m_BridgeScripts[i].Color + " bridge of ID" + m_BridgeScripts[i].Id);
        }
    }
}
