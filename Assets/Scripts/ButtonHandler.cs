using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject helpObject;

    private void Start()
    {
        if (helpObject != null)
        {
            AudioManager.Instance.PlayBGM(AudioManager.BackgroundSound.Menu);
        }
    }
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
        AudioManager.Instance.StopMusic();
        SceneManager.LoadScene("MainScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void MainMenuButton()
    {
        LevelManager.Instance.FinalText.gameObject.SetActive(false);
        AudioManager.Instance.PlayBGM(AudioManager.BackgroundSound.Menu);
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

    public void SoundEffect()
    {
        AudioManager.Instance.PlayEffect(AudioManager.Effects.Button);
    }
}
