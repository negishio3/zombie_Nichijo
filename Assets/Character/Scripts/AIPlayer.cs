using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AIPlayer : AIBase
{
    public enum MoveState
    {
        WAIT,       //待機
        MOVE,       //移動
        STALKING,   //追跡
        ATACK,      //攻撃
        ITEM,        //アイテムを取りに行く
        AREA
    }
    [SerializeField]
    protected MoveState moveState=MoveState.MOVE;

    public GameObject createpos;
    [SerializeField]
    private GameObject atk;
    [SerializeField]
    private gero geroScr;

    protected bool atkFlg=true;         //攻撃中か否か
    protected float randomPosRange=30;  //移動場所ランダム範囲
    protected GameObject atackedTarget; //直近の攻撃済みのtarget

    private GameObject itemTarget;

    private Animator anim;
    private AnimatorStateInfo stateInfo;
    private AreaGuideLine areaGuideLine;

    private float defaltSpeed=5.2f;
    private float timecCountOfRecast=0;

    private Vector3 areaPos;

    public Vector3 AreaPos
    {
        set { areaPos = value;
            moveState = MoveState.AREA;
            if (navMeshAgent)
            {
                navMeshAgent.SetDestination(areaPos);
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        targetTag = "Mob";
        navMeshAgent.speed=defaltSpeed;
        areaGuideLine = GetComponentInChildren<AreaGuideLine>();
    }

    protected override void Update()
    {
        if (!recastFlg)
        {
            timecCountOfRecast += Time.deltaTime;
            if (timecCountOfRecast > 3.5f)
            {
                recastFlg = true;
            }
        }

        if (stateInfo.normalizedTime<0.9) { anim.SetBool("Bress", false); }

        if (stopTrackingTime <= 0)//一定時間視界に入らなかったら見失う
        {
            target = null;
            TrackingFlg = false;
        }
        else
        {
            stopTrackingTime -= Time.deltaTime;
        }

        base.Update();

        if (GameObject.FindGameObjectWithTag("Item"))
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Item").
                Where(e => Vector3.Distance(transform.position, e.transform.position) < 10).
                OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).ToArray();
            if (obj.Length != 0)
            {
                itemTarget = obj[0];
                moveState = MoveState.ITEM;
            }
        }

        switch (moveState)
        {
            case MoveState.WAIT:
                Wait();
                break;
            case MoveState.MOVE:
                Move();
                break;
            case MoveState.STALKING:
                Stalking();
                break;
            case MoveState.ATACK:
                Atack();
                break;
            case MoveState.ITEM:
                Item();
                break;
            case MoveState.AREA:
                Area();
                break;
        }

        if (!recastFlg)//攻撃中なら対象のほうへ向く
        {
            AtackRotation();
        }


        if (transform.position == Mypos)
        {
            Ray raycast = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(raycast, out hit))
            {
                if (hit.collider.tag != "Area")
                {
                    navMeshAgent.SetDestination(areaPos);
                    moveState = MoveState.AREA;
                }
            }
        }

        Mypos = transform.position;
    }


    void Wait()
    {
        //待機処理  不要なら削除
        if (recastFlg)
        {
            moveState = MoveState.MOVE;
        }
    }

    void Move()
    {
        if (TrackingFlg&&recastFlg)
        {
            moveState = MoveState.STALKING;
            return;
        }
        if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero||Mypos==transform.position)
        {
            MoveRandom(randomPosRange);
        }
    }


    void Stalking()
    {
        if (!TrackingFlg||!target)//見失ったらMOVEに変更
        {
            MoveRandom(randomPosRange);
            moveState = MoveState.MOVE;
        }
        else if (target)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 4.5f&&atkFlg)//接触していたらATACKに変更
            {
                moveState = MoveState.ATACK;
                StartCoroutine(AtkCor());
            }
            if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero || Mypos == transform.position)
            {
                if (navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    nextPos = target.transform.position;
                    navMeshAgent.SetDestination(nextPos);
                }
            }
        }
    }

    void Atack()
    {
        if (target&&recastFlg)
        {
            //攻撃処理
            anim.SetBool("Bress", true);
            recastFlg = false;
            moveState = MoveState.WAIT;
            StartCoroutine(RecastTime(waitMoveTime));
            atackedTarget = target;
            //MobChangeSystem.MobChanger(atackedTarget.transform.position, playerNum);
            //Destroy(atackedTarget);
            target = null;

        }
        else
        {
            moveState = MoveState.MOVE;
        }
    }

    void AtackRotation()//攻撃するとき対象に向く処理
    {
        if (atackedTarget)//対象がいるか確認
        {
            float rotationSpeed = 20;
            Vector3 dir = new Vector3(atackedTarget.transform.position.x, transform.position.y, atackedTarget.transform.position.z) - transform.position;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, rotationSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newdir);
        }
    }

    void Item()
    {
        if (itemTarget)
        {
            navMeshAgent.SetDestination(itemTarget.transform.position);
        }
        else
        {
            moveState = MoveState.MOVE;
        }
    }

    void Area()
    {
        areaGuideLine.LineRender.enabled = true;
        Ray raycast = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(raycast,out hit))
        {
            if (hit.collider.tag == "Area")
            {
                moveState = MoveState.MOVE;
                areaGuideLine.LineRender.enabled = false;
            }
        }
        areaGuideLine.LineWrite(transform.position, areaPos);
    }

    protected override void MoveRandom(float range)
    {
        if (GetRandomPosition(transform.position, range, out nextPos))
        {
            Vector3 nexdis = nextPos - transform.position;
            if (!ViewingAngle(nexdis,transform.forward, 1.5f))
            {
                MoveRandom(range);
            }
            else
            {
                navMeshAgent.SetDestination(nextPos);
            }
        }
    }

    protected override void SearchObj(string tag, out GameObject[] objs)
    {
        if (GameObject.FindGameObjectWithTag(tag))//tagのオブジェクトの存在確認
        {
            if (!recastFlg)
            {
                objs = GameObject.FindGameObjectsWithTag(tag).
                Where(e => Vector3.Distance(transform.position, e.transform.position) < searchDistance).//範囲内で
                Where(e=>Vector3.Distance(transform.position,e.transform.position)>3).
                Where(e => e.GetComponent<PlayerNumber>().PlayerNum != playerNum).                      //番号が異なるなら取得
                OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).ToArray();     //近い順に並び替え
            }
            else
            {
                objs = GameObject.FindGameObjectsWithTag(tag).
                Where(e => Vector3.Distance(transform.position, e.transform.position) < searchDistance).//範囲内で
                Where(e => e.GetComponent<PlayerNumber>().PlayerNum != playerNum).                      //番号が異なるなら取得
                OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).ToArray();     //近い順に並び替え
            }
        }
        else
        {
            objs = null;
        }
    }


    protected override void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Item")
        {
            col.GetComponent<ItemBase>().Execution(gameObject);
        }
    }
    IEnumerator AtkCor()
    {
        atkFlg = false;
        yield return new WaitForSeconds(0.6f);
        geroScr.ThrowingBall();
        yield return new WaitForSeconds(1.3f);
        atkFlg = true;
    }

    public void SpeedUp()
    {
        StartCoroutine(SpeedUpCoroutine());
    }

    IEnumerator SpeedUpCoroutine()
    {
        navMeshAgent.speed = defaltSpeed * 2;
        yield return new WaitForSeconds(10f);
        navMeshAgent.speed = defaltSpeed;
    }
}