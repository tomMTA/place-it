using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInit : MonoBehaviour
{
    public int width = 12, length = 9;
    GameObject boardBase;
    GameObject boardGrid;
    GameObject[] horizontalBorder;
    GameObject[] verticalBorder;
    GameObject boardFrame;
    GameObject[] horizontalFrame;
    GameObject[] verticalFrame;

    // Start is called before the first frame update
    void Start()
    {
        //create base
        boardBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boardBase.name = "Board Base";
        boardBase.transform.localScale = new Vector3(width, 1, length);
        boardBase.transform.SetParent(transform);

        //create grid
        boardGrid = new GameObject("Board Grid");
        boardGrid.transform.SetParent(transform);
        horizontalBorder = new GameObject[length - 1];
        for (int i = 0; i < length - 1; i++)
        {
            horizontalBorder[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            horizontalBorder[i].name = "Horizontal Border";
            horizontalBorder[i].transform.localScale = new Vector3(width, 1, 0.25f);
            horizontalBorder[i].transform.SetParent(boardGrid.transform);
            horizontalBorder[i].transform.localPosition = new Vector3(0, 1, i - (float)(length - 2) / 2);
        }
        verticalBorder = new GameObject[width - 1];
        for (int i = 0; i < width - 1; i++)
        {
            verticalBorder[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            verticalBorder[i].name = "Vertical Border";
            verticalBorder[i].transform.localScale = new Vector3(0.25f, 1, length);
            verticalBorder[i].transform.SetParent(boardGrid.transform);
            verticalBorder[i].transform.localPosition = new Vector3(i - (float)(width - 2) / 2, 1, 0);
        }

        //create frame
        boardFrame = new GameObject("Board Frame");
        boardFrame.transform.SetParent(transform);
        horizontalFrame = new GameObject[2];
        for (int i = 0; i < 2; i++)
        {
            horizontalFrame[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            horizontalFrame[i].name = "Horizontal Frame";
            horizontalFrame[i].transform.localScale = new Vector3(width + 1, 2, 0.5f);
            horizontalFrame[i].transform.SetParent(boardFrame.transform);
        }
        horizontalFrame[0].transform.localPosition = new Vector3(0, 0.5f, ((float)length / -2) - 0.25f);
        horizontalFrame[1].transform.localPosition = new Vector3(0, 0.5f, ((float)length / 2) + 0.25f);
        verticalFrame = new GameObject[2];
        for (int i = 0; i < 2; i++)
        {
            verticalFrame[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            verticalFrame[i].name = "Vertical Frame";
            verticalFrame[i].transform.localScale = new Vector3(0.5f, 2, length + 1);
            verticalFrame[i].transform.SetParent(boardFrame.transform);
        }
        verticalFrame[0].transform.localPosition = new Vector3(((float)width / -2) - 0.25f, 0.5f, 0);
        verticalFrame[1].transform.localPosition = new Vector3(((float)width / 2) + 0.25f, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}