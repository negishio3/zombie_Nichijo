using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackTest : MonoBehaviour {//そのうち作り直す可能性あり
    private int parNum;

    public int ParNum
    {
        get { return parNum; }
        set { parNum = value; }
    }
	void Start () {
        Destroy(gameObject, 1f);

	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Mob" && col.GetComponent<PlayerNumber>().PlayerNum != parNum) {
            Quaternion qua = col.transform.rotation;
            MobChangeSystem.MobChanger(col.gameObject, parNum);
            Destroy(gameObject, 0.05f);
        }
    }
}
