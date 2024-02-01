using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] GameObject graphic;
    [SerializeField] ParticleSystem particle;
    [SerializeField] float speed = 100;
    [SerializeField] int dmg = 10;
    [SerializeField] Vector3 dir;

    private void Start() {
        IgnoreLayer(col);
        
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

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            StopBullet();
        }
    }

    public void StopBullet()
    {
        rb.velocity = Vector2.zero;
        graphic.SetActive(false);
        particle.Play();
        Destroy(gameObject, 2.0f);
    }

    public int GetDamage()
    {
        return dmg;
    }
}
