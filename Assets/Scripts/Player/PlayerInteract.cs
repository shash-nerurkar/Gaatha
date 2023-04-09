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

    void Start() {
        player = GetComponentInParent<Player>();
    }

    void OnTriggerEnter2D(Collider2D collided) {
        if(currentInteractables.Count > 0) return;

        Interactable collidedInteractable = collided.GetComponent<Interactable>();
        if( collidedInteractable != null ) {
            currentInteractables.Add(collidedInteractable);

            // UPDATE HUD
            player.UI.ToggleWeaponPickupButtonInteractable( isInteractable: true );
            // player.UI.InteractablePromptPanel.UpdateText(interactable.onInteractableSeenMessage);
        }
    }

    void OnTriggerExit2D(Collider2D collided) {
        Interactable collidedInteractable = collided.GetComponent<Interactable>();

        if( currentInteractables.Contains( collidedInteractable ) ) {
            ClearInteractable(collidedInteractable);

            // UPDATE HUD
            // player.UI.InteractablePromptPanel.UpdateText(interactable.onInteractableSeenMessage);
        }

        if(currentInteractables.Count <= 0) {
            // UPDATE HUD
            player.UI.ToggleWeaponPickupButtonInteractable( isInteractable: false );
        }
    }
    
    // From HUD WeaponPickupInput.cs
    public void PickUpWeapon() {
        if(currentInteractables.Count > 0) currentInteractables[0].Interact();
    }
}
