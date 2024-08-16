using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    [SerializeField] Notepad notepad;
    [SerializeField] Color color;

    const float EXPANDED_SCALE = 1.4f;

    private void Update()
    {
        if (notepad.GetCurrentColor() == color)
        {
            transform.localScale = new Vector3(EXPANDED_SCALE, 1, 1);
            return;
        }
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            notepad.SetColor(color);
        }
    }
}
