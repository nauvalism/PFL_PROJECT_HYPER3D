using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimatorCatcher : MonoBehaviour
{
    [SerializeField] Character _char;
    
    
    public void DoShoot()
    {
        _char.DoShoot();
        //Debug.Log("Shoot");
    }
}
