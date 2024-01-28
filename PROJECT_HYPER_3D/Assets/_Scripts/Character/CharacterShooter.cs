using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooter : MonoBehaviour
{
    [SerializeField] Character rootChar;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Vector3 dir;
    [SerializeField] Transform bulletSpawnPlace;
    [SerializeField] Transform reference;
    [SerializeField] Vector3 shootDirection;

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
        
        GameObject go;
        go = (GameObject)Instantiate(bulletPrefab, bulletSpawnPlace.position, Quaternion.identity);
        BaseBullet bullet = go.GetComponent<BaseBullet>();

        bullet.IgnoreLayer(rootChar.GetHitBox());
        dir = (reference.position - bulletSpawnPlace.position).normalized;
        bullet.LaunchBullet(dir);


    }
}
