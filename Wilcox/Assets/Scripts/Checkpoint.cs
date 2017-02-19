using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.checkpoint = gameObject;
        }

        other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
