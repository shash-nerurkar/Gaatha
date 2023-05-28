using UnityEngine;
using System;

public class StartGamePortal : Interactable
{
    private BoxCollider2D cd;
    
    // ACTIONS
    public static event Action<SceneBuildIndex> StartGameAction;

    void Awake() {
        cd = GetComponent<BoxCollider2D>();
    }

    public override void Interact() {
        StartGameAction?.Invoke( SceneBuildIndex.World );
    }
}
