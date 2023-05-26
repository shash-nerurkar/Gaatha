using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponCraftingCurrentComponentPanel : MonoBehaviour
{
    // COMPONENTS
    private Image image;

    // VARIABLES
    private WeaponCraftingComponentCard hoveringComponentCard;

    private WeaponCraftingComponentCard selectedComponentCard;
    public WeaponCraftingComponentCard SelectedComponentCard {
        get { return selectedComponentCard; }
        private set { selectedComponentCard = value; }
    }

    void Awake() {
        image = GetComponent<Image>();
    }

    // WHEN SOME COLLIDABLE OBJECT IS OVERLAPPING
    void OnTriggerStay2D( Collider2D collided ) {
        WeaponCraftingComponentOnDragCard componentOnDragCard = collided.GetComponent<WeaponCraftingComponentOnDragCard>();

        if( componentOnDragCard != null && componentOnDragCard.ComponentCard.IsCardDraggable ) {
            // IF THE OBJECT IS IN SELECTED FOR ANOTHER CURRENT-COMPONENT-PANEL, THEN RETURN
            if( componentOnDragCard.ComponentCard.CurrentAvailableComponentPanel ) return;

            // OTHERWISE, SET COMPONENT CARD AS CURRENT
            hoveringComponentCard = componentOnDragCard.ComponentCard;
            componentOnDragCard.ComponentCard.CurrentAvailableComponentPanel = this;
            image.DOFade( endValue: 0.5f, duration: 0.25f );
        }
    }

    // WHEN SOME OVERLAPPING COLLIDABLE OBJECTS STOPS OVERLAPPING
    void OnTriggerExit2D(Collider2D collided) {
        WeaponCraftingComponentOnDragCard componentOnDragCard = collided.GetComponent<WeaponCraftingComponentOnDragCard>();

        if( componentOnDragCard != null && componentOnDragCard.ComponentCard.IsCardDraggable ) {
            // IF COMPONENT CARD IS NOT THE CURRENT HOVERING COMPONENT CARD, THEN RETURN
            if( componentOnDragCard.ComponentCard != hoveringComponentCard ) return;

            // OTHERWISE, CLEAR THE CURRENT HOVERING COMPONENT CARD
            hoveringComponentCard = null;
            componentOnDragCard.ComponentCard.CurrentAvailableComponentPanel = null;
            image.DOFade( endValue: 1f, duration: 0.25f );
        }
    }

    public void SelectComponent( WeaponCraftingComponentCard componentCard ) {
        // IF CURRENT COMPONENT IS EMPTY, REMOVE CARD FROM AVAILABLE COMPONENTS
        if( SelectedComponentCard == null ) {
            SelectedComponentCard = componentCard;
            componentCard.SetParent( parentTransform: transform );
            componentCard.SetData( IsCardDraggable: false );

        }
        // ELSE, EXCHANGE THE CURRENT COMPONENT AND THE CARD COMPONENT
        else {
            WeaponCraftingComponentData selectedComponentData = SelectedComponentCard.ComponentData;
            IWeapon selectedComponentWeapon = SelectedComponentCard.Weapon;
            Ingredient selectedComponentIngredient = SelectedComponentCard.Ingredient;

            SelectedComponentCard.SetData( componentData: componentCard.ComponentData );
            SelectedComponentCard.SetComponentTypeData( weapon: componentCard.Weapon, ingredient: componentCard.Ingredient );

            componentCard.SetData( componentData: selectedComponentData );
            componentCard.SetComponentTypeData( weapon: selectedComponentWeapon, ingredient: selectedComponentIngredient );
        }
    }

    public void ClearSelectedComponent() {
        if( SelectedComponentCard != null ) {
            Destroy( SelectedComponentCard.gameObject );
            image.DOFade( endValue: 1f, duration: 0.25f );
        }
    }
}
