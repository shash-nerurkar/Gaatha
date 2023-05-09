using System.Threading.Tasks;
using System.Collections.Generic;

public class TrophyRoomManager : ToLoadingScreen
{
    private HUD HUD;
    private Player player;
    private ScreenTransition screenTransition;
    private List<Trophy> trophiesList;
    
    void Awake() {
        StartGamePortal.StartGameAction += StartTransitionToLoading;

        HUD = FindObjectOfType<HUD>();
        player = FindObjectOfType<Player>();    
        screenTransition = FindObjectOfType<ScreenTransition>();
        trophiesList = new List<Trophy>();
        foreach( Trophy trophy in FindObjectsOfType<Trophy>() )
            trophiesList.Add( trophy );
    }

    async void Start() {
        screenTransition.Show();

        await Task.Delay( millisecondsDelay: 2000 );

        screenTransition.FadeOut();

        player.Fight.Init();
    }
}
