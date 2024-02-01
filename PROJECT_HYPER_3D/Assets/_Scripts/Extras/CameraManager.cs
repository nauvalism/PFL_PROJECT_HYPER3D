using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance {get;set;}
    
    private void Awake() {
        instance = this;
    }

    [SerializeField] CinemachineVirtualCamera personalCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin pCamShake;

    public void ShakeCamera()
    {
        LeanTween.cancel(personalCamera.gameObject);
        pCamShake.m_AmplitudeGain = 3.0f;
        LeanTween.value(personalCamera.gameObject, pCamShake.m_AmplitudeGain, .0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            pCamShake.m_AmplitudeGain = f;
        });
    }

    public void ShakeCamera(float hold, float magnitude)
    {
        LeanTween.cancel(personalCamera.gameObject);
        pCamShake.m_AmplitudeGain = magnitude;
        LeanTween.value(personalCamera.gameObject, pCamShake.m_AmplitudeGain, .0f, 2.0f).setDelay(hold).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            pCamShake.m_AmplitudeGain = f;
        });
    }
}
