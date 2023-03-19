using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RockerScript : MonoBehaviour
{
    public bool _isContainerPushed = false;

    [Header("Timing")]
    [SerializeField] float _takeOffDelayTime=1f;
    [SerializeField] float _burningDelayTime=1f;


    [Header("Bools")]
    [SerializeField] bool _canFly=false;
    [SerializeField] bool _canAddContainer = true;

    [Header("Rocket")]
    [SerializeField] Rigidbody2D _rockerRb;
    [SerializeField] float _rocketSpeed;
    [SerializeField] float _speedMultiplier=1f;
    [SerializeField] [Range(1,2)] float _speedMultiplierRate = 1;
    [SerializeField] [Range(2,4)] float _speedBoostMultiplierRate =2.5f ;
    private float boostUpValue=1;

    [Header("Container Variables")]
    [SerializeField] float _containerBurnTime;
    public Transform _currentContainer;
    [SerializeField] float _currentContainerYScale;

    [Header("External Scripts")]
    [SerializeField] Container_Holder _containerHolder;

    [Header("Trail Handler")]
    [SerializeField] public Transform _trailObject;
    public Transform _currentTrailHandler;

    [Header("head")]
    [SerializeField] Transform _head;
    [SerializeField] Transform _headBaseTransform;
    [SerializeField] float _thrustForce;
    [SerializeField] GameObject _containersHolderObject;

    [Header("particle system")]
    [SerializeField] ParticleSystem _BoostEffect;
    [SerializeField] GameObject _dieParticleEffect;

    //DoTween variables
    public Tween _currentTween;

    public event Action<int> _LaunchStarted;
     
    void Awake()
    {
        _currentTrailHandler = _containerHolder.getCurrentTrailTransform();
    }
    void  Start()
    {
        //assign the components
        _rockerRb = GetComponent<Rigidbody2D>();
        _containerHolder=GetComponent<Container_Holder>();
    }
    public void StartRocket()
    {
        _canAddContainer = false;
        StartCoroutine(StartRockerCoroutine());
    }
    IEnumerator StartRockerCoroutine()
    {
        yield return new WaitForSeconds(_takeOffDelayTime);
        AudioManager._instance.RocketLaunchSound();
        if (_LaunchStarted != null) _LaunchStarted(1);
        _canFly = true;
        yield return new WaitForSeconds(_burningDelayTime);
        _currentContainer = _containerHolder.getLastContainer();
        BurnTheContainer(_currentContainer);

    }

    private void FixedUpdate()
    {
        if(_canFly)
        {
            _rockerRb.velocity = Vector2.up * _rocketSpeed*Time.fixedDeltaTime*_speedMultiplier*boostUpValue;
        }
    }
    private void Update()
    {
        if(_currentTrailHandler!=null)
        {
        _trailObject.position = _currentTrailHandler.position;
        }
        if (_canFly)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SkipOneContainer();
            }
        }
        if (_canAddContainer)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _containerHolder.AddContainer();
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Restart());
        }
        
    }
    IEnumerator BoostUpRocket(bool _isBoost=false)
    {
        if (_isBoost)
            boostUpValue = _speedMultiplierRate;
        else
            boostUpValue = _speedBoostMultiplierRate;

        yield return new WaitForSeconds(.3f);
        boostUpValue = 1f;
    }
    public void SkipOneContainer()
    {
            CameraManager._instance._smallShake();
            _BoostEffect.Emit(5);
            if (_currentTween != null && !_currentTween.IsComplete())
            {
            StartCoroutine(BoostUpRocket());
            _isContainerPushed = true;
                //DOTween.Kill(_currentTween);
                _currentTween.Kill(true);
            }

    }
    void BurnTheContainer(Transform _containerToBurn)
    {
        if (_containerToBurn != null)
        {

            _trailObject.GetComponent<TrailRenderer>().startWidth = _currentContainer.localScale.x - (_currentContainer.localScale.x/3);
            _currentTween = _containerToBurn.DOScaleY(0, _containerBurnTime).SetEase(Ease.Linear).OnUpdate(() =>
            {
                _currentContainerYScale = _containerToBurn.localScale.y;
            }).OnComplete(() =>
            {
                if (_isContainerPushed)
                {
                    AudioManager._instance._TapSound();
                    AudioManager._instance.BoostSound();
                    
                    _isContainerPushed = false;
                    _currentContainer.localScale = new Vector3(_currentContainer.localScale.x, 1f, _currentContainer.localScale.z);
                    FinishTheCurrentContainer();
                }
                else
                {
                    //die 
                    _speedMultiplier = 2f;
                    //_containerToBurn = null;
                    AudioManager._instance.DieSoundPlay();
                    AudioManager._instance.ContainerBlastSound();
                    Instantiate(_dieParticleEffect, _currentContainer.position, Quaternion.identity);
                    UIManager._instance.DeathMenu();
                    //StartCoroutine(Restart());
                    _containersHolderObject.SetActive(false);
                    _head.gameObject.SetActive(false);
                    _trailObject.gameObject.SetActive(false);
                }
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
        if (_containerHolder.getContainerCount() > -1)
        {
            _currentTrailHandler = _containerHolder.getCurrentTrailTransform();
        _currentContainer = _containerHolder.getLastContainer();
            BurnTheContainer(_currentContainer);
        }
        else
        {
            Destroy(GetComponent<Rigidbody2D>());
            _canFly = false;
            Rigidbody2D headRb= _head.AddComponent<Rigidbody2D>();
            _head.DORotate(new Vector3(0f, 0f, 30f), 1f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                headRb.AddForce(headRb.transform.up * _thrustForce);
            }).OnComplete(() =>
            {
                _head.DORotate(new Vector3(0f, 0f, 180f), 1f).SetEase(Ease.Linear);
            });
            
            //_currentContainer = _headBaseTransform;
            _currentTrailHandler = _headBaseTransform;
            //Debug.Log("Win");
            UIManager._instance.DeathMenu();
            AudioManager._instance.VictorySoundPlay();
            if (_LaunchStarted != null) _LaunchStarted(2);
            //StartCoroutine(Restart());
        }
    }
    public void thrustHeadContainer()
    {
        //
    }
    IEnumerator Restart()
    {
        
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("endPoint"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
