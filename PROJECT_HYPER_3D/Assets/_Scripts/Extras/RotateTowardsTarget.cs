using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
	public Transform target;
	
	public float speed = 5.0f;
	public float rotateSpeed = 400.0f;
	
	public Rigidbody2D rb;
	public Transform angleGuide;
	public bool instant = false;
	public float offset = 180;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	protected void FixedUpdate()
	{
		if(!instant)
		{
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.right * -1).z;
            rb.angularVelocity = rotateAmount * rotateSpeed;
        }
		else
		{
            var dir = angleGuide.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - offset, Vector3.forward);
        }
		
		
	}
}
