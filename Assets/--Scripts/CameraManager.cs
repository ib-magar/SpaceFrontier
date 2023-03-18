using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager _instance;

    [SerializeField] Transform _rocketTransform;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera[] _Cameras;


    [SerializeField] Transform _mainCamera;

    [Header("small shake variables")]
    [SerializeField] float duration=.2f;
    [SerializeField] float strength=90f;
    [SerializeField] int vibrato = 10;
    [SerializeField] float randomness = 90f;

    [Header("external scripts")]
    [SerializeField] RockerScript _rocketScript;
    void Awake()
    {
        if(_instance==null && _instance!=this)
            _instance = this;

        //_mainCamera = Camera.main.transform;
        SwitchToContainerCamera(0);
    }
    public void _smallShake()
    {
        _mainCamera.DOShakeRotation(duration,strength,vibrato,randomness);
        //_mainCamera.DOShakeScale(duration,strength,vibrato,randomness);
    }
    IEnumerator Start()
    {
        _rocketScript._LaunchStarted += SwitchToContainerCamera;

        yield return new WaitForSeconds(.2f);
        //set the beginning container camera
        _Cameras[1].Follow = _rocketScript._trailObject;
        //SetContainerCamera();
    }
    public void SwitchToContainerCamera(int _n)
    {
        for(int i=0;i<3;i++)
        {
            if (i == _n) _Cameras[_n].gameObject.SetActive(true);
            else _Cameras[i].gameObject.SetActive(false);
        }
    }
}


