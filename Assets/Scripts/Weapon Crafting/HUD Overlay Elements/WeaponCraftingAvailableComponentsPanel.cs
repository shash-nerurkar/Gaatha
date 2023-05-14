using UnityEngine;

public class WeaponCraftingAvailableComponentsPanel : MonoBehaviour
{
    private GridLayoutAutoExpand gridLayoutAutoExpand;
    [SerializeField] private GameObject elementsContainer;
    public GameObject ElementsContainer { 
        get {
            return elementsContainer;
        }
    }

    void Awake() {
        gridLayoutAutoExpand = GetComponent<GridLayoutAutoExpand>();
    }

    public void AdjustGrid() {
        gridLayoutAutoExpand.RecalculateGridLayout();
    }

    public void ClearGrid() {
        for( int i = 0; i < transform.childCount; i++ ) {
            Destroy( transform.GetChild(i).gameObject );
        }
    }
}
