using UnityEngine;
using System.Collections.Generic;

public class EnemyPatrolState : EnemyBaseState
{
    Vector3 dest;
    private float patrolSpeed = 1.0f;
    private Vector2 distance;
    bool value = false;
    
    public override void Enter() {
        enemy.isPatrolling = true;
        dest = new Vector3(enemy.transform.position.x + (4*enemy.transform.localScale.x), enemy.transform.position.y, 0);
    }

    public override void Perform() {
        value = enemy.MoveEnemy(dest, patrolSpeed);
        if(enemy.PlayerProximityCheck())
            stateMachine.ChangeState(stateMachine.followState);
        if(value)
            stateMachine.ChangeState(stateMachine.idleState);
    }

    public override void Exit() {
        enemy.isPatrolling = false;
    }

    
}
