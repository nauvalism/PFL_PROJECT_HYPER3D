using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterEnemySensor : MonoBehaviour
{
    [SerializeField] Character coreChar;
    [SerializeField] Collider theSensor;
    [SerializeField] List<Collider> Enemies;
    [SerializeField] List<BaseEnemy> sensoredEnemy;
    [SerializeField] List<DistanceComparerF> sensoredEnemyDistance;
    [SerializeField] float defaultRadius = 5.0f;
    [SerializeField] float radius = 5.0f;
    private void Start() {
        Enemies = new List<Collider>();
        sensoredEnemyDistance = new List<DistanceComparerF>();
    }

    public void AddEnemy()
    {

    }

    public void RotateTowardsNearestEnemy()
    {

    }

    public void ResetSensor()
    {
        radius = defaultRadius;
    }

    public void RefreshSensor()
    {
        radius = coreChar.GetAttribute().pickupRadius;
    }

    public void RegisterEnemy(Collider g, BaseEnemy e)
    {
        Enemies.Add(g);
        coreChar.AddTarget(g.transform);
        sensoredEnemy.Add(e);
        sensoredEnemyDistance.Add(e.GetDistanceComparer());
    }

    public void RemoveTarget(Collider c, BaseEnemy e)
    {
        Enemies.Remove(c);
        sensoredEnemy.Remove(e);
        coreChar.NullifyTarget();
        //coreChar.RemoveTargetOnly(c.transform);
        if(sensoredEnemy.Count > 1)
            ReSort();

        try
        {
            coreChar.AddTarget(sensoredEnemy[0].GetMover());
        }catch(System.Exception x)
        {

        }
        
    }

    public void UnRegisterEnemy(Collider c, BaseEnemy e)
    {
        Debug.Log("UnregisterEnemy");
        //Physics.IgnoreCollision(theSensor, c);
        sensoredEnemy.Remove(e);
        Enemies.Remove(c);
        coreChar.NullifyTarget();
        //coreChar.RemoveTargetOnly(c.transform);
        if(sensoredEnemy.Count > 1)
            ReSort();
            
        try
        {
            coreChar.AddTarget(sensoredEnemy[0].GetMover());
        }catch(System.Exception x)
        {

        }
        
    }

    public void ReSort()
    {

        //sensoredEnemy.OrderBy(dis => Vector3.Distance)

        try
        {
            sensoredEnemy.Sort((delegate(BaseEnemy a, BaseEnemy b)
            {return Vector3.Distance(coreChar.GetMover().position,a.GetMover().position)
            .CompareTo(
                Vector3.Distance(coreChar.GetMover().position,b.GetMover().position) );
            }));
        }catch(System.Exception e)
        {

        }
        
    }

    public bool NoEnemy()
    {
        return (sensoredEnemy.Count == 0);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log(other.name);
            RegisterEnemy(other, other.gameObject.transform.parent.parent.parent.GetComponent<BaseEnemy>());
        }
        
    }

    private void OnTriggerStay(Collider other) {
        
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Enemy"))
        {
            RemoveTarget(other,other.gameObject.transform.parent.parent.parent.GetComponent<BaseEnemy>());
        }
    }
}
