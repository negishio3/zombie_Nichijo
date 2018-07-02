using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGuideLine : MonoBehaviour {


    public LineRenderer LineRender { get; set; }
    private Vector3 offset = new Vector3(0, 0.5f, 0f);
	void Start () {
        LineRender = GetComponent<LineRenderer>();
        LineRender.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LineWrite(Vector3 playerpos,Vector3 areaPos)
    {
        LineRender.SetPosition(0, playerpos+offset);
        LineRender.SetPosition(1, areaPos+offset);
    } 
}
