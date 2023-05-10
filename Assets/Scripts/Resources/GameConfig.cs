using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    #region Materials
    [SerializeField] private Material green;
    [SerializeField] private Material red;
    #endregion

    #region Properties
    public Material Green => green;
    public Material Red => red;
    #endregion
}
