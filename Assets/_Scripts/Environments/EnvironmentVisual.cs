using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentVisual : MonoBehaviour
{
    const float REVEAL_DURATION = 1;

    public bool IsRevealing { get; private set; }

    public IEnumerator Reveal(SpriteRenderer tableRenderer)
    {
        IsRevealing = true;
        gameObject.SetActive(true);

        float t = Time.time + REVEAL_DURATION;
        while (Time.time < t)
        {
            tableRenderer.color = new Color(1, 1, 1, tableRenderer.color.a - REVEAL_DURATION / t);
            yield return new WaitForEndOfFrame();
        }
        IsRevealing = false;
    }
}
