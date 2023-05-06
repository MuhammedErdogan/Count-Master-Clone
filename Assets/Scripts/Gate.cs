using Interface;
using Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour, IContactable
{
    [SerializeField] private GameObject leftGate, rightGate;
    [SerializeField] private int leftValue, rightValue;
    [SerializeField] private Operations LeftOperation, rightOperation;


    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.OnGameStarted, Init);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnGameStarted, Init);
    }

    private void Init(object[] objects)
    {
        var leftSign = LeftOperation switch { Operations.Add => "+", Operations.Subtract => "-", Operations.Multiply => "x", Operations.Divide => "/", _ => "" };
        var rightSign = rightOperation switch { Operations.Add => "+", Operations.Subtract => "-", Operations.Multiply => "x", Operations.Divide => "/", _ => "" };
        leftGate.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"{leftSign}{leftValue}";
        rightGate.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"{rightSign}{rightValue}";
    }

    public void OnContactEnter(GameObject other, Vector3 point)
    {
        EventManager.TriggerEvent(EventKeys.OnGateContactEnter, new object[] { other, point, point.x < 0 ? leftValue : rightValue, point.x < 0 ? LeftOperation : rightOperation });
    }

    public void OnContactExit(GameObject other, Vector3 point)
    {
        //TO DO destroy gate with animation
    }
}
