using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Container_Holder : MonoBehaviour
{

    [SerializeField] List<Transform> _containersList = new List<Transform>();
    [SerializeField] int _currentContainerCount;

    [Header("Containers List")]
    [SerializeField] GameObject _Container_1;
    [SerializeField] float _container_1_Height;
    [SerializeField] GameObject _Container_2;
    [SerializeField] float _container_2_Height;
    [Header("timing")]
    [SerializeField] float _containerAddTime;

    [SerializeField] private RockerScript _rocket;

    [Header("Container addition")]
    [SerializeField] Transform _containerHolder;

    private void Awake()
    {
        _currentContainerCount = _containersList.Count-1;
        
    }
    private void Start()
    {
        //Check the container list is null or not 
        // assign the last container reference

    }
    public Transform getLastContainer()
    {
        if (_currentContainerCount - 1 > -2)
            return _containersList[_currentContainerCount--];
        else return null;   
    }
    public void destroyLastContainer()
    {
        //remove the last container from the rocker
        //_containersList[_containersList.Count-1].gameObject.SetActive(false);
        Transform _containerToDestroy = _containersList[_containersList.Count-1];

            _containerToDestroy.gameObject.AddComponent<Rigidbody2D>();
            Color _containerColor = _containerToDestroy.GetChild(0).GetComponent<SpriteRenderer>().color;
            _containerColor.a = .4f;
            _containerToDestroy.GetChild(0).GetComponent<SpriteRenderer>().color = _containerColor;
        
        _containersList.RemoveAt(_containersList.Count - 1);
        //Destroy(_containersList[_containersList.Count-1].gameObject);
    }
    public int getContainerCount()
    { 
        return _currentContainerCount;
    }
     public void addToLastContainer()
    {
        //add one container to the last container 
    }
    public Transform getCurrentTrailTransform()
    {
        return _containersList[_currentContainerCount].GetChild(1).transform;
    }
    public void AddContainer()
    {
        AudioManager._instance.ContainerAddSound();
        _rocket.transform.DOMoveY(_rocket.transform.position.y + _container_1_Height, _containerAddTime).OnStart(() =>
        {
            
        }).SetEase(Ease.InBounce).OnComplete(() =>
        {
           _containersList.Add(Instantiate(_Container_1, _containersList[_currentContainerCount].GetChild(1).position, Quaternion.identity, _containerHolder).transform);
            _currentContainerCount++;
            _rocket._currentTrailHandler = _containersList[_currentContainerCount].GetChild(1);
        });
    }

}
