using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    public void UpdateText(int newTime)
    {
        timerText.text = newTime.ToString();
    }
}
