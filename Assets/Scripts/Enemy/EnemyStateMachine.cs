using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyBaseState activeState;
    public EnemyPatrolState patrolState;
    // public EnemyIdleState idleState;

    public void Initialize() {
        patrolState = new EnemyPatrolState();
        ChangeState(newState: patrolState);
    }

    void Update() {
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
