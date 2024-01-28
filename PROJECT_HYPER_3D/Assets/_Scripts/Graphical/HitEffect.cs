using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] Renderer render;

    private void Awake() {
        render = GetComponent<Renderer>();
    }
    // private void OnCollisionEnter(Collision other) {
    //     render.material.color = Color.red;
    // }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Bullet"))
        {
            render.material.color = Color.red;
        }
        
    }

    public void BlinkFlash()
    {
        LeanTween.cancel(gameObject);
        
    }
}
