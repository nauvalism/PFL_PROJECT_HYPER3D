using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    [SerializeField] string target;
    [SerializeField] Collider col;
    [SerializeField] int dmg;


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag(target))
        {
            other.SendMessage("Attacked", dmg);
        }
    }
}
