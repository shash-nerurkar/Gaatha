using UnityEngine;


[CreateAssetMenu(fileName = "IngredientData", menuName = "Ingredient/IngredientData")]
public class IngredientData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    // [Header("Attacking")]
    public IngredientId Id;

    [Header("Weapon Crafting Component")]
    public WeaponCraftingComponentData componentData;

    [Header("Element")]
    public ElementData elementData;
}
