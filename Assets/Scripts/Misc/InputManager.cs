using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions fpsPlayerInputActions;
    public PlayerInputActions.OnFootActions OnFootActions { get; private set; }
    private Player player;

    private Timer weaponSwitchDebounceTimer;

    void Awake()
    {
        // FIND OBJECTS
        player = FindObjectOfType<Player>();
        weaponSwitchDebounceTimer = gameObject.AddComponent<Timer>();
        fpsPlayerInputActions = new PlayerInputActions();
        OnFootActions = fpsPlayerInputActions.OnFoot;

        // PLAYER WEAPON ATTACK
        OnFootActions.WeaponAttack.started += ctx => player.Fight.OnWeaponAttack(IsAttacking: true);
        OnFootActions.WeaponAttack.canceled += ctx => player.Fight.OnWeaponAttack(IsAttacking: false);

        // PLAYER WEAPON RELOAD
        OnFootActions.WeaponReload.started += ctx => player.Fight.OnWeaponReload();

        // PLAYER WEAPON SWITCH
        weaponSwitchDebounceTimer.StartTimer(maxTime: 0.7f, onTimerFinish: OnWeaponSwitchingDebounceTimerFinished);
        OnFootActions.WeaponSwitch.started += ctx => {
            player.Fight.OnWeaponSwitch(switchDirection: Mathf.Sign(OnFootActions.WeaponSwitch.ReadValue<float>()));
            OnFootActions.WeaponSwitch.Disable();
            weaponSwitchDebounceTimer.StartTimer(maxTime: 0.7f, onTimerFinish: OnWeaponSwitchingDebounceTimerFinished);
        };
    }

    void FixedUpdate() {
        player.Movement.OnMove(moveValue: OnFootActions.Movement.ReadValue<Vector2>());
        player.Movement.OnLook(lookDirection: OnFootActions.Look.ReadValue<Vector2>().normalized);
    }

    public void OnWeaponSwitchingDebounceTimerFinished() {
        OnFootActions.WeaponSwitch.Enable();
    }

    void OnEnable() {
        OnFootActions.Enable();
        OnFootActions.WeaponSwitch.Disable();
    }

    void OnDisable() {
        OnFootActions.Disable();
    }
}
