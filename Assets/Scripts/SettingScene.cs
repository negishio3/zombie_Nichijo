using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SettingScene : MonoBehaviour {
    private static float f_ItemTime;//アイテムの沸き間隔 
    private static float f_AreaTime;//1つのエリアでの時間
    private static float f_ChangeMoveTime;//エリア間の時間
    private static int i_MobNumber;//沸くモブの数
    private static int i_BGMVolume;//BGM音量
    private static int i_SEVolume;//SE音量

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
        foreach (GameObject obj in gameObject.transform)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Setting"))
        {
            foreach (GameObject obj in gameObject.transform)
            {
                obj.SetActive(true);
            }
        }
        if (!selected)
        {
            settingType = SettingType.NOSELECT;
            for (int i = 1; i < 5; i++)
            {
                if (Input.GetButtonDown("Jump" + i.ToString()))
                {
                    foreach (GameObject obj in gameObject.transform)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            for (int i = 1; i < 5; i++)
            {
                //選択を上下に移動
                if (Input.GetAxis("Horizontal" + i.ToString()) >= 1 && settingSelect > 1)
                {
                    settingSelect--;
                }
                else if (Input.GetAxis("Horizontal" + i.ToString()) <= -1 && settingSelect < 6)
                {
                    settingSelect++;
                }
            }
        }
        if (selected)
        {
            Select();
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

        }
    }
}
