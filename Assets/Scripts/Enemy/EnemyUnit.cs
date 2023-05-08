using Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour, IContactable
{
    [SerializeField] private Animator _animator;
    public void Init()
    {
        if (_animator == null)
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

    public void MoveToPlayer(Vector3 playerPos, Vector3 playerUnitPos)
    {
        var distance = transform.position - playerPos;
        if (distance.magnitude < .75f)
        {
            return;
        }

        Vector3 refPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerUnitPos.x, transform.position.y, playerUnitPos.z), Time.deltaTime * Mathf.Clamp(distance.magnitude / 2, .25f, 2));

        var playerDirection = refPos - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection), 3f * Time.deltaTime);

    }

    public void OnContactEnter(GameObject other, Vector3 point)
    {
        Debug.Log("EnemyUnit OnContactEnter");
        gameObject.SetActive(false);
    }

    public void OnContactExit(GameObject other, Vector3 point)
    {
    }
}
