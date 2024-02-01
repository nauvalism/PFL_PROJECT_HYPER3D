using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCollisionCatcher : MonoBehaviour
{
    [SerializeField] Character main;
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy"))
        {

        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            
        }
    }
}
