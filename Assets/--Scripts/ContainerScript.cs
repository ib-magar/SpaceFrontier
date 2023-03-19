using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerScript : MonoBehaviour
{

    [SerializeField] Transform _hitTarget;
    [SerializeField] float _hitTargetScale;
    [SerializeField] Transform _parentTarget;
    private void Update()
    {

        if(_parentTarget.localScale.y>=1f)
        {
            _hitTarget.localScale = new Vector2(1f, _hitTargetScale);
        }
        else if(_parentTarget.localScale.y < 1 && _parentTarget.localScale.y >= _hitTargetScale)
        {
            _hitTarget.localScale = new Vector2(1f, (_hitTargetScale + (1 - _parentTarget.localScale.y)));
        }
        else
        {
            _hitTarget.localScale = new Vector2(1f, 1f);
        }
    }

}
