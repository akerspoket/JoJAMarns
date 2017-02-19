using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

    public GameObject hookSegment;
    public GameObject hookEnd;

    GameObject lastJoint = null;

    public float maxHookLength = 1000.0f;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (lastJoint != null)
            {
                Destroy(lastJoint);
                Destroy(GetComponent<ConfigurableJoint>());
                lastJoint = null;
            }

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitinfo;
            if(Physics.Raycast(ray, out hitinfo, maxHookLength))
            {
                Vector3 vectorChain = hitinfo.point - transform.position;

                // VectorChain length / chain segment
                // get hookSegment length from collision box
                Debug.Log(transform.forward);
                Debug.Log(vectorChain.normalized);
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
	}
}
