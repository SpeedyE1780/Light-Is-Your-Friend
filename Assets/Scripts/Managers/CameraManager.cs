using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance => _instance;

    public Light SpotLight;
    public Vector3 CameraOffset;
    public Transform Target;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        //Set the spotlight range to the distance between the camera and player
        SpotLight.range = CameraOffset.magnitude;
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        transform.position = Target.position + CameraOffset;
        transform.LookAt(Target.position);
    }

#if UNITY_EDITOR

    //Place camera in right position after changing the values of the camera
    private void OnValidate()
    {
        FollowTarget();
    }

#endif

}