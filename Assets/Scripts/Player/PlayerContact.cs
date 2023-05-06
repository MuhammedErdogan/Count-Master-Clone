using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Interface;

namespace Player
{
    public class PlayerContact : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IContactable contactable))
            {
                return;
            }

            contactable.OnContactEnter(other.gameObject, other.ClosestPoint(transform.position));
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out IContactable contactable))
            {
                return;
            }

            contactable.OnContactExit(other.gameObject, other.ClosestPoint(transform.position));
        }
    }
}
