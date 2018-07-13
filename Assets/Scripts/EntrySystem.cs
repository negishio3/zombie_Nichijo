using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EntrySystem : MonoBehaviour {
    public static bool[] entryFlg = { false, false, false, false };
    [SerializeField]
    private Text[] text;
    public GameObject[] Abuttons;
    private string entryText = "ENTRY";
    private string noEntryText = "NPC";
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private GameObject[] pObj;

    private SettingScene settingScene;

    public bool[] EntryFlg
    {
        get { return entryFlg; }
        set { entryFlg = value; }
    }

	void Start () {
		for(int i = 0; i < 4; i++)
        {
            text[i].text = noEntryText;
        }
        settingScene = GameObject.Find("Setting").GetComponent<SettingScene>();
	}

    // Update is called once per frame
    void Update() {
        if (!settingScene.NowSetting)
        {
            if (Input.GetButtonDown("Fire1") && !entryFlg[0])
            {
                entryFlg[0] = true;
                text[0].text = entryText;
                GameObject g = (GameObject)Instantiate(pObj[0], new Vector3(-7, -1, 0), Quaternion.Euler(0, -210, 0));
                g.name = pObj[0].name;
                Abuttons[0].SetActive(false);
            }
            if (Input.GetButtonDown("Fire2") && !entryFlg[1])
            {
                entryFlg[1] = true;
                text[1].text = entryText;
                GameObject g = (GameObject)Instantiate(pObj[1], new Vector3(-2, -1, 0), Quaternion.Euler(0, -190, 0));
                g.name = pObj[1].name;
                Abuttons[1].SetActive(false);
            }
            if (Input.GetButtonDown("Fire3") && !entryFlg[2])
            {
                entryFlg[2] = true;
                text[2].text = entryText;
                GameObject g = (GameObject)Instantiate(pObj[2], new Vector3(2, -1, 0), Quaternion.Euler(0, -170, 0));
                g.name = pObj[2].name;
                Abuttons[2].SetActive(false);
            }
            if (Input.GetButtonDown("Fire4") && !entryFlg[3])
            {
                entryFlg[3] = true;
                text[3].text = entryText;
                GameObject g = (GameObject)Instantiate(pObj[3], new Vector3(7, -1, 0), Quaternion.Euler(0, -150, 0));
                g.name = pObj[3].name;
                Abuttons[2].SetActive(false);
            }


            if (Input.GetButtonDown("Jump1") && entryFlg[0])
            {
                entryFlg[0] = false;
                text[0].text = noEntryText;
                Destroy(GameObject.Find("Entry1"));
                Abuttons[0].SetActive(true);
            }
            if (Input.GetButtonDown("Jump2") && entryFlg[1])
            {
                entryFlg[1] = false;
                text[1].text = noEntryText;
                Destroy(GameObject.Find("Entry2"));
                Abuttons[1].SetActive(true);
            }
            if (Input.GetButtonDown("Jump3") && entryFlg[2])
            {
                entryFlg[2] = false;
                text[2].text = noEntryText;
                Destroy(GameObject.Find("Entry3"));
                Abuttons[2].SetActive(true);
            }
            if (Input.GetButtonDown("Jump4") && entryFlg[3])
            {
                entryFlg[3] = false;
                text[3].text = noEntryText;
                Destroy(GameObject.Find("Entry4"));
                Abuttons[3].SetActive(true);
            }

            for (int i = 1; i < 5; i++)
            {
                if (Input.GetButtonDown("Start" + i.ToString()))
                {
                    ChangeScene();
                }
            }
        }
    }

    void ChangeScene()
    {
        GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect(sceneName);
    }
}
