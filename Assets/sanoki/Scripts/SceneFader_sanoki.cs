using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader_sanoki : MonoBehaviour
{
    ///////////////////////////////////////////////////////////
    /// このscriptはシーン上のCanvasにアタッチしてください。/// 
    ///////////////////////////////////////////////////////////

    float red, green, blue; //RGBを操作するための変数
    public static string next_Scene;//遷移先のシーン名を入れる
    public static bool isFade = false;//フェード中かどうか
    int fade;//FadeIn：１ FadeOut：０

    public Color fadeColor;//フェード時の色

    float fadeSpeed = 1.5f;//フェードの速さ

    void Start()
    {
        fade = 0;//シーンの読み込み時にフェードアウトさせる
        StartCoroutine(SceneFade(fadeSpeed, false));//フェードさせる
    }

    /// <summary>
    /// 引数のシーンにフェードして遷移する
    /// </summary>
    /// <param name="nextSceneName">遷移先のシーン名</param>
    public void StageSelect(string nextSceneName)
    {
        if (!isFade)//フェード中でないなら
        {
            isFade = true;//フェード中に変更する
            next_Scene = nextSceneName;//引数を遷移先に指定
            fade = 1;//フェードインさせる
            StartCoroutine(SceneFade(fadeSpeed,true));//フェードさせる
        }
    }

    /// <summary>
    /// フェードだけする
    /// </summary>
    /// <param name="fadeSpeed">フェードの速度</param>
    public void OnFade(float fadeSpeed)
    {
        if (!isFade)//フェード中でないなら
        {
            isFade = true;//フェード中に変更
            fade = 1;//フェードインに設定
            StartCoroutine(SceneFade(fadeSpeed, false));//コルーチンの呼び出し
        }
    }

    //フェード用のGUI
    void OnGUI()
    {   
        GUI.color = fadeColor;//設定されている色にする
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),Texture2D.whiteTexture);//が面大の幕を表示
    }

    /// <summary>
    /// フェード用コルーチン
    /// </summary>
    /// <param name="seconds">何秒かけてフェードさせるのか</param>
    /// <param name="sceneChange">シーン遷移させるのかどうか(true:させる　false:させない)</param>
    /// <returns></returns>
    public IEnumerator SceneFade(float seconds,bool sceneChange)
    {
        float t = 0.0f;//時間をリセット
        switch (fade)//1:フェードイン　0:フェードアウト
        {
            case 1:
                //Debug.Log("フェードイン");
                while (t < 1.0f)//時間になるまで繰り返す
                {
                    t += Time.deltaTime / seconds;//時間経過
                    fadeColor.a = Mathf.Lerp(0.0f, 1.0f, t);//アルファ値を徐々に上げる
                    yield return null;//1フレーム待つ
                }
                if (sceneChange) { SceneManager.LoadScene(next_Scene); }//シーン遷移
                else
                {
                    fade = 0;//フェードアウトに設定
                    StartCoroutine(SceneFade(fadeSpeed, false));//コルーチンを呼び出し
                }
                break;
            case 0:
                while (t < 1.0f)//時間になるまで繰り返す
                {
                    t += Time.deltaTime / seconds;//時間経過
                    fadeColor.a = Mathf.Lerp(1.0f, 0.0f, t);//アルファ値を徐々に下げる
                    yield return null;//1フレーム待つ
                }
                isFade = false;//フェード中状態を解除
                break;
        }
    }
}