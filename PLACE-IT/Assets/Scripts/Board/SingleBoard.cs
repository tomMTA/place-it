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
}
