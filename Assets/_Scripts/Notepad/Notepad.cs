using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notepad : MonoBehaviour
{
    [Header("Anchoring")]
    [SerializeField] Vector2 unfocusedPoint;
    [SerializeField] Vector2 focusedPoint;
    [SerializeField] LayerMask focusMask;
    [SerializeField] float slideSpeed, minSlideSpeed, maxSlideSpeed;

    [Header("Drawing")]
    [SerializeField] GameObject brush;
    [SerializeField] LayerMask mask;

    Color currentColor;

    LineRenderer currentLineRenderer;
    Vector2 lastPos;

    private void Update()
    {
        // Focusing
        Vector2 interpolateTarget = unfocusedPoint;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Physics2D.OverlapPoint(mousePos, focusMask))
        {
            interpolateTarget = focusedPoint;
        }
        MoveToPlacement(interpolateTarget);

        // Drawing
        if (GameManager.Instance.gameState == GameManager.GameState.DrawPhase)
        {
            Draw();
        }
    }

    void MoveToPlacement(Vector2 interpolateTarget)
    {
        float fraction = Mathf.Abs(transform.position.x - interpolateTarget.x) + Mathf.Abs(transform.position.y - interpolateTarget.y);
        Vector2 movement = Vector2.MoveTowards(transform.position, interpolateTarget, Mathf.Clamp(fraction * slideSpeed, minSlideSpeed, maxSlideSpeed) * Time.deltaTime);
        transform.position = movement;
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
