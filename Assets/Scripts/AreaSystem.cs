﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaSystem : MonoBehaviour
{
    [SerializeField, Header("変更間隔")]
    private float changeTime;
    [SerializeField, Header("WAVE間の時間")]
    private float nextTime;
    [SerializeField, Header("プレイヤー")]
    private List<GameObject> PlayerList=new List<GameObject>();
    [SerializeField, Header("AIプレイヤー")]
    private List<GameObject> AIPlayerobjList=new List<GameObject>();
    [SerializeField]
    private FlowText flowtext;
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private GameObject spawnSmoke;
    [SerializeField]
    private Text timeText;
    [SerializeField,Header("アイテム")]
    private List<GameObject> Items=new List<GameObject>();

    [SerializeField]
    private GameObject[] AreaObject;

    private float timer;//残り時間

    private const int spcount = 20;//沸き数
    private const int areDis = 8;//エリアの中心からの距離

    private int nowArea;

    private Vector3 smoke_Offset = new Vector3(0, 0, -5);

    MultipleTargetCamera multipleCamera;


    void Start()
    {
        nowArea = 0;
        StartCoroutine(AreaEnumerator());
        timer = changeTime+1;
        PlayerSpawn(AreaObject[0].transform.position);
        foreach (AIPlayer Aip in FindObjectsOfType<AIPlayer>())
        {
            Aip.AreaPos = AreaObject[0].transform.position;
        }
        foreach (PlayerMove Pm in FindObjectsOfType<PlayerMove>())
        {
            Pm.AreaPos = AreaObject[0].transform.position;
        }
        multipleCamera = FindObjectOfType<MultipleTargetCamera>();
    }

    void Update()
    {
        timeText.text = Mathf.Floor(timer).ToString();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(timer<0)
        {
            timer = 0;
        }
    }


    void AreaFinish(Vector3 pos)
    {
        foreach (AIPlayer Aip in FindObjectsOfType<AIPlayer>())
        {
            Aip.AreaPos = pos;
        }
        foreach (PlayerMove Pm in FindObjectsOfType<PlayerMove>())
        {
            Pm.AreaPos = pos;
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(g);
        }
        MobChangeSystem.MobDelete();      
    }

    IEnumerator AreaStart(Vector3 pos)
    {
        foreach (AIPlayer Aip in FindObjectsOfType<AIPlayer>())
        {
            Aip.AreaPos = pos;
        }
        foreach (PlayerMove Pm in FindObjectsOfType<PlayerMove>())
        {
            Pm.AreaPos = pos;
        }
        Instantiate(spawnSmoke, pos+smoke_Offset, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < spcount; i++)
        {
            Vector3 spwpos;
            Quaternion qua = RandomQua();
            MobSpawnPos(pos, out spwpos);
            MobChangeSystem.HumanSpawn(spwpos, qua);
        }
    }

    void MobSpawnPos(Vector3 areapos, out Vector3 sppos)
    {
        float x = 0, z = 0;
        int c=0;
        Ray ray;
        RaycastHit hit;
        do
        {
            if (c > 20)
            {
                x = areapos.x;z = areapos.z;
                break;
            }
            x = Random.Range(areapos.x + areDis, areapos.x - areDis);
            z = Random.Range(areapos.z + areDis, areapos.z - areDis);
            ray = new Ray(new Vector3(x, 20, z), Vector3.down);
            Physics.Raycast(ray, out hit);
            c++;
        } while (hit.collider.tag != "Area");
        sppos = new Vector3(x, 1.5f, z);
    }

    IEnumerator AreaEnumerator()
    {
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(ItemCoroutine());
            timer = changeTime + 1;

            flowtext.ChangeWave = true;
            nowArea = i;                //アイテム処理で使用
            yield return StartCoroutine(AreaStart(AreaObject[i].transform.position));
            if (i < 3)
            {
                yield return new WaitWhile(() => timer >= 10);
                AreaObject[i + 1].SetActive(true);
            }
            yield return new WaitWhile(() => timer >= 0);

            AreaObject[i].SetActive(false);
            //次のwaveへ
            if (i < 3)
            {
                nowArea = i + 1;
                AreaFinish(AreaObject[i + 1].transform.position);
                timer = nextTime + 1;
                flowtext.WaveFinish = true;
                yield return new WaitWhile(() => timer >= 0);
            }
        }
        //リザルトへ
        GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect(sceneName);
    }

    void PlayerSpawn(Vector3 pos)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 spwpos;
            Quaternion qua = RandomQua();
            MobSpawnPos(pos, out spwpos);
            GameObject obj;
            if (EntrySystem.entryFlg[i])
            {
                obj = (GameObject)Instantiate(PlayerList[i], spwpos, qua);
            }
            else
            {
                obj = (GameObject)Instantiate(AIPlayerobjList[i], spwpos, qua);
            }
            obj.GetComponent<PlayerNumber>().PlayerNum = i + 1;
            CameraAddPlayer(obj);
        }
    }


    Quaternion RandomQua()
    {
        Quaternion qu;
        float Ry = Random.Range(0f, 360f);
        qu = Quaternion.Euler(0f, Ry, 0f);
        return qu;
    }

    IEnumerator ItemCoroutine()
    {
        int item = 0;
        while (true)
        {
            yield return new WaitForSeconds(11.25f);
            item = Random.Range(0, 2);
            if (item == 0)
            {
                ItemCreate(item);
            }
            else if (item == 1 && !GameObject.Find(Items[item].name))
            {
                ItemCreate(item);
            }
        }
    }

    void ItemCreate(int item)
    {
        int Qx=0;
        switch (item)
        {
            case 0:
                Qx = 0;
                break;
            case 1:
                Qx = -90;
                break;
        }
        Vector3 offset = new Vector3(0, -0.5f, 0);
        Vector3 spwpos;
        MobSpawnPos(AreaObject[nowArea].transform.position, out spwpos);
        GameObject obj = (GameObject)Instantiate(Items[item], spwpos+offset, Quaternion.Euler(Qx,0,0));
        obj.name = Items[item].name;
    }

    void CameraAddPlayer(GameObject obj)
    {
        MultipleTargetCamera mtc = GameObject.Find("Camera").GetComponent<MultipleTargetCamera>();
        mtc.targets.Add(obj);
    }
}
