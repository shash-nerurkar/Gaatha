using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCraftingCurrentComponentPanelsPanel : MonoBehaviour
{
    // ACTIONS
    public static event Action<IWeapon> DeleteWeaponAction;

    private List<WeaponCraftingCurrentComponentPanel> currentComponentPanels;

    public bool CanCraft {
        get {
            int filledCurrentComponentPanelsCount = 0;
            
            foreach( WeaponCraftingCurrentComponentPanel currentComponentPanel in currentComponentPanels )
                if( currentComponentPanel.SelectedComponentCard != null )
                    if(++filledCurrentComponentPanelsCount > 1)
                        return true;
            
            return false;
        }
    }

    void Awake() {
        currentComponentPanels = new List<WeaponCraftingCurrentComponentPanel>();

        foreach( WeaponCraftingCurrentComponentPanel currentComponentPanel in transform.GetComponentsInChildren<WeaponCraftingCurrentComponentPanel>() )
            currentComponentPanels.Add( currentComponentPanel );
    }

    public List<WeaponCraftingComponentData> GetCurrentComponentDatas() {
        List<WeaponCraftingComponentData> componentDatas = new List<WeaponCraftingComponentData>();

        foreach( WeaponCraftingCurrentComponentPanel currentComponentPanel in transform.GetComponentsInChildren<WeaponCraftingCurrentComponentPanel>() )
            if( currentComponentPanel.SelectedComponentCard != null )
                componentDatas.Add( currentComponentPanel.SelectedComponentCard.ComponentData );
        
        return componentDatas;
    }

    public void ClearAllSelectedComponents() {
        foreach( WeaponCraftingCurrentComponentPanel currentComponentPanel in currentComponentPanels )
            currentComponentPanel.ClearSelectedComponent();
    }

    public void DeleteOriginalObjectsForSelectedComponents() {
        foreach( WeaponCraftingCurrentComponentPanel currentComponentPanel in currentComponentPanels ) {
            if( currentComponentPanel.SelectedComponentCard.Ingredient != null )
                Destroy( currentComponentPanel.SelectedComponentCard.Ingredient.gameObject );
            else
                DeleteWeaponAction?.Invoke( currentComponentPanel.SelectedComponentCard.Weapon );
        }
    }
}
