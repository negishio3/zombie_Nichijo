using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public Camera cam;//使用するカメラ

    public List<GameObject> targets=new List<GameObject>();//画面内に収めたいオブジェクト

    public float offset;//オフセット値
    private void Reset()
    {
        cam = GetComponent<Camera>();//カメラコンポーネントを取得
    }

    private void Update()
    {
        //targets = GameObject.FindGameObjectsWithTag("Target").ToList();
        transform.position = Vector3.Lerp(transform.position, calcCameraPosition(), 1.0f);
    }

    //一番遠いオブジェクトを調べる
    private Vector3 calcCameraPosition()
    {
        //if (targets.Count== 0) return targets[0].transform.position;
        Vector3[] targetPosArray = new Vector3[targets.Count];
        Vector3 centerPointWorld = new Vector3();
        Vector3 centerPointScreen = new Vector3();
        for(int i = 0; i < targets.Count; i++)
        {
            targetPosArray[i] = targets[i].transform.position;
            Debug.DrawRay(targetPosArray[i], Vector3.up * 5,Color.red);
            centerPointWorld += targetPosArray[i];
        }
        centerPointWorld /= targets.Count;
        centerPointScreen = Camera.main.WorldToScreenPoint(centerPointWorld);
        Debug.DrawRay(centerPointWorld, Vector3.up);
        float farDistans = new float();
        for (int i = 0; i < targets.Count; i++)
        {
            Vector3 diff = targetPosArray[i] - centerPointWorld;
            diff.x *= (float)Screen.height / Screen.width;
            float newDistans = diff.magnitude;
            if (farDistans < newDistans)
            {
                farDistans = newDistans;
            }
        }
        float cameraLength = farDistans / Mathf.Tan(Camera.main.fieldOfView / 2 * Mathf.Deg2Rad);
        return centerPointWorld - transform.forward * (cameraLength + offset);
    }
}