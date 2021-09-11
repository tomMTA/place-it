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
        const int verticalDistance = 5;
        const int horizontalDistance = 5;
        m_BridgeScripts = Resources.FindObjectsOfTypeAll<Bridge>();
        m_BridgeObjects = new GameObject[m_BridgeScripts.Length];

        for (int i = 1; i < m_BridgeScripts.Length; i++)
        {
            m_BridgeObjects[i] = (GameObject)Instantiate(m_BridgeScripts[i].gameObject,
                transform.position + new Vector3(i - horizontalDistance, i * verticalDistance, 0), transform.rotation);

            Debug.Log("Instansiated " + m_BridgeScripts[i].Color + " bridge of ID" + m_BridgeScripts[i].Id);
        }
    }
}
