using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoockAtCamera : MonoBehaviour {
    Transform camPos;
	// Use this for initialization
	void Start () {
        camPos = GameObject.Find("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(new Vector3(camPos.position.x, transform.rotation.y, camPos.position.z));
	}
}
