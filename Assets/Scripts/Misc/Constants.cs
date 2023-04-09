public class Constants {
    // GAMEOBJECT TAGS
    public const string PLAYER_TAG = "Player";
    public const string HUD_TAG = "HUD";
    public const string MAIN_CAMERA_TAG = "MainCamera";
    public const string BULLET_CONTAINER_TAG = "Bullet Container";
    public const string ENEMY_CONTAINER_TAG = "Enemy Container";
    public const string INGREDIENT_CONTAINER_TAG = "Ingredient Container";
    public const string FLOOR_WEAPON_CONTAINER_TAG = "Floor Weapon Container";

    // LAYER MASKS
    public const string UI_LAYERMASK = "UI";

    // ANIMATION STATE NAMES
    public const string IDLE_ANIMATION_STATE_NAME = "idle";
    public const string ATTACK_ANIMATION_STATE_NAME = "attack";
}


public enum WeaponAttackType {
    SemiAutomatic,
    BurstFire,
    FullyAutomatic,
}