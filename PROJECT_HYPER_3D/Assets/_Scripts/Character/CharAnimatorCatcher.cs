using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimatorCatcher : MonoBehaviour
{
    [SerializeField] Character _char;
    [SerializeField] Animator anim;
    
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
}
