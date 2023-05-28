using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D cd;

    private float moveSpeed;

    void Awake() {
        moveSpeed = 30;

        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    void FixedUpdate() {
        rb.MovePosition(transform.position + transform.up * moveSpeed * Time.deltaTime);
    }

    // UPON ENTERING COLLISION
    async void OnTriggerEnter2D( Collider2D collided ) {
        cd.enabled = false;
        
        // collided.GetComponent<Player>()?.Fight.TakeDamage(
        //     damage: damage,
        //     hitterPosition: transform.position,
        //     hitKnockback: knockback
        // );

        moveSpeed = 0;
        
        await System.Threading.Tasks.Task.Delay(1000);
        
        Destroy(gameObject);
    }
}
