using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {
    public float forwardForce;
    public float rightForce;
    public float jumpForce;
    public float rotateSpeed;

    public float maxSpeed;

    public bool canJumpOnSameWall;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private float upForce;

    private bool canSlide = false;
    private GameObject lastSlidedWall;
    private GameObject collidingWall;
    private Vector3 collisionNormal;
    private float glideTimer = 0.0f;
    public float maxGlideTime;
    private bool sliding = false;
    private Vector3 totalMoveForce;

    private bool activeScript = false;
    private int totalCollidingObjects = 0;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (!activeScript)
        {
            return;
        }
        totalMoveForce = new Vector3(0, 0, 0);
        if (canSlide && Input.GetKey(KeyCode.Space))
        {
            lastSlidedWall = collidingWall;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
            sliding = true;
            canSlide = false;
        }
        else if (sliding && Input.GetKeyUp(KeyCode.Space))
        {
            sliding = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
        if (!sliding)
        {
            KeyMovement();
        }
        else
        {
            SlideMovement();
        }
        MouseMovement();
    }

    void FixedUpdate()
    {
        if (!activeScript)
        {
            return;
        }
        this.GetComponent<Rigidbody>().AddForce(totalMoveForce);
    }

    void OnCollisionEnter(Collision collision)
    {
        totalCollidingObjects++;
        activeScript = true;
        //doesnt work for multiplecollisions at once ERROR
        collisionNormal = collision.contacts[0].normal;
        
        if (canJumpOnSameWall || lastSlidedWall != collision.gameObject)
        {
            canSlide = true;
        }
        collidingWall = collision.gameObject;
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        totalCollidingObjects--;
        if (totalCollidingObjects == 0)
        {
            activeScript = false;
        }
        // print("No longer in contact with " + collisionInfo.transform.name);
        canSlide = false;
        collidingWall = null;
    }

    void KeyMovement()
    {
        print(GetComponent<Rigidbody>().velocity);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Add force to where the object's we are standing on's transform is pointing
            if (collidingWall != null)
            {
                Vector3 jumpDirection = collisionNormal + new Vector3(0, 1, 0);
                totalMoveForce += jumpDirection.normalized * jumpForce;
            }
        }

    }

    void MouseMovement()
    {

        // Rotation now depending on how fast you move mouse
        yaw += rotateSpeed * Input.GetAxis("Mouse X");
        pitch -= rotateSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private void SlideMovement()
    {
        Vector3 projectedForward = Vector3.Project(GetComponent<Rigidbody>().velocity, this.collidingWall.transform.forward);
        // Check that we're not flying too fast
        if (projectedForward.magnitude < maxSpeed)
        {
            this.GetComponent<Rigidbody>().AddForce(projectedForward.normalized * forwardForce);
        }
    }
}
