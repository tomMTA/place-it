using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color tempColor;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnMouseEnter()
    {
        tempColor = meshRenderer.material.color;
        meshRenderer.material.color = tempColor + new Color(0.1f, 0.1f, 0.1f);
    }
    void OnMouseExit()
    {
        meshRenderer.material.color = tempColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
