using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootState : EnemyBaseState
{
    Vector3[] spawnOffsets = {new Vector3(0, -0.63f), new Vector3(0.55f, 0), new Vector3(0, 0.63f), new Vector3(-0.55f, 0)};
    int index = 0;
    public override void Enter(){
        enemy.isShooting = true;
        index = 0;
        enemy.StartCoroutine(BurstFire());
    }
    public override void Perform(){
            
    }
    public override void Exit(){
        enemy.isShooting = false;
        enemy.StopCoroutine(BurstFire());
    }
    IEnumerator BurstFire(){
        for(int i=0; i<3; i++){
            index = enemy.GetAnimatorLookState();
            if(index==1 && enemy.transform.localScale.x<0)
                index = 3;
            Vector3 spawnPoint = spawnOffsets[index];
            Vector2 dir = (enemy.player.transform.position  - enemy.transform.position).normalized; // can add spawn offset for more accuracy
            float yAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
            enemy.BulletSpawnner(spawnPoint, yAngle);
            yield return new WaitForSeconds(0.4f); 
        }
        stateMachine.ChangeState(stateMachine.followState);
    }
}

