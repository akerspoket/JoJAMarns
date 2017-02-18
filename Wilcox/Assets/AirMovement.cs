using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovement : MonoBehaviour
{

    public float forwardForce;
    public float rightForce;
    public float rotateSpeed;

    public float maxSpeed;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private bool active = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            Vector3 totalMoveForce = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                // Check that we're not flying too fast
                if (Vector3.Project(GetComponent<Rigidbody>().velocity, this.transform.forward).magnitude < maxSpeed || Vector3.Dot(GetComponent<Rigidbody>().velocity, this.transform.forward) < 0)
                {
                    // Add force to where the object's transform is pointing
                    totalMoveForce += this.transform.forward * forwardForce;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {            // Check that we're not flying too fast
                if (Vector3.Project(GetComponent<Rigidbody>().velocity, -1 * this.transform.forward).magnitude < maxSpeed || Vector3.Dot(GetComponent<Rigidbody>().velocity, -1 * this.transform.forward) < 0)
                {
                    // Add force to where the object's transform is pointing
                    totalMoveForce += -1 * this.transform.forward * forwardForce;
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
            this.GetComponent<Rigidbody>().AddForce(totalMoveForce);

            ///Mouse movement
            yaw += rotateSpeed * Input.GetAxis("Mouse X");
            pitch -= rotateSpeed * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
	}

    void OnCollisionEnter(Collision collisioninfo)
    {
        active = false;
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        active = true;
    }
}
