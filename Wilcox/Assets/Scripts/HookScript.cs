using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {
    public enum HookType
    {
        Chain,
        Pole,
        BreakablePole,
    }
    public HookType hookType;
    public GameObject hookSegment;
    public GameObject hookEnd;

    public float segmentLength = 1.0f;
    
    GameObject lastJoint = null;
    float chainLength = 0.0f;
    public float pullSpeed = 0.0f;
    public bool isActive = false;
    public float maxHookLength = 1000.0f;
    Vector3 hitPos = new Vector3(0, 0, 0);

    List<GameObject> lastChain = new List<GameObject>();

    float epsilon = 1.0f;

    bool fulFix = false;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (fulFix)
        {
            ConfigurableJoint joint = lastChain[lastChain.Count - 1].GetComponent<ConfigurableJoint>();
            joint.transform.position = hitPos;
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
        }

	    if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (hookType)
            {
                case HookType.Chain:
                    ShootChain();
                    break;
                case HookType.Pole:
                    ShootPole();
                    break;
                case HookType.BreakablePole:
                    ShootBreakablePole();
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (hookType)
            {
                case HookType.Chain:
                    break;
                case HookType.Pole:
                    break;
                case HookType.BreakablePole:
                    CheckIfBreak();
                    break;
                default:
                    break;
            }
        }
        //if(Input.GetKey(KeyCode.Mouse1))
        //{
        //    switch (hookType)
        //    {
        //        case HookType.Chain:
        //            PullChain();
        //            break;
        //        case HookType.Pole:
        //            break;
        //        default:
        //            break;
        //    }
        //}
	}

    void ShootChain()
    {
        foreach (var item in lastChain)
        {
            Destroy(item);
            Destroy(GetComponent<ConfigurableJoint>());
        }
        lastChain.Clear();
        isActive = false;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, maxHookLength))
        {
            isActive = true;
            hitPos = hitinfo.point;
            Vector3 vectorChain = hitinfo.point - transform.position;
            int numSegments = (int)Mathf.Ceil(vectorChain.magnitude/segmentLength);
            chainLength = vectorChain.magnitude;

            ConfigurableJoint prevJoint = gameObject.AddComponent<ConfigurableJoint>();
            prevJoint.xMotion = ConfigurableJointMotion.Locked;
            prevJoint.yMotion = ConfigurableJointMotion.Locked;
            prevJoint.zMotion = ConfigurableJointMotion.Locked;
            prevJoint.anchor = new Vector3(0, 0, 0);

            for (int i = 0; i < numSegments; i++)
            {
                // position should be halfway between us and point
                lastJoint = Instantiate(hookSegment, transform.position + i* vectorChain.normalized*segmentLength + vectorChain.normalized * segmentLength*0.5f, transform.rotation);
                lastJoint.transform.localScale = new Vector3(lastJoint.transform.localScale.x, lastJoint.transform.localScale.y, segmentLength);
                ConfigurableJoint jointChain = lastJoint.GetComponent<ConfigurableJoint>();
                jointChain.anchor = jointChain.anchor;
                prevJoint.connectedBody = jointChain.GetComponent<Rigidbody>();
                prevJoint = jointChain;
                lastChain.Add(jointChain.gameObject);
            }
            lastChain[lastChain.Count - 1].GetComponent<BoxCollider>().enabled = false;
        }
    }

    void ShootPole()
    {
        if (lastJoint != null)
        {
            isActive = false;
            Destroy(lastJoint);
            Destroy(GetComponent<ConfigurableJoint>());
            lastJoint = null;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, maxHookLength))
        {
            isActive = true;
            Vector3 vectorChain = hitinfo.point - transform.position;

            // VectorChain length / chain segment
            // get hookSegment length from collision box
            ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.anchor = new Vector3(0, 0, 0);

            // position should be halfway between us and point
            lastJoint = Instantiate(hookSegment, transform.position + vectorChain * 0.5f, transform.rotation);
            lastJoint.transform.localScale = new Vector3(lastJoint.transform.localScale.x, lastJoint.transform.localScale.y, vectorChain.magnitude);
            ConfigurableJoint jointChain = lastJoint.GetComponent<ConfigurableJoint>();
            jointChain.anchor = jointChain.anchor;
            joint.connectedBody = jointChain.GetComponent<Rigidbody>();
        }
    }

    void ShootBreakablePole()
    {
        if (lastJoint != null)
        {
            isActive = false;
            Destroy(lastJoint);
            Destroy(GetComponent<ConfigurableJoint>());
            lastJoint = null;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, maxHookLength))
        {
            isActive = true;
            hitPos = hitinfo.point;
            Vector3 vectorChain = hitinfo.point - transform.position;
            chainLength = vectorChain.magnitude;

            // VectorChain length / chain segment
            // get hookSegment length from collision box
            ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.anchor = new Vector3(0, 0, 0);

            // position should be halfway between us and point
            lastJoint = Instantiate(hookSegment, transform.position + vectorChain * 0.5f, transform.rotation);
            lastJoint.transform.localScale = new Vector3(lastJoint.transform.localScale.x, lastJoint.transform.localScale.y, vectorChain.magnitude);
            ConfigurableJoint jointChain = lastJoint.GetComponent<ConfigurableJoint>();
            jointChain.anchor = jointChain.anchor;
            joint.connectedBody = jointChain.GetComponent<Rigidbody>();
        }
    }

    void CheckIfBreak()
    {
        if (lastJoint == null)
        {
            return;
        }

        Ray ray = new Ray(transform.position, hitPos - transform.position);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, maxHookLength))
        {
            Debug.Log("New ray!");

            Vector3 vectorChain = hitinfo.point - transform.position;
            if (vectorChain.magnitude < chainLength + epsilon)
            {
                Debug.Log("Creating new rope");

                ConfigurableJoint joint = gameObject.GetComponent<ConfigurableJoint>();

                // position should be halfway between us and point
                lastJoint = Instantiate(hookSegment, transform.position + vectorChain * 0.5f, transform.rotation);
                lastJoint.transform.localScale = new Vector3(lastJoint.transform.localScale.x, lastJoint.transform.localScale.y, vectorChain.magnitude);
                ConfigurableJoint jointChain = lastJoint.GetComponent<ConfigurableJoint>();
                jointChain.anchor = jointChain.anchor;
                joint.connectedBody = jointChain.GetComponent<Rigidbody>();
            }
        }
    }

    void PullChain()
    {
        // Find last chain object
        // Move in direction "virtual" to next chain
        // If passes length, remove last chain obj

        if (lastChain.Count == 0)
        {
            return;
        }

        chainLength -= Time.deltaTime* pullSpeed;

        int segmentsToRemove = lastChain.Count - (int)Mathf.Ceil(chainLength/segmentLength);

        if (segmentsToRemove < 0)
        {
            return;
        }

        for (int i = 0; i < segmentsToRemove; i++)
        {
            int index = lastChain.Count - i - 1;
            Destroy(lastChain[index]);
            lastChain.RemoveAt(index);
        }

        ConfigurableJoint joint = lastChain[lastChain.Count - 1].GetComponent<ConfigurableJoint>();
        joint.connectedBody = null;
        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;
        fulFix = true;
    }
}
