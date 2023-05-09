using UnityEngine;

public class WeaponCraftingAvailableComponentsPanel : MonoBehaviour
{
    private GridLayoutAutoExpand gridLayoutAutoExpand;

    void Awake() {
        gridLayoutAutoExpand = GetComponent<GridLayoutAutoExpand>();
    }

    public void AdjustGrid() => gridLayoutAutoExpand.RecalculateGridLayout();

    public void ClearGrid() {
        for( int i = 0; i < transform.childCount; i++ ) {
            Destroy( transform.GetChild(i).gameObject );
        }
    }
}
