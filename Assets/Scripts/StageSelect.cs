using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour {

    [SerializeField]
    private Image[] Arrow = new Image[2];
    [SerializeField]
    private GameObject[] StageObj;

    SettingScene settingScene;

    private int selectStage;
    private const float moveWaitTime=0.15f;

    private bool isExecute;
    private bool moveSelect;

	void Start () {
        settingScene = GameObject.Find("Setting").GetComponent<SettingScene>()
;	}

    // Update is called once per frame
    void Update()
    {
        if (!settingScene.NowSetting)
        {

            //すべてのコントローラ検知用
            for (int i = 1; i < 5; i++)
            {
                //決定
                if (Input.GetButtonDown("Fire" + i))
                {
                    GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect("Stage" + selectStage);
                }
                //右へ
                else if (Input.GetAxis("HorizontalL" + i) > 0.9 && moveSelect)
                {
                    moveSelect = false;
                    selectStage++;
                    StartCoroutine(MoveTimeWait());
                }
                //左へ
                else if (Input.GetAxis("HorizontalL" + i) < -0.9 && moveSelect)
                {
                    moveSelect = false;
                    selectStage--;
                    StartCoroutine(MoveTimeWait());
                }
            }

            SelectNumberLoop(selectStage, out selectStage);

        }

    }

    void StageMove()
    {
        StageObj[selectStage].gameObject.transform.eulerAngles += new Vector3(0, 0.1f, 0);

    }

    /// <summary>
    /// ループ用
    /// </summary>
    void SelectNumberLoop(int value,out int i)
    {
        if (value < 0)
        {
            i = StageObj.Length - 1;

        }
        else if (value > StageObj.Length - 1)
        {
            i = 0;
        }
        i = value;
    }


    /// <summary>
    /// Selectの待ち時間
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveTimeWait()
    {
        if (isExecute || moveSelect)
        {
            yield break;
        }
        isExecute = true;
        yield return new WaitForSeconds(moveWaitTime);
        moveSelect = true;
        isExecute = false;
    }
}
