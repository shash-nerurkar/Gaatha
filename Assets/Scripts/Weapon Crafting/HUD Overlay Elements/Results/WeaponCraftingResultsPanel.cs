using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeaponCraftingResultsPanel : MonoBehaviour
{
    [SerializeField] private GameObject weaponCraftingComponentCard;

    
    // COMPONENTS
    [SerializeField] private Button craftButton;
    [SerializeField] private TextMeshProUGUI currentResultAdhesiveRequirementLabel;
    [SerializeField] private GameObject resultsListObject;

    // VARIABLES
    private int currentResultAdhesiveRequirement = -1;
    private List<WeaponData> results;
    private WeaponCraftingComponentCard currentResultComponentCard;

    // ACTIONS
    public static event Action<WeaponData, int> CraftWeaponAction;

    void Awake() {
        results = new List<WeaponData>();
        
        // CRAFT WEAPON ON CRAFT PANEL CLICK
        craftButton.onClick.AddListener(delegate { 
            if( currentResultComponentCard != null && currentResultComponentCard.WeaponData != null )
                CraftWeaponAction?.Invoke( currentResultComponentCard.WeaponData, currentResultAdhesiveRequirement );
        });
    }

    // WHEN MORE THAN 1 COMPONENT IS SET AS CURRENT
    public void SetResults( List<WeaponData> weaponDatas ) {
        foreach( WeaponData weaponData in weaponDatas ) {
            // INSTANTIATE AND ADD A COMPONENT CARD
            GameObject componentCardInstance =  Instantiate(
                original: weaponCraftingComponentCard, 
                position: Vector3.zero, 
                rotation: Quaternion.identity,
                parent: resultsListObject.transform
            );
            WeaponCraftingComponentCard componentCardInstanceCard = componentCardInstance.GetComponent<WeaponCraftingComponentCard>();

            // SET PARENT DATA
            componentCardInstanceCard.SetParent( parentTransform: resultsListObject.transform );

            // SET DATA
            componentCardInstanceCard.SetData(
                componentData: weaponData.componentData,
                IsCardDraggable: false,
                IsLayoutEnabled: true,
                IsCardCollidable: true
            );

            // SET WEAPON DATA
            componentCardInstanceCard.SetWeaponData( weaponData: weaponData );

            results.Add( weaponData );

            if( weaponDatas.IndexOf( weaponData ) == 0 )
                SetComponentCardAsResult( componentCard: componentCardInstance.GetComponent<WeaponCraftingComponentCard>() );
        }
    }

    // WHEN WEAPONS CRAFTING PANEL IS CLOSED
    public void ClearResults() {
        // CLEAR UP AND DISABLE CRAFT BUTTON
        ToggleCraftButton( currentResultAdhesiveRequirement: -1 );

        // REMOVE ALL RESULT CARDS
        foreach( Transform childTransform in resultsListObject.transform ) {
            WeaponCraftingComponentCard childComponentCard = childTransform.GetComponent<WeaponCraftingComponentCard>();
            if( childComponentCard != null && childComponentCard.WeaponData != null )
                Destroy( childTransform.gameObject );
        }
    }

    // FROM WeaponCraftingResultDetector
    // WHEN SOME COLLIDABLE OBJECT IS OVERLAPPING
    public void OnTriggerStay2D( Collider2D collided ) {
        WeaponCraftingComponentCard resultComponentCard = collided.GetComponent<WeaponCraftingComponentCard>();

        if( resultComponentCard != null && resultComponentCard.WeaponData != null && currentResultComponentCard == null )
            SetComponentCardAsResult( componentCard: resultComponentCard );
    }

    void SetComponentCardAsResult( WeaponCraftingComponentCard componentCard ) {
        currentResultComponentCard = componentCard;

        // ENABLE CRAFT BUTTON AND SET COST
        ToggleCraftButton( currentResultAdhesiveRequirement: WeaponCraftingResultFinder.GetCraftingCost( currentResultComponentCard.WeaponData ) ); 
    }

    // FROM WeaponCraftingResultDetector
    // WHEN SOME OVERLAPPING COLLIDABLE OBJECTS STOPS OVERLAPPING
    public void OnTriggerExit2D(Collider2D collided) {
        WeaponCraftingComponentCard resultComponentCard = collided.GetComponent<WeaponCraftingComponentCard>();

        if( resultComponentCard != null && resultComponentCard == currentResultComponentCard ) {
            currentResultComponentCard = null;
            
            // CLEAR UP AND DISABLE CRAFT BUTTON
            ToggleCraftButton( currentResultAdhesiveRequirement: -1 );
        }
    }

    void ToggleCraftButton( int currentResultAdhesiveRequirement ) {
        this.currentResultAdhesiveRequirement = currentResultAdhesiveRequirement;
        if( this.currentResultAdhesiveRequirement == -1 ) {
            craftButton.interactable = false;

            currentResultAdhesiveRequirementLabel.text = "";
        }
        else {
            craftButton.interactable = true;

            currentResultAdhesiveRequirementLabel.text = this.currentResultAdhesiveRequirement.ToString();
        }
    }
}
