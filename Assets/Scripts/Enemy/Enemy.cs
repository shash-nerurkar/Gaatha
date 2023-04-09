using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private EnemyStateMachine stateMachine;
    private Rigidbody2D rb;
    public bool isIdle;
    public bool isPatrolling;
    public bool isFollowing;
    public float healthPoints;
    private Vector2 distance = new Vector2();
    private Vector2 dir;
    public Player player;

    void Awake() {
        stateMachine = GetComponent<EnemyStateMachine>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
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

    public bool PlayerProximityCheck(){
        if(Vector2.Dot(player.transform.position, rb.position)> 0.3 && Vector2.Dot(player.transform.position, rb.position)< 1 && transform.localScale.x>0){
            isFollowing = true;
            return true;
        }
        else if(Vector2.Dot(player.transform.position, rb.position)< -0.3 && Vector2.Dot(player.transform.position, rb.position)> -1 && transform.localScale.x<0){
            isFollowing = true;
            return true;
        }        
        else if(Vector2.Distance(transform.position, player.transform.position) >= (20- Random.Range(3, 5))){
            isFollowing = false;
            return false;
        }
        else {
            return false;
        }
    }

}
