using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNumber : MonoBehaviour {
    //Player,Mob共通
    [SerializeField, Header("P番号")]
    protected int playerNum;//0は市民
    protected int teamNum;  //チーム番号 0は市民

    public int PlayerNum
    {
        get { return playerNum; }
        set { playerNum = value; }
    }

    public int TeamNum
    {
        get { return teamNum; }
        set { teamNum = value; }
    }
}
