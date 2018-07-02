using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEClipInfo : MonoBehaviour {

	[System.Serializable]
    public struct SE
    {
        public string name;
        public AudioClip seClip;
    }

    public List<SE> SEClips;
}
