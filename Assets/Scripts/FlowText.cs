using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowText : MonoBehaviour {
    public enum FlowState
    {
        STATEPOSITION,
        FLOWMOVING,
        STOPMOVE,
        FINISH
    }
    FlowState flowState;

    private RectTransform rect;
    private Text text;
    private int wave=1;   //現在のウェーブ
    private float speed=12f;

    public bool ChangeWave { get; set; }//waveがはじまったか
    public bool WaveFinish { get; set; }//waveがおわったか
    public bool GameFinish { get; set; }//ゲームが終わったか

    void Start() {
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        //初期位置に移動
        rect.localPosition = new Vector3(550f, 0, 0);
    }

    // Update is called once per frame
    void Update() {
        switch (flowState)
        {
            case FlowState.STATEPOSITION:
                StatePosition();
                break;
            case FlowState.FLOWMOVING:
                FlowMoving();
                break;
            case FlowState.STOPMOVE:
                StopMove();
                break;
            case FlowState.FINISH:
                Finish();
                break;
        }
    }

    void StatePosition()
    {
        //初期位置処理
        flowState = FlowState.FLOWMOVING;
        ChangeWave = false;
        text.text = "WAVE" + wave.ToString();
    }

    void FlowMoving()
    {
        //右から左へ流れる
        rect.localPosition -= new Vector3(Mathf.Abs(speed), 0, 0);
        //中心付近で原則
        if (rect.localPosition.x > -100 && rect.localPosition.x < 100)
        {
            speed = 5f;
        }
        //-550以降まで流れないように
        else if (rect.localPosition.x < -550)
        {
            flowState = FlowState.STOPMOVE;
        }
        //基本速度
        else
        {
            speed = 12f;
        }
    }

    void StopMove()
    {
        //流れた後の待機処理
        //初期位置に戻る
        rect.localPosition = new Vector3(550f, 0, 0);

        //ゲームが終わったか
        if (GameFinish)
        {
            flowState = FlowState.FINISH;
        }
        //1Waveが終わったか
        else if (WaveFinish)
        {
            flowState = FlowState.FLOWMOVING;
            WaveFinish = false;
            text.text = "NEXT";
        }
        //Waveが始まったか
        else if (ChangeWave)
        {
            flowState = FlowState.STATEPOSITION;
            wave++;
        }

    }

    void Finish()
    {
        text.text = "FINISH";
        rect.localPosition = new Vector3(0f, 0, 0);
    }
}
