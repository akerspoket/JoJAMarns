using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendelActivator : MonoBehaviour {
    public PendulumMotion[] pendels;

    public enum PendelActivateType
    {
        All,
        Sequence,
    }

    public PendelActivateType activateType;

    public float delayTime = 0.0f;
    public float delayBetween = 0.0f;
    int currObj = 0;
    bool activated = false;
    // Use this for initialization
    void Start()
    {
        // Stop all pendels
        foreach (var item in pendels)
        {
            item.Deactivate();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (activated == false)
        {
            float delay = delayTime;
            foreach (var item in pendels)
            {
                //Activate pendel, additionaly could start animation of trigger
                switch (activateType)
                {
                    case PendelActivateType.All:
                        Invoke("Activate", 0.0f);
                        break;
                    case PendelActivateType.Sequence:
                        Invoke("Activate", delay);
                        delay += delayBetween;
                        break;
                    default:
                        break;
                }
            }
            activated = true;
        }
    }

    void Activate()
    {
        PendulumMotion pendel = pendels[currObj];
        pendel.Activate();
        currObj++;
    }
}
