using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Init(object[] objects)
    {
        
    }

    public void DestroyUnit()
    {
        //TO DO destroy unit with animation
        gameObject.SetActive(false);
    }
}
