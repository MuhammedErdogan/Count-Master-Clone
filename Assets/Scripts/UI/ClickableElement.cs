using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interface;

public class ClickableElement : MonoBehaviour, IClickable
{
    [SerializeField] private ButtonType buttonType;

    public void OnClick()
    {
        EventManager.TriggerEvent(EventKeys.Buttonclicked, new object[] { buttonType});
        Debug.Log("Button clicked: " + buttonType);
    }
}
