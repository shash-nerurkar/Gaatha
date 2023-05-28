using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class WeaponSwitchInput : MonoBehaviour, HUDElement
{
    [SerializeField] private Vector3 showPosition;
    public Vector3 ShowPosition {
        get { return showPosition; }
    }
    
    [SerializeField] private Vector3 hidePosition;
    public Vector3 HidePosition {
        get { return hidePosition; }
    }

    private HUD hUD;
    public HUD HUD { 
        get { return hUD; }
        private set { hUD = value; }
    }

    public void Init( HUD HUD ) => this.HUD =  HUD;

    public void ToggleInteractable( bool isInteractable ) {
        button.interactable = isInteractable;

        foreach( Image weaponImage in weaponImages )
            weaponImage.enabled = button.interactable;
        
        backgroundImage.color = button.interactable 
            ? Color.white
            : new Color( 0.78f, 0.78f, 0.78f, 0.5f );
    }

    // COMPONENTS
    private Button button;
    private Animator animator;
    private Image backgroundImage;
    private List<Image> weaponImages;
    private RectTransform rectTransform;
    public RectTransform RectTransform {
        get { return rectTransform; }
    }

    // ACTIONS
    public static event Action WeaponSwitchAction;

    void Awake() {
        PlayerFight.SwitchInputToggleAction += ToggleInteractable;

        PlayerFight.WeaponSpriteUpdateAction += SetWeaponSprite;

        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
        
        backgroundImage = transform.GetChild(0).GetComponent<Image>();

        weaponImages = new List<Image>();
        weaponImages.Add( transform.GetChild(1).GetComponent<Image>() );
        weaponImages.Add( transform.GetChild(2).GetComponent<Image>() );
    }

    void Start() {
        button.onClick.AddListener(delegate {
            animator.SetInteger( "weaponNo", animator.GetInteger("weaponNo") == 1 ? 2 : 1 );

            WeaponSwitchAction?.Invoke();
        });
    }

    // SET WEAPON IMAGE WIDGET SPRITE
    public void SetWeaponSprite(Sprite sprite, int index) {
        weaponImages[index].sprite = sprite;
    }

    void OnDestroy() {
        PlayerFight.SwitchInputToggleAction -= ToggleInteractable;

        PlayerFight.WeaponSpriteUpdateAction -= SetWeaponSprite;
    }
}
