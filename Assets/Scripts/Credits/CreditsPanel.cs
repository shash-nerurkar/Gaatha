using UnityEngine;
using UnityEngine.UI;

public class CreditsPanel : MonoBehaviour
{
    // CHILDREN
    [SerializeField] private Button backButton;
    
    // VARIABLES
    private bool isPanelActive = false;
    
    void Awake() {
        // HIDE PANEL ON BACK BUTTON CLICK
        backButton.onClick.AddListener(delegate { 
            TogglePanelActive( toggleStatus: false );
        });
    
    }

    void TogglePanelActive( bool toggleStatus ) {
        isPanelActive = toggleStatus;

        if( isPanelActive ) {}
        else {}

        for( int index = 0; index < transform.childCount; index++ )
            transform.GetChild(index).gameObject.SetActive( toggleStatus );
    }

}
