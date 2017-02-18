using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {


    public float forwardForce;
    public float rightForce;
    public float maxForce;
    public float jumpForce;
    public float rotateSpeed;

    public float maxSpeed;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private float upForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        KeyMovement();
        MouseMovement();
        if(upForce>0)
        upForce-=0.1f;

    }

    void KeyMovement()
    {
        Vector3 totalMoveForce = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            // Add force to where the object's transform is pointing
            totalMoveForce += this.transform.forward * forwardForce;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Add force to where the object's transform is pointing
            totalMoveForce += -1 * this.transform.forward * forwardForce;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // Add force to where the object's transform is pointing
            totalMoveForce += -1 * this.transform.right * rightForce;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Add force to where the object's transform is pointing
            totalMoveForce += this.transform.right * rightForce;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            upForce = 10;
        }
        totalMoveForce += upForce * new Vector3(0,1,0);

        //totalMoveForce = totalMoveForce.normalized * maxForce;

        //if ((GetComponent<Rigidbody>().velocity.magnitude < maxSpeed))
        {
            this.GetComponent<Rigidbody>().AddForce(totalMoveForce);
        }

        


        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Add force to where the object's transform is pointing
            this.GetComponent<Rigidbody>().AddForce(this.transform.up * jumpForce);
        }

    }

    void MouseMovement()
    {

        // Rotation now depending on how fast you move mouse
        yaw += rotateSpeed * Input.GetAxis("Mouse X");
        pitch -= rotateSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

}
