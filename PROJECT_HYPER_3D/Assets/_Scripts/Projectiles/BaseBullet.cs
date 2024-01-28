using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] float speed = 100;
    [SerializeField] Vector3 dir;

    private void Start() {
        //rb.AddForce(new Vector3(0,0,1) * 500);
    }

    public void IgnoreLayer(Collider _col)
    {
        Physics.IgnoreCollision(col, _col, true);
    }

    public void LaunchBullet(Vector3 dir)
    {
        
        this.dir = dir;
        rb.AddForce(dir * speed);
    }
}
