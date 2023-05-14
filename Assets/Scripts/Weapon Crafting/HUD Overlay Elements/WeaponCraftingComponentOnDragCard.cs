using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponCraftingComponentOnDragCard : MonoBehaviour
{
    // COMPONENTS
    private RectTransform rectTransform;
    private Image panelImage;
    private Collider2D cd;
    [SerializeField] private WeaponCraftingComponentCard componentCard;
    public WeaponCraftingComponentCard ComponentCard { 
        get { return componentCard; }
     }
    [SerializeField] private Image componentImage;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        panelImage = GetComponent<Image>();
        cd = GetComponent<Collider2D>();
    }

    public void ToggleCollider( bool active ) => cd.enabled = active;

    public void SetComponentData( WeaponCraftingComponentData componentData ) {
        // SET COMPONENT data
        componentImage.sprite = componentData.PanelSprite;
    }

    public void SetClickPosition( Vector3 anchoredPosition ) {
        rectTransform.anchoredPosition = anchoredPosition;
    }

    public void ResetPosition( float duration ) {
        rectTransform.DOAnchorPos( endValue: Vector2.zero, duration: duration );
    }
}
