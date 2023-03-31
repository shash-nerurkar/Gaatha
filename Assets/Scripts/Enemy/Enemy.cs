using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyStateMachine stateMachine;

    private string currentState;

    void Start() {
        stateMachine = GetComponent<EnemyStateMachine>();
        
        stateMachine.Initialize();
    }
}
