using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Animator animator;

    // MOVEMENT
    private Vector2 velocity;
    private float WalkSpeed;
    private float SprintSpeed;
    public float MoveSpeed { get; private set; }
    // SPRINT
    public bool IsSprinting { get; private set; }
    // LOOK
    private Vector2 lookDirection;

    void Awake() {
        WalkSpeed = 3.0f;
        SprintSpeed = 7.0f;
        MoveSpeed = WalkSpeed;

        IsSprinting = false;
    }

    void Start() {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // FROM InputManager.cs
    public void OnMove(Vector2 moveValue) {
        velocity = moveValue.normalized;

        this.IsSprinting = velocity == moveValue && velocity != Vector2.zero;

        MoveSpeed = IsSprinting ? SprintSpeed : WalkSpeed;
    }

    void Update() {
        rb.velocity = velocity * MoveSpeed;
    }
}

