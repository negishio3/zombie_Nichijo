using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public abstract class AIBase : PlayerNumber {
    [SerializeField, Header("歩き移動速度")]
    protected float walkSpeed = 1.5f;
    [SerializeField, Header("走り移動速度")]
    protected float runSpeed = 3.7f;
    [SerializeField, Header("obj探索範囲")]
    protected float searchDistance;
    [SerializeField, Header("視野角")]
    protected float viewAng=0.85f;
    [SerializeField, Header("再行動開始待ち時間")]
    protected float waitMoveTime = 2;
    //[SerializeField, Header("頭オブジェクト")]
    //protected GameObject headObj;



    protected NavMeshAgent navMeshAgent;
    protected GameObject[] targetMobs;          //対象候補
    protected GameObject target;                //現在の対象

    protected Vector3 Mypos;                    //自分の現在地
    protected Vector3 nextPos;                  //次の移動地点
    protected int num;                          //対象を選ぶ番号
    protected int numberToLookAround;           //見回すまでの移動回数
    protected const int loseSightTime = 5;      //見失うまでの時間
    protected float targetDistance;             //ターゲットと次の移動場所の距離
    protected float nextPosDistance;            //次の場所までの距離
    protected float stopTrackingTime;           //0になったら見失う
    protected bool TrackingFlg;                 //見失ったかどうか
    protected bool recastFlg;                   //攻撃後行動開始できるか否か
    protected string targetTag;                 //検索対象のタグ
    protected const int navmeshMask = ~(1 << 3);


    protected virtual void Start ()
    {
        //Renderer r;
        //r = GetComponent<Renderer>();
        //switch (playerNum)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        r.material.color = Color.red;
        //        break;
        //    case 2:
        //        r.material.color = Color.blue;
        //        break;
        //    case 3:
        //        r.material.color = Color.yellow;
        //        break;
        //    case 4:
        //        r.material.color = Color.green;
        //        break;
        //}

        navMeshAgent = GetComponent<NavMeshAgent>();
        recastFlg = true;
        stopTrackingTime = 0;

    }

	protected virtual void Update ()
    {
        InSight();
	}

    protected abstract void OnTriggerEnter(Collider col);

    protected void InSight()//視界に入っているか確認
    {
        SearchObj(targetTag, out targetMobs);
        if (targetMobs != null)//targetmobsに中身があるならtrue
        {
            if (targetMobs.Length!=0)
            {
                num = 0;
                while (targetMobs.Length > num)
                {
                    if (RayCheck())
                    {
                        target = targetMobs[num];
                        stopTrackingTime = loseSightTime;
                        TrackingFlg = true;
                        return;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
        }
    }

    bool RayCheck()//レイが通るならtrue
    {
        if (targetMobs.Length != 0)//対象が存在するか
        {
            Vector3 dir = targetMobs[num].transform.position - transform.position;

            if (ViewingAngle(dir,transform.forward,viewAng))//視界に入っているか
            {
                Ray ray = new Ray(transform.position, dir);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == targetTag)
                    {
                        //Debug.Log("Insight");
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    protected bool ViewingAngle(Vector3 dir,Vector3 dir2,float viewAngle)//正面にいるならtrue
    {
        dir.Normalize();
        dir2.Normalize();
        float dot = Vector3.Dot(dir, dir2);
        float rad = Mathf.Acos(dot);
        if (rad < viewAngle)//視野角に入っているか
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void SearchObj(string tag, out GameObject[] objs)//条件を満たしているobjを近い順に取得する
    {
        if (GameObject.FindGameObjectWithTag(tag))//市民の処理
        {
            objs = GameObject.FindGameObjectsWithTag(tag).
            Where(e => Vector3.Distance(transform.position, e.transform.position) < searchDistance).//範囲内で
            OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).ToArray();     //近い順に並び替え
        }
        else
        {
            objs = null;
        }
    }

    protected bool GetRandomPosition(Vector3 center, float range, out Vector3 result)
    //ランダムに移動場所を取得
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, navmeshMask))
            {
                Ray ray; RaycastHit rayhit;
                ray = new Ray(hit.position, Vector3.down);
                if (Physics.Raycast(ray,out rayhit, 1f))
                {
                    if (rayhit.collider.tag == "Area")
                    {
                        result = hit.position;
                        return true;
                    }
                }
            }
        }
        result = Vector3.zero;
        return false;
    }

    protected abstract void MoveRandom(float range);//移動処理

    //protected virtual void ToLookAround()
    //{
    //    //あたりを見回す

    //}

    protected IEnumerator RecastTime(float time)//攻撃後硬直
    {
        yield return new WaitForSeconds(time);
        recastFlg = true;
    }


}
