 using UnityEngine;
 using UnityEngine.UI;
 
 public class GridLayoutAutoExpand : MonoBehaviour
 {
    // COMPONENTS
    private RectTransform rectTransform;
    private GridLayoutGroup gridLayout;

    // VARIABLES
    [SerializeField] private int amountPerColumn;
     
    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        gridLayout = GetComponent<GridLayoutGroup>();
    }
         
    public void RecalculateGridLayout () {
        int childCount = gridLayout.transform.childCount;
        Vector3 cellSize = gridLayout.cellSize;
        Vector3 spacing = gridLayout.spacing;

        Vector2 scale = rectTransform.sizeDelta;

        int amountPerRow = Mathf.CeilToInt( (float)childCount / (float)amountPerColumn );

        cellSize.x = ( scale.x - spacing.x * ( amountPerColumn - 1 ) ) / amountPerColumn;
        cellSize.y = ( scale.y - spacing.y * ( amountPerRow - 1 ) ) / amountPerRow;

        gridLayout.cellSize = cellSize;
    }
}