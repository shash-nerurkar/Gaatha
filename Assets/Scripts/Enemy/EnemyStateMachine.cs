using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyBaseState activeState;
    public EnemyPatrolState patrolState;
    public EnemyIdleState idleState;
    public EnemyFollowState followState;
    public EnemyFindState findState;
    public EnemyShootState shootState;
    public void Initialize() {
        patrolState = new EnemyPatrolState();
        idleState = new EnemyIdleState();
        followState = new EnemyFollowState();
        findState = new EnemyFindState();
        shootState = new EnemyShootState();
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
