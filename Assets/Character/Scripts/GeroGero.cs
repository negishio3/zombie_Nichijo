using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeroGero : MonoBehaviour {
    [SerializeField]
    private int plNum;
void OnTriggerEnter(Collider obj)
    {
        if ((obj.tag == "Mob" && obj.GetComponent<PlayerNumber>().PlayerNum == 0))
        {
            Quaternion qua = obj.transform.rotation;
            MobChangeSystem.MobChanger(obj.gameObject, plNum);
        }
        else if (obj.tag == "Mob" && obj.GetComponent<PlayerNumber>().PlayerNum != 0)
        {
            MobChangeSystem.MobChanger(obj.gameObject, plNum);
        }
    }
}
