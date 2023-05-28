using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCraftingComponentData", menuName = "Weapon Component/WeaponCraftingComponentData")]
public class WeaponCraftingComponentData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public Sprite PanelSprite;
    public Sprite ComponentIcon;

    // [Header("Attacking")]
    // [SerializeField]
    public WeaponCraftingComponentId Id;
    public List<IngredientData> Ingredients;
}
