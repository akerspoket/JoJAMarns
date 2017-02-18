using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public GameObject teleportObj = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(teleportObj != null)
        {
            // check if obj is player? 
            other.transform.position = teleportObj.transform.position;
            other.transform.rotation = teleportObj.transform.rotation;
        }
        else
        {
            Debug.LogError("No teleport object set");
        }
    }
}
