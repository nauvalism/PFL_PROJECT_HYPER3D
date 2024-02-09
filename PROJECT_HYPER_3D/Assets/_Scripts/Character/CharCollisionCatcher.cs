using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCollisionCatcher : MonoBehaviour
{
    [SerializeField] Character main;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(other.gameObject);
            BaseEnemy be = other.gameObject.transform.parent.parent.GetComponent<BaseEnemy>();
            GameplayController.instance.PlayerDamaged(be.GetHitDmg());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            BaseEnemy be = other.gameObject.transform.parent.parent.parent.GetComponent<BaseEnemy>();
            GameplayController.instance.PlayerDamaged(be.GetHitDmg());
        }
    }
}
