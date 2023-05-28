using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

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
    [SerializeField] private WeaponCraftingComponentOnDragCard onDragCard;
    private LayoutElement LayoutElement;
    private Collider2D cd;
    private DragDroppableUI dragDroppableUI;
    private RectTransform rectTransform;
    private WeaponCraftingCurrentComponentPanel currentAvailableComponentPanel;
    public WeaponCraftingCurrentComponentPanel CurrentAvailableComponentPanel {
        get { return currentAvailableComponentPanel; }
        set { currentAvailableComponentPanel = value; }
    }

    // VARIABLES
    [HideInInspector] public bool IsCardDraggable;
    private List<Tweener> onDragTweeners = new List<Tweener>();
    public WeaponCraftingComponentData ComponentData { get; private set; }
    public IWeapon Weapon { get; private set; }
    public WeaponData WeaponData { get; private set; }
    public Ingredient Ingredient { get; private set; }

    // ACTIONS
    public static event Action<WeaponCraftingComponentCard> OnRemoveComponentAction;
    public static event Action OnComponentSelectedAction;

    // INITIALIZING THE CARD
    public void SetParent( Transform parentTransform ) {
        transform.SetParent( parentTransform );
        transform.localScale = new Vector3( 1, 1, 1 );
    }

    public void SetComponentTypeData( IWeapon weapon, Ingredient ingredient ) {
        Ingredient = ingredient;
        Weapon = weapon;
        
        SetWeaponData( weaponData: Weapon == null ? null : Weapon.WeaponData );
    }

    // SET THE WEAPON-SPECIFIC DATA TO THE COMPONENT
    public void SetWeaponData( WeaponData weaponData ) {
        WeaponData = weaponData;

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

    public void SetData(
        WeaponCraftingComponentData componentData = null,
        bool IsCardDraggable = true,
        bool IsLayoutEnabled = false,
        bool IsCardCollidable = false
    ) {
        // SET COMPONENT DATA
        if( componentData != null ) {
            ComponentData = componentData;

            // SET COMPONENT DATA
            componentImage.sprite = componentData.PanelSprite;
            componentNameLabel.text = componentData.name;
            componentTypeIcon.sprite = componentData.ComponentIcon;
        }
        
        // SET ELEMENT DATA
        if( componentData != null ) {
            // CLEAR ELEMENTS PANEL
            for( int i = 0; i < elementsPanel.transform.childCount; i++ )
                Destroy( elementsPanel.transform.GetChild( i ).gameObject );

            // FIND ELEMENTS PRESENT IN COMPONENT
            List<ElementData> elementsInComponent = new List<ElementData>();
            foreach( IngredientData ingredient in componentData.Ingredients )
                if( !elementsInComponent.Contains( ingredient.elementData ) )
                    elementsInComponent.Add( ingredient.elementData );

            // SET ELEMENT CARDS IN COMPONENT CARD
            foreach( ElementData elementData in elementsInComponent )
                AddElementCard( elementData: elementData );
        }
    
        // SET ON-DRAG CARD'S DATA
        if( componentData != null ) 
            onDragCard.SetComponentData( componentData: componentData );

        this.IsCardDraggable = IsCardDraggable;

        LayoutElement.enabled = IsLayoutEnabled;

        cd.enabled = IsCardCollidable;
    }

    void Awake() {
        IsCardDraggable = true;

        cd = GetComponent<Collider2D>();
        LayoutElement = GetComponent<LayoutElement>();
        dragDroppableUI = GetComponent<DragDroppableUI>();
        rectTransform = GetComponent<RectTransform>();

        dragDroppableUI.OnSnapBackDoneAction += OnSnapBackDone;

        InputManager.WeaponCraftingScreenPositionAction += dragDroppableUI.OnScreenPositionPerformed;

        InputManager.WeaponCraftingComponentCardPressPerformedAction += OnPressPerformed;

        InputManager.WeaponCraftingComponentCardPressCanceledAction += OnPressedCanceled;
    }

    // CONVERT COMPONENT CARD TO ON-DRAG CARD
    void OnPressPerformed() {
        if( !dragDroppableUI.IsClickedOn || !IsCardDraggable ) return;

        // CALL DRAG LOGIC
        dragDroppableUI.OnClickPerformed();

        // CHANGE ON-DRAG CARD BACK TO NORMAL
        ToggleCardDragStatus( isDragging: true );
    }

    // CONVERT ON-DRAG CARD BACK TO COMPONENT CARD
    void OnPressedCanceled() {
        if( !dragDroppableUI.IsDragging || !IsCardDraggable ) return;

        if( CurrentAvailableComponentPanel != null ) {
            bool currentPanelHasComponent = CurrentAvailableComponentPanel.SelectedComponentCard != null;

            // DON'T SNAP BACK IF THE CARD IS GOING TO BE REMOVED FROM AVAILABLE COMPONENTS PANEL
            if( !currentPanelHasComponent ) 
                dragDroppableUI.ShouldSnapBack = false;

            // ADD A COMPONENT CARD TO THE CURRENT COMPONENT AT GIVEN INDEX
            CurrentAvailableComponentPanel?.SelectComponent( componentCard: this );

            // ON REMOVING CARD FROM AVAILABLE COMPONENTS
            if( !currentPanelHasComponent ) 
                OnRemoveComponentAction?.Invoke( this );
            
            OnComponentSelectedAction?.Invoke();
        }
        
        // CALL DRAG LOGIC
        dragDroppableUI.OnClickCanceled();

        // CHANGE ON-DRAG CARD BACK TO NORMAL
        ToggleCardDragStatus( isDragging: false );
    }

    void OnSnapBackDone() {}

    void ToggleCardDragStatus( bool isDragging ) {
        // CLEAR EXISTING DRAG TWEENERS
        foreach( Tweener tweener in onDragTweeners )
            tweener.Kill();
        onDragTweeners.Clear();

        // FADE CARD COMPONENTS
        for( int i = 0; i < transform.childCount; i++ ) {
            Transform currentChildTransform = transform.GetChild( i );
            if( currentChildTransform.gameObject == onDragCard.gameObject ) {
                foreach( Graphic childGraphic in currentChildTransform.GetComponentsInChildren<Graphic>() )
                    onDragTweeners.Add( childGraphic.DOFade( endValue: isDragging ? 1f : 0f, duration: isDragging ? 0.25f : 0.15f ) );
            }
            else {
                foreach( Graphic childGraphic in currentChildTransform.GetComponentsInChildren<Graphic>() )
                    onDragTweeners.Add( childGraphic.DOFade( endValue: isDragging ? 0f : 1f, duration: isDragging ? 0.15f : 0.25f ) );
            }
        }
        onDragTweeners.Add( GetComponent<Graphic>()?.DOFade( endValue: isDragging ? 0f : 1f, duration: isDragging ? 0.15f : 0.25f ) );

        // SET ON-DRAG CARD POSITION
        if( isDragging ) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle( rectTransform, Input.mousePosition, Camera.main, out Vector2 localPt );
            onDragCard.SetClickPosition( anchoredPosition: localPt );
        }
        else {
            onDragCard.ResetPosition( duration: 0.15f );
        }
        
        // SET ON-DRAG CARD COLLIDER
        onDragCard.ToggleCollider( active: isDragging );
    }

    // SET THE CARD'S DATA
    public void SetCardSnapData() {
        dragDroppableUI.SetSnapBackAnchoredPosition( snapBackAnchoredPosition: transform.GetComponent<RectTransform>().anchoredPosition );
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

    void OnDestroy() {
        InputManager.WeaponCraftingScreenPositionAction -= dragDroppableUI.OnScreenPositionPerformed;

        InputManager.WeaponCraftingComponentCardPressPerformedAction -= OnPressPerformed;

        InputManager.WeaponCraftingComponentCardPressCanceledAction -= OnPressedCanceled;
    }
}
