using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineBasicMultiChannelPerlin cmMultiChannelPerlin;

    private float shakeTimer;
    private float shakeTimerTotal;

    private float startingIntensity;
    private float startingFrequency;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            CinemachineVirtualCamera cmVirtualCam = GetComponent<CinemachineVirtualCamera>();
            if (cmVirtualCam != null)
            {
                cmMultiChannelPerlin = cmVirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShakeCamera(float intensity, float frequency, float time)
    {
        cmMultiChannelPerlin.m_AmplitudeGain = intensity;
        cmMultiChannelPerlin.m_FrequencyGain = frequency;

        startingIntensity = intensity;
        startingFrequency = frequency;

        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            cmMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(0f, startingIntensity, shakeTimer / shakeTimerTotal);
            cmMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(1f, startingFrequency, shakeTimer / shakeTimerTotal);
        }
    }
}
