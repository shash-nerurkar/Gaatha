using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions fpsPlayerInputActions;
    public PlayerInputActions.OnFootActions OnFootActions { get; private set; }
    private Player player;

    void Awake()
    {
        // FIND OBJECT
        player = FindObjectOfType<Player>();

        fpsPlayerInputActions = new PlayerInputActions();
        OnFootActions = fpsPlayerInputActions.OnFoot;
    }

    void FixedUpdate() {
        // MOVE PLAYER
        player.Movement.OnMove(moveValue: OnFootActions.Movement.ReadValue<Vector2>());
        // SET PLAYER LOOK
        player.Look.OnLook(lookDirection: OnFootActions.Look.ReadValue<Vector2>().normalized);
        // USE PLAYER WEAPON
        player.Fight.OnWeaponAttack(IsAttacking: OnFootActions.Look.ReadValue<Vector2>().magnitude == 1);
    }

    void OnEnable() {
        OnFootActions.Enable();
        OnFootActions.WeaponSwitch.Disable();
    }

    void OnDisable() {
        OnFootActions.Disable();
    }
}
