using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update() { 
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * Time.deltaTime*5, 0, 0);
        
		
	}
}
