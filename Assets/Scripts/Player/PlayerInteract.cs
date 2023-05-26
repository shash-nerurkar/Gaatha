using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Player player;

    // VARIABLES
    private List<Interactable> currentInteractables = new List<Interactable>();
    public void ClearInteractable(Interactable interactable) {
        if( currentInteractables.Contains( interactable ) ) currentInteractables.Remove( interactable );
    }

    void Awake() {
        player = GetComponentInParent<Player>();

        InteractInput.InteractPressedAction += InteractWithInteractable;

        WeaponCraftingPanel.FetchComponentsInRangeAction += SendInteractableIngredients;
    }

    void OnTriggerEnter2D(Collider2D collided) {
        // if(currentInteractables.Count > 0) return;

        Interactable collidedInteractable = collided.GetComponent<Interactable>();
        if( collidedInteractable != null ) {
            currentInteractables.Add(collidedInteractable);

            // UPDATE HUD
            player.UI.ToggleInteractableButton( isInteractable: true );
        }
    }

    void OnTriggerExit2D(Collider2D collided) {
        Interactable collidedInteractable = collided.GetComponent<Interactable>();

        if( currentInteractables.Contains( collidedInteractable ) )
            ClearInteractable(collidedInteractable);

        // UPDATE HUD
        if(currentInteractables.Count <= 0)
            player.UI.ToggleInteractableButton( isInteractable: false );
    }
    
    public void InteractWithInteractable() {
        if( currentInteractables.Count == 0 ) return;

        List<Interactable> ingredientInteractables = new List<Interactable>();
        List<Interactable> startGamePortals = new List<Interactable>();
        List<Interactable> trophyInteractables = new List<Interactable>();
        List<Interactable> weaponInteractables = new List<Interactable>();

        foreach( Interactable interactable in currentInteractables ) {
            if( interactable is IngredientInteract ) ingredientInteractables.Add( interactable );
            else if( interactable is WeaponInteract ) weaponInteractables.Add( interactable );
            else if( interactable is TrophyInteract ) trophyInteractables.Add( interactable );
            else if( interactable is StartGamePortal ) startGamePortals.Add( interactable );
        }

        if( weaponInteractables.Count > 0 ) {
            weaponInteractables[0].Interact();
        }
        else if( trophyInteractables.Count > 0 ) {
            trophyInteractables[0].Interact();
        }
        else if( ingredientInteractables.Count > 0 ) {
            foreach( Interactable interactable in ingredientInteractables ) 
                interactable.Interact();
        }
        else if( startGamePortals.Count > 0 ) {
            startGamePortals[0].Interact();
        }
    }
    
    void SendInteractableIngredients( out List<Ingredient> ingredientsInRange, out List<IWeapon> weaponsInRange ) {
        ingredientsInRange = new List<Ingredient>();            
        weaponsInRange = new List<IWeapon>();

        if( currentInteractables.Count > 0 ) {
            foreach( Interactable interactable in currentInteractables ) {
                if( interactable is IngredientInteract ) 
                    ingredientsInRange.Add( ( interactable as IngredientInteract ).Ingredient );
                else if( interactable is WeaponInteract ) 
                    weaponsInRange.Add( ( interactable as WeaponInteract ).Weapon );
            }
        }
    }

    void OnDestroy() {
        InteractInput.InteractPressedAction -= InteractWithInteractable;

        WeaponCraftingPanel.FetchComponentsInRangeAction -= SendInteractableIngredients;
    }
}
