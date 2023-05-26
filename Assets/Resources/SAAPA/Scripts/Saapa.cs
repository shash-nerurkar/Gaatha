using UnityEngine;

public class Saapa : MonoBehaviour
{
    // VARIABLES;
    [SerializeField] protected Transform target;
    [SerializeField] [Range( 10.0f, 20.0f )] private float rotationSpeed = 12;
    [SerializeField] [Range( 3.0f, 10.0f )] private float moveSpeed = 5;
    private Vector2 targetDirection;
    private Vector2 targetPosition;

    State state;
    enum State {
        Idle,
        Approach,
        Encircle,
    }

    void Awake() {
        ChangeState( newState: State.Idle );
    }

    void OnStateChange() {}

    void ChangeState( State? newState = null ) {
        if( newState == null ) {
            float distanceFromTarget = (target.position - transform.position).magnitude;

            if( distanceFromTarget < 2 ) {
                newState = State.Encircle;
            }
            else {
                newState = State.Approach;
            }
        }
        else {}

        if( newState != null && newState != state ) {
            state = newState ?? State.Idle;
            OnStateChange();
        }
    }

    void Update() {
        ChangeState();
        
        switch(state) {
            case State.Idle:
                break;
            
            case State.Approach:
                // SET targetDirection TO MOUSE IF NO TARGET IS PROVIDED
                if( target != null )
                    targetDirection = target.position - transform.position;
                else
                    targetDirection = Camera.main.ScreenToWorldPoint( Input.mousePosition ) - transform.position;
                
                // ROTATE TO targetDirection
                float angle = Mathf.Atan2( targetDirection.y, targetDirection.x ) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis( angle, Vector3.forward );
                transform.rotation = Quaternion.Slerp( transform.rotation, rotation, rotationSpeed * Time.deltaTime );

                // SET targetPosition TO MOUSE IF NO TARGET IS PROVIDED
                if( target != null )
                    targetPosition = target.position;
                else
                    targetPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
                
                // MOVE TO targetPosition
                // transform.position = Vector2.MoveTowards( transform.position, targetPosition, moveSpeed * Time.deltaTime );
                transform.position = transform.position + transform.right * moveSpeed * Time.deltaTime;
                break;
            
            case State.Encircle:
                break;
        }
    }
}
