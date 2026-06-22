using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string combatHudSceneName = "GameHUDScene";

    public void StartGame()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadGameWithHud());
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadGameWithHud()
    {
        AsyncOperation gameLoad = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Single);

        while (!gameLoad.isDone)
        {
            yield return null;
        }

        if (!SceneManager.GetSceneByName(combatHudSceneName).isLoaded)
        {
            yield return SceneManager.LoadSceneAsync(combatHudSceneName, LoadSceneMode.Additive);
        }

        Destroy(gameObject);
    }
}
