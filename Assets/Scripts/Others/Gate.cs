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
        EventManager.StartListening(EventKeys.LevelLoaded, Init);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.LevelLoaded, Init);
    }

    private void Init(object[] objects)
    {
        var leftSign = LeftOperation switch { Operations.Add => "+", Operations.Subtract => "-", Operations.Multiply => "x", Operations.Divide => "÷", _ => "" };
        var rightSign = rightOperation switch { Operations.Add => "+", Operations.Subtract => "-", Operations.Multiply => "x", Operations.Divide => "÷", _ => "" };

        leftGate.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"{leftSign}{leftValue}";
        rightGate.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"{rightSign}{rightValue}";

        SetGateColors();
    }

    private void SetGateColors()
    {
        var gameConfig = Resources.Load<GameConfig>("GameConfig");
        Debug.Log(gameConfig.ToString());

        Debug.Log(LeftOperation);
        switch (LeftOperation)
        {
            case Operations.Add:
            case Operations.Multiply:
                leftGate.GetComponent<MeshRenderer>().material = gameConfig.Green;
                Debug.Log("Green");
                break;
            case Operations.Subtract | Operations.Divide:
                leftGate.GetComponent<MeshRenderer>().material = gameConfig.Red;
                Debug.Log("Red");
                break;
        };

        Debug.Log(rightOperation);
        switch (rightOperation)
        {
            case Operations.Add:
            case Operations.Multiply:
                rightGate.GetComponent<MeshRenderer>().material = gameConfig.Green;
                break;
            case Operations.Subtract:
            case Operations.Divide:
                rightGate.GetComponent<MeshRenderer>().material = gameConfig.Red;
                break;
        };
    }

    public void OnContactEnter(GameObject other, Vector3 point)
    {
        bool isLeft = point.x < 0;
        EventManager.TriggerEvent(EventKeys.OnGateContactEnter, new object[] { other, point, isLeft ? leftValue : rightValue, isLeft ? LeftOperation : rightOperation });
    }

    public void OnContactExit(GameObject other, Vector3 point)
    {
        //TO DO destroy gate with animation
    }
}
