using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {
    public float forwardForce;
    public float rightForce;
    public float jumpForce;
    public float slideForce;
    public float inwardForce;

    public float maxSpeed;
    public float maxSlideSpeed;

    public bool canJumpOnSameWall;
    public float canSlideAngle;


    private float upForce;

    private bool canSlide = false;
    private GameObject lastSlidedWall;
    private GameObject collidingWall;
    private Vector3 collisionNormal;
    private float glideTimer = 0.0f;
    public float maxGlideTime;
    private bool sliding = false;
    private Vector3 totalMoveForce;
    private Vector3 totalNoImpulsForce;
    private Vector3 collisionPoint;
    private Vector3 myCollisionPosition;

    private bool activeScript = false;
    private int totalCollidingObjects = 0;

    private float jumpTimer = 0;
    private float jumpCoolDownTimer = 0;
    private float jumpOnSameWallCoolDownTimer = 0;
    public float leftWallJumpTimer = 0.2f;
    public float OkToJumpDistance = 1;
    public float jumpCoolDown = 0.2f;
    public float jumpOnSameWallCoolDown = 0.7f;
    public float jumpOffForce;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        totalMoveForce = new Vector3(0, 0, 0);
        totalNoImpulsForce = new Vector3(0, 0, 0);
        // Make sure we are not moving to fast
        jumpTimer -= Time.deltaTime;
        jumpCoolDownTimer -= Time.deltaTime;
        jumpOnSameWallCoolDownTimer -= Time.deltaTime;

        JumpMovement();
        if (!activeScript && GetComponent<HookScript>().isActive)
        {

            return;
        }


        Vector3 xzVelocity = Vector3.ProjectOnPlane(GetComponent<Rigidbody>().velocity, new Vector3(0, 1, 0));
        if (sliding && GetComponent<Rigidbody>().velocity.magnitude > maxSlideSpeed)
        {
            //print("Setvelo");
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxSlideSpeed;
        }    
        //else if ((xzVelocity).magnitude >= maxSpeed)
        //{
        //    GetComponent<Rigidbody>().velocity = xzVelocity.normalized * maxSpeed + new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
        //}

        // FIgure out if we are sliding
        if (canSlide && Input.GetKey(KeyCode.LeftShift))
        {
            lastSlidedWall = collidingWall;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
            sliding = true;
            canSlide = false;
        }
        else if (sliding && Input.GetKeyUp(KeyCode.LeftShift))
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
            //glideTimer += Time.deltaTime;
            //if (glideTimer >= this.maxGlideTime)
            //{
            //    sliding = false;
            //    glideTimer = 0;
            //    GetComponent<Rigidbody>().useGravity = false;
            //}
        }
        //JumpMovement();
    }

    void FixedUpdate()
    {
        this.GetComponent<Rigidbody>().AddForce(totalMoveForce, ForceMode.Impulse);
        totalMoveForce = new Vector3(0, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Entercollision");
        totalCollidingObjects++;
        activeScript = true;
        //doesnt work for multiplecollisions at once ERROR
        bool newNormal = collisionNormal != collision.contacts[0].normal;
        collisionNormal = collision.contacts[0].normal;
        collisionPoint = collision.contacts[0].point;
        myCollisionPosition = transform.position;
        if (canJumpOnSameWall || lastSlidedWall != collision.gameObject || newNormal)
        {
            canSlide = true;
            jumpOnSameWallCoolDownTimer = 0;
            if (Mathf.Abs(Vector3.Dot(collisionNormal, new Vector3(0,1,0))) > canSlideAngle)
            {
                canSlide = false;
            }
        }

        collidingWall = collision.gameObject;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        collisionNormal = collisionInfo.contacts[0].normal;
        collisionPoint = collisionInfo.contacts[0].point;
        myCollisionPosition = transform.position;

    }
    void OnCollisionExit(Collision collisionInfo)
    {
        //if (sliding)
        //{
        // Does not work
        //    // print("No longer in contact with " + collisionInfo.transform.name);
        //    RaycastHit hit;
        //    Vector3 currentVelocity = this.GetComponent<Rigidbody>().velocity;
        //    if (Physics.Raycast(transform.position + currentVelocity * 0.1f, -collisionNormal, out hit, 4))
        //    {
        //        //print(hit.transform.name);

        //        float currentSpeed = currentVelocity.magnitude;
        //        Vector3 newVelocity = Vector3.ProjectOnPlane(currentVelocity, hit.normal);
        //        print(currentVelocity);
        //        newVelocity = newVelocity.normalized * currentSpeed;
        //        print(newVelocity);
        //        this.GetComponent<Rigidbody>().velocity = newVelocity;
        //        totalCollidingObjects--;
        //        this.totalMoveForce = new Vector3(0, 0, 0);
        //        return;
        //    }
        //}
        totalCollidingObjects--;
        if (totalCollidingObjects == 0)
        {
            jumpTimer = leftWallJumpTimer;
            activeScript = false;
            canSlide = false;
            collidingWall = null;
            sliding = false;
            GetComponent<Rigidbody>().useGravity = true;
            this.totalMoveForce = new Vector3(0, 0, 0);
        }

    }

    void KeyMovement()
    {
        Vector3 xzForward = Vector3.ProjectOnPlane(transform.forward, new Vector3(0, 1, 0));
        Vector3 forceDirection = new Vector3(0,0,0);
        //Vector3 xzForward = Vector3.ProjectOnPlane(transform.forward, new Vector3(0, 1, 0));
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
        //totalMoveForce += forceDirection;
       

    }

    private void JumpMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCoolDownTimer <= 0.0f)
        {
            //RaycastHit hit;
            //if (Physics.SphereCast(transform.position + new Vector3(0, 0.3f, 0), OkToJumpDistance, new Vector3(0, -1, 0), out hit, 0.6f))
            //{
            //    print("Jumping on: " + hit.transform.name);
            //    if (hit.transform.gameObject != this.transform.gameObject)
            //    {
            //        if (jumpOnSameWallCoolDownTimer < 0)
            //        {

            //            Vector3 jumpDirection = hit.point - transform.position;
            //            Vector3 jumpDirectionXZ = new Vector3(jumpDirection.x, 0, jumpDirection.z);
            //            Vector3 jumpDirectionY = new Vector3(0, jumpDirection.y, 0);
            //            this.GetComponent<Rigidbody>().AddForce(-jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            //            //this.GetComponent<Rigidbody>().AddForce(-jumpDirectionY.normalized * jumpForce, ForceMode.Impulse);
            //            print("Jumping on: " + hit.transform.name + "In direction" + jumpDirection);
            //            jumpCoolDownTimer = jumpCoolDown;
            //            jumpOnSameWallCoolDownTimer = jumpOnSameWallCoolDown;
            //            jumpTimer = 0;
            //        }
            //    }
            //}

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, new Vector3(0, -1.0f, 0), out hit, OkToJumpDistance))
            //{
            //    Vector3 jumpDirection = hit.normal;
            //    totalMoveForce += jumpDirection.normalized * jumpForce;
            //    jumpCoolDownTimer = jumpCoolDown;
            //}
            // Add force to where the object's we are standing on's transform is pointing
            if ((collidingWall != null || jumpTimer > 0) && jumpOnSameWallCoolDownTimer < 0)
            {
                Vector3 jumpDirection = collisionNormal + new Vector3(0, 1.0f, 0);
                jumpDirection = jumpDirection.normalized;
                Vector3 jumpDirectionXZ = new Vector3(jumpDirection.x, 0, jumpDirection.z);
                Vector3 jumpDirectionY = new Vector3(0, jumpDirection.y, 0);
                this.GetComponent<Rigidbody>().AddForce(jumpDirectionXZ * jumpOffForce, ForceMode.Impulse);
                this.GetComponent<Rigidbody>().AddForce(jumpDirectionY * jumpForce, ForceMode.Impulse);
                //print("Making jump " + jumpDirection.normalized * jumpForce);
                // this.GetComponent<Rigidbody>().AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
                jumpCoolDownTimer = jumpCoolDown;
                jumpOnSameWallCoolDownTimer = jumpOnSameWallCoolDown;
                jumpTimer = 0;
            }
        }
    }

    private void SlideMovement()
    {
        GetComponent<Rigidbody>().useGravity = false;
        Vector3 cn = collisionNormal;
        Vector3 cp = collisionPoint;
        Vector3 op = myCollisionPosition;
        Vector3 cnInPlane = Vector3.ProjectOnPlane(cn, new Vector3(0, 1, 0));
        Vector3 cpopInPlane = Vector3.ProjectOnPlane(cp - op, new Vector3(0, 1, 0));
        Vector3 signedUp = Vector3.Cross(cnInPlane, cpopInPlane);
        Vector3 target = Vector3.Cross(cnInPlane, signedUp);

        if (Vector3.Project(GetComponent<Rigidbody>().velocity, target.normalized).magnitude < maxSlideSpeed)
        {
            //print(target.normalized);
            totalMoveForce += target.normalized * slideForce;
        }
        Vector3 addForce = (-1 * cn.normalized * inwardForce * GetComponent<Rigidbody>().velocity.magnitude);// 
        // print(addForce);
        totalMoveForce += addForce;
        // totalNoImpulsForce += addForce;
        //print(totalMoveForce.magnitude + "" + cn + " " + totalMoveForce + "" + GetComponent<Rigidbody>().velocity);
    }
}
