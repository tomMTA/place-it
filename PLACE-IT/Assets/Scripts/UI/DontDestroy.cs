using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] public string m_Tag;

    void Awake()
    {
        GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag(m_Tag);
        if (musicPlayers.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
