using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionCatcher : MonoBehaviour
{
    [SerializeField] BaseEnemy root;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Bullet"))
        {
            int dmg = other.gameObject.transform.GetComponent<BulletPhysicsCatcher>().GetDmg();
            root.GetHit(dmg);
        }
    }

    public void Attacked()
    {

    }
}
