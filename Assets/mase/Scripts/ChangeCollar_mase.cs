using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollar_mase : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != transform.tag)
        { 
            Renderer enemyMaterial = other.GetComponent<Renderer>();
            Renderer PlayerMaterial = GetComponent<Renderer>();
            enemyMaterial.material = PlayerMaterial.material;
            other.tag = (transform.tag);
        }
    }
}