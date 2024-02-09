using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimatorCatcher : MonoBehaviour
{
    [SerializeField] Character _char;
    [SerializeField] Animator anim;
    [SerializeField] BaseWeapon currentWeapon;
    [SerializeField] SoundManager shootSound;
    [SerializeField] SoundManager footSound;
    
    

    public void DoShoot()
    {
        //return;
        _char.DoShoot();
        //Debug.Log("Shoot");
    }

    public void ChangeSpeed(float to)
    {
        anim.SetFloat("SpeedModifier", to);
    }

    public void AddSpeed(float by)
    {
        float t = anim.GetFloat("SpeedModifier");
        t += by;
        anim.SetFloat("SpeedModifier", t);
    }

    public void ResetSpeed()
    {
        anim.SetFloat("SpeedModifier",1);
    }

    public void Idle()
    {
        anim.Play("IDLE",0);
    }

    public void Move()
    {
        anim.Play("WALK");
    }

    public void Shoot()
    {
        anim.Play("SHOOT", 1);
    }

    public void Pause()
    {
        anim.enabled = false;
    }

    public void Resume()
    {
        anim.enabled = true;
    }

    public void PlayShootSound()
    {
        shootSound.Play(0);
    }

    public void PlayFootSound()
    {
        shootSound.Play(0);
    }
}
