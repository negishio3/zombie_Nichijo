using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeroOnGround : MonoBehaviour {
    [SerializeField, Tooltip("owe")]
    private GameObject Owe;
    private Vector3 plusy = new Vector3(0, 0.3f, 0);
    
    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Mob" && col.tag != "Player"&&col.tag!="Item")
        {
            Vector3 hitpos= col.ClosestPointOnBounds(this.transform.position);
            Instantiate(Owe, hitpos+plusy, Quaternion.Euler(-90, 0, 0));
            Destroy(gameObject);
        }
    }
}
