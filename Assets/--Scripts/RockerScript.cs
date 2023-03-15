using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.Timeline;
using UnityEngine;

public class RockerScript : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] float _takeOffDelayTime=1f;
    [SerializeField] float _burningDelayTime=1f;


    [Header("Bools")]
    [SerializeField] bool _canFly=false;

    [Header("Rocket")]
    [SerializeField] Rigidbody2D _rockerRb;
    [SerializeField] float _rocketSpeed;
    [SerializeField] float _speedMultiplier=1f;

    [Header("Container Variables")]
    [SerializeField] float _containerBurnTime;
    public Transform _currentContainer;


    [Header("External Scripts")]
    [SerializeField] Container_Holder _containerHolder;

    [Header("Trail Handler")]
    [SerializeField] public Transform _trailObject;
    [SerializeField] Transform _currentTrailHandler;

    [Header("head")]
    [SerializeField] Transform _head;

    public event Action _LaunchStarted;
    IEnumerator  Start()
    {
        //assign the components
        _rockerRb = GetComponent<Rigidbody2D>();
        _containerHolder=GetComponent<Container_Holder>();

        yield return new WaitForSeconds(_takeOffDelayTime);
        _canFly = true;
        _currentTrailHandler = _containerHolder.getCurrentTrailTransform();
        yield return new WaitForSeconds(_burningDelayTime);
        _currentContainer = _containerHolder.getLastContainer();
        BurnTheContainer(_currentContainer);
        if (_LaunchStarted != null) _LaunchStarted();

    }

    private void FixedUpdate()
    {
        if(_canFly)
        {
            _rockerRb.velocity = Vector2.up * _rocketSpeed*Time.fixedDeltaTime*_speedMultiplier;
        }
    }
    private void Update()
    {
        if(_currentTrailHandler!=null)
        {
        _trailObject.position = _currentTrailHandler.position;
        }
        
    }
    void BurnTheContainer(Transform _containerToBurn)
    {
        _trailObject.GetComponent<TrailRenderer>().startWidth = _currentContainer.localScale.x-.2f;
        _containerToBurn.DOScaleY(0, _containerBurnTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            _containerHolder.destroyLastContainer();
            _currentTrailHandler = _containerHolder.getCurrentTrailTransform();
            if (_containerHolder.getContainerCount() > -1)
            {
                _currentContainer = _containerHolder.getLastContainer();
                BurnTheContainer(_currentContainer);
            }
                            // make sure the head and the neck container has the smae width.
            if(_containerHolder.getContainerCount()==-1)            //when the last container is going on   
            {
                _trailObject.GetComponent<TrailRenderer>().startWidth = _head.localScale.x;
            }
           
        });
    }

}
