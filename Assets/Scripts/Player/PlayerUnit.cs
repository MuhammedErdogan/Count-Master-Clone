using DG.Tweening;
using Interface;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerUnit : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private bool _isJumping = false;

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.FinishTriggered, ForwardCheck);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.FinishTriggered, ForwardCheck);
        }

        public void Init(PlayerState state)
        {
            if (_animator == null)
            {
                _animator = transform.GetChild(0).GetComponent<Animator>();
            }

            AnalyseState(state);
        }

        public void ChangeState(PlayerState state)
        {
            AnalyseState(state);
        }

        private void AnalyseState(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Idle:
                    _animator.SetBool(Constants.Animations.RUN, false);
                    break;
                case PlayerState.Run | PlayerState.Attack:
                    _animator.SetBool(Constants.Animations.RUN, true);
                    break;
                default:
                    _animator.SetBool(Constants.Animations.RUN, true);
                    break;
            }
        }

        public void MoveToEnemy(Vector3 enemyPos)
        {
            var distance = transform.position - enemyPos;
            if (distance.magnitude < .75f)
            {
                return;
            }

            Vector3 refPos = new Vector3(enemyPos.x, transform.position.y, enemyPos.z);
            transform.position = Vector3.Lerp(transform.position, refPos, Time.deltaTime * Mathf.Clamp(0.75f / distance.magnitude, .25f, 2));

            var enemyDirection = refPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemyDirection), 3f * Time.deltaTime);
        }

        public void DestroyUnit()
        {
            gameObject.SetActive(false);
            transform.SetParent(PoolManager.Instance.transform);
        }

        private void Jump()
        {
            if (_isJumping)
            {
                return;
            }

            var startPosition = transform.localPosition;
            _isJumping = true;
            transform.DOLocalJump(new Vector3(startPosition.x, 0, startPosition.z), 1.35f, 1, 1f).OnComplete(() =>
            {
                _isJumping = false;
            });
        }

        private void ForwardCheck(object[] obj)
        {
            StartCoroutine(ForwardCheckCroutine());
        }


        private IEnumerator ForwardCheckCroutine()
        {
            while (true)
            {
                var ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out _, .5f, 1 << Constants.LayerIndexes.STAIR))
                {
                    transform.parent.SetParent(null);
                }
                yield return 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.RAMP))
            {
                Jump();
                return;
            }

            if (other.TryGetComponent(out IContactable contactable))
            {
                contactable.OnContactEnter(gameObject, other.ClosestPoint(transform.position));
                EventManager.TriggerEvent(EventKeys.OnPlayerUnitHit, new object[] { other, this });
            }
        }
    }
}
