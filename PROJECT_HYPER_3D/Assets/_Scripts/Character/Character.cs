using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Character : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] CinemachineBasicMultiChannelPerlin vShake;
    [SerializeField] CharacterShooter shooter;
    [SerializeField] CharacterRotator rotator;
    [SerializeField] Collider hitBox;

    public void AddTarget(Transform t)
    {
        rotator.AddTarget(t);
    }

    public void DoShoot()
    {
        shooter.DoShoot();
    }

    public void ShakeCamera(float magnitude, float hold, float duration)
    {
        LeanTween.cancel(vCam.gameObject);
        vShake.m_AmplitudeGain = magnitude;
        LeanTween.value(vCam.gameObject, vShake.m_AmplitudeGain, .0f, duration).setDelay(hold).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            vShake.m_AmplitudeGain = f;
        });
    }

    public Transform GetShootReference()
    {
        return rotator.GetCurrentReference();
    }

    public Collider GetHitBox()
    {
        return hitBox;
    }
}
