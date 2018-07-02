using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;

public class MobChangeSystem : MonoBehaviour
{
    public Text[] tx =new Text[5];
    [SerializeField]
    private GameObject[] objs;//モブのprefab

    [SerializeField]
    private Material[] humanS=new Material[4];

    [SerializeField]
    private Material[] outLine = new Material[4];

    private static Material[] OutLine = new Material[4];
    private static Material[] HumanMaterialS = new Material[4];
    private static GameObject[] mobZombies=new GameObject[5];//モブのprefab
    public static int[] scoreCount=new int[4];
    private int[] NowZombiNum=new int[5];

    //0は市民,1～4がゾンビ
    
    
    
    void Awake()
    {
        for(int i = 0; i < objs.Length; i++)
        {
            mobZombies[i] = objs[i];
        }
        for(int i = 0; i < outLine.Length; i++)
        {
            OutLine[i] = outLine[i];
        }
        for (int i = 0; i < humanS.Length; i++)
        {
            HumanMaterialS[i] = humanS[i];
        }
    }

    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if (tx[i])
            {
                NowZombiNum[i] = MobCount(i);
                tx[i].text = NowZombiNum[i].ToString();
            }
        }
    }

    public static int MobCount(int num)//mobオブジェクトの番号がnumと一致するオブジェクトの数をcountに渡すはず
    {
        GameObject[] mobs;
        mobs = GameObject.FindGameObjectsWithTag("Mob").
        Where(e => e.GetComponent<PlayerNumber>().PlayerNum == num).
        ToArray();
        return mobs.Length;
    }

    public static void MobChanger(GameObject obj, int num)
    {

        //Instantiate(pars[num - 1], new Vector3(obj.transform.position.x,0.7f,obj.transform.position.z),Quaternion.Euler(-90,0,0));
        if (obj.GetComponent<PlayerNumber>().PlayerNum == 0)
        {
            if (obj.GetComponent<HumanMove>().Smoke == false)
            {
                GameObject zombi;
                zombi = (GameObject)Instantiate(mobZombies[num], obj.transform.position, obj.transform.rotation);
                zombi.GetComponent<NavMeshAgent>().enabled = true;
                Destroy(obj);
            }
            else
            {
                return;
            }
        }
        else
        {
            SkinnedMeshRenderer s = obj.GetComponentInChildren<SkinnedMeshRenderer>();
            s.material = OutLine[num - 1];
            obj.GetComponent<PlayerNumber>().PlayerNum = num;
        }
    }

    public static void HumanSpawn(Vector3 pos,Quaternion qu)
    {
        int s = UnityEngine.Random.Range(0,4);
        GameObject obj;
        obj = (GameObject)Instantiate(mobZombies[0], pos, qu);
        obj.GetComponent<NavMeshAgent>().enabled = true;
        switch(s)
        {
            case 0:
                obj.GetComponent<HumanMove>().skin.material = HumanMaterialS[0];
                break;

            case 1:
            obj.GetComponent<HumanMove>().skin.material =HumanMaterialS[1];
                break;

            case 2:
                obj.GetComponent<HumanMove>().skin.material = HumanMaterialS[2];
                break;

            case 3:
                obj.GetComponent<HumanMove>().skin.material = HumanMaterialS[3];
                break;
        }
    }

    public static void MobDelete()
    {
        for(int i = 0; i < 4; i++)
        {
            scoreCount[i] += MobCount(i + 1);
        }
        foreach (GameObject mob in GameObject.FindGameObjectsWithTag("Mob"))
        {
            Destroy(mob);
        }
    }
}

