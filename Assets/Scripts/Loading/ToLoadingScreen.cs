using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLoadingScreen : MonoBehaviour
{
    public void StartTransitionToLoading( SceneBuildIndex SceneBuildIndexToLoad ) {
        ScreenTransition.OnTransitionDone += GoToLoadingScreen;

        LoadingScreenManager.SceneBuildIndexToLoad = SceneBuildIndexToLoad;

        ScreenTransition.instance.FadeIn();
    }

    void GoToLoadingScreen() {
        ScreenTransition.OnTransitionDone -= GoToLoadingScreen;
        
        SceneManager.LoadScene(sceneBuildIndex: (int)SceneBuildIndex.LoadingScreen);
    }
}
