using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Camera mainCam;
    [SerializeField] Vector3 movement;
    [SerializeField] Vector3 mousePos;
    [SerializeField] Transform mover;
    [SerializeField] Transform rotator;
    [SerializeField] Transform rotatorReference;
    [SerializeField] float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.z = Input.GetAxisRaw("Vertical");

        
    }

    public void SetMovement(Vector3 dir)
    {
        movement.x = -dir.x;
        movement.z = -dir.y;
    }

    private void FixedUpdate() {
        Vector3 moveDirection = new Vector3(movement.x, 0, movement.z).normalized;
        mover.Translate(speed * Time.deltaTime * moveDirection);
    }
}