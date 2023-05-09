using System;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    // COMPONENTS
    private Collider2D cd;
    private SpriteRenderer spriteRenderer;
    private IngredientInteract ingredientInteract;

    // VARIABLES
    [SerializeField] private IngredientData ingredientData;
    public IngredientData IngredientData {
        get { return ingredientData; }
    }

    void Awake() {
        cd = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ingredientInteract = GetComponent<IngredientInteract>();
    }
}
