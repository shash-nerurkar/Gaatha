using UnityEngine;
using System.Threading.Tasks;

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

        WeaponCraftingComponentCard.OnRemoveComponentAction += OnRemoveElement;
    }

    async public void AdjustGrid() {
        gridLayoutAutoExpand.RecalculateGridLayout();

        await Task.Delay( millisecondsDelay: Mathf.CeilToInt(Time.deltaTime*1000) );

        // SET CARD DATA FOR ALL COMPONENT CARDS, AFTER GRID HAS BEEN ADJUSTED
        foreach( WeaponCraftingComponentCard componentCard in elementsContainer.GetComponentsInChildren<WeaponCraftingComponentCard>() )
            componentCard.SetCardSnapData();
    }

    public void ClearGrid() {
        for( int i = 0; i < transform.childCount; i++ )
            Destroy( transform.GetChild(i).gameObject );
    }

    void OnRemoveElement( WeaponCraftingComponentCard componentCard ) {
        AdjustGrid();
    }

    void OnDestroy() {
        WeaponCraftingComponentCard.OnRemoveComponentAction -= OnRemoveElement;
    }
}
