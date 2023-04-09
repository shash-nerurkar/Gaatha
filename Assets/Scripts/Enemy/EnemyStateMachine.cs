using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyBaseState activeState;
    public EnemyPatrolState patrolState;
    public EnemyIdleState idleState;
    public EnemyFollowState followState;

    public void Initialize() {
        patrolState = new EnemyPatrolState();
        idleState = new EnemyIdleState();
        followState = new EnemyFollowState();
        ChangeState(newState: patrolState);
    }

    void FixedUpdate() {
        activeState?.Perform();
    }

    public void ChangeState(EnemyBaseState newState) {
        activeState?.Exit();
        
        activeState = newState;

        if(activeState != null) {
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}
