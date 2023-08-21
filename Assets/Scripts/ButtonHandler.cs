using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject helpObject;
    public void PlayButton()
    {
        if (LevelManager.Instance != null)
        {
            Destroy(LevelManager.Instance.gameObject);
        }
        if (BrickManager.Instance != null)
        {
            Destroy(BrickManager.Instance.gameObject);
        }
        SceneManager.LoadScene("MainScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void MainMenuButton()
    {
        LevelManager.Instance.FinalText.gameObject.SetActive(false);
        SceneManager.LoadScene("MenuScene");
    }

    public void HelpButton()
    {
        if (!helpObject.activeSelf)
        {
            helpObject.SetActive(true);
        }
        else
        {
            helpObject.SetActive(false);
        }
    }
}
