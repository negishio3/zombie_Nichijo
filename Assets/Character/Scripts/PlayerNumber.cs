using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNumber : MonoBehaviour {
    //Player,Mob共通
    [SerializeField, Header("P番号")]
    protected int playerNum;//0は一般人
    //[SerializeField,]
    //protected int pattern;

    public int PlayerNum
    {
        get { return playerNum; }
        set { playerNum = value; }
    }
    //public int Pattern
    //{
    //    get { return pattern; }
    //    set { pattern = value; }
    //}
}
