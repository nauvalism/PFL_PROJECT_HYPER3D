using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooter : MonoBehaviour
{
    [SerializeField] Character rootChar;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Vector3 dir;
    [SerializeField] Transform bulletSpawnPlace;
    [SerializeField] ParticleSystem spawnParticle;
    [SerializeField] Transform reference;
    [SerializeField] Vector3 shootDirection;
    [SerializeField] SphereCollider shootRangeCollider;
    [SerializeField] List<Transform> shootPlaces;
    [SerializeField] List<Transform> shootReferences;

    [Header("WEAPON")]
    [SerializeField] Transform weaponParent;
    [SerializeField] BaseWeapon activeWeapon;
    [SerializeField] List<BaseWeapon> activeWeapons;

    [Header("SOUND")]
    [SerializeField] SoundManager sound;

    private void OnValidate() {
        if(weaponParent != null)
        {
            RefreshWeapon();
        }
    }

    private void Update() {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 to = reference.position;
        Gizmos.DrawLine(bulletSpawnPlace.position, to);
    }

    public void SyncShootDirection(Vector3 v)
    {
        shootDirection = v - bulletSpawnPlace.position;
    }

    public void DoShoot()
    {
        //return;
        GameObject go;
        go = (GameObject)Instantiate(activeWeapon.GetBullet(), bulletSpawnPlace.position, Quaternion.identity);
        BaseBullet bullet = go.GetComponent<BaseBullet>();

        bullet.IgnoreLayer(rootChar.GetHitBox());
        int dmg = (int)activeWeapon.GetDmg() + (int)rootChar.GetAttribute().baseDamage;


        dmg = dmg + ((dmg * (int)rootChar.GetAttribute().damageMultiplier) / 10);


        dir = (reference.position - bulletSpawnPlace.position).normalized;
        spawnParticle.Play();
        bullet.LaunchBullet(dir, reference, dmg);
        rootChar.ShakeCamera(1.0f, 0, 0, 0.5f);

    }

    public void RefreshWeapon()
    {
        activeWeapon = weaponParent.GetChild(0).GetComponent<BaseWeapon>();
        activeWeapons = new List<BaseWeapon>();
        foreach(Transform t in weaponParent)
        {
            activeWeapons.Add(t.GetComponent<BaseWeapon>());
        }
    }

    public void ShootIdle()
    {
        bulletSpawnPlace = shootPlaces[0];
        reference = shootReferences[0];
    }

    public void ShootMove()
    {
        bulletSpawnPlace = shootPlaces[1];
        reference = shootReferences[1];
    }







    public void ChangeShootRadius(float to)
    {
        shootRangeCollider.radius = to;
    }

    public void AddShootRadius(float to)
    {
        float current = shootRangeCollider.radius;
        current += to;
        shootRangeCollider.radius = current;
    }
}
