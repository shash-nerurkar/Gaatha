using UnityEngine;

public class WeaponCraftingResultDetector : MonoBehaviour
{
    [SerializeField] private WeaponCraftingResultsPanel weaponCraftingResultsPanel;

    void OnTriggerStay2D( Collider2D collided ) => weaponCraftingResultsPanel.OnTriggerStay2D( collided: collided );

    void OnTriggerExit2D(Collider2D collided) => weaponCraftingResultsPanel.OnTriggerExit2D( collided: collided );
}
