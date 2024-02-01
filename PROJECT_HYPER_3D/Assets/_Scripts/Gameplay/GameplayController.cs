using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public enum GameState
{
    Opening = 0,
    MidGame = 1,
    LevelUp = 2,
    Processing = 3,
    Ending = 4
}

public enum ChoiceMemberID
{
    HP = 0,
    peed  = 1,
    baseDamage = 2,
    defaultDamage = 3,
    damageMultiplier = 4,
    baseAspd = 5,
    range = 6,
    pickupRadius = 7

}

public class GameplayController : MonoBehaviour
{
    [SerializeField] EXP charExp;
    [SerializeField] BattleStatistics battle;
    [SerializeField] UIManager ui;
    [SerializeField] GameState state;

    private void OnValidate() {
        if(charExp != null)
        {
            if(charExp.levelTxts != null)
            {
                charExp.ProcessLevelTxt();
            }
        }
    }

    private void Start() {
        ResetStatistics();
    }

    public void ResetExp()
    {
        charExp = new EXP();
    }

    public void ResetStatistics()
    {
        battle = new BattleStatistics();
    }

    public void KillEnemy()
    {
        battle.AddEnemyKilled();
    }

    public void DealDamage(int dmg)
    {
        battle.DealDamage(dmg);
    }

    public void AddExp(int exp)
    {
        bool levelup = charExp.AddExp(exp);
        if(levelup)
        {

        }
    }

    public void ShowPowerup()
    {

    }

    public void SetState(GameState state)
    {
        this.state = state;
    }
}

[System.Serializable]
public class BattleStatistics
{
    public int enemyKilled = 0;
    public int damageDealt = 0;

    public BattleStatistics()
    {
        enemyKilled = 0;
        damageDealt = 0;
    }

    public void AddEnemyKilled()
    {
        ++enemyKilled;
    }

    public void DealDamage(int val)
    {
        damageDealt += damageDealt;
    }
}


[System.Serializable]
public class CharacterAttribute
{
    public int HP = 100;
    public float speed  = 1;
    public float baseDamage = 10;
    public float defaultDamage = 10;
    public float damageMultiplier = 1;

    public virtual void AddStat(CharacterAttribute stat)
    {
        this.HP += stat.HP;
        this.speed += stat.speed;
        this.baseDamage += stat.baseDamage;
        this.defaultDamage += stat.defaultDamage;
        this.damageMultiplier += stat.damageMultiplier;
    }

    public virtual void AddStat(MainCharacterAttribute stat)
    {

    }

    public virtual void AddStat(EnemyAttribute stat)
    {

    }

}

[System.Serializable]
public class MainCharacterAttribute : CharacterAttribute
{
    public float baseAspd = 1;
    public float range = 10;
    public float pickupRadius = 1;


    public override void AddStat(MainCharacterAttribute stat)
    {
        this.HP += stat.HP;
        this.speed += stat.speed;
        this.baseDamage += stat.baseDamage;
        this.defaultDamage += stat.defaultDamage;
        this.damageMultiplier += stat.damageMultiplier;
        this.baseAspd -= stat.baseAspd;
        this.range += stat.range;
        this.pickupRadius += stat.pickupRadius;
    }

}


[System.Serializable]
public class EnemyAttribute : CharacterAttribute
{
    public float explosionRadius = 5;
    public int enemyExp = 10;

    public override void AddStat(EnemyAttribute stat)
    {
        this.HP += stat.HP;
        this.speed += stat.speed;
        this.baseDamage += stat.baseDamage;
        this.defaultDamage += stat.defaultDamage;
        this.damageMultiplier += stat.damageMultiplier;
        this.explosionRadius += stat.explosionRadius;
    }
}

[System.Serializable]
public class BulletProfile
{
    public float bulletDmg = 10;
    public float speed = 500;
    public int bulletHealth = 1;
    public float explosionRadius = 10;
    public bool knockBack = false;
    public float critChance = 0;
}

[System.Serializable]
public class EXP
{
    public int currentExp = 0;
    public int currentLevel = 0;
    public TextAsset levelTxts;
    public List<int> maxExp;

    public EXP()
    {
        currentExp = 0;
        currentLevel = 0;
        if(levelTxts != null)
        {
            ProcessLevelTxt();
        }
    }

    public void ProcessLevelTxt()
    {
        maxExp = new List<int>();
        var pro = JSON.Parse(levelTxts.text);
        var data = pro["data"];
        for(int i = 0 ; i < data.Count ; i++)
        {
            var _data = data[i];
            int _exp = _data["maxExp"];
            maxExp.Add(_exp);
        }
    }

    public bool AddExp(int howMuch)
    {
        int tempNextExp = (currentExp + howMuch);
        if(tempNextExp > maxExp[currentLevel])
        {
            currentExp = (tempNextExp - maxExp[currentLevel]);
            ++currentLevel;
            return true;
        }
        else
        {
            currentExp = tempNextExp;
            return false;
        }
    }

    

    public bool Maxlevel(int lvl)
    {
        if(lvl >= (maxExp.Count - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
