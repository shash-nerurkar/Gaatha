using System;
using UnityEngine;

public class IngredientInteract : Interactable
{
    public Ingredient Ingredient { get; private set; }
    private BoxCollider2D cd;
    
    // ACTIONS
    public static event Action<Ingredient> InteractWithIngredientAction;

    void Awake() {
        cd = GetComponent<BoxCollider2D>();
        Ingredient = GetComponent<Ingredient>();
    }

    public override void Interact() {
        InteractWithIngredientAction?.Invoke( Ingredient );
    }
}
