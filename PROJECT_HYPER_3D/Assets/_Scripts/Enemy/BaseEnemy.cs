using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] int ebID = -1;
    [Header("HP Decrases")]
    [SerializeField] List<Transform> hpDecreasePlace;
    [SerializeField] GameObject dmgTextPrefab;
    [SerializeField] Transform dmgTxtParent;
    [Header("Main Attribute")]
    [SerializeField] GameObject root;
    [SerializeField] Collider col;
    [SerializeField] Rigidbody rb;
    [SerializeField] EnemyAnimationCatcher anim;
    [SerializeField] EnemyAttribute attribute;
    [SerializeField] GraphicActor graphic;
    [SerializeField] Transform target;
    [SerializeField] Character targetChar;
    [SerializeField] Transform mover;
    [SerializeField] DistanceComparerF distanceCompare;
    [SerializeField] Transform face;
    [SerializeField] int hitDmg = 15;
    [SerializeField] float speed = 2;
    [SerializeField] float distanceFromTarget = 0;
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] List<ParticleSystem> projectileDeathEffect;
    [SerializeField] bool enableAI = true;

    [Header("Drops")]
    [SerializeField] DropList dropList;

    [Header("UI")]
    [SerializeField] CUIManager ui;

    [Header("Extras")]
    [SerializeField] SoundManager sound;
    [SerializeField] SoundManager dSound;

    protected virtual void Awake()
    {
        distanceCompare = new DistanceComparerF();
        distanceCompare.correspond = this;
    }

    protected virtual void Start()
    {
        ui.InitHPSlider(attribute.HP);
        target = FindObjectOfType<CharacterMovement>().GetMover();
        targetChar = FindObjectOfType<Character>();
    }

    public void Init(int id)
    {
        this.ebID = id;
    }

    protected virtual void FixedUpdate() {
        if(GameplayController.instance.GetState() == GameState.LevelUp)
        return;


        if(enableAI == true)
        {
            Vector3 dir = target.position - mover.position;
            dir.Normalize();
            mover.Translate(dir * (attribute.speed/10));
            distanceCompare.distance = Vector3.Distance(mover.position, target.position);
            distanceFromTarget = Vector3.Distance(mover.position, target.position);
        }
        face.LookAt(target);
        
        
        
    }

    public DropList GetHit(int dmg = 10)
    {
        
        graphic.GetHit();
        hitEffect.Play();
        DisplayHPDecrease(dmg);
        //GameplayController.instance.AddStatistic(StatisticID.damageDealt, dmg);
        GameplayController.instance.DealDamage(dmg);
        bool result = DecreaseHP(dmg);
        if(result)
        {
            
            Dead();
            GameplayController.instance.AddExp(dropList.curNum[0]);
            return dropList;
        }
        sound.Play(0);
        dSound.Play(0);
        return null;
    }

    public void DisplayHPDecrease(int dmg)
    {
        //Debug.Log("Displaying HP Decrease");
        Transform t = hpDecreasePlace[Random.Range(0, hpDecreasePlace.Count)];
        GameObject go;
        go = (GameObject)Instantiate(dmgTextPrefab, t.position, Quaternion.identity, dmgTxtParent);
        go.GetComponent<DamageText>().DisplayText(dmg, dmgTxtParent);
    }

    public bool DecreaseHP(int dmg)
    {
        attribute.HP -= dmg;
        if(attribute.HP <= 0)
        {
            return true;
        }

        return false;
    }

    public void Dead()
    {
        GameplayController.instance.KillEnemy();
        EnemySpawner.instance.KillEnemy(this);
        col.enabled = false;
        targetChar.RemoveEnemyTarget(this);
        deathEffect.Play();
        graphic.DisableGraphic();
        enableAI = false;
        sound.Play(1);
        dSound.Play(1);
        Destroy(gameObject, 1.0f);
    }

    public void Remove()
    {
        col.enabled = false;
        rb.isKinematic = true;
        LeanTween.scale(mover.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            Destroy(gameObject);
        });
    }

    public void PauseAll()
    {
        anim.Pause();
    }

    public void ResumeAll()
    {
        anim.Resume();
    }

    public void ScaleDownAndDestroy()
    {
        PauseAll();
        LeanTween.cancel(mover.gameObject);
        LeanTween.scale(mover.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(()=>{
            Destroy(root);
        });
    }



    public Collider GetCollider()
    {
        return col;
    }

    public DistanceComparerF GetDistanceComparer()
    {
        return distanceCompare;   
    }

    public Transform GetMover()
    {
        return mover;
    }

    public int GetHitDmg()
    {
        return (int)attribute.baseDamage;
    }
}

[System.Serializable]
public class DistanceComparer : IComparer<Transform>
{
    private Transform target;

    public DistanceComparer(Transform distanceToTarget)
    {
        target = distanceToTarget;
    }

    public int Compare(Transform a, Transform b)
    {
        var targetPosition = target.position;
        return Vector3.Distance(a.position, targetPosition).CompareTo(Vector3.Distance(b.position, targetPosition));
    }
}

public class DistCom : IComparer<float>
{
    public int Compare(float a, float b)
    { 
        return a.CompareTo(b);
    }
}

public class DistanceComparerF : IComparer<float>
{
    public BaseEnemy correspond;
    public float distance;


    public int Compare(float a, float b)
    { 
        return a.CompareTo(b);
    }
}


[System.Serializable]
public class DropList
{
    public List<CurrencyEnum> cur;
    public List<int> curNum;
    public List<GameObject> curPrefabs;
}