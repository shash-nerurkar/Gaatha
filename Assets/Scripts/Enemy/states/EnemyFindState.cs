using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindState : EnemyBaseState
{
    Vector3[] path;
    Vector3 currentWaypoint;
    int targetIndex;
    float followSpeed = 3f;
    bool assignFirstPoint = true;

    public override void Enter(){
        Debug.Log("Find state");
        enemy.isFollowing = true;
        PathRequestManager.RequestPath(enemy.transform.position, enemy.player.transform.position, OnPathFound);
        assignFirstPoint = true;
    }

    public override void Perform(){
        if(path.Length>0){
            if(assignFirstPoint){
                currentWaypoint = path[path.Length-3];//path [0]; //Added new line for better following by skipping path points (Delete if errors)
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

        // If player found and is in short distance change state to shoot

        // If player found and distance is mediocre then follow player

        // If player found and distance is too large then change state to patrol

    }

    public override void Exit(){
        enemy.isFollowing = false;
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if(pathSuccessful){
            path = newPath;
        }
        targetIndex = path.Length - 4; //Added new line for better following by skipping path points. (Delete if errors)
	}

    void CheckAndRequest(){
        if(enemy.DistToPlayer() > enemy.greaterPlayerDistance){
            stateMachine.ChangeState(stateMachine.patrolState);
        }
        targetIndex = 0;
        path = new Vector3[0];
        assignFirstPoint = true;
        PathRequestManager.RequestPath(enemy.transform.position, enemy.player.transform.position, OnPathFound);
    }    
}
