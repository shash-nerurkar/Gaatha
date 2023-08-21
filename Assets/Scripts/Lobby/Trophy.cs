using UnityEngine;
using UnityEngine.UI;
using System;

public class Trophy : MonoBehaviour
{
    // COMPONENTS
    [SerializeField] private Button popupButton;
    public GameObject TrophyObject;
    public GameObject RewardObject;
    private Collider2D cd;
    private TrophyInteract trophyInteract;

    // VARIABLES
    public TrophyData TrophyData;

    // ACTIONS
    public static event Action<Trophy> TrophyInteractAction;

    void Awake() {
        cd = GetComponent<Collider2D>();
        trophyInteract = GetComponent<TrophyInteract>();
    }

    void Start() {
        popupButton.onClick.AddListener(delegate {
            TrophyInteractAction?.Invoke( GetComponent<Trophy>() );
        });
    }

    // UPON ENTERING COLLISION
    void OnTriggerEnter2D( Collider2D collided ) => popupButton?.gameObject.SetActive( true );

    // UPON ENTERING COLLISION
    void OnTriggerExit2D( Collider2D collided ) => popupButton?.gameObject.SetActive( false );
}
