using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    private DragDroppableUI dragDroppableUI;
    private RectTransform rectTransform;
    private bool isCurrentComponent;
    public bool IsCurrentComponent {
        get { return isCurrentComponent; }
        set { isCurrentComponent = value; }
    }

    // VARIABLES
    private List<Tweener> onDragTweeners = new List<Tweener>();

    void Awake() {
        dragDroppableUI = GetComponent<DragDroppableUI>();
        rectTransform = GetComponent<RectTransform>();

        dragDroppableUI.OnSnapBackDoneAction += OnSnapBackDone;

        InputManager.WeaponCraftingScreenPositionAction += dragDroppableUI.OnScreenPositionPerformed;

        InputManager.WeaponCraftingComponentCardPressPerformedAction += OnPressPerformed;

        InputManager.WeaponCraftingComponentCardPressCanceledAction += OnPressedCanceled;
    }

    // CONVERT COMPONENT CARD TO ON-DRAG CARD
    void OnPressPerformed() {
        if( !dragDroppableUI.IsClickedOn ) return;

        // CLEAR EXISTING DRAG TWEENERS
        foreach( Tweener tweener in onDragTweeners )
            tweener.Kill();
        onDragTweeners.Clear();

        // CALL DRAG LOGIC
        dragDroppableUI.OnClickPerformed();

        // FADE OUT CARD COMPONENTS
        for( int i = 0; i < transform.childCount; i++ ) {
            Transform currentChildTransform = transform.GetChild( i );
            if( currentChildTransform.gameObject == onDragCard.gameObject ) {
                foreach( Graphic childGraphic in currentChildTransform.GetComponentsInChildren<Graphic>() )
                    onDragTweeners.Add( childGraphic.DOFade( endValue: 1f, duration: 0.25f ) );
            }
            else {
                foreach( Graphic childGraphic in currentChildTransform.GetComponentsInChildren<Graphic>() )
                    onDragTweeners.Add( childGraphic.DOFade( endValue: 0f, duration: 0.15f ) );
            }
        }
        onDragTweeners.Add( GetComponent<Graphic>()?.DOFade( endValue: 0f, duration: 0.15f ) );

        // SET ON-DRAG CARD
        RectTransformUtility.ScreenPointToLocalPointInRectangle( rectTransform, Input.mousePosition, Camera.main, out Vector2 localPt );
        onDragCard.SetClickPosition( anchoredPosition: localPt );
        onDragCard.ToggleCollider( active: true );
    }

    // CONVERT ON-DRAG CARD BACK TO COMPONENT CARD
    void OnPressedCanceled() {
        if( !dragDroppableUI.IsDragging ) return;

        // CLEAR EXISTING DRAG TWEENERS
        foreach( Tweener tweener in onDragTweeners )
            tweener.Kill();
        onDragTweeners.Clear();

        // CALL DRAG LOGIC
        dragDroppableUI.OnClickCanceled();

        // FADE IN CARD COMPONENTS
        for( int i = 0; i < transform.childCount; i++ ) {
            Transform currentChildTransform = transform.GetChild( i );
            if( currentChildTransform.gameObject == onDragCard.gameObject ) {
                foreach( Graphic childGraphic in currentChildTransform.GetComponentsInChildren<Graphic>() )
                    onDragTweeners.Add( childGraphic.DOFade( endValue: 0f, duration: 0.15f ) );
            }
            else {
                foreach( Graphic childGraphic in currentChildTransform.GetComponentsInChildren<Graphic>() )
                    onDragTweeners.Add( childGraphic.DOFade( endValue: 1f, duration: 0.25f ) );
            }
        }
        onDragTweeners.Add( GetComponent<Graphic>()?.DOFade( endValue: 1f, duration: 0.25f ) );

        // RESET ON-DRAG CARD
        onDragCard.ResetPosition( duration: 0.15f );
        onDragCard.ToggleCollider( active: false );
    }

    void OnSnapBackDone() {}

    // SET THE CARD'S DATA
    public void SetCardData() {
        dragDroppableUI.SetSnapBackAnchoredPosition( snapBackAnchoredPosition: transform.GetComponent<RectTransform>().anchoredPosition );
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
        
        // SET ON-DRAG CARD'S DATA
        onDragCard.SetComponentData( componentData: componentData );
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
    
    void OnDestroy() {
        InputManager.WeaponCraftingScreenPositionAction -= dragDroppableUI.OnScreenPositionPerformed;

        InputManager.WeaponCraftingComponentCardPressPerformedAction -= OnPressPerformed;

        InputManager.WeaponCraftingComponentCardPressCanceledAction -= OnPressedCanceled;
    }
}
