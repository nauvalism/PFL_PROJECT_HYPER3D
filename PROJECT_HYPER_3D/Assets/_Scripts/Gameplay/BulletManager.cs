using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance {get;set;}

    private void Awake() {
        instance = this;
    }

    [SerializeField] Transform bulletExplosionParent;
    [SerializeField] Transform bulletParent;

    public GameObject SpawnBullet(GameObject prefab, Vector3 where)
    {
        GameObject go;
        go = (GameObject)Instantiate(prefab, new Vector3(1000,1000,1000), Quaternion.identity, bulletParent);
        go.transform.position = where;
        return go;
    }

    public GameObject SpawnBulletEffect(GameObject prefab, Vector3 where)
    {
        GameObject go;
        go = (GameObject)Instantiate(prefab, new Vector3(1000,1000,1000), Quaternion.identity, bulletExplosionParent);
        go.transform.position = where;
        return go;
    }
}
