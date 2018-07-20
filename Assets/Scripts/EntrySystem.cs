using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EntrySystem : MonoBehaviour {
    [SerializeField]
    private Text[] text;

    public GameObject[] Abuttons;

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private GameObject[] pObj;

    [SerializeField]
    private GameObject[] cursor = new GameObject[4];

    [SerializeField]
    private Material[] outline=new Material[4];




    private List<GameObject> PlayerObj = new List<GameObject>{null,null,null,null};
    private RectTransform[] cursorRect = new RectTransform[4];

    private SettingScene settingScene;
    private Vector3[] spawnPos = {
        new Vector3(-8, -1, 0),
        new Vector3(-2.6f, -1, 0),
        new Vector3(2.6f, -1, 0),
        new Vector3(8, -1, 0)
    };

    private Vector3 posMinus = new Vector3(0, 100, 0);

    private float[] quaternion =
    {
        150,160,-170,-160
    };

    public static bool[] existenceFlg = { false, false, false, false };
    public static bool[] entryFlg = { false, false, false, false };
    public static int[] teamNumber = { 0, 1 ,2, 3 };

    private int[] selectNumber= {0,1,2,3 };

    private bool[] selectMode=new bool[4];      //選択モード
    private bool[] select = new bool[4];        //選択をした
    private bool[] slideCursor = new bool[4];   //カーソルをスライド可能か
    private bool[] isExecute = new bool[4];     //コルーチンを実行しているか
    private bool anyEntry;                      //誰かがエントリーしているか

    private string entryText = "ENTRY";
    private string NPCText = "NPC";
    private string noEntryText = "NoEntry";

    private const float cursorMoveTime = 0.1f;

    public bool[] EntryFlg
    {
        get { return entryFlg; }
        set { entryFlg = value; }
    }

    void Start() {
        for (int i = 0; i < 4; i++)
        {
            text[i].text = noEntryText;
            cursorRect[i] = cursor[i].GetComponent<RectTransform>();
            slideCursor[i] = true;
        }
        if (GameObject.Find("Setting"))
        {
            settingScene = GameObject.Find("Setting").GetComponent<SettingScene>();
        }
        cursor[0].GetComponent<Image>().color = Color.red;
        cursor[1].GetComponent<Image>().color = Color.blue;
        cursor[2].GetComponent<Image>().color = Color.green;
        cursor[3].GetComponent<Image>().color = Color.yellow;
    }

    // Update is called once per frame
    void Update() {
        if (!settingScene.NowSetting)
        {
            CursorControl();
            for (int i = 0; i < 4; i++)
            {
                if (select[i])
                {
                    if (existenceFlg[selectNumber[i]])
                    {
                        //selectNumberの対象の参加を取り消す

                        if (Input.GetButtonUp("Jump" + (i + 1).ToString()))
                        {
                            if (entryFlg[selectNumber[i]])
                            {
                                NotEntry(selectNumber[i]);
                                select[i] = false;
                                selectMode[i] = false;
                            }
                            else
                            {
                                NotEntry(selectNumber[i]);
                                select[i] = false;
                            }
                        }


                        //戻る
                        else if (Input.GetButtonUp("Fire" + (i + 1).ToString()))
                        {
                            select[i] = false;
                            Debug.Log("selectend");
                        }
                        //チーム変更
                        else if (Input.GetAxis("HorizontalL" + (i + 1).ToString()) >= 1 && slideCursor[i])
                        {
                            slideCursor[i] = false;
                            StartCoroutine(CursorMoveTimeWait(i));
                            teamNumber[selectNumber[i]] = I_NumberLoop(teamNumber[selectNumber[i]], 1, 3);
                            PlayerObj[selectNumber[i]].GetComponentInChildren<SkinnedMeshRenderer>().material = outline[teamNumber[selectNumber[i]]];
                            Debug.Log(PlayerObj[selectNumber[i]].name + teamNumber[selectNumber[i]]);
                        }
                        else if (Input.GetAxis("HorizontalL" + (i + 1).ToString()) <= -1 && slideCursor[i])
                        {
                            slideCursor[i] = false;
                            StartCoroutine(CursorMoveTimeWait(i));
                            teamNumber[selectNumber[i]] = I_NumberLoop(teamNumber[selectNumber[i]], -1, 3);
                            PlayerObj[selectNumber[i]].GetComponentInChildren<SkinnedMeshRenderer>().material = outline[teamNumber[selectNumber[i]]];
                            Debug.Log(PlayerObj[selectNumber[i]].name + teamNumber[selectNumber[i]]);
                        }
                        
                    }
                    //自分の番号を選択
                    else if (!entryFlg[selectNumber[i]] && selectNumber[i] == i)
                    {
                        //参加
                        if (Input.GetButtonUp("Fire" + (i + 1).ToString()) && !existenceFlg[i])
                        {
                            Debug.Log("PLEntry");
                            existenceFlg[i] = true;
                            entryFlg[i] = true;
                            text[i].text = entryText;
                            if (!PlayerObj[i])
                            {
                                PlayerObj[i] = (GameObject)Instantiate(pObj[0], spawnPos[i], Quaternion.Euler(0,quaternion[selectNumber[i]],0));
                                PlayerObj[i].name = pObj[i].name;
                            }
                            Abuttons[i].SetActive(false);
                        }

                        //戻る
                        else if (Input.GetButtonUp("Fire" + (i + 1).ToString()))
                        {
                            select[i] = false;
                            Debug.Log("selectEnd");
                        }
                    }

                    //自分以外の番号を選択
                    else if (!entryFlg[selectNumber[i]]&&selectNumber[i]!=i)
                    {
                        if (Input.GetButtonUp("Fire" + (i + 1).ToString())&&!existenceFlg[selectNumber[i]])
                        {
                            AICreate(i);
                            //Abuttons[i].SetActive(false);
                        }

                        //戻る
                        else if (Input.GetButtonUp("Fire" + (i + 1).ToString()))
                        {
                            select[i] = false;
                            Debug.Log("selectEnd");
                        }
                    }

                }

                //セレクト処理
                else if (selectMode[i]&&!select[i])
                {
                    if (Input.GetAxis("HorizontalL" + (i+1).ToString()) >= 1&&slideCursor[i])
                    {
                        slideCursor[i] = false;
                        StartCoroutine(CursorMoveTimeWait(i));
                        selectNumber[i] = I_NumberLoop(selectNumber[i], 1,3);
                    }
                    else if (Input.GetAxis("HorizontalL" + (i+1).ToString()) <= -1&&slideCursor[i])
                    {
                        slideCursor[i] = false;
                        StartCoroutine(CursorMoveTimeWait(i));
                        selectNumber[i] = I_NumberLoop(selectNumber[i], -1,3);
                    }

                    if (Input.GetButtonUp("Fire" + (i + 1).ToString()))
                    {
                        select[i] = true;
                        Debug.Log(selectNumber[i]);
                    }

                    if (Input.GetButtonUp("Jump" + (i + 1).ToString()))
                    {
                        selectMode[i] = false;
                        Debug.Log("selectmode=False");
                    }
                }

                

                //Player参加
                else if (Input.GetButtonUp("Fire"+(i+1).ToString()) && !entryFlg[i])
                {
                    PLCreate(i);
                }
                //参加取り消し
                else if (Input.GetButtonUp("Jump"+ (i + 1).ToString()) && entryFlg[i]&&!selectMode[i])
                {
                    NotEntry(i);
                    select[i] = false;
                    selectMode[i] = false;
                }

                //選択モード移行処理
                else if (entryFlg[i] && Input.GetButtonUp("Fire" + (i + 1).ToString()))
                {
                    selectMode[i] = true;
                    Debug.Log("selectmode");
                }
                //選択モード解除
                else if (selectMode[i] && Input.GetButtonUp("Jump" + (i + 1).ToString()))
                {
                    selectMode[i] = false;
                    Debug.Log("selectmodeend");
                }

                //誰かがEntyしたら一度だけNPCをだす
                if (entryFlg[i] && anyEntry == false)
                {
                    anyEntry = true;
                    for(int j = 0; j < 4; j++)
                    {
                        if (!entryFlg[j])
                        {
                            AICreate(j);
                        }
                    }
                }
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

    /// <summary>
    /// プレイヤーとしての生成
    /// </summary>
    /// <param name="i"></param>
    void PLCreate(int i)
    {
        existenceFlg[i] = true;
        entryFlg[i] = true;
        text[i].text = entryText;
        if (!PlayerObj[i])
        {
            PlayerObj[i] = (GameObject)Instantiate(pObj[0], spawnPos[i], Quaternion.Euler(0, quaternion[selectNumber[i]], 0));
            PlayerObj[i].name = pObj[i].name;
            Debug.Log("PLCreate");
        }
        else
        {
            Debug.Log("yetCreate");
        }
        Abuttons[i].SetActive(false);
    }

    /// <summary>
    /// AIとしての生成
    /// </summary>
    /// <param name="i"></param>
    void AICreate(int i)
    {
        Debug.Log("aiEntry");
        existenceFlg[selectNumber[i]] = true;
        text[selectNumber[i]].text = NPCText;
        if (!PlayerObj[selectNumber[i]])
        {
            PlayerObj[selectNumber[i]] = (GameObject)Instantiate(pObj[selectNumber[i]], spawnPos[selectNumber[i]], Quaternion.Euler(0, quaternion[selectNumber[i]], 0));
            PlayerObj[selectNumber[i]].name = pObj[selectNumber[i]].name;
        }
    }

    /// <summary>
    /// 参加を消す
    /// </summary>
    /// <param name="i"></param>
    void NotEntry(int i)
    {
        existenceFlg[i] = false;
        entryFlg[i] = false;
        text[i].text = noEntryText;
        PlayerObj[i] = null;
        Destroy(GameObject.Find("Entry" + (i + 1).ToString()));
        Abuttons[i].SetActive(true);
    }

    /// <summary>
    /// カーソル操作
    /// </summary>
    void CursorControl()
    {
        for(int i = 0; i < 4; i++)
        {
            if (selectMode[i])
            {
                cursor[i].SetActive(true);
                if (select[i])
                {
                    cursorRect[i].sizeDelta = new Vector2(130, 270);
                }
                else
                {
                    cursorRect[i].sizeDelta = new Vector2(140, 280);
                }
                cursorRect[i].position = GameObject.Find((selectNumber[i] + 1).ToString() + "pText").GetComponentInChildren<RectTransform>().position-posMinus;
            }
            else
            {
                cursor[i].SetActive(false);
            }
        }
    }


    /// <summary>
    /// Cursorの待ち時間処理
    /// </summary>
    /// <returns></returns>
    IEnumerator CursorMoveTimeWait(int i)
    {
        if (isExecute[i] || slideCursor[i])
        {
            yield break;
        }
        isExecute[i] = true;
        yield return new WaitForSeconds(cursorMoveTime);
        slideCursor[i] = true;
        isExecute[i] = false;
    }

    /// <summary>
    /// intループ処理(1,-1のみ)
    /// </summary>
    /// <param name="num">対象</param>
    /// <param name="value">値</param>
    /// <param name="max">最大値</param>
    /// <returns></returns>
    int I_NumberLoop(int num,int value,int max)
    {
        num += value;
        if (num < 0)
        {
            num = max;
        }
        else if(num > max)
        {
            num = 0;
        }
        return num;
    }

    /// <summary>
    /// シーン切り替え
    /// </summary>
    void ChangeScene()
    {
        GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect(sceneName);
    }
}
