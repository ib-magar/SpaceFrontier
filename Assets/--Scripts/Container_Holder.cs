using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Container_Holder : MonoBehaviour
{

    [SerializeField] List<Transform> _containersList = new List<Transform>();
    [SerializeField] int _currentContainerCount;
    private void Start()
    {
        //Check the container list is null or not 
        // assign the last container reference

        _currentContainerCount = _containersList.Count-1;
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
        _containersList[_containersList.Count-1].gameObject.SetActive(false);
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

}
