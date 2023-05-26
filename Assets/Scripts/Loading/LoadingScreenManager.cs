using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoadingScreenManager : MonoBehaviour
{
    public static SceneBuildIndex SceneBuildIndexToLoad;

    async void Start() {
        ScreenTransition.instance.FadeOut();

        await Task.Delay(millisecondsDelay: 3000);

        StartTransitionToDestination();
    }

    public void StartTransitionToDestination() {
        ScreenTransition.OnTransitionDone += GoToDestinationScreen;

        ScreenTransition.instance.FadeIn();
    }

    void GoToDestinationScreen() {
        ScreenTransition.OnTransitionDone -= GoToDestinationScreen;
        
        SceneManager.LoadScene( sceneBuildIndex: (int)SceneBuildIndexToLoad );
    }
}
