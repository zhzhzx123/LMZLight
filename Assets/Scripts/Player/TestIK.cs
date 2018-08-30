using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIK : MonoBehaviour {

    public Transform lookAtTarget;

    private Animator _animator;
 
     void Start()
     {
         _animator = this.GetComponent<Animator>();
     }

    void OnAnimatorIK(int layerIndex)
    {
        if (_animator != null)
        {
            //仅仅是头部跟着变动
            _animator.SetLookAtWeight(1);
            //身体也会跟着转, 弧度变动更大
            //_animator.SetLookAtWeight(1, 1, 1, 1);
            if (lookAtTarget != null)
            {
                _animator.SetLookAtPosition(lookAtTarget.position);
            }
        }
    }
}
