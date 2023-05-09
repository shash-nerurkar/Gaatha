using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(DragDroppableUI))]
public class WeaponCraftingComponentCard : MonoBehaviour
{
    [SerializeField] private GameObject elementCard;

    // CHILD COMPONENETS
    [SerializeField] private Image componentImage;
    [SerializeField] private Image componentTypeIcon;
    [SerializeField] private TextMeshProUGUI componentNameLabel;
    [SerializeField] private GameObject elementsPanel;
    [SerializeField] private GameObject weaponStatsPanel;
    [SerializeField] private TextMeshProUGUI weaponDamageLabel;
    [SerializeField] private TextMeshProUGUI weaponAttackSpeedLabel;
    [SerializeField] private TextMeshProUGUI weaponKnockbackLabel;
    private DragDroppableUI dragDroppableUI;

    void Awake() {
        dragDroppableUI = GetComponent<DragDroppableUI>();

        InputManager.WeaponCraftingScreenPositionAction += dragDroppableUI.OnScreenPositionPerformed;

        InputManager.WeaponCraftingComponentCardPressPerformedAction += dragDroppableUI.OnClickPerformed;

        InputManager.WeaponCraftingComponentCardPressCanceledAction += dragDroppableUI.OnClickCanceled;
    }

    // SET THE COMPONENT AND ITS ELEMENT'S DATA
    public void SetComponentData( WeaponCraftingComponentData componentData ) {
        // SET COMPONENT data
        componentImage.sprite = componentData.PanelSprite;
        componentNameLabel.text = componentData.name;
        componentTypeIcon.sprite = componentData.ComponentIcon;

        // FIND ELEMENTS PRESENT IN COMPONENT
        List<ElementData> elementsInComponent = new List<ElementData>();
        foreach( IngredientData ingredient in componentData.Ingredients ) {
            if( !elementsInComponent.Contains( ingredient.elementData ) )
                elementsInComponent.Add( ingredient.elementData );
        }

        // SET ELEMENT CARDS IN COMPONENT CARD
        foreach( ElementData elementData in elementsInComponent ) {
            AddElementCard( elementData: elementData );
        }
    }

    // ADD AN ELEMENT CARD TO THE COMPONENT CARD
    void AddElementCard( ElementData elementData ) {
        GameObject elementCardInstance =  Instantiate(
            original: elementCard, 
            position: Vector3.zero, 
            rotation: Quaternion.identity
        );
        elementCardInstance.transform.SetParent(elementsPanel.transform);
        elementCardInstance.transform.localScale = new Vector3(1, 1, 1);

        elementCardInstance.GetComponent<WeaponCraftingComponentElementCard>().SetElementData(
            elementData: elementData
        );
    }

    // SET THE WEAPON-SPECIFIC DATA TO THE COMPONENT
    public void SetWeaponData( WeaponData weaponData ) {
        if( weaponData != null ) {
            weaponDamageLabel.text = weaponData.Damage == -1 || weaponData.Damage == 0
                ? Constants.INFINITY_TEXT 
                : weaponData.Damage.ToString();
            weaponAttackSpeedLabel.text = weaponData.RecoveryTime == -1 || weaponData.RecoveryTime == 0
                ? Constants.INFINITY_TEXT 
                : weaponData.RecoveryTime.ToString();
            weaponKnockbackLabel.text = weaponData.Knockback == -1 || weaponData.Knockback == 0
                ? Constants.INFINITY_TEXT 
                : weaponData.Knockback.ToString();
        }
        else {
            weaponDamageLabel.text = "-";
            weaponAttackSpeedLabel.text = "-";
            weaponKnockbackLabel.text = "-";
        }
    }
}
