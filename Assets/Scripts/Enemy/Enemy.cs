using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private EnemyStateMachine stateMachine;
    private Rigidbody2D rb;
    public bool isIdle;
    public bool isPatrolling;
    public bool isFollowing;
    public bool isShooting;
    public float healthPoints;
    private Vector2 distance = new Vector2();
    private Vector2 dir;
    public Player player;
    public float lesserPlayerDistance = 3f;
    public float playerDistance = 10f;
    public float greaterPlayerDistance = 15f;
    Animator animator;
    public GameObject bullet;
    GameObject bulletContainer;


    void Awake() {
        stateMachine = GetComponent<EnemyStateMachine>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        bulletContainer = GameObject.FindGameObjectWithTag(Constants.BULLET_CONTAINER_TAG);
        isIdle = false;
        isPatrolling = false;
        isFollowing = false;
        stateMachine.Initialize();
    }
    public bool MoveEnemy(Vector3 destination, float speed) {
        distance = (Vector2)destination - rb.position;   
        dir = distance.normalized;
        rb.MovePosition(rb.position + dir * Time.fixedDeltaTime * speed);    
        if (distance.magnitude < 0.03f){    
            rb.position = destination;   
            return true;
        }
        return false;    
    }

    public bool SpotPlayer(){
        if(player!=null){
            if(Vector2.Dot(player.transform.position, rb.position)> 0.3 && Vector2.Dot(player.transform.position, rb.position)< 1 && transform.localScale.x>0){
                isFollowing = true;
                return true;
            }
            else if(Vector2.Dot(player.transform.position, rb.position)< -0.3 && Vector2.Dot(player.transform.position, rb.position)> -1 && transform.localScale.x<0){
                isFollowing = true;
                return true;
            }        
            else {
                return false;
            }
        }
        return false;
    }

    public float DistToPlayer(){
        return Vector2.Distance(transform.position, player.transform.position);
    }

// Set lookState in enemy only if enemy is not in patrol or idle state.
// Alternative: Set lookState only if player is visible or in range.
//if(stateMachine.activeState == stateMachine.patrolState);
    void Update(){
          if(stateMachine.activeState == stateMachine.followState || stateMachine.activeState == stateMachine.findState){
            Vector2 lookDirection = (player.transform.position - transform.position).normalized; 
            
            // TOPMOST POINT IS 0, BOTTOM MOST IS 180 => lookAngle RANGE IS [-180, 180]
            float lookAngle = Mathf.Atan2( lookDirection.x, lookDirection.y ) * Mathf.Rad2Deg;

            // ROTATING PLAYER
            transform.localScale = new Vector3(1, 1, 1);

            float tempLookAngle = Mathf.Abs(lookAngle);
            //Top
            if(tempLookAngle < 45.0f) {
                animator.SetInteger("lookState", 2);
            }
            //Right
            else if(tempLookAngle < 135.0f) {
                transform.localScale = new Vector3(Mathf.Sign(lookAngle), 1, 1);
                animator.SetInteger("lookState", 1);
            }
            //Bottom
            else {
                animator.SetInteger("lookState", 0);
            }
        }      
    } 
    public int GetAnimatorLookState(){
        return animator.GetInteger("lookState");
    }
    public void BulletSpawnner(Vector3 position, float yAngle){
        GameObject bulletInstance = Instantiate(bullet, transform.position + position, Quaternion.Euler(new Vector3( 0, 0, yAngle - 90 )));
        bulletInstance.transform.SetParent(bulletContainer.transform);
    }
}
