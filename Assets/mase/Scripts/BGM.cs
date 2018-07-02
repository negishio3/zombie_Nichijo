using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip bgm;
    public AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(bgm);

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
