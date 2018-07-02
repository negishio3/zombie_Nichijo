using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour {
    [SerializeField, Header("アイテムID")]
    protected int itemID;

    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }


    public abstract void Execution(GameObject obj);
}
