using UnityEngine;
using System;

public class PlayerUI : MonoBehaviour
{
    private Player player;

    // private InteractablePromptPanel InteractablePromptPanel;

    // private ValueBar HealthBar;

    // EVENTS
    public static event Action<bool> InteractableToggleAction;
    
    void Start() {
        player = GetComponentInParent<Player>();
    }

    public void ToggleInteractableButton ( bool isInteractable ) {
        InteractableToggleAction( isInteractable );
    }
}
