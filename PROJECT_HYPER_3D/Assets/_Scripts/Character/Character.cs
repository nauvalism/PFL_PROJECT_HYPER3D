using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Character : MonoBehaviour
{
    [Header("MainAttribute")]
    [SerializeField] MainCharacterAttribute baseAttribute;
    

    [Header("Components")]
    [SerializeField] int hp = 100;
    [SerializeField] bool invulnerable = false;
    [SerializeField] CharacterShooter shooter;
    [SerializeField] CharacterRotator rotator;
    [SerializeField] CharacterMovement mover;
    [SerializeField] CharAnimatorCatcher charAnim;
    [SerializeField] CharacterEnemySensor charSensor;
    [SerializeField] Collider hitBox;

    [Header("BARRIER")]
    [SerializeField] GameObject barrier;
    [SerializeField] Renderer barrierRenderer;
    [SerializeField] Material barrierMaterial;

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

    [Header("Levelup")]
    [SerializeField] LevelupEffect levelupEffect;

    [Header("Dead")]
    [SerializeField] ParticleSystem deadEffect;

    [SerializeField] SoundManager sound;

    private void OnValidate() {
        vShake = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start() {
        barrierRenderer = barrier.GetComponent<Renderer>();
        barrierMaterial = barrierRenderer.GetComponent<Material>();
        
    }

    public void ResetScript()
    {
        
    }

    public void DoLevelUp()
    {
        GameplayController.instance.AddStatistic(StatisticID.level, 1);
        levelupEffect.DoLvlUp();
    }

    public void AddTarget(Transform t)
    {
        charAnim.Shoot();
        rotator.AddTarget(t);
    }

    public void RemoveTargetOnly(Transform t)
    {
        rotator.RemoveTarget(t);
    }

    public void RemoveEnemyTarget(BaseEnemy enemy)
    {
        charSensor.UnRegisterEnemy(enemy.GetCollider(), enemy);
        if(charSensor.NoEnemy())
        {
            charAnim.Idle();
        }
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

    public bool GetHit(int dmg)
    {
        hp -= dmg;
        GameplayController.instance.AddStatistic(StatisticID.damageTaken, dmg);
        if(hp <= 0)
        {
            GameplayController.instance.AddStatistic(StatisticID.deathTimes, 1);
            return true;
        }

        
        return false;
    }

    public Transform GetMover()
    {
        return mover.GetMover();
    }

    public void ApplyStat(List<int> values)
    {
        baseAttribute.AddStat(values);
    }

    public MainCharacterAttribute GetAttribute()
    {
        return baseAttribute;
    }

    public void NullifyTarget()
    {
        rotator.NullifyTarget();
    }


    public void Invulnerable()
    {
        this.invulnerable = true;
    }

    public void UnInvulnerable()
    {
        this.invulnerable = false;
    }

    public void ShowBarrier()
    {
        LeanTween.cancel(barrier);
        LeanTween.scale(barrier, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            LeanTween.value(barrier.gameObject, .0f, 1.0f, 1.5f).setOnUpdate((float f)=>{
                Color a = barrierMaterial.color;
                barrierMaterial.color = new Color(a.r, a.g, a.b, f);
            }).setLoopPingPong(3).setOnComplete(()=>{
                HideBarrier();
            });
        });
    }

    public void HideBarrier()
    {
        LeanTween.cancel(barrier);
        LeanTween.value(barrier.gameObject, 1.0f, .0f, .25f).setOnUpdate((float f)=>{
            Color a = barrierMaterial.color;
            barrierMaterial.color = new Color(a.r, a.g, a.b, f);
        }).setOnComplete(()=>{
            LeanTween.scale(barrier, Vector3.zero, 0.25f).setEase(LeanTweenType.easeOutQuad);
        });
    }

    public bool GetInvulnerable()
    {
        return invulnerable;
    }

    public void PauseAnim()
    {
        charAnim.Pause();
    }

    public void ResumeAnim()
    {
        charAnim.Resume();
    }
}
