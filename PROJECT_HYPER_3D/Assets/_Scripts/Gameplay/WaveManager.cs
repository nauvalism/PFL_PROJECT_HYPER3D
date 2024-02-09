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

    public void ResetScript()
    {
        currentWave = 0;
        waveIdentity.ReCount(currentWave);

    }

    public bool EnemyKilled()
    {
        return waveIdentity.EnemyKilled(currentWave);
    }

    public EnemyCountPair GetWaveData()
    {
        return waveIdentity.GetWaveData(currentWave);
    }
    
}


[System.Serializable]
public class WaveIdenity
{
    public int enemyInThisWave;
    public int enemyKilledInThisWave;
    
    public List<EnemyCountPair> waveData;

    public void ReCount(int waveID)
    {
        enemyInThisWave = waveData[waveID].totalEnemy;
    }

    public int WaveTotalEnemy(int waveID)
    {
        enemyInThisWave = waveData[waveID].totalEnemy;
        return enemyInThisWave;
    }

    public bool EnemyKilled(int waveID)
    {
        ++enemyKilledInThisWave;
        return waveData[waveID].EnemyKilled(enemyKilledInThisWave);
    }

    public EnemyCountPair GetWaveData(int waveID)
    {
        return waveData[waveID];
    }
}

[System.Serializable]
public class EnemyCountPair
{
    public int totalEnemy;
    public List<int> enemyID;
    public List<int> enemyCount;
    public List<float> when;


    public int GetTotalEnemy()
    {
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