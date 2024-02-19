using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCollisionCatcher : MonoBehaviour
{
    [SerializeField] Character main;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            
            Debug.Log("Player Hit "+other.gameObject);

            if(main.GetInvulnerable() == false)
            {
                BaseEnemy be = other.gameObject.transform.parent.parent.GetComponent<BaseEnemy>();
                GameplayController.instance.PlayerDamaged(be, be.GetHitDmg());
            }


            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player Hit Trigger "+other.transform.parent.gameObject.name+"--"+gameObject.name);
            BaseEnemy be = other.gameObject.transform.parent.parent.parent.GetComponent<BaseEnemy>();
            GameplayController.instance.PlayerDamaged(be, be.GetHitDmg());
        }
    }
}
