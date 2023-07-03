using System.Collections.Generic;
using UnityEngine;

// enum WaxTile

[CreateAssetMenu(fileName = "WaxTilesData", menuName = "WorldGen/WaxTilesData")]
public class WaxTilesData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public ElementData elementData;

    // [Header("Weapon Crafting Component")]
}
