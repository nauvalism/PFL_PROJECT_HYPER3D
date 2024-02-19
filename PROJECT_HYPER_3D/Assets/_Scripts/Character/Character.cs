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
    [SerializeField] GraphicActor charGraphic;
    [SerializeField] Collider hitBox;

    [Header("BARRIER")]
    [SerializeField] GameObject barrier;
    [SerializeField] Transform barrierScaler;
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

    private void Awake() {
        charGraphic.DisableGraphic();
        mover.DisableMovement();
    }

    private void Start() {
        barrierRenderer = barrier.GetComponent<Renderer>();
        barrierMaterial = barrierRenderer.GetComponent<Material>();
        //characterUI.InitHPSlider(baseAttribute.HP);
        
    }

    public void Setup()
    {
        charGraphic.EnableGraphic();
        //mover.EnableMovement();
    }

     public void SetupAll()
    {
        
        
    }

    public void ClearGame()
    {
        charGraphic.DisableGraphic();
        mover.DisableMovement();
    }

    public void ResetScript()
    {
        this.baseAttribute.ResetStat();
        this.mover.RefreshSpeed();
        this.charAnim.ResetSpeed();
        this.charSensor.ResetSensor();
        hp = baseAttribute.HP;
        characterUI.InitHPSlider(baseAttribute.HP);
    }
    

    public void DoLevelUp()
    {
        GameplayController.instance.AddStatistic(StatisticID.level, 1);
        levelupEffect.DoLvlUp();
    }

    public void AddTarget(Transform t)
    {
        
        rotator.AddTarget(t);
        if(GetInvulnerable() == false)
        charAnim.Shoot();
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

    public void Idling()
    {
        charAnim.Idle();
        shooter.ShootIdle();
    }

    public void MoveAnim()
    {
        charAnim.Move();
        shooter.ShootMove();
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

    public bool GetHit(Transform byWhere, int dmg)
    {
        float trueDamage = dmg - (baseAttribute.armor);
        //trueDamage = trueDamage + ((trueDamage * baseAttribute.damageMultiplier) / 10);
        hp -= (int)trueDamage;
        
        if(hp <= 0)
        {
            characterUI.UpdateSlider(0);
            Dead();
            //GameplayController.instance.AddStatistic(StatisticID.deathTimes, 1);
            return true;
        }
        characterUI.UpdateSlider(hp);
        Vector3 throwDirection = (mover.GetMover().position - byWhere.position);
        throwDirection.y = 0;
        throwDirection.Normalize();
        mover.Throw(throwDirection);
        GetHitAnim();
        rotator.ForceLookAt(byWhere);
        ShakeCamera(3.0f, .250f, 1.0f);
        sound.Play(0);
        //GameplayController.instance.PlayerDamaged(dmg);
        //Invulnerable();
        //GameplayController.instance.AddStatistic(StatisticID.damageTaken, dmg);
        

        
        return false;
    }

   

    public void Dead()
    {
        sound.Play(1);
        PauseAnim();
        ShakeCamera(10.0f, .0f, 1.0f);
        deadEffect.Play();
        charGraphic.DisableGraphic();
        hitBox.enabled = false;
        mover.DisableMovement();
        GameplayController.instance.PlayerDead();
    }

    public void GetHitAnim()
    {
        rotator.UnRotating();
        charAnim.GetHit();
    }

    public void NormalizeAnim()
    {
        charAnim.Idle();
        rotator.Rotating();
    }

    public Transform GetMover()
    {
        return mover.GetMover();
    }

    public void ApplyStat(List<int> values)
    {
        baseAttribute.AddStat(values);
        mover.RefreshSpeed();
        charSensor.RefreshSensor();
        this.charAnim.RefreshSpeed();
        this.hp = baseAttribute.HP;
        this.characterUI.InitHPSlider(this.hp);
        //mover.SetSpeed(values[(int)ChoiceMemberID.Movement_Speed]);

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
        ShowBarrier();
        this.invulnerable = true;
    }

    public void UnInvulnerable()
    {
        HideBarrier();
        this.invulnerable = false;
    }

    public void ShowBarrier()
    {
        LeanTween.cancel(barrierScaler.gameObject);
        LeanTween.cancel(barrier);
        Color a = barrierRenderer.material.color;
        barrierRenderer.material.color = new Color(a.r,a.g,a.b,0);
        LeanTween.scale(barrierScaler.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            LeanTween.value(barrier.gameObject, .0f, 1.0f, .5f).setOnUpdate((float f)=>{
                Color a = barrierRenderer.material.color;
                barrierRenderer.material.color = new Color(a.r, a.g, a.b, f);
            }).setLoopType(LeanTweenType.pingPong).setOnComplete(()=>{
                //HideBarrier();
            });
        });
    }

    public void HideBarrier()
    {
        LeanTween.cancel(barrierScaler.gameObject);
        LeanTween.cancel(barrier.gameObject);
        LeanTween.value(barrier.gameObject, 1.0f, .0f, 1.0f).setOnUpdate((float f)=>{
            Color a = barrierRenderer.material.color;
            barrierRenderer.material.color = new Color(a.r, a.g, a.b, f);
        }).setOnComplete(()=>{
            LeanTween.scale(barrierScaler.gameObject, Vector3.zero, 0.25f).setEase(LeanTweenType.easeOutQuad);
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
        mover.EnableMovement();
    }

    public float GetDmg()
    {
        return baseAttribute.baseDamage;
    }

}
