using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Animator animator;

    // MOVEMENT
    private Vector2 velocity;
    private float[] SpeedRange = { 3.0f, 6.0f };
    public bool IsMoving { get; private set; }
    Tweener startMoveTweener = null;
    Tweener endMoveTweener = null;
    public float MoveSpeed { get; private set; }
    // LOOK
    private Vector2 lookDirection;

    void Awake() {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        InputManager.OnPlayerMoveAction += OnMove;
    }

    // FROM InputManager.cs
    public void OnMove(Vector2 moveValue) {
        // IF MOVEMENT STATUS HAS CHANGED  
        if( moveValue.Equals( Vector2.zero ) ) {
            IsMoving = false;

            if( endMoveTweener == null ) {
                startMoveTweener.Kill();
                startMoveTweener = null;
                endMoveTweener = DOVirtual.Float( MoveSpeed, 0, duration: 0.25f, v => MoveSpeed = v );
            }
        }
        else {
            IsMoving = true;

            if( startMoveTweener == null ) {
                endMoveTweener.Kill();
                endMoveTweener = null;
                startMoveTweener = DOVirtual.Float( SpeedRange[0], SpeedRange[1], duration: 0.25f, v => MoveSpeed = v );
            }

            velocity = moveValue.normalized;
        }
    }

    void Update() {
        rb.velocity = velocity * MoveSpeed;
    }

    void OnDestroy() {
        InputManager.OnPlayerMoveAction -= OnMove;
    }
}

