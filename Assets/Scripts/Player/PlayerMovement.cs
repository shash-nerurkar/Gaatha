using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

        lookDirection = new Vector2(0, -1);
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // FROM InputManager.cs
    public void OnMove(Vector2 moveValue) {
        velocity = moveValue.normalized;

        this.IsSprinting = velocity == moveValue && velocity != Vector2.zero;

        MoveSpeed = IsSprinting ? SprintSpeed : WalkSpeed;
    }

    // FROM InputManager.cs
    public void OnLook(Vector2 lookDirection) {
        if(lookDirection != Vector2.zero) this.lookDirection = lookDirection;
    }

    private void SetLookDir() {
        float lookAngle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;

        transform.localScale = new Vector3(Mathf.Sign(lookAngle), 1, 1);

        lookAngle = Mathf.Abs(lookAngle);
        if(lookAngle < 22.5f) {
            animator.SetInteger("lookState", 4);
        }
        else if(lookAngle < 67.5f) {
            animator.SetInteger("lookState", 3);
        }
        else if(lookAngle < 112.5f) {
            animator.SetInteger("lookState", 2);
        }
        else if(lookAngle < 157.5f) {
            animator.SetInteger("lookState", 1);
        }
        else {
            animator.SetInteger("lookState", 0);
        }
    }

    void Update() {
        SetLookDir();   

        print("velocity: " + velocity + " MoveSpeed: " + MoveSpeed);
        rb.velocity = velocity * MoveSpeed;
    }
}

