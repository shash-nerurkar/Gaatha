using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;

public class WeaponShootInput : MonoBehaviour, HUDElement
{
    [SerializeField] private Vector3 showPosition;
    public Vector3 ShowPosition {
        get { return showPosition; }
    }
    
    [SerializeField] private Vector3 hidePosition;
    public Vector3 HidePosition {
        get { return hidePosition; }
    }
    
    private RectTransform rectTransform;
    public RectTransform RectTransform {
        get { return rectTransform; }
    }
    
    private HUD hUD;
    public HUD HUD { 
        get { return hUD; }
        private set { hUD = value; }
    }


    public void Init( HUD HUD ) => this.HUD =  HUD;
    public void ToggleInteractable( bool isInteractable ) {}

    // COMPONENTS
    private OnScreenStick shootJoystick;
    private Button shootJoystickButton;
    private CustomButtonEventHandler buttonHandler;
    // private PlayerInputActions fpsPlayerInputActions;
    // private PlayerInputActions.OnFootActions OnFootActions;

    // VARIABLES
    [HideInInspector] public bool IsShootPressed = false;
    private float joystickMovementRange = 0;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        shootJoystickButton = GetComponentInChildren<Button>();
        buttonHandler = GetComponentInChildren<CustomButtonEventHandler>();
        shootJoystick = GetComponentInChildren<OnScreenStick>();

        joystickMovementRange = shootJoystick.movementRange;

        buttonHandler.onPointerDown += onShootJoystickButtonPressed;
        buttonHandler.onPointerUp += onShootJoystickButtonReleased;
        
        // fpsPlayerInputActions = new PlayerInputActions();
        // OnFootActions = fpsPlayerInputActions.OnFoot;
        // OnFootActions.Enable();
    }

    void onShootJoystickButtonPressed() => IsShootPressed = true;
    void onShootJoystickButtonReleased() => IsShootPressed = false;

    void FixedUpdate() {
        // if( OnFootActions.Look.ReadValue<Vector2>().magnitude < 0.1 ) {
        //     shoo
        // }
        // shootJoystick = 0;

        // shootJoystick = 50;

        // shootJoystick = 100;
    }
}
