using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TitleSpawn : MonoBehaviour {
    [SerializeField, Header("AIプレイヤー")]
    private List<GameObject> AIPlayerobjList = new List<GameObject>();
    [SerializeField]
    private string sceneName;
    private const int areDis = 15;
    private const int spcount = 15;
	void Start () {
        for (int i = 0; i < 4; i++)
        {
            Quaternion qua = RandomQua();
            GameObject obj;

            obj = (GameObject)Instantiate(AIPlayerobjList[i], MobSpawnPos(), qua);

            obj.GetComponent<NavMeshAgent>().enabled = true;
            obj.GetComponent<PlayerNumber>().PlayerNum = i + 1;
        }
        for (int i = 0; i < spcount; i++)
        {
            Quaternion qua = RandomQua();
            MobChangeSystem.HumanSpawn(MobSpawnPos(), qua);
        }

    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 1; i < 5; i++)
        {
            if (Input.GetButtonDown("Start" + i.ToString()))
            {
                GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect(sceneName);
            }
            else if (Input.anyKeyDown)
            {
                GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect(sceneName);
            }
        }
    }
    Quaternion RandomQua()
    {
        Quaternion qu;
        float Ry = Random.Range(0f, 360f);
        qu = Quaternion.Euler(0f, Ry, 0f);
        return qu;
    }
    Vector3 MobSpawnPos()
    {
        float x = 0, z = 0;
        int c = 0;
        Ray ray;
        RaycastHit hit;
        do
        {
            if (c > 20)
            {
                x =0; z =0;
                break;
            }
            x = Random.Range(areDis,- areDis);
            z = Random.Range(areDis,- areDis);
            ray = new Ray(new Vector3(x, 20, z), Vector3.down);
            Physics.Raycast(ray, out hit);
            c++;
        } while (hit.collider.tag != "Area");
        return new Vector3(x, 1.5f, z);
    }
}
