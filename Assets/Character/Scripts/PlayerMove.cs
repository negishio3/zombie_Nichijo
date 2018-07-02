using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : PlayerNumber
{
    //public GameObject atk;
    //public GameObject createpos;
    [SerializeField, Header("速度")]
    private float movespeed;
    [SerializeField]
    private gero geroScri;


    private CharacterController cCon;
    private Animator anim;
    private AnimatorStateInfo stateInfo;

    private AreaGuideLine areaGuideLine;


    private Vector3 vecInput;
    private bool atkFlg = true;
    private bool inArea=false;

    private float defaltSpeed;
    private float atktime=0;

    public Vector3 AreaPos { get; set; }
    //private Vector3 velocity;
    //private Vector3 vecRot;
    //private Vector3 graVelocity;

    //private int layerMask;

    void Start()
    {
        cCon = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        defaltSpeed = movespeed;
        areaGuideLine = GetComponentInChildren<AreaGuideLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!atkFlg)
        {
            atktime+=Time.deltaTime;
            if (atktime > 3.5f)
            {
                atkFlg = true;
            }            
        }
        if (stateInfo.normalizedTime < 0.1) { anim.SetBool("Bress", false); }

        if (Input.GetButtonDown("Fire" + playerNum.ToString())&&atkFlg)
        {
            anim.SetBool("Bress", true);
            StartCoroutine(AtkCor());
        }

        vecInput = new Vector3(Input.GetAxis("HorizontalL" + playerNum.ToString()), 0, Input.GetAxis("VerticalL" + playerNum.ToString()));
        vecInput *= movespeed;
        cCon.Move(vecInput * Time.deltaTime);
        if (Input.GetAxis("HorizontalL" + playerNum.ToString())!=0 || Input.GetAxis("VerticalL" + playerNum.ToString())!=0)
        {
            transform.rotation = Quaternion.LookRotation(transform.position +
            (Vector3.right * Input.GetAxisRaw("HorizontalL" + playerNum.ToString())) +
            (Vector3.forward * Input.GetAxisRaw("VerticalL" + playerNum.ToString()))
            - transform.position);
        }

        Ray raycast = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(raycast, out hit))
        {
            if (hit.collider.tag == "Area")
            {
                inArea = true;
                if (areaGuideLine.LineRender.enabled)
                {
                    areaGuideLine.LineRender.enabled = false;
                }
            }
            else
            {
                inArea = false;
                if (!areaGuideLine.LineRender.enabled)
                {
                    areaGuideLine.LineRender.enabled = true;
                }
            }
        }
        if (!inArea)
        {
            areaGuideLine.LineWrite(transform.position, AreaPos);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Item")
        {
            col.GetComponent<ItemBase>().Execution(gameObject);
        }
    }

    IEnumerator AtkCor()
    {
        if (atkFlg == false)
        {
            yield break;
        }
        atkFlg = false;
        atktime = 0;
        yield return new WaitForSeconds(0.6f);
        geroScri.ThrowingBall();
        yield return new WaitForSeconds(1.3f);
        atkFlg = true;
    }

    public void SpeedUp()
    {
        StartCoroutine(SpeedUpCoroutine());
    }

    IEnumerator SpeedUpCoroutine()
    {
        movespeed = defaltSpeed * 2;
        yield return new WaitForSeconds(10f);
        movespeed = defaltSpeed;
    }
}