using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public void FinishedIn()
    {
        TransitionHandler.Instance.FinishedTransitionIn();
    }

    public void FinishedOut()
    {
        TransitionHandler.Instance.FinishedTransitionOut();
    }
}
