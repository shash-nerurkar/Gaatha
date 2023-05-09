using System;

public class Constants {
    // GAMEOBJECT TAGS
    public const string PLAYER_TAG = "Player";
    public const string HUD_TAG = "HUD";
    public const string MAIN_CAMERA_TAG = "MainCamera";
    public const string BULLET_CONTAINER_TAG = "Bullet Container";
    public const string ENEMY_CONTAINER_TAG = "Enemy Container";
    public const string INGREDIENT_CONTAINER_TAG = "Ingredient Container";
    public const string FLOOR_WEAPON_CONTAINER_TAG = "Floor Weapon Container";
    public const string BACKGROUND_CONTAINER_TAG = "Background Container";
    public const string INFINITY_TEXT = "ꝏ";

    // LAYER MASKS
    public const string LAYERMASK_UI = "UI";

    // ANIMATION STATE NAMES
    public const string IDLE_ANIMATION_STATE_NAME = "idle";
    public const string ATTACK_ANIMATION_STATE_NAME = "attack";

    // SOUND NAMES
    public const string WAX_CROSSBOW_SOUND = "Wax Crossbow Sound";
    public const string WAX_SWORD_SOUND = "Wax Sword Sound";

    // SORTING LAYERS
    public const string SORTING_LAYER_FLOOR = "Floor";
    public const string SORTING_LAYER_FLOOR_OBJECT = "Floor object";
    public const string SORTING_LAYER_CHARACTER = "Character";
    public const string SORTING_LAYER_FOREGROUND = "Foreground";
    public const string SORTING_LAYER_CAMERA = "Camera";
    public readonly string[] SORTING_LAYERS = {
        SORTING_LAYER_FLOOR,
        SORTING_LAYER_FLOOR_OBJECT,
        SORTING_LAYER_CHARACTER,
        SORTING_LAYER_FOREGROUND,
        SORTING_LAYER_CAMERA
    };
}

public enum WeaponAttackType {
    SemiAutomatic,
    BurstFire,
    FullyAutomatic,
}

public enum SceneBuildIndex {
    LoadingScreen,
    MainMenu,
    TrophyRoom,
    World,
}

[Serializable]
public enum IngredientId {
    // FIRE INGREDIENTS
    FireIngredient1,
    FireIngredient2,
    FireIngredient3,
    
    // WATER INGREDIENTS
    WaterIngredient1,
    WaterIngredient2,
    WaterIngredient3,
    
    // ELECTRIC INGREDIENTS
    ElectricIngredient1,
    ElectricIngredient2,
    ElectricIngredient3,

    // ICE INGREDIENTS
    IceIngredient1,
    IceIngredient2,
    IceIngredient3
}

[Serializable]
public enum WeaponId {
    // FIRE WEAPONS
    WaxCrossbow,
    WaxSwordAndHammer,

    // WATER WEAPONS

    // ELECTRIC WEAPONS

    // ICE WEAPONS

    // FIRE + WATER WEAPONS

    // FIRE + ELECTRIC WEAPONS

    // FIRE + ICE WEAPONS

    // WATER + ELECTRIC WEAPONS

    // WATER + ICE WEAPONS

    // ELECTRIC + ICE WEAPONS
}

[Serializable]
public enum WeaponCraftingComponentId {
    // FIRE INGREDIENTS
    FireIngredientComponent1,
    FireIngredientComponent2,
    FireIngredientComponent3,
    
    // WATER INGREDIENTS
    WaterIngredientComponent1,
    WaterIngredientComponent2,
    WaterIngredientComponent3,
    
    // ELECTRIC INGREDIENTS
    ElectricIngredientComponent1,
    ElectricIngredientComponent2,
    ElectricIngredientComponent3,

    // ICE INGREDIENTS
    IceIngredientComponent1,
    IceIngredientComponent2,
    IceIngredientComponent3,

    // FIRE WEAPONS
    WaxCrossbowComponent,
    WaxSwordAndHammerComponent,

    // WATER WEAPONS

    // ELECTRIC WEAPONS

    // ICE WEAPONS

    // FIRE + WATER WEAPONS

    // FIRE + ELECTRIC WEAPONS

    // FIRE + ICE WEAPONS

    // WATER + ELECTRIC WEAPONS

    // WATER + ICE WEAPONS

    // ELECTRIC + ICE WEAPONS
};