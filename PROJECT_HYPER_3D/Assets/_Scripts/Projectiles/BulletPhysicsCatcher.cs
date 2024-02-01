using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysicsCatcher : MonoBehaviour
{
    [SerializeField] BaseBullet core;
    

    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            core.StopBullet();
            //Debug.Log("Kena Enemy"+ other.gameObject.name);
        }
    }

    public int GetDmg()
    {
        return core.GetDamage();
    }
}
