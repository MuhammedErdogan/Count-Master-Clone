using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public void Init()
    {
        if(_animator == null)
        {
            _animator = transform.GetChild(0).GetComponent<Animator>();
        }
        _animator.SetBool(Constants.Animations.RUN, false);
        Debug.Log("EnemyUnit Init");
    }

    public void Attack()
    {
        Debug.Log("EnemyUnit Attack");
        _animator.SetBool(Constants.Animations.RUN, true);
    }

    public void StopAttack()
    {
        Debug.Log("EnemyUnit StopAttack");
        _animator.SetBool(Constants.Animations.RUN, false);
    }
}
