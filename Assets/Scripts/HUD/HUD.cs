using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // HUD ELEMENTS
    public MovementInput MovementInput { get; private set; }
    public WeaponShootInput WeaponShootInput { get; private set; }
    public WeaponSwitchInput WeaponSwitchInput { get; private set; }
    public InteractInput InteractInput { get; private set; }
    public WeaponCraftingInput WeaponCraftingInput { get; private set; }
    public PauseInput PauseInput { get; private set; }

    // VARIABLES
    private List<Tweener> HUDElementTweeners;

    void Awake() {
        HUDElementTweeners = new List<Tweener>();

        MovementInput = GetComponentInChildren<MovementInput>();
        WeaponShootInput = GetComponentInChildren<WeaponShootInput>();
        WeaponSwitchInput = GetComponentInChildren<WeaponSwitchInput>();
        InteractInput = GetComponentInChildren<InteractInput>();
        WeaponCraftingInput = GetComponentInChildren<WeaponCraftingInput>();
        PauseInput = GetComponentInChildren<PauseInput>();

        MovementInput.Init( HUD: GetComponent<HUD>() );
        WeaponShootInput.Init( HUD: GetComponent<HUD>() );
        WeaponSwitchInput.Init( HUD: GetComponent<HUD>() );
        InteractInput.Init( HUD: GetComponent<HUD>() );
        WeaponCraftingInput.Init( HUD: GetComponent<HUD>() );
        PauseInput.Init( HUD: GetComponent<HUD>() );

        // HIDE ALL ELEMENTS INITIALLY
        AdjustHUDElements(
            showMovementInput: false,
            showWeaponShootInput: false,
            showWeaponSwitchInput: false,
            showInteractInput: false,
            showWeaponCraftingInput: false,
            showPauseInput: false,
            duration: 0
        );
    }

    public void AdjustHUDElements(
        bool showMovementInput,
        bool showWeaponShootInput,
        bool showWeaponSwitchInput,
        bool showInteractInput,
        bool showWeaponCraftingInput,
        bool showPauseInput,
        float duration
    ) {
        foreach( Tweener tweener in HUDElementTweeners )
            tweener.Kill();
        HUDElementTweeners.Clear();

        // MOVEMENT BUTTON
        HUDElementTweeners.Add(
            MovementInput.RectTransform.DOAnchorPos( 
                showMovementInput ? MovementInput.ShowPosition : MovementInput.HidePosition, 
                duration 
            )
        );
        

        // WEAPON SHOOT BUTTON
        HUDElementTweeners.Add(
            WeaponShootInput.RectTransform.DOAnchorPos( 
                showWeaponShootInput ? WeaponShootInput.ShowPosition : WeaponShootInput.HidePosition, 
                duration 
            )
        );

        // WEAPON SWITCH BUTTON
        HUDElementTweeners.Add(
            WeaponSwitchInput.RectTransform.DOAnchorPos( 
                showWeaponSwitchInput ? WeaponSwitchInput.ShowPosition : WeaponSwitchInput.HidePosition, 
                duration 
            )
        );
        
        // INTERACT BUTTON
        HUDElementTweeners.Add(
            InteractInput.RectTransform.DOAnchorPos( 
                showInteractInput ? InteractInput.ShowPosition : InteractInput.HidePosition, 
                duration 
            )
        );
        
        // WEAPON CRAFTING BUTTON
        HUDElementTweeners.Add(
            WeaponCraftingInput.RectTransform.DOAnchorPos( 
                showWeaponCraftingInput ? WeaponCraftingInput.ShowPosition : WeaponCraftingInput.HidePosition,  
                duration 
            )
        );

        // PAUSE BUTTON
        HUDElementTweeners.Add(
            PauseInput.RectTransform.DOAnchorPos( 
                showPauseInput ? PauseInput.ShowPosition : PauseInput.HidePosition, 
                duration 
            )
        );
    }
}
