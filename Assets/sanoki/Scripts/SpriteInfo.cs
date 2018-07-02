using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteInfo : MonoBehaviour {
    public List<SpritMem> sprits;
    [System.Serializable]
    public struct SpritMem
    {
        public string name;
        public Sprite spr;
    }
}
