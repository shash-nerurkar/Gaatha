using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions.OnFootActions OnFootActions;
    private PlayerInputActions.InWeaponCraftingActions InWeaponCraftingActions;

    // ACTIONS
    // PLAYER ON FOOT ACTIONS
    public static event Action<Vector2> OnPlayerMoveAction;
    public static event Action<Vector2> OnPlayerLookAction;
    public static event Action<bool> OnPlayerWeaponAttackAction;

    // WEAPON CRAFTING ACTIONS
    public static event Action<Vector3> WeaponCraftingScreenPositionAction;
    public static event Action WeaponCraftingComponentCardPressPerformedAction;
    public static event Action WeaponCraftingComponentCardPressCanceledAction;

    void Awake() {
        PlayerInputActions fpsPlayerInputActions = new PlayerInputActions();
        
        OnFootActions = fpsPlayerInputActions.OnFoot;
        InWeaponCraftingActions = fpsPlayerInputActions.InWeaponCrafting;

        WeaponCraftingPanel.EnablePlayerOnFootActionsAction += EnableOnFootActions;
        WeaponCraftingPanel.EnableWeaponCraftingActionsAction += EnableInWeaponCraftingActions;

		InWeaponCraftingActions.ScreenPosition.performed += context => {
			WeaponCraftingScreenPositionAction?.Invoke( context.ReadValue<Vector2>() );
		};
		
		InWeaponCraftingActions.ComponentCardPress.performed += _ => {
			WeaponCraftingComponentCardPressPerformedAction?.Invoke();
		};
		InWeaponCraftingActions.ComponentCardPress.canceled += _ => {
			WeaponCraftingComponentCardPressCanceledAction?.Invoke();
		};
    }

    void FixedUpdate() {
        // MOVE PLAYER
        OnPlayerMoveAction?.Invoke( OnFootActions.Movement.ReadValue<Vector2>() );

        // SET PLAYER LOOK
        OnPlayerLookAction?.Invoke( OnFootActions.Look.ReadValue<Vector2>().normalized );

        // USE PLAYER WEAPON
        OnPlayerWeaponAttackAction?.Invoke( OnFootActions.Look.ReadValue<Vector2>().magnitude == 1 );
    }

    public void EnableOnFootActions() {
        OnFootActions.Enable();

        InWeaponCraftingActions.Disable();
    }

    public void EnableInWeaponCraftingActions() {
        InWeaponCraftingActions.Enable();

        OnFootActions.Disable();
    }
    
    void OnDisable() {
        OnFootActions.Disable();
        InWeaponCraftingActions.Disable();
    }
}
