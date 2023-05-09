using UnityEngine;


[CreateAssetMenu(fileName = "ElementData", menuName = "Element/ElementData")]
public class ElementData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public Sprite Icon;
    public Color Color;
}
