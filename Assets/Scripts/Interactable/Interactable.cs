using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string OnInteractableSeenMessage { get; }
    
    public abstract void Interact();
}
