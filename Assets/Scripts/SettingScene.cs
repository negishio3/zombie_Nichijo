using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingScene : MonoBehaviour {

    //BGM、SEのAudioSourceのoutputにAudioMixerのBGMとSEを割り当てる

    [SerializeField]
    private Slider[] _slider=new Slider[6];     //スライダー
    [SerializeField]
    private Toggle _toggle;
    [SerializeField]
    private Text[] _text = new Text[7];         //値用
    [SerializeField]
    private RectTransform[] textRect=new RectTransform[8];             //文のrecttransform
    [SerializeField]
    private Image Cursor;                       //カーソルのイメージ
    [SerializeField]
    private AudioMixer audioMixer;              //オーディオミキサー
    [SerializeField]
    private GameObject[] enabledFalseObj;         //設定画面を開いたとき非表示にするUI

  //  public static float f_ItemTime = 10;        //アイテムの沸き間隔 
    public static bool b_ItemFlg=true;          //アイテムのオンオフ
    public static float f_AreaTime = 45;        //1つのエリアでの時間
    public static float f_ChangeMoveTime = 15;  //エリア間の時間
    public static int i_MobNumber = 20;         //沸くモブの数
    public static int i_AreaNumber = 4;         //エリアの数
    private int i_BGMVolume = 50;               //BGM音量
    private int i_SEVolume = 50;                //SE音量


    private int settingSelect=1;                //選択項目を表す
    private int controlPlayerNum;                //操作するプレイヤー

    private const float cursorMoveTime=0.1f;    //カーソルが再移動可能になるまでの時間
    private const float sliderMoveTime = 0.06f; //スライダーがスライド可能になるまでの時間

    private bool selected;                      //その項目を選択
    private bool slideCursor=true;              //カーソルがスライド可能か
    private bool slideSlider = true;            //スライダーがスライド可能か
    private bool isExecute;                     //コルーチンが実行中か

    private RectTransform cursorRect;           //カーソルのrect

    //設定画面のオブジェクトのリスト
    private List<GameObject> SettingObj = new List<GameObject>();

    public bool NowSetting { get; set; }        //現在設定画面にいるか

    public int I_BGMVolume
    {
        get { return i_BGMVolume; }
        set { i_BGMVolume = value;
            BGMSet();
        }
    }

    public int I_SEVolume
    {
        get { return i_SEVolume; }
        set { i_SEVolume = value;
            SESet();
        }
    }


    public enum SettingType
    {
        NOSELECT,
        ITEMBOOL,
        //ITEMTIME,
        AREATIME,
        CHANGEMOVETIME,
        MOBNUMBER,
        AREANUMBER,
        BGMVOLUME,
        SEVOLUME,
        BACK
    }

    private SettingType settingType;


	void Start () {
        SettingReset();
        SettingUI();
        cursorRect = Cursor.GetComponent<RectTransform>();
        foreach (Transform obj in gameObject.transform)
        {
            SettingObj.Add(obj.gameObject);
            obj.gameObject.SetActive(false);
        }
    }


    void Update() {
        for (int i = 1; i < 5; i++)
        {
            if (!NowSetting&&Input.GetButtonDown("Setting"+i.ToString()))
            {   //設定画面を開く
                controlPlayerNum = i;
                NowSetting = true;
                foreach (GameObject obj in SettingObj)
                {
                    obj.SetActive(true);
                }
                foreach(GameObject obj in enabledFalseObj)
                {
                    obj.SetActive(false);
                }
            }
        }
        if (NowSetting)//設定画面を開いているか
        {
            TextUpdate();
            ChoiceSelection();


            //決定
            if (!selected && Input.GetButtonUp("Fire" + controlPlayerNum.ToString()))
            {
                selected = true;
                ItoE();
            }
            //選択解除
            else if (selected && (Input.GetButtonUp("Jump" + controlPlayerNum.ToString()) || (Input.GetButtonUp("Fire" + controlPlayerNum.ToString()))&&settingType!=SettingType.ITEMBOOL&&selected))
            {
                selected = false;
                settingType = SettingType.NOSELECT;
            }
            //設定画面を閉じる
            else if (Input.GetButtonUp("Jump" + controlPlayerNum.ToString()))
            {
                BackSeting();
            }


            if (!selected)//項目を選択しているか
            {
                settingType = SettingType.NOSELECT;

                //選択を上下に移動
                if (Input.GetAxis("VerticalL" + controlPlayerNum.ToString()) >= 1&&slideCursor)
                {
                    slideCursor = false;
                    StartCoroutine(CursorMoveTimeWait());
                    settingSelect--;
                }
                else if (Input.GetAxis("VerticalL" + controlPlayerNum.ToString()) <= -1&&slideCursor)
                {
                    slideCursor = false;
                    StartCoroutine(CursorMoveTimeWait());
                    settingSelect++;
                }

                //選択をループさせる
                if (settingSelect <= 0)
                {
                    settingSelect = 8;
                }
                else if (settingSelect >= 9)
                {
                    settingSelect = 1;
                }
            }

            if (selected)//項目を選択しているか
            {
                SwitchSettingType();
            }
        }
    }

    /// <summary>
    /// Cursorの待ち時間処理
    /// </summary>
    /// <returns></returns>
    IEnumerator CursorMoveTimeWait()
    {
        if (isExecute||slideCursor)
        {
            yield break;
        }
        isExecute = true;
        yield return new WaitForSeconds(cursorMoveTime);
        slideCursor = true;
        isExecute = false;
    }

    /// <summary>
    /// Sliderの待ち時間処理
    /// </summary>
    /// <returns></returns>
    IEnumerator SliderMoveTimeWait()
    {
        if (isExecute || slideSlider)
        {
            yield break;
        }
        isExecute = true;
        yield return new WaitForSeconds(cursorMoveTime);
        slideSlider = true;
        isExecute = false;
    }

    /// <summary>
    /// settingTypeをsettingSelectの番号のものに変える
    /// </summary>
    void ItoE()
    {
        settingType = (SettingType)Enum.ToObject(typeof(SettingType), settingSelect);
    }


    /// <summary>
    /// カーソル処理
    /// </summary>
    void ChoiceSelection()
    {
        if (settingType == SettingType.NOSELECT)//何も選択していない
        {
            //「戻る」用処理
            if (settingSelect == 8)
            {
                cursorRect.sizeDelta = new Vector2(100, 60);
                cursorRect.position = textRect[0].position;
            }
            //「戻る」以外
            else
            {
                cursorRect.sizeDelta = new Vector2(265, 50);
                cursorRect.position = textRect[settingSelect].position;
            }
        }
        else if (settingType == SettingType.ITEMBOOL)
        {
            cursorRect.sizeDelta = new Vector2(40, 40);
            cursorRect.position = _toggle.GetComponent<RectTransform>().position;
        }
        else//何か選択した
        {
            //スライダーにカーソルを合わせる
            cursorRect.sizeDelta = new Vector2(350, 50);
            cursorRect.position = _slider[settingSelect-2].GetComponent<RectTransform>().position;
        }

    }


    /// <summary>
    /// SettingTypeの対象の処理を実行
    /// </summary>
    void SwitchSettingType()
    {
        switch (settingType)
        {
            case SettingType.NOSELECT:
                break;

            //case SettingType.ITEMTIME:
            //    SliderControl(_slider[0]);
            //    break;

            case SettingType.ITEMBOOL:
                ToggleControll(_toggle);
                break;

            case SettingType.AREATIME:
                SliderControl(_slider[0]);
                break;

            case SettingType.CHANGEMOVETIME:
                SliderControl(_slider[1]);
                break;

            case SettingType.MOBNUMBER:
                SliderControl(_slider[2]);
                break;

            case SettingType.AREANUMBER:
                SliderControl(_slider[3]);
                break;

            case SettingType.BGMVOLUME:
                SliderControl(_slider[4]);
                break;

            case SettingType.SEVOLUME:
                SliderControl(_slider[5]);
                break;

            case SettingType.BACK:
                BackSeting();
                break;
        }
    }

    /// <summary>
    /// Textの内容を更新する
    /// </summary>
    void TextUpdate()
    {
        if (_toggle.isOn)
        {
            _text[0].text = "On";
        }
        else
        {
            _text[0].text = "Off";
        }
        for (int i = 0; i < 6; i++)
        {
            _text[i+1].text = _slider[i].value.ToString();
        }
    }

    /// <summary>
    /// AudioMixerのBGMに設定する
    /// </summary>
    void BGMSet()
    {
        audioMixer.SetFloat("BGM", i_BGMVolume-80);
    }

    /// <summary>
    /// AudioMixerのSEに設定する
    /// </summary>
    void SESet()
    {
        audioMixer.SetFloat("SE", i_SEVolume-80);
    }

    /// <summary>
    /// 設定画面を閉じる
    /// </summary>
    void BackSeting()
    {
        foreach (GameObject obj in SettingObj)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in enabledFalseObj)
        {
            obj.SetActive(true);
        }
        settingSelect = 1;
        settingType = SettingType.NOSELECT;
        slideCursor = true;
        selected = false;
        NowSetting = false;
    }

    /// <summary>
    /// スライダー操作メソッド
    /// </summary>
    /// <param name="slider">操作するスライダー</param>
    void SliderControl(Slider c_slider)
    {
        if (Input.GetAxis("HorizontalL" + controlPlayerNum.ToString()) > 0.6f && slideSlider)
        {
            slideSlider = false;
            StartCoroutine(SliderMoveTimeWait());
            c_slider.value++;
        }
        else if (Input.GetAxis("HorizontalL" + controlPlayerNum.ToString()) < -0.6f && slideSlider)
        {
            slideSlider = false;
            StartCoroutine(SliderMoveTimeWait());
            c_slider.value--;
        }
    }

    /// <summary>
    /// トグル操作メソッド
    /// </summary>
    /// <param name="b_toggle"></param>
    void ToggleControll(Toggle b_toggle)
    {
        //true,false切り替え
        if (Input.GetButtonDown("Fire" + controlPlayerNum.ToString()))
        {
            if (_toggle.isOn)
            {
                _toggle.isOn = false;
            }
            else if (!_toggle.isOn)
            {
                _toggle.isOn = true;
            }
        }
        else if (Input.GetButtonUp("Jump" + controlPlayerNum.ToString()))
        {
            selected = false;
            settingType = SettingType.NOSELECT;
        }
    }

    /// <summary>
    /// 設定初期化
    /// </summary>
    void SettingReset()
    {
        // f_ItemTime = 10;
        b_ItemFlg = true;
        f_AreaTime = 45;
        f_ChangeMoveTime = 15;
        i_MobNumber = 20;
        i_AreaNumber = 4;
        I_BGMVolume = 50;
        I_SEVolume = 50;
    }


    /// <summary>
    /// 初期設定用
    /// </summary>
    void SettingUI()
    {
        //_slider[0].onValueChanged.AddListener(S_ItemTime);
        _toggle.onValueChanged.AddListener(T_ItemFlg);
        _slider[0].onValueChanged.AddListener(S_AreaTime);
        _slider[1].onValueChanged.AddListener(S_ChangeMoveTime);
        _slider[2].onValueChanged.AddListener(S_MobNumber);
        _slider[3].onValueChanged.AddListener(S_AreaNumber);
        _slider[4].onValueChanged.AddListener(S_BGMVolume);
        _slider[5].onValueChanged.AddListener(S_SEVolume);

       // _slider[0].maxValue = 60;
        _slider[0].maxValue = 90;
        _slider[1].maxValue = 20;
        _slider[2].maxValue = 50;
        _slider[3].maxValue = 20;
        _slider[4].maxValue = 100;
        _slider[5].maxValue = 100;


        //_slider[0].value = f_ItemTime;
        _toggle.isOn = true;
        _slider[0].value = f_AreaTime;
        _slider[1].value = f_ChangeMoveTime;
        _slider[2].value = i_MobNumber;
        _slider[3].value = i_AreaNumber;
        _slider[4].value = I_BGMVolume;
        _slider[5].value = I_SEVolume;

       // _slider[0].minValue = 5;
        _slider[0].minValue = 10;
        _slider[1].minValue = 3;
        _slider[2].minValue = 4;
        _slider[3].minValue = 1;
        _slider[4].minValue = 0;
        _slider[5].minValue = 0;

    }


    //以下アタッチ用

    //void S_ItemTime(float value)
    //{
    //    f_ItemTime = value;
    //}

    void T_ItemFlg(bool value)
    {
        b_ItemFlg = value;
    }

    void S_AreaTime(float value)
    {
        f_AreaTime = value;
    }

    void S_ChangeMoveTime(float value)
    {
        f_ChangeMoveTime = value;
    }

    void S_MobNumber(float value)
    {
        i_MobNumber = (int)value;
    }

    void S_AreaNumber(float value)
    {
        i_AreaNumber = (int)value;
    }

    void S_BGMVolume(float value)
    {
        I_BGMVolume = (int)value;
    }

    void S_SEVolume(float value)
    {
        I_SEVolume = (int)value;
    }
}
