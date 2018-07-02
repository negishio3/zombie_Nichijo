using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashingArrow : MonoBehaviour {

    private float alpha=0.5f;
    private float cr,cg,cb;
    private float minus = 0.5f;
    private MeshRenderer meshrender;

    void Start () {
        meshrender = GetComponent<MeshRenderer>();
        cr = meshrender.material.color.r;
        cg = meshrender.material.color.g;
        cb = meshrender.material.color.b;
    }
	
	// Update is called once per frame
	void Update () {
        if (alpha < 0.1f) { minus *= -1; }
        else if(alpha>0.7f){ minus *= -1; }
        alpha += Time.deltaTime * minus;
        meshrender.material.color = new Color(cr, cg, cb, alpha);
    }
}
