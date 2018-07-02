using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purification : MonoBehaviour {

    void Start()
    {
        Destroy(gameObject,5f);
    }

    private void OnParticleCollision(GameObject obj)
    {
        if ((obj.tag == "Mob" && obj.GetComponent<PlayerNumber>().PlayerNum != 0))
        {
            Destroy(obj);
            MobChangeSystem.HumanSpawn(obj.transform.position,obj.transform.rotation);
        }
        if (obj.tag == "Mob" && obj.GetComponent<PlayerNumber>().PlayerNum == 0)
        {
            obj.GetComponent<HumanMove>().Smoke = true;
        }
    }
}
