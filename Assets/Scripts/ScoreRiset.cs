using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRiset : MonoBehaviour {

	void Start () {
        for(int i = 0; i< 4; i++)
        {
            EntrySystem.entryFlg[i] = false;
            MobChangeSystem.scoreCount[i] = 0;
        }
    }

}
