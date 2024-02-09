using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance {get;set;}
    
    private void Awake() {
        instance = this;
    }
}
