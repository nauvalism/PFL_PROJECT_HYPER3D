using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysicsCatcher : MonoBehaviour
{
    [SerializeField] BaseBullet core;
    

    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy"))
        {
            
        }
    }
}
