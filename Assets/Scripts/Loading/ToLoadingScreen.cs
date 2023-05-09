using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLoadingScreen : MonoBehaviour
{
    public void StartTransitionToLoading() {
        ScreenTransition.OnTransitionDone += GoToLoadingScreen;

        ScreenTransition.instance.FadeIn();
    }

    void GoToLoadingScreen() {
        ScreenTransition.OnTransitionDone -= GoToLoadingScreen;
        
        SceneManager.LoadScene(sceneBuildIndex: (int)SceneBuildIndex.LoadingScreen);
    }
}
