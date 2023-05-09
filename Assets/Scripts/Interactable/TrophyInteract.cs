using UnityEngine;
using System;

public class TrophyInteract : Interactable
{
    private BoxCollider2D cd;
    private Trophy trophy;
    
    // EVENTS
    public static event Action<Trophy> TrophyInteractAction;

    void Awake() {
        cd = GetComponent<BoxCollider2D>();

        trophy = transform.GetComponentInParent<Trophy>();
    }

    public override void Interact() {
        TrophyInteractAction?.Invoke( trophy );
    }
}
