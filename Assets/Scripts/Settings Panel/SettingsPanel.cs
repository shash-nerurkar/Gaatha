using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    // CHILDREN
    [SerializeField] private Image backgroundBlurPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private Button panelBackButton;

    [Header("Sound")]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    
    [Header("Misc")]
    [SerializeField] private Toggle colorBlindCheckBox;
    [SerializeField] private Toggle difficultyCheckBox;
    
    // VARIABLES
    [SerializeField] private bool isPoppable;
    private bool isPanelActive = false;
    
    void Awake() {
        MainMenuButtonsPanel.ShowSettingsPanelAction += ShowPanel;

        PausePanel.ShowSettingsPanelAction += ShowPanel;

        if( isPoppable ) {
            Destroy( backgroundBlurPanel.gameObject );
            Destroy( backButton.gameObject );

            // HIDE PANEL ON PANEL BACK BUTTON CLICK
            panelBackButton?.onClick.AddListener(delegate { 
                TogglePanelActive( toggleStatus: false );
            });
        }
        else {
            Destroy( panelBackButton.gameObject );

            // HIDE PANEL ON BACK BUTTON CLICK
            backButton?.onClick.AddListener(delegate { 
                TogglePanelActive( toggleStatus: false );
            });
        }
    }

    void ShowPanel() => TogglePanelActive( toggleStatus: true );

    void TogglePanelActive( bool toggleStatus ) {
        isPanelActive = toggleStatus;

        if( isPanelActive ) {}
        else {}

        for( int index = 0; index < transform.childCount; index++ )
            transform.GetChild(index).gameObject.SetActive( toggleStatus );
    }

    void OnDestroy() {
        MainMenuButtonsPanel.ShowSettingsPanelAction -= ShowPanel;

        PausePanel.ShowSettingsPanelAction -= ShowPanel;
    }
}
