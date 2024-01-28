using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
    [SerializeField] CharacterShooter shooter;
    [SerializeField] Transform rotator;
    [SerializeField] Transform currentReference;
    [SerializeField] Transform refer;
    [SerializeField] Transform refer2;
    [SerializeField] Camera mainCam;
    [SerializeField] Collider planeCollider;
    [SerializeField] RaycastHit hit;
    [SerializeField] Ray ray;
    [SerializeField] Transform target;

    private void Awake() {
        currentReference = refer2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddTarget(Transform t)
    {
        this.target = t;
    }

    public void ReceiveAnalogInput(Vector3 dir)
    {
        refer2.localPosition = new Vector3(-dir.x, refer2.localPosition.y, -dir.y);
    }

    // Update is called once per frame
    void Update()
    {

        if(target == null)
        {
            // ray = mainCam.ScreenPointToRay(Input.mousePosition);
            // if(Physics.Raycast(ray, out hit))
            // {
            //     if(hit.collider == planeCollider)
            //     {
            //         refer.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
            //         rotator.LookAt(refer);
            //     }
            // }
            currentReference = refer2;
            rotator.LookAt(refer2);
            
            shooter.SyncShootDirection(refer2.position);
        }
        else
        {
            currentReference = target;
            rotator.LookAt(target);
            shooter.SyncShootDirection(target.position);
        }

        
    }

    public Transform GetCurrentReference()
    {
        return currentReference;
    }
}
