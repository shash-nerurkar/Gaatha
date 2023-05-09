using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class WeaponCraftingPanel : MonoBehaviour
{
    [SerializeField] private GameObject weaponCraftingComponentCard;

    // CHILDREN
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI adhesiveAmountText;
    [SerializeField] private WeaponCraftingAvailableComponentsPanel availableComponents;
    [SerializeField] private Button craftButton;
    [SerializeField] private Image resultImage;
    
    // VARIABLES
    private bool isPanelActive = false;

    // ACTIONS
    public static event Action EnableWeaponCraftingActionsAction;
    public static event Action EnablePlayerOnFootActionsAction;

    // FUNCS
    public delegate void GetWeaponAction(out List<IWeapon> weaponsInArm);
    static public WeaponCraftingPanel.GetWeaponAction FetchWeaponInArmAction;

    void Awake() {
        IngredientInteract.InteractWithIngredientAction += OnInteractWithIngredient;

        // HIDE PANEL ON BACK BUTTON CLICK
        backButton.onClick.AddListener(delegate { 
            togglePanelActive( toggleStatus: false );
        });
    }

    void OnInteractWithIngredient( Ingredient ingredient ) {
        // IF PANEL ISN'T SHOWN YET
        if( !isPanelActive ) {
            // SHOW PANEL
            togglePanelActive( toggleStatus: true );

            // GET WEAPONS IN ARM
            List<IWeapon> weaponsInArm = null;
            FetchWeaponInArmAction?.Invoke( out weaponsInArm );
            if( weaponsInArm == null ) return;

            // ADD COMPONENT CARDS FOR WEAPONS IN ARM
            foreach( IWeapon weapon in weaponsInArm )
                AddAvailableComponentCard( componentData: weapon.WeaponData.componentData, weapon: weapon );
        }

        // ADD COMPONENT CARD FOR THIS INGREDIENT
        AddAvailableComponentCard( componentData: ingredient.IngredientData.componentData, weapon: null );
        availableComponents.AdjustGrid();
    }

    void AddAvailableComponentCard( WeaponCraftingComponentData componentData, IWeapon weapon ) {
        // INSTANTIATE AND ADD A COMPONENT CARD
        GameObject componentCardInstance =  Instantiate(
            original: weaponCraftingComponentCard, 
            position: Vector3.zero, 
            rotation: Quaternion.identity
        );
        componentCardInstance.transform.SetParent(availableComponents.transform);
        componentCardInstance.transform.localScale = new Vector3(1, 1, 1);

        // SET COMPONENT CARD DATA
        componentCardInstance.GetComponent<WeaponCraftingComponentCard>().SetComponentData(
            componentData: componentData
        );

        // SET COMPONENT CARD WEAPON DATA
        componentCardInstance.GetComponent<WeaponCraftingComponentCard>().SetWeaponData(
            weaponData: weapon != null 
                ? weapon.WeaponData 
                : null
        );
    }

    void togglePanelActive( bool toggleStatus ) {
        isPanelActive = toggleStatus;

        if( isPanelActive ) {
            EnableWeaponCraftingActionsAction?.Invoke();
        }
        else {
            EnablePlayerOnFootActionsAction?.Invoke();
            availableComponents.ClearGrid();
        }

        for( int index = 0; index < transform.childCount; index++ ) {
            transform.GetChild(index).gameObject.SetActive( toggleStatus );
        }
    }

    void FindCraftingResult() {
        // FIND RESULT
        // CraftingResultFinder.FindCraftingResult

        // SET RESULT SPRITE
        // resultImage.sprite = 
    }
}
