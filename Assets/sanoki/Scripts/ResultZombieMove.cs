using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultZombieMove : MonoBehaviour {
    BoxCollider col;//ボックスコライダー
    Animator ani;//animator
    Vector3 pos;//座標
    Vector3 qua;//回転
    float moveSpeed = 10.0f;//落ちる速度
    bool moveflg = true;//動くフラグ
    private void Start()
    {
        col = GetComponent<BoxCollider>();//ボックスコライダーを取得
        ani = GetComponent<Animator>();//Animatorコンポーネントを取得
        pos = transform.position;//初期位置を代入
        qua.y = Random.value * 360;//0.0f～360.0fの間のランダムな数字を取得
        transform.Rotate(qua);//回転
    }

    void Update()
    {
        if (moveflg)//moveFlgがtrueなら
        {
            pos.y -= moveSpeed * Time.deltaTime;//Y座標の値をひたすらマイナス
            transform.position = pos;//オブジェクトの座標に反映
        }
    }

    public bool rtnFlg()
    {
        return moveflg;
    }
    public string rtnTag()
    {
        return transform.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (moveflg)//moveFlgがtrueなら
        {
            moveflg = false;//フラグをfalseに
            if (other.tag == "grownd")//地面に着地したときの処理
            {
                BoxCollider stageCol=GetComponent<BoxCollider>();//地面のコライダーを取得
                pos.y = other.gameObject.transform.position.y + ((other.transform.localScale.y + col.size.y)/2)-col.center.y;//正しい着地位置に微調整
            }
            else//ゾンビの上に着地したときの処理
            {
                pos.y = other.gameObject.transform.position.y + ((other.transform.localScale.y + col.size.y) / 2) - col.center.y;//正しい着地位置に微調整
                transform.parent = other.transform;//触れたオブジェクトの子に設定
            }
            transform.position = pos;//微調整した位置に移動
            ani.SetTrigger("landing");//地面に触れたら落下モーションを再生
            //Debug.Log("落ちた");
        }
    }

}
