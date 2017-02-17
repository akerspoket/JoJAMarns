using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {


    public float forwardForce;
    public float rightForce;
    public float maxForce;
    public float jumpForce;
    public float rotateSpeed;


    private float yaw = 0.0f;
    private float pitch = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        KeyMovement();
        MouseMovement();

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
        totalMoveForce = totalMoveForce.normalized * maxForce;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Add force to where the object's transform is pointing
            totalMoveForce += this.transform.up * jumpForce;
        }

        this.GetComponent<Rigidbody>().AddForce(totalMoveForce);
    }

    void MouseMovement()
    {

        // Rotation now depending on how fast you move mouse
        yaw += rotateSpeed * Input.GetAxis("Mouse X");
        pitch -= rotateSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

}
