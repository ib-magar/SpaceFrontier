using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //DoTween variables
    public Tween _currentTween;

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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SkipOneContainer();
        }
        
    }
    public void SkipOneContainer()
    {
            if (_currentTween != null && !_currentTween.IsComplete())
            {
                //DOTween.Kill(_currentTween);
                _currentTween.Kill(true);
                //FinishTheCurrentContainer();
                Debug.Log("Tween is not finished!");
            }

    }
     void BurnTheContainer(Transform _containerToBurn)
    {
        if(_containerToBurn!=null)
        {

            _trailObject.GetComponent<TrailRenderer>().startWidth = _currentContainer.localScale.x - .18f;
            _currentTween = _containerToBurn.DOScaleY(0, _containerBurnTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                FinishTheCurrentContainer();
                //Debug.Log("tween finished");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
        else
        {
            Debug.Log("game over");
        }
    }

    private void FinishTheCurrentContainer()
    {
        _containerHolder.destroyLastContainer();
        _currentTrailHandler = _containerHolder.getCurrentTrailTransform();
        if (_containerHolder.getContainerCount() > -1)
        {
            _currentContainer = _containerHolder.getLastContainer();
            BurnTheContainer(_currentContainer);
        }
    }

}
