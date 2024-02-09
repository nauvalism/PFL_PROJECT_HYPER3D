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
    Max_Health_Points = 0,
    Movement_Speed  = 1,
    Base_Damage = 2,
    Body_Armor = 3,
    Damage_Multiplier = 4,
    Attack_Speed = 5,
    Shoot_Range = 6,
    Pickup_Radius = 7

}

public enum StatisticID
{
    timeSurvive = 0,
    zombieKilled = 1,
    damageDealt = 2,
    damageTaken = 3,
    deathTimes = 4,
    level = 5,
    stage = 6,
    wave = 7,
    currency_1 = 8,
    currency_2 = 9,
    currency_3 = 10
}

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance {get;set;}

    private void Awake() {
        instance = this;
    }


    [SerializeField] Character mainChar;
    [SerializeField] EXP charExp;
    [SerializeField] BattleStatistics battle;
    [SerializeField] UIManager ui;
    [SerializeField] GameState state;
    [SerializeField] float invulnerableDuration = 2;
    [SerializeField] float startWaitTime = 3;
    
    
    [SerializeField] StatisticsValue sValue;
    [SerializeField] MusicManager music;

    private void OnValidate() {
        if(charExp != null)
        {
            if(charExp.levelTxts != null)
            {
                charExp.ProcessLevelTxt();
            }
        }

        
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.B))
        {
            mainChar.DoLevelUp();
        }


        if(state == GameState.MidGame)
        {
            sValue.AddValue(StatisticID.timeSurvive, Time.deltaTime);
        }
    }

    private void Start() {
        mainChar = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        ResetStatistics();
        ResetExp();
        ui.SetExp(charExp.maxExp[0], charExp.currentExp);
       
    }

    public void StartGameCountDown()
    {
        StartCoroutine(StartGame());
        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(startWaitTime);

        }
    }

    public void ResetAll()
    {
        WaveManager.instance.ResetScript();
        mainChar.ResetScript();
        EnemySpawner.instance.ResetScript();
        ChoiceManager.instance.ResetChoices();
    }


    public void ResetExp()
    {
        charExp.ResetExp();
    }

    public void ResetStatistics()
    {
        battle = new BattleStatistics();
        sValue = new StatisticsValue();
    }

    public void KillEnemy()
    {
        battle.AddEnemyKilled();
    }

    public void DealDamage(int dmg)
    {
        battle.DealDamage(dmg);
    }

    public void PlayerDamaged(int dmg)
    {
        bool dead = mainChar.GetHit(dmg);
        if(dead)
        {
            ui.HideVignette();
        }
        else
        {
            StartCoroutine(PlayerDamaging());
        }

        IEnumerator PlayerDamaging()
        {
            mainChar.Invulnerable();
            ui.ShowVignette(Color.red, invulnerableDuration);
            yield return new WaitForSeconds(invulnerableDuration);
            ui.HideVignette();
            mainChar.UnInvulnerable();
        }
        
    }

    public void AddExp(int exp)
    {
        bool levelup = charExp.AddExp(exp);
        if(levelup)
        {
            
            ShowPowerup();
        }
        float cur = this.charExp.currentExp;
        float max = this.charExp.GetMaxExp();
        ui.SetExp(max,cur,charExp.currentLevel, true);
    }

    public void PauseEnemyAndCharacter()
    {
        mainChar.PauseAnim();
        EnemySpawner.instance.PauseAll();
    }

    public void ResumeEnemyAndCharacter()
    {
        mainChar.ResumeAnim();
        EnemySpawner.instance.ResumeAll();
    }

    public void ShowPowerup()
    {
        ChoiceManager.instance.ShowChoices(false);
    }

    public void SetState(GameState state)
    {
        this.state = state;
    }

    public GameState GetState()
    {
        return state;
    }

    public void ApplyStat(ChoiceAttribute attribute)
    {
        List<int> toBeAdded = attribute.values;
        mainChar.ApplyStat(toBeAdded);
    }

    public void Flash(System.Action during, System.Action after)
    {
        ui.Flash(during,after);
    }

    public void SyncStatistics()
    {
        
    }

    public void SetStatistic(StatisticID what, float value)
    {
        sValue.SetValue(what, value);
    }

    public void AddStatistic(StatisticID what, float value)
    {
        sValue.AddValue(what,value);
    }

    public void SyncBasicStats(MainCharacterAttribute attr)
    {
        sValue.SetValue(attr);
    }
}

[System.Serializable]
public class StatisticsValue
{
    public List<float> statisticValue;
    public List<float> baseStatValues;

    public StatisticsValue()
    {
        statisticValue = new List<float>();
        baseStatValues = new List<float>();

        for(int i = 0 ; i < System.Enum.GetNames(typeof(StatisticID)).Length ; i++)
        {
            statisticValue.Add(0);
        }

        for(int i = 0 ; i < System.Enum.GetNames(typeof(ChoiceMemberID)).Length ; i++)
        {
            baseStatValues.Add(0);
        }
        
    }

    public void AddValue(StatisticID id, float value)
    {
        statisticValue[(int)id] += value;
    }

    public void SetValue(StatisticID id, float value)
    {
        statisticValue[(int)id] = value;
    }

    public void SetValue(MainCharacterAttribute attr)
    {
        baseStatValues[(int)ChoiceMemberID.Base_Damage] = attr.baseDamage;
        baseStatValues[(int)ChoiceMemberID.Attack_Speed] = attr.baseAspd;
        baseStatValues[(int)ChoiceMemberID.Damage_Multiplier] = attr.damageMultiplier;
        baseStatValues[(int)ChoiceMemberID.Body_Armor] = attr.armor;
        baseStatValues[(int)ChoiceMemberID.Max_Health_Points] = attr.HP;
        baseStatValues[(int)ChoiceMemberID.Movement_Speed] = attr.speed;
        baseStatValues[(int)ChoiceMemberID.Pickup_Radius] = attr.pickupRadius;
        baseStatValues[(int)ChoiceMemberID.Shoot_Range] = attr.range;
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
    public float armor = 10;
    public float damageMultiplier = 1;

    public virtual void AddStat(CharacterAttribute stat)
    {
        this.HP += stat.HP;
        this.speed += stat.speed;
        this.baseDamage += stat.baseDamage;
        this.armor += stat.armor;
        this.damageMultiplier += stat.damageMultiplier;
    }

    public virtual void AddStat(MainCharacterAttribute stat)
    {

    }

    public virtual void AddStat(EnemyAttribute stat)
    {

    }

    public virtual void AddStat(List<int> values)
    {

    }

    public virtual void RefreshStatToStatistic()
    {

    }

    public virtual void RefreshStatToStatistic(CharacterAttribute ca)
    {

    }

    public virtual void RefreshStatToStatistic(MainCharacterAttribute mac)
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
        this.armor += stat.armor;
        this.damageMultiplier += stat.damageMultiplier;
        this.baseAspd -= stat.baseAspd;
        this.range += stat.range;
        this.pickupRadius += stat.pickupRadius;
    }

    public override void AddStat(List<int> values)
    {
        base.AddStat(values);
        this.HP += values[(int)ChoiceMemberID.Max_Health_Points];
        this.speed += values[(int)ChoiceMemberID.Movement_Speed];
        this.baseDamage += values[(int)ChoiceMemberID.Base_Damage];
        this.armor += values[(int)ChoiceMemberID.Body_Armor];
        this.damageMultiplier += values[(int)ChoiceMemberID.Damage_Multiplier];
        this.baseAspd += values[(int)ChoiceMemberID.Attack_Speed];
        this.range += values[(int)ChoiceMemberID.Shoot_Range];
        this.pickupRadius += values[(int)ChoiceMemberID.Pickup_Radius];
    }

    public override void RefreshStatToStatistic()
    {

    }

    public override void RefreshStatToStatistic(CharacterAttribute ca)
    {

    }

    public override void RefreshStatToStatistic(MainCharacterAttribute mac)
    {
        
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
        this.armor += stat.armor;
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

    public void ResetExp()
    {
        currentExp = 0;
        currentLevel = 0;
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
        if(tempNextExp >= maxExp[currentLevel])
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

    public int GetMaxExp(int lvl)
    {
        return maxExp[lvl];
    }

    public int GetMaxExp()
    {
        return maxExp[currentLevel];
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
