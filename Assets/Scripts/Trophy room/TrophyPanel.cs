using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trophyName;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button hidePanelParentButton;
    [SerializeField] private Button hidePanelButton2;
    [SerializeField] private GameObject trophyObject;
    [SerializeField] private GameObject rewardObject;
    [SerializeField] private TextMeshProUGUI trophyDescription;

    void Awake() {
        // Event from Trophy.cs
        Trophy.TrophyInteractAction += SetPanelData;

        // Event from TrophyInteract.cs
        TrophyInteract.TrophyInteractAction += SetPanelData;
    }

    void Start() {
        hidePanelParentButton.onClick.AddListener(delegate {
            hidePanelParentButton.gameObject.SetActive( false );
            panel.SetActive( false );
        });

        hidePanelButton2.onClick.AddListener(delegate {
            hidePanelParentButton.gameObject.SetActive( false );
            panel.SetActive( false );
        });
    }

    void SetPanelData( Trophy trophy ) {
        hidePanelParentButton.gameObject.SetActive( true );
        panel.SetActive( true );

        trophyName.text = trophy.TrophyData.name;
        trophyDescription.text = trophy.TrophyData.description;

        trophyObject.GetComponent<Image>().sprite = trophy.TrophyObject.GetComponent<SpriteRenderer>().sprite;
        rewardObject.GetComponent<Animator>().runtimeAnimatorController = trophy.TrophyData.panelRewardAnimatorController;
    }
}
