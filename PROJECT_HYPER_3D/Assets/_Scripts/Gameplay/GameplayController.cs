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

    [SerializeField] MainCharacterAttribute defaultCharAttribute;
    [SerializeField] Character mainChar;
    [SerializeField] EXP charExp;
    [SerializeField] TextAsset raw;
    [SerializeField] List<MainCharacterAttribute> expAttributes;
    [SerializeField] BattleStatistics battle;
    [SerializeField] UIManager ui;
    [SerializeField] GameState state;
    [SerializeField] float invulnerableDuration = 2;
    [SerializeField] float startWaitTime = 3;
    [SerializeField] float sessionSecond = 0;
    
    [SerializeField] StatisticsValue sValue;
    [SerializeField] SoundManager generalUISound;
    [SerializeField] MusicManager music;

    private void OnValidate() {
        if(charExp != null)
        {
            if(charExp.levelTxts != null)
            {
                charExp.ProcessLevelTxt();
            }
        }

        if(raw != null)
        {
            ProcessAttributes();
        }
        
    }

    public void ProcessAttributes()
    {
        expAttributes = new List<MainCharacterAttribute>();
        var pro = JSON.Parse(raw.text);
        
        var data = pro["data"];
        for(int i = 0 ; i < data.Count ; i++)
        {
            MainCharacterAttribute attr = new MainCharacterAttribute();
            List<int> val = new List<int>();
            var datai = data[i];
            for(int j = 0 ; j < 8 ; j++)
            {
                int _v = datai[j];
                val.Add(_v);
            }
            attr.SetStat(val);
            expAttributes.Add(attr);
        }
    }

    public void ApplyPowerupStat(int lvl)
    {
        MainCharacterAttribute attr = expAttributes[lvl];
        mainChar.GetAttribute().AddStat(attr);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.B))
        {
            StartGameCountDown();
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            InstaDead();
        }


        if(state == GameState.MidGame)
        {
            sValue.AddValue(StatisticID.timeSurvive, Time.deltaTime);
            sessionSecond += Time.deltaTime;
        }


    }

    public void InstaDead()
    {
        GameObject go = new GameObject();
        go.transform.position = new Vector3(.0f, .0f, 1.0f);
        bool dead = mainChar.GetHit(go.transform, 100);
        if(dead)
        {
            AddStatistic(StatisticID.deathTimes,1);
            string tSpent;
            System.TimeSpan t = System.TimeSpan.FromSeconds(sessionSecond);
            tSpent = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            SetStatistic(StatisticID.timeSurvive, sessionSecond);
            SyncValuestoUI();
            ui.HideVignette();
        }
    }

    private void Start() {
        //mainChar = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        //ResetStatistics();
        //ResetExp();
        ResetAll();
       
    }

    public void CharSetup()
    {
        mainChar.Setup();
    }

    public void ResetAll()
    {
        AnalogManager.instance.DisableAnalog();
        WaveManager.instance.ResetScript();
        mainChar.ResetScript();
        EnemySpawner.instance.ResetScript();
        ChoiceManager.instance.ResetChoices();
        ResetExp();
        ResetStatistics();
    }

    public void StartGameCountDown()
    {
        
        StartCoroutine(StartGame());
        IEnumerator StartGame()
        {
            ResetAll();
            yield return new WaitForSeconds(startWaitTime);
            ui.StartCountdownWave(()=>{
                AnalogManager.instance.ResetAnalog();
                ResumeEnemyAndCharacter();
                SetState(GameState.MidGame);
                SpawnWave();
                music.Play(0);
            });
            
            
        }
    }

    public void SpawnWave()
    {
        
        EnemySpawner.instance.SpawnEnemies();
    }



    public void AddWave(bool addWaveOnly)
    {
        int _wave = WaveManager.instance.GetCurrentWave();
        int _dWave = _wave + 1;
        ui.WaveComplete(_dWave, ()=>{
            if(addWaveOnly)
            {
                WaveManager.instance.AddWaveOnly();
            }
            else
            {
                WaveManager.instance.AddWave();
            }
            
        });
    }

    public void AddWave(bool addWaveOnly, System.Action after)
    {
        int _wave = WaveManager.instance.GetCurrentWave();
        int _dWave = _wave + 1;
        ui.WaveComplete(_dWave, ()=>{
            if(addWaveOnly)
            {
                WaveManager.instance.AddWaveOnly();
            }
            else
            {
                WaveManager.instance.AddWave();
            }
            
            if(after != null)
            {
                after();
            }
        });
    }

    public void CompleteWave()
    {
        PauseEnemyAndCharacter();
        generalUISound.Play(0);
        
        Flash(()=>{
            EnemyCountPair waveData = WaveManager.instance.GetNextWaveData();
            
            if(waveData != null)
            {
                AddWave(false, ()=>{
                    ProcessNextWave();
                });
            }
            else
            {
                AddWave(true, ()=>{
                    ProcessNextWave();
                });
            }
            
        },()=>{
            // bool isEnding = WaveManager.instance.IsEnding();
            // Debug.Log(isEnding);
            // if(isEnding)
            // {
            //     //EnemySpawner.instance.RemoveAllEnemies();
            //     GameEnds();
            //     PlayerDead();
                
            // }
            // else
            // {
            //     ui.StartCountdownWave(()=>{
            //         ResumeEnemyAndCharacter();
            //         EnemySpawner.instance.SpawnEnemies();
            //     });
            // }


            

        });
    }

    
    public void ProcessNextWave()
    {
        bool isEnding = WaveManager.instance.IsEnding();
        Debug.Log(isEnding);
        if(isEnding)
        {
            //EnemySpawner.instance.RemoveAllEnemies();
            GameEnds();
            PlayerDead();
            
        }
        else
        {
            ui.StartCountdownWave(()=>{
                ResumeEnemyAndCharacter();
                EnemySpawner.instance.SpawnEnemies();
            });
        }
    }

    public void ResetExp()
    {
        charExp.ResetExp();
        ui.SetExp(charExp.maxExp[0], charExp.currentExp);
    }

    public void ResetStatistics()
    {
        battle = new BattleStatistics();
        sValue = new StatisticsValue();
    }

    public void KillEnemy()
    {
        battle.AddEnemyKilled();
        
        AddStatistic(StatisticID.zombieKilled,1);
        bool complete = WaveManager.instance.EnemyKilled();
        ui.UpdateEnemyKilled(WaveManager.instance.GetTotalEnemyKilled());
        if(complete)
        {
            CompleteWave();
        }
    }



    public void DealDamage(int dmg)
    {
        battle.DealDamage(dmg);
        AddStatistic(StatisticID.damageDealt,dmg);
    }

    

    public void PlayerDamaged(BaseEnemy by, int dmg)
    {
        bool dead = mainChar.GetHit(by.GetMover(), dmg);
        AddStatistic(StatisticID.damageTaken,dmg);
        if(dead)
        {
            AddStatistic(StatisticID.deathTimes,1);
            //ui.SyncGameOverStats(sValue.statisticValue);
            SyncValuestoUI();
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

    public void GameEnds()
    {
        mainChar.ClearGame();
    }

    public void PlayerDead()
    {
        EnemySpawner.instance.RemoveAllEnemies();
        StartCoroutine(ShowingGameOver());
        IEnumerator ShowingGameOver()
        {
            music.Stop();
            yield return new WaitForSeconds(1.0f);
            ui.ShowGameOver();
        }
        
    }

    public void AddExp(int exp)
    {
        bool levelup = charExp.AddExp(exp);
        if(levelup)
        {
            int lvl = charExp.currentLevel;
            ApplyPowerupStat(lvl);
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
        AnalogManager.instance.DisableAnalog();
        PauseEnemyAndCharacter();
        SetState(GameState.LevelUp);
        Flash(()=>{
            mainChar.DoLevelUp();
        },()=>{
            ChoiceManager.instance.ShowChoices(false);
        });
        
        
        
       
        
    }

    public void HidePowerup()
    {
        SetState(GameState.MidGame);
        ResumeEnemyAndCharacter();
        AnalogManager.instance.ResetAnalog();
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

    public void Flash(System.Action during, System.Action after, bool withSound = true)
    {
        if(withSound)
            generalUISound.Play(0);
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

    public void SyncValuestoUI()
    {
        ui.SyncGameOverStats(sValue.statisticValue);
    }

    public void SyncBasicStats(MainCharacterAttribute attr)
    {
        sValue.SetValue(attr);
    }

    public float GetSessionSecond()
    {
        return sessionSecond;
    }

    public  MainCharacterAttribute GetDefaultAttribute()
    {
        return defaultCharAttribute;
    }

    public void ShowOpeningUI()
    {
        ui.ShowOpening();
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

    public virtual void SetStat(List<int> _val)
    {

    }

    public virtual void SetStat(MainCharacterAttribute attr)
    {

    }

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

    public virtual void ResetStat()
    {

    }

    public virtual void ResetStat(CharacterAttribute reference)
    {

    }

    public virtual void ResetStat(MainCharacterAttribute reference)
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

    public override void SetStat(List<int> values)
    {
        this.HP = values[(int)ChoiceMemberID.Max_Health_Points];
        this.speed = values[(int)ChoiceMemberID.Movement_Speed];
        this.baseDamage = values[(int)ChoiceMemberID.Base_Damage];
        this.armor = values[(int)ChoiceMemberID.Body_Armor];
        this.damageMultiplier = values[(int)ChoiceMemberID.Damage_Multiplier];
        this.baseAspd = values[(int)ChoiceMemberID.Attack_Speed];
        this.range = values[(int)ChoiceMemberID.Shoot_Range];
        this.pickupRadius = values[(int)ChoiceMemberID.Pickup_Radius];
    }

    public override void SetStat(MainCharacterAttribute stat)
    {
        this.HP = stat.HP;
        this.speed = stat.speed;
        this.baseDamage = stat.baseDamage;
        this.armor = stat.armor;
        this.damageMultiplier = stat.damageMultiplier;
        this.baseAspd = stat.baseAspd;
        this.range = stat.range;
        this.pickupRadius = stat.pickupRadius;
    }

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

    public override void ResetStat()
    {
        MainCharacterAttribute reference = GameplayController.instance.GetDefaultAttribute();
        ResetStat(reference);
    }

    public override void ResetStat(MainCharacterAttribute reference)
    {
        base.ResetStat(reference);
        this.HP = reference.HP;
        this.speed = reference.speed;
        this.baseDamage = reference.baseDamage;
        this.armor = reference.armor;
        this.damageMultiplier = reference.damageMultiplier;
        this.baseAspd = reference.baseAspd;
        this.range = reference.range;
        this.pickupRadius = reference.pickupRadius;
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
