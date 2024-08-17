using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform followTarget;

    public void EnableFollow(Transform target)
    {
        followTarget = target;
    }

    private void Update()
    {
        if (followTarget == null) return;

        Debug.Log("following " + followTarget.name);

    }
}
