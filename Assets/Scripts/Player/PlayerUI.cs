using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private Player player;

    // private InteractablePromptPanel InteractablePromptPanel;

    // private ValueBar HealthBar;
    
    void Start() {
        player = GetComponentInParent<Player>();
    }

    public void ToggleWeaponPickupButtonInteractable( bool isInteractable ) {
        player.HUD.weaponPickupInput.ToggleInteractable( isInteractable: isInteractable );
    }
}
