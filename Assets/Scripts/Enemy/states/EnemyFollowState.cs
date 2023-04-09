using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : EnemyBaseState
{
    Vector3[] path;
    Vector3 currentWaypoint;
    int targetIndex;
    float followSpeed = 3f;
    bool assignFirstPoint = true;
    bool skip = false;

    public override void Enter(){
        enemy.isFollowing = true;
        PathRequestManager.RequestPath(enemy.transform.position, enemy.player.transform.position, OnPathFound);
        assignFirstPoint = true;
    }

    public override void Perform(){
        if(path.Length>0){
            if(assignFirstPoint){
                currentWaypoint = path [0];
                assignFirstPoint = false;
            }
            if (enemy.transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    CheckAndRequest();
                }
                if(targetIndex>1)
                    currentWaypoint = path [targetIndex];
            }
            enemy.MoveEnemy(currentWaypoint, followSpeed);
        }
    }

    public override void Exit(){
        enemy.isFollowing = false;
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if(pathSuccessful){
            path = newPath;
        }
	}

    void CheckAndRequest(){
        skip = true;
        if(!enemy.PlayerProximityCheck()){
            if(!enemy.isFollowing)
                stateMachine.ChangeState(stateMachine.patrolState);
        }
        targetIndex = 0;
        path = new Vector3[0];
        assignFirstPoint = true;
        PathRequestManager.RequestPath(enemy.transform.position, enemy.player.transform.position, OnPathFound);
    }    
}
