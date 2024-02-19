using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Character main;
    [SerializeField] Camera mainCam;
    [SerializeField] Vector3 movement;
    [SerializeField] Vector3 mousePos;
    [SerializeField] Transform mover;
    [SerializeField] Transform rotator;
    [SerializeField] Transform rotatorReference;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10;
    [SerializeField] float defaultSpeed = 2;
    [SerializeField] bool move = true;
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
        movement.x = dir.x;
        movement.z = dir.y;

        if(dir.magnitude == 0)
        {

        }
        else
        {
            if(move)
                main.MoveAnim();
        }
    }

    private void FixedUpdate() {
        if(!move)
        return;

        if(GameplayController.instance.GetState() == GameState.LevelUp)
        return;

        Vector3 moveDirection = new Vector3(movement.x, 0, movement.z).normalized;
        mover.Translate(speed * Time.deltaTime * moveDirection);

        
    }

    public void Throw(Vector3 direction, float force = 7.5f)
    {
        UnMove();
        rb.velocity = direction * force;
        //rb.AddForce(direction * force, ForceMode.Impulse);
        LeanTween.value(rb.gameObject, rb.velocity, Vector3.zero, 1.0f).setEase(LeanTweenType.linear).setOnUpdate((Vector3 v)=>{
            rb.velocity = v;
        }).setOnComplete(()=>{
            Move();
            Idling();
            main.NormalizeAnim();
           
        });

    }

    public void SetSpeed(float value)
    {
        this.speed = defaultSpeed + value;
    }

    public void RefreshSpeed()
    {
        this.speed = defaultSpeed + (main.GetAttribute().speed / 10);
    }

    

    public void Idling()
    {
        if(move)
        main.Idling();
    }

    public Transform GetMover()
    {
        return mover;
    }

    public void Move()
    {
        move = true;
    }

    public void UnMove()
    {
        move = false;
    }

    public void DisableMovement()
    {
        move = false;
        rb.isKinematic = true;
    }

    public void EnableMovement()
    {
        move = true;
        rb.isKinematic = false;
    }
}
