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

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

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

        public void DestroyUnit()
        {
            //TO DO destroy unit with animation
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IContactable contactable))
            {
                EventManager.TriggerEvent(EventKeys.OnPlayerUnitHit, new object[] {other, this});
            }
        }
    }
}
