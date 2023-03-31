public class EnemyPatrolState : EnemyBaseState
{
    public override void Enter() {

    }

    public override void Perform() {
        PatrolCycle();
    }

    public override void Exit() {
        
    }

    public void PatrolCycle() {
        // IF FINAL DEST HAS BEEN REACHED
        // stateMachine.ChangeState(newState: stateMachine.idleState);
    }
}
