using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBoard : MonoBehaviour
{
    private static SingleBoard instance = new SingleBoard();
    KeyValuePair<int, bool>[][] boardState;

    private SingleBoard() { }

    public static SingleBoard getInstance()
    {
        if(instance == null)
        {
            instance = new SingleBoard();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
