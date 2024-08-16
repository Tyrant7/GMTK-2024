using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notepad : MonoBehaviour
{
    [SerializeField] GameObject brush;
    [SerializeField] LayerMask mask;

    Color currentColor;

    LineRenderer currentLineRenderer;
    Vector2 lastPos;

    private void Update()
    {
        Draw();
    }

    void Draw()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!Physics2D.OverlapPoint(mousePos, mask))
        {
            currentLineRenderer = null;
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (mousePos != lastPos && currentLineRenderer != null)
            {
                AddPoint(mousePos);
                lastPos = mousePos;
            }
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!Physics2D.OverlapPoint(mousePos, mask))
        {
            return;
        }

        GameObject brushInstance = Instantiate(brush, transform);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        currentLineRenderer.startColor = currentColor;
        currentLineRenderer.endColor = currentColor;

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    public Color GetCurrentColor()
    {
        return currentColor;
    }

    public void SetColor(Color newColor)
    {
        currentColor = newColor;
    }
}
