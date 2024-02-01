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
        
        
    }

    private void OnCollisionEnter(Collision other) {
        // if(other.gameObject.CompareTag("Bullet"))
        // {
        //     //render.material.color = Color.red;
        //     BlinkFlash();
        // }
    }

    public void BlinkFlash()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, Color.red, Color.white, .25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((Color c)=>{
            //Debug.Log("Warna");
            render.material.color = c;
        });
    }
}
