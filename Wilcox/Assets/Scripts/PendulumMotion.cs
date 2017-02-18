using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumMotion : MonoBehaviour {

    // Use this for initialization
    Transform baseGameObject;
    Transform endGameObject;
    Transform[] allGameObjects;
	void Start ()
    {

        allGameObjects = gameObject.GetComponentsInChildren<Transform>();

        foreach (var item in allGameObjects)
        {
            if (item.name== "PendBase")
            {
                baseGameObject = gameObject.GetComponentInChildren<Transform>();
                print("Hittat basePend");
            }
            if(item.name == "PendEnd")
            {
                endGameObject = gameObject.GetComponentInChildren<Transform>();
                print("Hittat endPend");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
