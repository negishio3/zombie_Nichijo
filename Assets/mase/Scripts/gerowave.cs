using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gerowave : MonoBehaviour
{
    public ParticleSystem particle;
    public GameObject pat;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "gero")
        {
            //particle.Play();
            Instantiate(pat, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            
            Debug.Log("消えるよ");
        }
    }
}
