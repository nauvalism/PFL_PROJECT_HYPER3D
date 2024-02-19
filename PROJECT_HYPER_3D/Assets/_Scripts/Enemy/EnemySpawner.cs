using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance {get;set;}


    [SerializeField] SphereCollider radius;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<BaseEnemy> spawnedEnemy;
    [SerializeField] Transform enemyParent;
    [SerializeField] int currentWave = 0;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SpawnEnemies(10);
        }
    }

    public void SpawnEnemies(int no)
    {
        for(int i = 0 ; i < no ; i++)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemies()
    {
        EnemyCountPair data = WaveManager.instance.GetWaveData();
        StartCoroutine(SpawningEnemies());
        IEnumerator SpawningEnemies()
        {
            for(int i = 0 ; i < data.enemyID.Count; i++)
            {
                yield return new WaitForSeconds(data.when[i]);
                int howMuch = data.enemyCount[i];
                for(int j = 0 ; j < howMuch ; j++)
                {
                    SpawnEnemy(data.enemyID[i]);
                }
                
            }
        }
    }

    public void SpawnEnemy()
    {
        Vector2 spawnLocation2 = Random.insideUnitCircle * radius.radius;
        Vector3 spawnLocation = new Vector3(spawnLocation2.x, 0, spawnLocation2.y);

        GameObject go;
        go = (GameObject)Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        go.transform.SetParent(enemyParent,true);
        BaseEnemy be = go.GetComponent<BaseEnemy>();
        spawnedEnemy.Add(be);
    }

    public void SpawnEnemy(int id)
    {
        
        Vector2 spawnLocation2 = Random.insideUnitCircle * radius.radius;
        Vector3 spawnLocation = new Vector3(spawnLocation2.x, 0, spawnLocation2.y);
        if(Mathf.Abs(spawnLocation.x) < 3.0f)
            spawnLocation.x += 5.0f;
        
        if(Mathf.Abs(spawnLocation.z) < 3.0f)
            spawnLocation.z += 5.0f;
        
        GameObject go;
        string _id = id.ToString("00");
        GameObject what = Resources.Load<GameObject>("Prefabs/Enemies/Enemy_"+id);
        go = (GameObject)Instantiate(what, spawnLocation, Quaternion.identity);
        go.transform.SetParent(enemyParent,true);
        BaseEnemy be = go.GetComponent<BaseEnemy>();
        spawnedEnemy.Add(be);
    }

    public void RemoveAllEnemies()
    {
        StopCoroutine("SpawningEnemies");
        for(int i = 0 ; i < spawnedEnemy.Count ; i++)
        {
            spawnedEnemy[i].Remove();
        }

        spawnedEnemy = new List<BaseEnemy>();
    }

    public void KillEnemy(BaseEnemy e)
    {
        spawnedEnemy.Remove(e);
    }

    public void AddWave()
    {
        GameplayController.instance.AddStatistic(StatisticID.wave, 1);
    }

    public void ResetScript()
    {

    }

    public void PauseAll()
    {
        for(int i = 0 ; i < spawnedEnemy.Count ; i++)
        {
            try
            {
                spawnedEnemy[i].PauseAll();
            }catch(System.Exception e)
            {

            }
            
        }
    }

    public void ResumeAll()
    {
        for(int i = 0 ; i < spawnedEnemy.Count ; i++)
        {
            try
            {
                spawnedEnemy[i].ResumeAll();
            }catch(System.Exception e)
            {

            }
        }
    }
}
