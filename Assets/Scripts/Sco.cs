using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sco : MonoBehaviour {
    //そのうち削除
    public Text t;
    [SerializeField]
    private string sceneName;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        t.text = "1P:" + MobChangeSystem.scoreCount[0]
                + "　2P:" + MobChangeSystem.scoreCount[1]
                + "  3P:" + MobChangeSystem.scoreCount[2]
                + "  4P:" + MobChangeSystem.scoreCount[3];

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect(sceneName);
        }
    }
}
