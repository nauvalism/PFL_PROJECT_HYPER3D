using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Character : MonoBehaviour
{
    [Header("MainAttribute")]
    [SerializeField] MainCharacterAttribute baseAttribute;
    

    [Header("Components")]
    
    [SerializeField] CharacterShooter shooter;
    [SerializeField] CharacterRotator rotator;
    [SerializeField] CharacterMovement mover;
    [SerializeField] CharAnimatorCatcher charAnim;
    [SerializeField] Collider hitBox;

    [Header("UI")]
    [SerializeField] CUIManager characterUI;

    [Header("HP Decrases")]
    [SerializeField] List<Transform> hpDecreasePlace;
    [SerializeField] GameObject dmgTextPrefab;
    [SerializeField] Transform dmgTxtParent;
    
    [Header("CAMERA")]
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] CinemachineBasicMultiChannelPerlin vShake;
    [SerializeField] List<float> bulletFreqs;

    private void OnValidate() {
        vShake = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start() {
        
    }

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
        vShake.m_FrequencyGain = bulletFreqs[bulletFreqs.Count -1];
        vShake.m_AmplitudeGain = magnitude;
        LeanTween.value(vCam.gameObject, vShake.m_AmplitudeGain, .0f, duration).setDelay(hold).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            vShake.m_AmplitudeGain = f;
        });
    }

    public void ShakeCamera(float magnitude, int freqIndex, float hold, float duration)
    {
        LeanTween.cancel(vCam.gameObject);
        vShake.m_FrequencyGain = bulletFreqs[freqIndex];
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

    public void GetHit()
    {

    }
    
}
