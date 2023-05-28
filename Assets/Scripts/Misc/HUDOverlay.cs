using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDOverlay : MonoBehaviour
{
	// COMPONENTS
	private GraphicRaycaster m_Raycaster;
    private EventSystem m_EventSystem;
    private RectTransform canvasRect;

    // VARIABLES
    PointerEventData m_PointerEventData;
    List<RaycastResult> objectsClickedOn;

    // DELEGATES
    public delegate void GetUIClickStatus(GameObject objectID, out bool status);

    void Awake() {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        canvasRect = GetComponent<RectTransform>();

        m_EventSystem = FindObjectOfType<EventSystem>();

        m_PointerEventData = new PointerEventData(m_EventSystem);

        DragDroppableUI.IsUIClickedAction += IsComponentCardClickedOn;
    }

    void Update() {
            m_PointerEventData.position = Input.mousePosition;

            objectsClickedOn = new List<RaycastResult>();
            m_Raycaster.Raycast( m_PointerEventData, objectsClickedOn );
    }

    void IsComponentCardClickedOn( GameObject incGameObject, out bool isUIClickedOn ) {
        if( objectsClickedOn.Count == 0 ) {
            isUIClickedOn = false;
        }
        else {
            foreach( RaycastResult raycastResult in objectsClickedOn ) {
                if( raycastResult.gameObject.GetComponent<WeaponCraftingComponentCard>() != null ) {
                    isUIClickedOn = incGameObject == raycastResult.gameObject;
                    return;
                }
            }

            isUIClickedOn = false;
        }
    }

    void OnDestroy() {
        DragDroppableUI.IsUIClickedAction -= IsComponentCardClickedOn;
    }
}
