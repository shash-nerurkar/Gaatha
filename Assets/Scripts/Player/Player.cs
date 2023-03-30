using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerFight Fight;
    [HideInInspector] public PlayerInteract Interact;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public PlayerUI UI;
    
    void Awake() {
        Fight = GetComponent<PlayerFight>();
        Interact = GetComponent<PlayerInteract>();
        Movement = GetComponent<PlayerMovement>();
        UI = GetComponent<PlayerUI>();
    }
}
