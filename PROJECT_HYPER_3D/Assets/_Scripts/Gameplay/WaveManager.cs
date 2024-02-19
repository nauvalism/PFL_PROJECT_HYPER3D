using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance {set;get;}
    [SerializeField] int currentWave = 0;
    [SerializeField] WaveIdenity waveIdentity;

    private void Awake() {
        instance = this;
    }

    private void OnValidate() {
        waveIdentity.RefreshTotalEnemy();
    }

    public void ResetScript()
    {
        currentWave = 0;
        waveIdentity.ReCount(currentWave);

    }

    public void AddWaveOnly()
    {
        //Debug.Log("Current Wave Adding Only");
        this.currentWave += 1;
    }

    public void AddWave()
    {
        this.currentWave += 1;
        waveIdentity.ReCount(currentWave);
    }



    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetEnemyKilled()
    {
        return waveIdentity.enemyKilledInThisWave;
    }

    public int GetTotalEnemyKilled()
    {
        return waveIdentity.totalEnemyKilled;
    }

    public bool EnemyKilled()
    {
        return waveIdentity.EnemyKilled(currentWave);
    }

    public EnemyCountPair GetWaveData()
    {
        return waveIdentity.GetWaveData(currentWave);
    }

    public EnemyCountPair GetNextWaveData()
    {
        int id = currentWave + 1;
        return waveIdentity.GetWaveData(id);
    }

    public bool IsEnding()
    {
        //Debug.Log("Current Wave : "+currentWave+"--"+waveIdentity.waveData.Count);
        if(currentWave >= waveIdentity.waveData.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}


[System.Serializable]
public class WaveIdenity
{
    public int enemyInThisWave;
    public int enemyKilledInThisWave;
    public int totalEnemyKilled;
    
    public List<EnemyCountPair> waveData;

    public void RefreshTotalEnemy()
    {
        for(int i = 0 ; i < waveData.Count ; i++)
        {
            waveData[i].RefreshTotalEnemy();
        }
    }

    public void ReCount(int waveID)
    {
        enemyInThisWave = waveData[waveID].totalEnemy;
        enemyKilledInThisWave = 0;
    }

    public int WaveTotalEnemy(int waveID)
    {
        enemyInThisWave = waveData[waveID].totalEnemy;
        return enemyInThisWave;
    }

    public bool EnemyKilled(int waveID)
    {
        ++enemyKilledInThisWave;
        ++totalEnemyKilled;
        return waveData[waveID].EnemyKilled(enemyKilledInThisWave);
    }

    public int GetTotalEnemyKilled()
    {
        return totalEnemyKilled;
    }

    public EnemyCountPair GetWaveData(int waveID)
    {
        //Debug.Log(waveID+"-"+waveData.Count);
        if(waveID >= waveData.Count)
        {
            return null;
        }
        else
        {
            return waveData[waveID];
        }
        
    }
}

[System.Serializable]
public class EnemyCountPair
{
    public int totalEnemy;
    public int enemyKilledInThis;
    public List<int> enemyID;
    public List<int> enemyCount;
    public List<float> when;

    public void RefreshTotalEnemy()
    {
        GetTotalEnemy();
    }

    public int GetTotalEnemy()
    {
        totalEnemy = 0;
        for(int i = 0 ; i < enemyCount.Count ; i++)
        {
            totalEnemy += enemyCount[i];
        }

        return totalEnemy;

    }

    public bool EnemyKilled(int total)
    {
        return (total == totalEnemy);
    }


}