using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieInstant : MonoBehaviour {
    public GameObject cam;
    public GameObject[] instantPos;//0:赤 1:青 2:緑 3:黄
    public GameObject[] zombiePre;//ゾンビプレハブ
    public GameObject[] playerZom;//プレイヤーキャラ
    public GameObject[] crowns;//王冠プレハブ
    public Text[] winnerTexts=new Text[4];
    public Image[] winnerImages;
    public GameObject drawImage;
    public Text[] drawTexts;
    public ParticleSystem[] particles=new ParticleSystem[4];
    GameObject[] particlesGam;
    public GameObject[] particlePoss;
    ParticleSystem[] particlesIns = new ParticleSystem[4];
    BoxCollider col;//ボックスコライダー
    Vector3 pos;//座標
    public int[] scores;//0:赤 1:青 2:緑 3:黄
    public int[] playerID= { 0, 1, 2, 3 };
    public int Count;
    public bool ParticlePlay;
    public bool isRanking = false;

    private void Start()
    {
        isRanking = true;
         col=zombiePre[0].GetComponent<BoxCollider>();//ボックスコライダーを取得
    }
    void Update () {
        if (Input.GetKeyDown(KeyCode.D)) Instans(scores);
        if (!SceneFader_sanoki.isFade && Count == 0 && MobChangeSystem.scoreCount[0] > 0)
        {
            Instans(MobChangeSystem.scoreCount);
        }
        if (!isRanking &&Input.anyKeyDown)
        {
            GameObject.Find("Canvas").GetComponent<SceneFader_sanoki>().StageSelect("Title");
        }
    }
    string autTagCnange(int preID)
    {
        string preTag = "None";
        switch (preID)
        {
            case 0:
                preTag = "Red";
                break;
            case 1:
                preTag = "Blue";
                break;
            case 2:
                preTag = "Green";
                break;
            case 3:
                preTag = "Yellow";
                break;
        }
        return preTag;
    }
    public void Instans(int[] score)
    {
        StartCoroutine(ScoreCount(score));//生成コルーチンを呼び出し
    }
    private IEnumerator ScoreCount(int[] score)
    {
        Count = 1;
        int highScore = score[0];//プレイヤー1のスコアを取得
        int highScorePlayer = 0;
        for (int i = 1; i < instantPos.Length; i++)//プレイヤーの数だけ繰り返す
        {
            if (highScore < score[i])//スコアの大きい方を比較
            {
                highScore = score[i];//ハイスコアの更新
                highScorePlayer = i;
            }
        }
        for (int j = 0; j <= highScore; j++)//ハイスコアの数だけ繰り返す
        {
            for (int k = 0; k < instantPos.Length; k++)//生成位置の数だけ繰り返す
            {
                if (j < score[k])//各プレイヤーのスコア以下なら
                {
                    pos = instantPos[k].transform.position;//生成位置座標を取得
                    //生成
                    Instantiate(
                        zombiePre[k],                        // プレハブ
                        instantPos[k].transform.position, // 座標
                        Quaternion.identity               // 回転
                        ).tag = autTagCnange(k);
                    pos.y += col.size.y;//生成位置をゾンビプレハブ１個分上にずらす
                    instantPos[k].transform.position = pos;
                }
            }
            yield return new WaitForSeconds(0.1f);//0.1秒待つ
        }
        int maxScore = SortArray(score, playerID,true)[0];
        if (SortArray(score,playerID,false)[0]!= SortArray(score, playerID, false)[1])
        {
            ParticleSwitch();
            Invoke("ParticleSwitch", 1.0f);
            crowns[SortArray(score, playerID, true)[0]].SetActive(true);
            FindObjectOfType<ResultCam_sanoki>().camMove(SortArray(score, playerID, true)[0]);
        }
        else if(SortArray(score, playerID, false)[0] == SortArray(score, playerID, false)[1])
        {
            for (int i = 0; i < score.Length; i++)
            {
                drawTexts[SortArray(score, playerID, true)[i]].text = SortArray(score, playerID, false)[i].ToString();
            }
            drawImage.SetActive(true);
            isRanking = false;
        }

    }
    void ParticleSwitch()
    {
        ParticlePlay = !ParticlePlay;
        if (ParticlePlay)
        {
            for (int i = 0; i < particles.Length; i++)
            {

                particlesIns[i] = Instantiate(particles[i], particlePoss[i].transform.position, Quaternion.identity);
                particles[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particlesIns[i].enableEmission = ParticlePlay;
            }
        }
    }
    int[] SortArray(int[] score, int[] playerID, bool isID)
    {
        bool isEnd = false;
        while (!isEnd)
        {
            bool loopSwap = false;
            for (int i = 0; i < score.Length - 1; i++)
            {
                if (score[i] < score[i + 1])
                {
                    int x = score[i];
                    int ID = playerID[i];
                    score[i] = score[i + 1];
                    playerID[i] = playerID[i + 1];
                    score[i + 1] = x;
                    playerID[i + 1] = ID;
                    loopSwap = true;
                }
            }
            if (!loopSwap) // Swapが一度も実行されなかった場合はソート終了
            {
                isEnd = true;
            }
        }
        if (score[0] != score[1])
        {
            for (int i = 0; i < score.Length; i++)
            {
                winnerTexts[i].text = score[i].ToString();
                winnerImages[i].sprite = FindObjectOfType<SpriteInfo>().sprits[playerID[i]].spr;
            }
        }
        if(isID)return playerID;
        return score;
    }
}
