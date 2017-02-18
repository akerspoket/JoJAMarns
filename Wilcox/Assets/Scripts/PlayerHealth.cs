using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    // Should be checked by movement components to interrupt new movements
    public bool isDead = false;
    public GameObject checkpoint = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isDead)
        {
            // Play death animation(?)

            // Check if death animation ended
            if(true)
            {
                // Respawn player
                if(checkpoint != null)
                {
                    transform.position = checkpoint.transform.position;
                    transform.rotation = checkpoint.transform.rotation;
                    isDead = false;
                }
            }
            
        }
	}

    // Used for infinity fall(animation etc)
    public void KillByFall()
    {
        isDead = true;
        // Start death animation

        Debug.Log("Dead");
    }
}
