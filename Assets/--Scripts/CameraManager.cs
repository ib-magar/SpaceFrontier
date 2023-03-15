using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{

    [SerializeField] Transform _rocketTransform;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera _rocketCamera;
    [SerializeField] CinemachineVirtualCamera _containerCamera;


    [Header("external scripts")]
    [SerializeField] RockerScript _rocketScript;
    IEnumerator Start()
    {
        _rocketScript._LaunchStarted += SwitchToContainerCamera;

        yield return new WaitForSeconds(.2f);
        //set the beginning container camera
        _containerCamera.Follow = _rocketScript._trailObject;
        //SetContainerCamera();
    }
    public void SwitchToContainerCamera()
    {
        _rocketCamera.gameObject.SetActive(false);
        _containerCamera.gameObject.SetActive(true);
    }
}


