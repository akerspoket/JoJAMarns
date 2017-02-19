using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    public bool isInfinityPlane = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Dead");

        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if(isInfinityPlane)
            health.KillByFall();
        else
        {
            // Spike trap
            health.KillByFall();

        }
    }
}
