using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponCraftingCurrentComponentPanel : MonoBehaviour
{
    // COMPONENTS
    private Image image;

    // VARIABLES
    private WeaponCraftingComponentCard currentHoveringComponentCard;

    void Awake() {
        image = GetComponent<Image>();
    }

    // WHEN SOME COLLIDABLE OBJECT IS OVERLAPPING
    void OnTriggerStay2D( Collider2D collided ) {
        WeaponCraftingComponentOnDragCard componentOnDragCard = collided.GetComponent<WeaponCraftingComponentOnDragCard>();

        if( componentOnDragCard != null ) {
            print("OnTriggerStay2D componentOnDragCard.ComponentCard.IsCurrentComponent: " + componentOnDragCard.ComponentCard.IsCurrentComponent);
            // IF THE OBJECT IS IN SELECTED FOR ANOTHER CURRENT-COMPONENT-PANEL, THEN RETURN
            if( componentOnDragCard.ComponentCard.IsCurrentComponent ) return;

            // OTHERWISE, SET COMPONENT CARD AS CURRENT
            currentHoveringComponentCard = componentOnDragCard.ComponentCard;
            componentOnDragCard.ComponentCard.IsCurrentComponent = true;
            image.DOFade( endValue: 0.5f, duration: 0.25f );
        }
    }

    // WHEN SOME OVERLAPPING COLLIDABLE OBJECTS STOPS OVERLAPPING
    void OnTriggerExit2D(Collider2D collided) {
        WeaponCraftingComponentOnDragCard componentOnDragCard = collided.GetComponent<WeaponCraftingComponentOnDragCard>();

        if( componentOnDragCard != null ) {
            print("OnTriggerStay2D componentOnDragCard.ComponentCard.IsCurrentComponent: " + componentOnDragCard.ComponentCard.IsCurrentComponent);
            // IF COMPONENT CARD IS NOT THE CURRENT HOVERING COMPONENT CARD, THEN RETURN
            if( componentOnDragCard.ComponentCard != currentHoveringComponentCard ) return;

            // OTHERWISE, CLEAR THE CURRENT HOVERING COMPONENT CARD
            currentHoveringComponentCard = null;
            componentOnDragCard.ComponentCard.IsCurrentComponent = false;
            image.DOFade( endValue: 1f, duration: 0.25f );
        }
    }
}
