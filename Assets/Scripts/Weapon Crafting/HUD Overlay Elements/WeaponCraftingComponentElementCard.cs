using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponCraftingComponentElementCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI elementName;
    [SerializeField] private Image elementIcon;
    [SerializeField] private Image elementPanelImage;
    // public Image ComponentImage {
    //     get { return componentImage; }
    //     private set { componentImage = value; }
    // }

    public void SetElementData( ElementData elementData ) {
        elementIcon.sprite = elementData.Icon;

        this.elementName.text = elementData.name;

        elementPanelImage.color = elementData.Color;
    }
}
