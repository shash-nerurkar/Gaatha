using UnityEngine;
using System;

public class PlayerUI : MonoBehaviour
{
    private Player player;

    // private InteractablePromptPanel InteractablePromptPanel;

    // private ValueBar HealthBar;

    // ACTIONS
    public static event Action<bool> InteractInputToggleAction;
    
    void Start() {
        player = GetComponentInParent<Player>();
    }

    public void ToggleInteractableButton ( bool isInteractable ) {
        InteractInputToggleAction?.Invoke( isInteractable );
    }
}
