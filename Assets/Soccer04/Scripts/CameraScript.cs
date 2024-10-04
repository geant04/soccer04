using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private bool Track = false;
    private bool FoundTarget = false;
    private Vector3 origin;
    private Vector3 TargetPosition;
    private Quaternion TargetQuat;

    private float CameraDistanceFromTarget = 5.0f;
    public GameObject CameraPivot;
    public Camera MainCamera;
    public GameObject FakeUI;

    public void ActivateTracking(Transform target)
    {
        Track = true;
        TargetPosition = target.position;
        TargetQuat = Quaternion.Euler(target.up * 90.0f);
    }

    private void TrackTarget()
    {
        Vector3 pivotPos = Vector3.Slerp(transform.position, TargetPosition, 2.0f * Time.deltaTime);
        Vector3 mCameraPos = Vector3.Slerp(MainCamera.transform.localPosition, new Vector3(CameraDistanceFromTarget, 0, 0), 2.0f * Time.deltaTime);
        Vector3 UIPos = Vector3.Slerp(FakeUI.transform.localPosition, new Vector3(-3.9f, -3.8f, 0), 2.0f * Time.deltaTime);
        
        MainCamera.transform.localPosition = mCameraPos;

        transform.position = pivotPos;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 2.0f * Time.deltaTime);

        CameraPivot.transform.Rotate(new Vector3(0, 1, 0), -24.0f * Time.deltaTime);

        FakeUI.transform.localPosition = UIPos;
    }

    private void EpicShot()
    {
        Vector3 pivotPos = Vector3.Slerp(transform.position, TargetPosition, Time.deltaTime);
        transform.position = pivotPos;
    }

    private void Start()
    {
        origin = transform.position;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCameraController(this);
        }
    }

    private void Update()
    {
        if (FoundTarget)
        {
            EpicShot();
            return;
        }

        if (Track)
        {
            TrackTarget();
            return;
        }
    }
}
