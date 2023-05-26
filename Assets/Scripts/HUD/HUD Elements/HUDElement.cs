using UnityEngine;

public interface HUDElement 
{
    public HUD HUD { get; }
    public RectTransform RectTransform { get; }

    public Vector3 ShowPosition { get; }
    public Vector3 HidePosition { get; }

    public void Init( HUD HUD );
    public void ToggleInteractable( bool isInteractable );
}
