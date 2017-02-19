using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovement : MonoBehaviour
{

    public float forwardForce;
    public float rightForce;

    public float maxSpeed;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private bool active = true;

    private Vector3 totalMoveForce;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (active)
        {
            totalMoveForce = new Vector3(0, 0, 0);
            Vector3 xzForward = Vector3.ProjectOnPlane(transform.forward, new Vector3(0, 1, 0));
            if (Input.GetKey(KeyCode.W))
            {
                // Check that we're not flying too fast
                if (Vector3.Project(GetComponent<Rigidbody>().velocity, xzForward).magnitude < maxSpeed || Vector3.Dot(GetComponent<Rigidbody>().velocity, xzForward) < 0)
                {
                    // Add force to where the object's transform is pointing
                    totalMoveForce += xzForward.normalized * forwardForce;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {            // Check that we're not flying too fast
                if (Vector3.Project(GetComponent<Rigidbody>().velocity, -1 * xzForward).magnitude < maxSpeed || Vector3.Dot(GetComponent<Rigidbody>().velocity, -1 * xzForward) < 0)
                {
                    // Add force to where the object's transform is pointing
                    totalMoveForce += -1 * xzForward.normalized * forwardForce;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                // Check that we're not flying too fast
                if (Vector3.Project(GetComponent<Rigidbody>().velocity, -1 * this.transform.right).magnitude < maxSpeed || Vector3.Dot(GetComponent<Rigidbody>().velocity, -1 * this.transform.right) < 0)
                {
                    // Add force to where the object's transform is pointing
                    totalMoveForce += -1 * this.transform.right * rightForce;
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                // Check that we're not flying too fast
                if (Vector3.Project(GetComponent<Rigidbody>().velocity, this.transform.right).magnitude < maxSpeed || Vector3.Dot(GetComponent<Rigidbody>().velocity, this.transform.right) < 0)
                {
                    // Add force to where the object's transform is pointing
                    totalMoveForce += this.transform.right * rightForce;
                }
            }
        }
	}
    void FixedUpdate()
    {
        if (active)
        {
            this.GetComponent<Rigidbody>().AddForce(totalMoveForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collisioninfo)
    {
        active = false;
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        active = true;
        totalMoveForce = new Vector3(0, 0, 0);
    }
}
