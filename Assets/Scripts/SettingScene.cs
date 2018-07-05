using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SettingScene : MonoBehaviour {
    public bool NowSetting { get; set; }//現在設定画面にいるか

    private static float f_ItemTime=10;//アイテムの沸き間隔 
    private static float f_AreaTime=45;//1つのエリアでの時間
    private static float f_ChangeMoveTime=15;//エリア間の時間
    private static int i_MobNumber=20;//沸くモブの数
    private static int i_BGMVolume=50;//BGM音量
    private static int i_SEVolume=50;//SE音量

    [SerializeField]
    private Slider[] _slider=new Slider[6];
    private List<GameObject> SettingObj=new List<GameObject>();

    private int settingSelect=1;
    private bool selected;//その項目を選択

    public enum SettingType
    {
        NOSELECT,
        ITEMTIME,
        AREATIME,
        CHANGEMOVETIME,
        MOBNUMBER,
        BGMVOLUME,
        SEVOLUME
    }

    private SettingType settingType;


	void Start () {
        SettingSlider();
        foreach (Transform obj in gameObject.transform)
        {
            SettingObj.Add(obj.gameObject);
            obj.gameObject.SetActive(false);
        }
    }


    void Update() {
        if (Input.GetButtonDown("Setting"))
        {
            NowSetting = true;
            foreach (GameObject obj in SettingObj)
            {
                obj.SetActive(true);
            }
        }
        if (!selected)
        {
            settingType = SettingType.NOSELECT;
            for (int i = 1; i < 5; i++)
            {
                //決定
                if (Input.GetButtonDown("Fire" + i.ToString()))
                {
                    selected = true;
                    Select();
                }
            }

            for (int i = 1; i < 5; i++)
            {
                //選択を上下に移動
                if (Input.GetAxis("VerticalL" + i.ToString()) >= 1 && settingSelect > 1)
                {
                    settingSelect--;
                }
                else if (Input.GetAxis("VerticalL" + i.ToString()) <= -1 && settingSelect < 6)
                {
                    settingSelect++;
                }
            }

            //設定画面を閉じる
            for (int i = 1; i < 5; i++)
            {
                if (Input.GetButtonDown("Jump" + i.ToString()))
                {
                    foreach (GameObject obj in SettingObj)
                    {
                        obj.SetActive(false);
                    }
                    NowSetting = false;
                }
            }

        }
        if (selected)
        {
            SwitchSettingType();
        }
    }
    void Select()
    {
        settingType = (SettingType)Enum.ToObject(typeof(SettingType), settingSelect);
    }

    void ChoiceSelection()
    {
        //選択している奴がわかりやすいかんじになる処理

    }

    void SwitchSettingType()
    {
        switch (settingType)
        {
            case SettingType.NOSELECT:
                break;

            case SettingType.ITEMTIME:
                SliderControl(_slider[0]);
                break;

            case SettingType.AREATIME:
                SliderControl(_slider[1]);
                break;

            case SettingType.CHANGEMOVETIME:
                SliderControl(_slider[2]);
                break;

            case SettingType.MOBNUMBER:
                SliderControl(_slider[3]);
                break;

            case SettingType.BGMVOLUME:
                SliderControl(_slider[4]);
                break;

            case SettingType.SEVOLUME:
                SliderControl(_slider[5]);
                break;
        }
    }

    /// <summary>
    /// スライダー操作メソッド
    /// </summary>
    /// <param name="slider">操作するスライダー</param>
    void SliderControl(Slider c_slider)
    {
        for (int i = 1; i < 5; i++)
        {
            if (Input.GetAxis("HorizontalL" + i.ToString()) > 0.9f)
            {
                c_slider.value++;
            }
            else if (Input.GetAxis("HorizontalL" + i.ToString()) < -0.9f)
            {
                c_slider.value--;
            }
        }

        //選択解除
        for (int i = 1; i < 5; i++)
        {
            if (Input.GetButtonDown("Jump" + i.ToString()))
            {
                selected = false;
                settingType = SettingType.NOSELECT;
            }
        }
    }

    void SettingReset()
    {
        f_ItemTime = 10;
        f_AreaTime = 45;
        f_ChangeMoveTime = 15;
        i_MobNumber = 20;
        i_BGMVolume = 50;
        i_SEVolume = 50;
    }


    /// <summary>
    /// スライダー初期設定用
    /// </summary>
    void SettingSlider()
    {
        _slider[0].onValueChanged.AddListener(S_ItemTime);
        _slider[1].onValueChanged.AddListener(S_AreaTime);
        _slider[2].onValueChanged.AddListener(S_ChangeMoveTime);
        _slider[3].onValueChanged.AddListener(S_MobNumber);
        _slider[4].onValueChanged.AddListener(S_BGMVolume);
        _slider[5].onValueChanged.AddListener(S_SEVolume);

        _slider[0].maxValue = 60;
        _slider[1].maxValue = 90;
        _slider[2].maxValue = 20;
        _slider[3].maxValue = 50;
        _slider[4].maxValue = 100;
        _slider[5].maxValue = 100;

        _slider[0].minValue = 5;
        _slider[1].minValue = 10;
        _slider[2].minValue = 3;
        _slider[3].minValue = 4;
        _slider[4].minValue = 0;
        _slider[5].minValue = 0;
    }

    void S_ItemTime(float value)
    {
        f_ItemTime = value;
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

    void S_BGMVolume(float value)
    {
        i_BGMVolume = (int)value;
    }

    void S_SEVolume(float value)
    {
        i_SEVolume = (int)value;
    }
}
