using System.Collections.Generic;
using UnityEngine;

public static class WeaponCraftingResultFinder
{
    static public WorldManager.GetAllWeaponsInGameDelegate GetAllGameWeaponsAction;

    public static List<WeaponData> GetCraftingResult( List<WeaponCraftingComponentData> componentDatas ) {
        List<IWeapon> allGameWeapons = null;
        GetAllGameWeaponsAction?.Invoke( out allGameWeapons );
        if( allGameWeapons == null ) return new List<WeaponData>();

        // FETCH AND CREATE A LIST OF ALL WEAPONS' DATAS
        List<WeaponData> allGameWeaponDatas = new List<WeaponData>();
        foreach( IWeapon weapon in allGameWeapons ) {
            allGameWeaponDatas.Add( weapon.WeaponData );
        }

        List<WeaponData> craftingResultWeaponDatas = new List<WeaponData>();

        // COMBINE THE COMPONENT INGREDIENT LISTS INTO ONE
        List<IngredientData> combinedIngredientList = new List<IngredientData>();
        foreach( WeaponCraftingComponentData componentData in componentDatas ) {
            foreach( IngredientData ingredientData in componentData.Ingredients )
                combinedIngredientList.Add( ingredientData );
        }

        // COMPARE THE LIST TO THE INGREDIENT LISTS OF ALL THE WEAPONS IN THE GAME, AND FIND THE RESEMBLANCE FACTOR EACH WEAPON
        List<int> resemblanceFactors = new List<int>();
        int maxResemblanceFactor = -1;
        foreach( WeaponData weaponData in allGameWeaponDatas ) {
            int resemblanceFactor = 0;
            List<IngredientData> dummyCombinedIngredientList = new List<IngredientData>( combinedIngredientList );
            
            foreach( IngredientData ingredientData in weaponData.componentData.Ingredients ) {
                if( dummyCombinedIngredientList.Contains( ingredientData ) ) {
                    dummyCombinedIngredientList.Remove( ingredientData );
                    ++resemblanceFactor;
                }
            }

            if( maxResemblanceFactor < resemblanceFactor )
                maxResemblanceFactor = resemblanceFactor;
            
            resemblanceFactors.Add( resemblanceFactor );
        }

        // MAKE A LIST OF ALL WEAPONS THAT HAVE THE HIGHEST RESEMBLANCE FACTOR
        for( int index = 0; index < resemblanceFactors.Count; index++ ) {
            if( resemblanceFactors[index] >= maxResemblanceFactor )
                craftingResultWeaponDatas.Add( allGameWeaponDatas[index] );
        }

        // RETURN THE LIST
        return craftingResultWeaponDatas;
    }

    public static int GetCraftingCost( WeaponData weaponData ) {
        return 1;
    }
}
