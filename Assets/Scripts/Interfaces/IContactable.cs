using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public interface IContactable
    {
        public void OnContactEnter(GameObject other, Vector3 point);
        public void OnContactExit(GameObject other, Vector3 point);
    }
}
