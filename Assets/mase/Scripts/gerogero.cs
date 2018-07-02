using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gerogero : MonoBehaviour
{
    public ParticleSystem particle;

	// Use this for initialization
	void Start ()
    {
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="gero")
        {
            particle.Play();
        }
    }
}
