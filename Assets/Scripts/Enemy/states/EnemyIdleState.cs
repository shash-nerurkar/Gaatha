using UnityEngine;
using System.Threading.Tasks;
public class EnemyIdleState : EnemyBaseState
{
    bool taskSuccess = false;   
    public override void Enter(){
        enemy.isIdle = true;
        taskSuccess = false;
    }

    public override void Perform(){
        DelayAndLook();
        if(taskSuccess){
            enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * -1, 1, 1);
            stateMachine.ChangeState(stateMachine.patrolState);
        }
        
    }

    public override void Exit(){
        enemy.isIdle = false;
        taskSuccess = false;
    }

    async void DelayAndLook(){
        // 3 second delay
        for(int i = 0; i < 10; i++){
            if (enemy.SpotPlayer()){
                stateMachine.ChangeState(stateMachine.followState);
            }
            await Task.Delay(100);
        }
        taskSuccess = true;
    }
}
