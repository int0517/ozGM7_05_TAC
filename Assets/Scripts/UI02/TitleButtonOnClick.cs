using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonOnClick : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("UI01TestTitleScene");
    }
}
