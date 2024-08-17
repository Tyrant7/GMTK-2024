using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal : MonoBehaviour
{
    [SerializeField] float revealDuration = 1;
    [SerializeField] SpriteRenderer sr;

    public bool IsRevealing { get; private set; }

    public IEnumerator AnimateReveal()
    {
        IsRevealing = true;
        gameObject.SetActive(true);

        float t = Time.time + revealDuration;
        while (Time.time < t)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - revealDuration * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        IsRevealing = false;
    }

    public IEnumerator AnimateConceal()
    {
        IsRevealing = true;
        gameObject.SetActive(true);

        float t = Time.time + revealDuration;
        while (Time.time < t)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + revealDuration * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        IsRevealing = false;
    }
}
