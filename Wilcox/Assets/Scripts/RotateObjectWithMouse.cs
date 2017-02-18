using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectWithMouse : MonoBehaviour {
    public float rotateSpeed;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MouseMovement();
	}

    void MouseMovement()
    {

        // Rotation now depending on how fast you move mouse
        yaw += rotateSpeed * Input.GetAxis("Mouse X");
        pitch -= rotateSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
