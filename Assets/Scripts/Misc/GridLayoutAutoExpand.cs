 using UnityEngine;
 using UnityEngine.UI;
 
 public class GridLayoutAutoExpand : MonoBehaviour
 {
    // COMPONENTS
    private GridLayoutGroup gridLayout;
    // [SerializeField] private Button button;

    // VARIABLES
    [SerializeField] private int amountPerColumn;
     
    void Awake() {
        gridLayout = GetComponent<GridLayoutGroup>();

        // button.onClick.AddListener(delegate { 
        //     RecalculateGridLayout();
        // });
    }
         
    public void RecalculateGridLayout () {
        int count = gridLayout.transform.childCount;

        Vector2 scale = GetComponent<RectTransform>().sizeDelta;

        Vector3 cellSize = gridLayout.cellSize;
        Vector3 spacing = gridLayout.spacing;

        int amountPerRow = Mathf.CeilToInt( (float)count / (float)amountPerColumn );

        float childWidth = ( scale.x - spacing.x * ( amountPerColumn - 1 ) ) / amountPerColumn;
        float childHeight = ( scale.y - spacing.y * ( amountPerRow - 1 ) ) / amountPerRow;

        cellSize.x = childWidth;
        cellSize.y = childHeight;

        gridLayout.cellSize = cellSize;
    }
}