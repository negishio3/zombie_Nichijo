using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCam_sanoki : MonoBehaviour {
    public GameObject[] camPos;
    public GameObject backImage;
    public GameObject buttonText;


    public void camMove(int playerID)
    {
        StartCoroutine(CamMove(camPos[playerID].transform.position));
    }

    IEnumerator CamMove(Vector3 movePos)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = movePos;

        while (time < 1.0f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, time);
            yield return null;
        }
        backImage.SetActive(true);
        buttonText.SetActive(true);
        FindObjectOfType<ZombieInstant>().isRanking = false;
    }
}
