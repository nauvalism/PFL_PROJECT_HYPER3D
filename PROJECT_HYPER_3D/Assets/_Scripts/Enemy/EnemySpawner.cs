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

    public void SpawnEnemy()
    {
        Vector2 spawnLocation2 = Random.insideUnitCircle * radius.radius;
        Vector3 spawnLocation = new Vector3(spawnLocation2.x, 0, spawnLocation2.y);

        GameObject go;
        go = (GameObject)Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        go.transform.SetParent(enemyParent,true);
    }

    public void KillEnemy(BaseEnemy e)
    {

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
