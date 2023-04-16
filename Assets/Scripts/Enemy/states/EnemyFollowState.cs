using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : EnemyBaseState
{
    RaycastHit2D hitInfo;
    float followSpeed = 2f;
    public override void Enter() {
        enemy.isFollowing = true;
    }

        //As long as player is visible and in mediocre distance follow  if not go to find player state
        //If distance is too large then switch to patrol
        //If distance is short and player is visible then change state to shoot    
    public override void Perform()
    {
        hitInfo = Physics2D.Raycast(enemy.transform.position, (enemy.player.transform.position - enemy.transform.position).normalized, enemy.greaterPlayerDistance, ~LayerMask.GetMask("Enemy"));        
        if(hitInfo){
            if(hitInfo.transform.name == "Player"){
                if(enemy.DistToPlayer()<=enemy.lesserPlayerDistance){
                    stateMachine.ChangeState(stateMachine.shootState);
                }
                else{
                    enemy.MoveEnemy(enemy.player.transform.position, followSpeed);
                }
            }
        }
        else if(enemy.DistToPlayer()<=enemy.greaterPlayerDistance){
            stateMachine.ChangeState(stateMachine.findState);
        }
        else if(enemy.DistToPlayer()>enemy.greaterPlayerDistance) {
            stateMachine.ChangeState(stateMachine.patrolState);
        }

    }
    public override void Exit(){ 
        enemy.isFollowing = false;
    }
}