using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] float baseDmg = 15;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] SoundManager sound;
    [SerializeField] SoundManager flyBy;

    public GameObject GetBullet(bool withSound = true)
    {
        sound.Play(0);
        if(Random.Range(0,2) == 0)
        {
            flyBy.Play(0);
        }
        return bulletPrefab;
    }

    public float GetDmg()
    {
        return baseDmg;
    }
}
