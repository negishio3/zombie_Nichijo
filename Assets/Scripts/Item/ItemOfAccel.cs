using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOfAccel : ItemBase {

    public override void Execution(GameObject obj)
    {
        int i=obj.GetComponent<PlayerNumber>().PlayerNum;
        if (EntrySystem.entryFlg[i-1])
        {
            PlayerMove PM= obj.GetComponent<PlayerMove>();
            PM.StopAllCoroutines();
            PM.SpeedUp();
        }
        else
        {
            AIPlayer AI = obj.GetComponent<AIPlayer>();
            AI.StopAllCoroutines();
            AI.SpeedUp();
        }
        Destroy(gameObject);
    }
}
