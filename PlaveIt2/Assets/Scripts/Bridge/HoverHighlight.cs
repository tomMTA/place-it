using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    void OnMouseOver()
    {
        if (!Input.GetMouseButton(0))
        {
            meshRenderer.material.color = originalColor + new Color(0.3f, 0.3f, 0.3f);
        }
    }
    void OnMouseExit()
    {
        if (!Input.GetMouseButton(0))
        {
           ResetColor();
        }
    }

    public void ResetColor()
    {
        meshRenderer.material.color = originalColor;
    }
}
