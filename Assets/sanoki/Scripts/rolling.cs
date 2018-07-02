using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rolling : MonoBehaviour {

    Vector3 rotation=Vector3.zero;
    float rollingSpeed=5;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 5, 0));
    }
}
