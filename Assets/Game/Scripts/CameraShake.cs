using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance { get { return instance; } }
    [SerializeField] public CinemachineFreeLook freeLookCamera;
    private CinemachineBasicMultiChannelPerlin topRigPerlin;
    private CinemachineBasicMultiChannelPerlin middleRigPerlin;
    private CinemachineBasicMultiChannelPerlin bottomRigPerlin;
    private void Awake()
    {
        instance = this;
        topRigPerlin = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        middleRigPerlin = freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        bottomRigPerlin = freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ShakeCamera(float amplitude, float shakeTIme)
    {
        if (topRigPerlin != null)
        {
            topRigPerlin.m_AmplitudeGain = amplitude;
        }
        if (middleRigPerlin != null)
        {
            middleRigPerlin.m_AmplitudeGain = amplitude;
        }
        if (bottomRigPerlin != null)
        {
            bottomRigPerlin.m_AmplitudeGain = amplitude;
        }
        StartCoroutine(WaitTime(shakeTIme));
    }
    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        ResetIntensity();
    }
    void ResetIntensity()
    {
        if (topRigPerlin != null)
        {
            topRigPerlin.m_AmplitudeGain = 0f;
        }
        if (middleRigPerlin != null)
        {
            middleRigPerlin.m_AmplitudeGain = 0f;
        }
        if (bottomRigPerlin != null)
        {
            bottomRigPerlin.m_AmplitudeGain = 0f;
        }
    }
}
