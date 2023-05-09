using System;
using UnityEngine;

public class IngredientInteract : Interactable
{
    private Ingredient ingredient;
    private BoxCollider2D cd;
    
    // EVENTS
    public static event Action<Ingredient> InteractWithIngredientAction;

    void Awake() {
        cd = GetComponent<BoxCollider2D>();
        ingredient = GetComponent<Ingredient>();
    }

    public override void Interact() {
        InteractWithIngredientAction?.Invoke( ingredient );
    }
}
