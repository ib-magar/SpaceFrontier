using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject _GameplayUI;
    [SerializeField] GameObject _MenuUI;

    private void Start()
    {
        setUI(false);
    }
    public void setUI(bool _isGameplayUIActive)
    {
        _GameplayUI.SetActive( _isGameplayUIActive);
        _MenuUI.SetActive(!_isGameplayUIActive);
    }
}
