using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuHUD : MonoBehaviour
{
    // HUD ELEMENTS
    public MainMenuTitlePanel MainMenuTitlePanel { get; private set; }
    public MainMenuButtonsPanel MainMenuButtonsPanel { get; private set; }
    
    // VARIABLES
    private List<Tweener> HUDElementTweeners;

    void Awake() {
        HUDElementTweeners = new List<Tweener>();

        MainMenuTitlePanel = GetComponentInChildren<MainMenuTitlePanel>();
        MainMenuButtonsPanel = GetComponentInChildren<MainMenuButtonsPanel>();

        // HIDE ALL ELEMENTS INITIALLY
        AdjustHUDElements(
            showTitlePanel: false,
            showButtonsPanel: false,
            duration: 0
        );
    }

    public void AdjustHUDElements(
        bool showTitlePanel,
        bool showButtonsPanel,
        float duration
    ) {
        foreach( Tweener tweener in HUDElementTweeners )
            tweener.Kill();
        HUDElementTweeners.Clear();

        // MOVEMENT BUTTON
        HUDElementTweeners.Add(
            MainMenuTitlePanel.RectTransform.DOAnchorPos( 
                showTitlePanel ? MainMenuTitlePanel.ShowPosition : MainMenuTitlePanel.HidePosition, 
                duration 
            )
        );

        // WEAPON SHOOT BUTTON
        HUDElementTweeners.Add(
            MainMenuButtonsPanel.RectTransform.DOAnchorPos( 
                showButtonsPanel ? MainMenuButtonsPanel.ShowPosition : MainMenuButtonsPanel.HidePosition, 
                duration 
            )
        );
    }
}
