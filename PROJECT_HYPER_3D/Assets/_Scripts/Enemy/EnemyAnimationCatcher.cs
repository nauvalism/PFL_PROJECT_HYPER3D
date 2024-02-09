using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationCatcher : MonoBehaviour
{
    [SerializeField] BaseEnemy root;
    [SerializeField] Animator anim;
    
    public void Idle()
    {
        anim.Play("IDLE");
    }

    public void Move()
    {
        anim.Play("WALK");
    }

    public void Attack()
    {
        anim.Play("ATTACK");
    }

    public void DoAttack()
    {
        
    }

    public void Pause()
    {
        anim.enabled = false;
    }

    public void Resume()
    {
        anim.enabled = true;
    }
}
