using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement Movement { get; private set; }
    public PlayerLook Look { get; private set; }
    public PlayerFight Fight { get; private set; }
    public PlayerInteract Interact { get; private set; }
    public PlayerUI UI { get; private set; }
    
    void Awake() {
        Fight = GetComponent<PlayerFight>();
        Interact = GetComponentInChildren<PlayerInteract>();
        Movement = GetComponent<PlayerMovement>();
        UI = GetComponent<PlayerUI>();
        Look = GetComponent<PlayerLook>();
    }
}
