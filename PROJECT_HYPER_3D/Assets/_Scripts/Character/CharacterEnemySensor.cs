using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnemySensor : MonoBehaviour
{
    [SerializeField] Character coreChar;
    [SerializeField] List<GameObject> Enemies;

    private void Start() {
        Enemies = new List<GameObject>();
    }

    public void AddEnemy()
    {

    }

    public void RotateTowardsNearestEnemy()
    {

    }

    public void RegisterEnemy(GameObject g)
    {
        Enemies.Add(g);
        coreChar.AddTarget(g.transform);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy"))
        {
            RegisterEnemy(other.gameObject);
        }
        
    }

    private void OnTriggerStay(Collider other) {
        
    }

    private void OnTriggerExit(Collider other) {
        
    }
}
