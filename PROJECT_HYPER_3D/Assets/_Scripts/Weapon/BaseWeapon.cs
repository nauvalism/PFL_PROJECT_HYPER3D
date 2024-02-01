using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] SoundManager sound;

    public GameObject GetBullet(bool withSound = true)
    {
        sound.Play(0);
        return bulletPrefab;
    }
}
