using UnityEngine;
using System;

public class StartGamePortal : Interactable
{
    private BoxCollider2D cd;
    
    // EVENTS
    public static event Action StartGameAction;

    void Awake() {
        cd = GetComponent<BoxCollider2D>();
    }

    public override void Interact() {
        StartGameAction?.Invoke();
    }
}
