using System.Threading.Tasks;

public class MainMenuManager : ToLoadingScreen
{
    private MainMenuHUD MainMenuHUD;
    
    void Awake() {
        MainMenuHUD = FindObjectOfType<MainMenuHUD>();

        MainMenuButtonsPanel.StartGameAction += StartTransitionToLoading;
    }

    async void Start() {
        ScreenTransition.instance.Show();

        await Task.Delay( millisecondsDelay: 2000 );

        SoundManager.instance.Play(Constants.SAD_HOPE_MUSIC);

        ScreenTransition.instance.FadeOut();

        MainMenuHUD.AdjustHUDElements(
            showTitlePanel: true,
            showButtonsPanel: true,
            duration: 0.75f
        );
    }

    void OnDestroy() {
        MainMenuButtonsPanel.StartGameAction -= StartTransitionToLoading;
    }
}
