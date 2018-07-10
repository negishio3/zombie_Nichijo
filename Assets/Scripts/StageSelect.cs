using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour {

    [SerializeField]
    private Image[] Arrow = new Image[2];
    [SerializeField]
    private GameObject[] StageObj;

    private const float moveWaitTime=0.15f;

    private bool isExecute;
    private bool moveSelect;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Selectの待ち時間
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveTimeWait()
    {
        if (isExecute || moveSelect)
        {
            yield break;
        }
        isExecute = true;
        yield return new WaitForSeconds(moveWaitTime);
        moveSelect = true;
        isExecute = false;
    }
}
