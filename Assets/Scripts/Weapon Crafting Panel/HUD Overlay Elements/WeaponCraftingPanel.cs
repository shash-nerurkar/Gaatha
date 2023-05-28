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
    [SerializeField] private WeaponCraftingCurrentComponentPanelsPanel currentComponents;
    [SerializeField] private WeaponCraftingResultsPanel results;
    
    // VARIABLES
    private bool isPanelActive = false;

    // ACTIONS
    public static event Action EnableWeaponCraftingActionsAction;
    public static event Action EnablePlayerOnFootActionsAction;
    public static event Action<IWeapon> SetCraftedWeaponAsPrimaryAction;

    // FUNCS
    public delegate void GetWeaponsAction( out List<IWeapon> weaponsInArm );
    static public WeaponCraftingPanel.GetWeaponsAction FetchWeaponInArmAction;
    public delegate void GetIngredientsAction( out List<Ingredient> ingredientsInRange, out List<IWeapon> weaponsInRange );
    static public WeaponCraftingPanel.GetIngredientsAction FetchComponentsInRangeAction;
    static public WorldManager.GetAllWeaponsInGameDelegate GetAllGameWeaponsAction;

    void Awake() {
        WeaponCraftingInput.StartWeaponCraftingAction += OnInteractWithWeaponCraftingInput;

        IngredientInteract.InteractWithIngredientAction += OnInteractWithIngredient;

        WeaponCraftingComponentCard.OnComponentSelectedAction += FindCraftingResult;

        WeaponCraftingResultsPanel.CraftWeaponAction += CraftWeapon;

        // HIDE PANEL ON BACK BUTTON CLICK
        backButton.onClick.AddListener(delegate { 
            TogglePanelActive( toggleStatus: false );
        });
    }

    void OnInteractWithWeaponCraftingInput() {
        // GET INGREDIENTS AND WEAPONS IN RANGE
        List<Ingredient> ingredientsInRange = null;
        List<IWeapon> weaponsInRange = null;
        FetchComponentsInRangeAction?.Invoke( out ingredientsInRange, out weaponsInRange );

        ActivatePanel( ingredientsInRange: ingredientsInRange, weaponsInRange: weaponsInRange );
    }

    void OnInteractWithIngredient( Ingredient ingredient ) {
        ActivatePanel( ingredientsInRange: new List<Ingredient>() { ingredient }, weaponsInRange: null );
    }

    void ActivatePanel( List<Ingredient> ingredientsInRange, List<IWeapon> weaponsInRange ) {
        // IF PANEL ISN'T SHOWN YET
        if( !isPanelActive ) {
            // SHOW PANEL
            TogglePanelActive( toggleStatus: true );

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
                        ingredient: null,
                        parentTransform: availableComponents.ElementsContainer.transform
                    );
        }

        // ADD COMPONENT CARD FOR INCOMING INGREDIENT(S)
        if( ingredientsInRange != null )
            foreach( Ingredient ingredient in ingredientsInRange )
                AddComponentCard(
                    componentData: ingredient.IngredientData.componentData, 
                    weapon: null,
                    ingredient: ingredient,
                    parentTransform: availableComponents.ElementsContainer.transform
                );

        // ADD COMPONENT CARD FOR INCOMING INGREDIENT(S)
        if( weaponsInRange != null )
            foreach( IWeapon weapon in weaponsInRange )
                AddComponentCard(
                    componentData: weapon.WeaponData.componentData, 
                    weapon: weapon,
                    ingredient: null,
                    parentTransform: availableComponents.ElementsContainer.transform
                );

        availableComponents.AdjustGrid();
    }

    void AddComponentCard(
        WeaponCraftingComponentData componentData,
        IWeapon weapon,
        Ingredient ingredient,
        Transform parentTransform
    ) {
        // INSTANTIATE AND ADD A COMPONENT CARD
        GameObject componentCardInstance =  Instantiate(
            original: weaponCraftingComponentCard, 
            position: Vector3.zero, 
            rotation: Quaternion.identity,
            parent: parentTransform
        );
        WeaponCraftingComponentCard componentCardInstanceCard = componentCardInstance.GetComponent<WeaponCraftingComponentCard>();

        // SET PARENT DATA
        componentCardInstanceCard.SetParent( parentTransform: parentTransform );

        // SET DATA
        componentCardInstanceCard.SetData( componentData: componentData );

        // SET COMPONENT TYPE DATA
        componentCardInstanceCard.SetComponentTypeData( weapon: weapon, ingredient: ingredient );
    }

    void TogglePanelActive( bool toggleStatus ) {
        isPanelActive = toggleStatus;

        if( isPanelActive ) {
            EnableWeaponCraftingActionsAction?.Invoke();
        }
        else {
            EnablePlayerOnFootActionsAction?.Invoke();

            currentComponents.ClearAllSelectedComponents();
            
            availableComponents.ClearGrid();

            results.ClearResults();
        }

        for( int index = 0; index < transform.childCount; index++ ) {
            transform.GetChild(index).gameObject.SetActive( toggleStatus );
        }
    }

    void FindCraftingResult() {
        if( !currentComponents.CanCraft ) return;

        results.ClearResults();

        // FIND RESULT
        List<WeaponData> resultWeaponsDatas = WeaponCraftingResultFinder.GetCraftingResult( componentDatas: currentComponents.GetCurrentComponentDatas() );

        // SET RESULT SPRITE
        results.SetResults( weaponDatas: resultWeaponsDatas );
    }
    
    async void CraftWeapon( WeaponData weaponData, int craftingCost ) {
        // FETCH ALL WEAPONS IN THE GAME FROM THE WORLD
        List<IWeapon> allGameWeapons = null;
        GetAllGameWeaponsAction?.Invoke( out allGameWeapons );
        if( allGameWeapons == null ) return;

        // GET THE WEAPON TO CRAFT
        GameObject weaponObjectToCraft = null;
        foreach( IWeapon weapon in allGameWeapons ) {
            if( weapon.WeaponData.Id == weaponData.Id ) {
                weaponObjectToCraft = weapon.GameObject;
                break;
            }
        }
        if( weaponObjectToCraft == null ) return;

        // DESTROY THE BASE COMPONENTS
        currentComponents.DeleteOriginalObjectsForSelectedComponents();

        await System.Threading.Tasks.Task.Delay( millisecondsDelay: Mathf.CeilToInt(Time.deltaTime * 1000) );

        // CREATE WEAPON TO CRAFT
        GameObject craftedWeaponObject = Instantiate( original: weaponObjectToCraft, position: Vector3.zero, rotation: Quaternion.identity );
        IWeapon craftedWeapon = craftedWeaponObject.GetComponent<IWeapon>();

        SetCraftedWeaponAsPrimaryAction?.Invoke( craftedWeapon );

        // CLOSE PANEL
        TogglePanelActive( toggleStatus: false );
    }

    void OnDestroy() {
        WeaponCraftingInput.StartWeaponCraftingAction -= OnInteractWithWeaponCraftingInput;

        IngredientInteract.InteractWithIngredientAction -= OnInteractWithIngredient;

        WeaponCraftingComponentCard.OnComponentSelectedAction -= FindCraftingResult;

        WeaponCraftingResultsPanel.CraftWeaponAction -= CraftWeapon;
    }
}
