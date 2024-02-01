using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("HP Decrases")]
    [SerializeField] List<Transform> hpDecreasePlace;
    [SerializeField] GameObject dmgTextPrefab;
    [SerializeField] Transform dmgTxtParent;
    [Header("Main Attribute")]
    [SerializeField] Collider col;
    
    [SerializeField] EnemyAttribute attribute;
    [SerializeField] GraphicActor graphic;
    [SerializeField] Transform target;
    [SerializeField] Transform mover;
    [SerializeField] Transform face;
    [SerializeField] float speed = 2;
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] List<ParticleSystem> projectileDeathEffect;
    [SerializeField] bool enableAI = true;

    [Header("Drops")]
    [SerializeField] DropList dropList;

    [Header("UI")]
    [SerializeField] CUIManager ui;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        ui.InitHPSlider(attribute.HP);
        target = FindObjectOfType<CharacterMovement>().GetMover();
    }

    

    protected virtual void FixedUpdate() {



        if(enableAI == true)
        {
            Vector3 dir = target.position - mover.position;
            dir.Normalize();
            mover.Translate(dir * (attribute.speed/10));
        }
        face.LookAt(target);

        
        
    }

    public void GetHit(int dmg = 10)
    {
        graphic.GetHit();
        DisplayHPDecrease(dmg);
        bool result = DecreaseHP(dmg);
        if(result)
        {
            
            Dead();
        }
    }

    public void DisplayHPDecrease(int dmg)
    {
        Debug.Log("Displaying HP Decrease");
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
        col.enabled = false;
        deathEffect.Play();
        graphic.DisableGraphic();
        enableAI = false;
        Destroy(gameObject, 2.0f);
    }
}



[System.Serializable]
public class DropList
{
    public List<CurrencyEnum> cur;
    public List<int> curNum;
    public List<GameObject> curPrefabs;
}