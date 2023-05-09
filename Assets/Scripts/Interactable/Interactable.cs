using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string OnInteractableSeenMessage;
    
    public abstract void Interact();
}
