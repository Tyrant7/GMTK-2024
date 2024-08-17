using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayText;

    public void Initialize(string text)
    {
        displayText.text = text;
    }

    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}
