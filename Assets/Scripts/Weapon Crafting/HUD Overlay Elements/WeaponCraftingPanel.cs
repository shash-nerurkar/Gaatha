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
    [SerializeField] private GameObject currentComponents;
    [SerializeField] private GameObject results;
    
    // VARIABLES
    private bool isPanelActive = false;

    // ACTIONS
    public static event Action EnableWeaponCraftingActionsAction;
    public static event Action EnablePlayerOnFootActionsAction;

    // FUNCS
    public delegate void GetWeaponsAction( out List<IWeapon> weaponsInArm );
    static public WeaponCraftingPanel.GetWeaponsAction FetchWeaponInArmAction;
    public delegate void GetIngredientsAction( out List<Ingredient> ingredientsInRange );
    static public WeaponCraftingPanel.GetIngredientsAction FetchIngredientsInRangeAction;

    void Awake() {
        WeaponCraftingInput.StartWeaponCraftingAction += OnInteractWithWeaponCraftingInput;

        IngredientInteract.InteractWithIngredientAction += OnInteractWithIngredient;

        // HIDE PANEL ON BACK BUTTON CLICK
        backButton.onClick.AddListener(delegate { 
            togglePanelActive( toggleStatus: false );
        });
    }

    void OnInteractWithWeaponCraftingInput() {
        // GET INGREDIENTS IN RANGE
        List<Ingredient> ingredientsInRange = null;
        FetchIngredientsInRangeAction?.Invoke( out ingredientsInRange );

        ActivatePanel( ingredientsInRange: ingredientsInRange );
    }

    void OnInteractWithIngredient( Ingredient ingredient ) {
        ActivatePanel( ingredientsInRange: new List<Ingredient>() { ingredient } );
    }

    async void ActivatePanel( List<Ingredient> ingredientsInRange ) {
        // IF PANEL ISN'T SHOWN YET
        if( !isPanelActive ) {
            // SHOW PANEL
            togglePanelActive( toggleStatus: true );

            // GET WEAPONS IN ARM
            List<IWeapon> weaponsInArm = null;
            FetchWeaponInArmAction?.Invoke( out weaponsInArm );
            if( weaponsInArm == null ) return;

            // ADD COMPONENT CARDS FOR WEAPONS IN ARM
            if( weaponsInArm != null )
                foreach( IWeapon weapon in weaponsInArm )
                    AddComponentCard( 
                        componentData: weapon.WeaponData.componentData, 
                        weapon: weapon,
                        parentTransform: availableComponents.ElementsContainer.transform
                    );
        }

        // ADD COMPONENT CARD FOR THIS INGREDIENT
        if( ingredientsInRange != null )
            foreach( Ingredient ingredient in ingredientsInRange )
                AddComponentCard(
                    componentData: ingredient.IngredientData.componentData, 
                    weapon: null,
                    parentTransform: availableComponents.ElementsContainer.transform
                );

        availableComponents.AdjustGrid();

        await System.Threading.Tasks.Task.Delay( millisecondsDelay: Mathf.CeilToInt(Time.deltaTime*1000) );

        // SET CARD DATA FOR ALL COMPONENT CARDS, AFTER GRID HAS BEEN ADJUSTED
        foreach( WeaponCraftingComponentCard componentCard in availableComponents.GetComponentsInChildren<WeaponCraftingComponentCard>() ) {
            componentCard.SetCardData();
        }
    }

    void AddComponentCard( WeaponCraftingComponentData componentData, IWeapon weapon, Transform parentTransform ) {
        // INSTANTIATE AND ADD A COMPONENT CARD
        GameObject componentCardInstance =  Instantiate(
            original: weaponCraftingComponentCard, 
            position: Vector3.zero, 
            rotation: Quaternion.identity
        );
        componentCardInstance.transform.SetParent( parentTransform );
        componentCardInstance.transform.localScale = new Vector3( 1, 1, 1 );

        // SET CARD COMPONENT-RELATED DATA
        componentCardInstance.GetComponent<WeaponCraftingComponentCard>().SetComponentData(
            componentData: componentData
        );

        // SET COMPONENT CARD WEAPON-RELATED DATA
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

    void SetComponentAsCurrent( WeaponCraftingComponentCard componentCard, int index ) {
        // IF CURRENT COMPONENT IS EMPTY, REMOVE CARD FROM AVAILABLE COMPONENTS
        // ELSE, EXCHANGE THE CURRENT COMPONENT AND THE CARD COMPONENT
        if( currentComponents.transform.GetChild( index ).childCount == 0 ) {
            
        }
        else {

        }

        // ADD A COMPONENT CARD TO THE CURRENT COMPONENT AT GIVEN INDEX

    }

    void FindCraftingResult() {
        // FIND RESULT
        // CraftingResultFinder.FindCraftingResult

        // SET RESULT SPRITE
        // resultImage.sprite = 
    }
}
