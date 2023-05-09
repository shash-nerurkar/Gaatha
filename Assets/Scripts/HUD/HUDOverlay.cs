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

    // OVERLAY ELEMENTS
    public WeaponCraftingPanel WeaponCraftingPanel { get; private set; }

    // VARIABLES
    PointerEventData m_PointerEventData;
    List<RaycastResult> objectsClickedOn;

    // DELEGATES
    public delegate void GetUIClickStatus(GameObject objectID, out bool status);

    void Awake() {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        canvasRect = GetComponent<RectTransform>();

        WeaponCraftingPanel = GetComponentInChildren<WeaponCraftingPanel>();
        m_EventSystem = FindObjectOfType<EventSystem>();

        m_PointerEventData = new PointerEventData(m_EventSystem);

        DragDroppableUI.IsUIClickedAction += IsUIClickedOn;
    }

    void Update() {
            m_PointerEventData.position = Input.mousePosition;

            objectsClickedOn = new List<RaycastResult>();
            m_Raycaster.Raycast( m_PointerEventData, objectsClickedOn );
    }

    void IsUIClickedOn( GameObject incGameObject, out bool isUIClickedOn ) {
        foreach( RaycastResult obj in objectsClickedOn ) {
            if( obj.gameObject == incGameObject ) {
                isUIClickedOn = true;
                return;
            }
        }

        isUIClickedOn = false;
    } 
}