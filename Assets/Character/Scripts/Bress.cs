using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bress : MonoBehaviour {
    [SerializeField]
    private int PlayerNum;

    private void OnParticleCollision(GameObject obj)
    {
        if ((obj.tag == "Mob" && obj.GetComponent<PlayerNumber>().PlayerNum ==0))
        {
            Quaternion qua = obj.transform.rotation;
            MobChangeSystem.MobChanger(obj.gameObject, PlayerNum);
        }
        else if (obj.tag == "Mob" && obj.GetComponent<PlayerNumber>().PlayerNum != 0)
        {
            MobChangeSystem.MobChanger(obj.gameObject, PlayerNum);
        }
    }
}
