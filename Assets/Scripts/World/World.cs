using UnityEngine;

public class World : MonoBehaviour
{
    private HUD HUD;
    private Player player;

    void Awake() {
        HUD = FindObjectOfType<HUD>();
        player = FindObjectOfType<Player>();    
    }

    void Start() {
        HUD.Init( player: player );
        player.Init( HUD: HUD );
    }
}
