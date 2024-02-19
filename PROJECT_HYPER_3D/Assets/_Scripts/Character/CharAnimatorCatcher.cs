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
    
    private void Update() {
        transform.localPosition = Vector3.zero;
    }

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
        Shoot();
    }

    public void Move()
    {
        if(isPlaying("WALK") == false)
        {
            anim.Play("WALK",0);
        }
        
    }

    public void Shoot()
    {
        anim.Play("SHOOT", 1);
    }

    public void GetHit()
    {
        anim.Play("HIT",0);
        anim.Play("HIT",1);
    }

    public void Normal()
    {
        Move();
        Shoot();
    }

    bool isPlaying(string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
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

    public void RefreshSpeed()
    {
        float to = 1 +(_char.GetAttribute().baseAspd/30);
        anim.SetFloat("SpeedModifier", to);
    }
}
