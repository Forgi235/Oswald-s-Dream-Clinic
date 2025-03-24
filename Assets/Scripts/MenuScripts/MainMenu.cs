using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject CharacterSelectPanel;
    [SerializeField] GameObject MainPanel;

    public void PlayGame()
    {
        MainPanel.SetActive(false);
        CharacterSelectPanel.SetActive(true);
    }
    public void ChoseCharacter1()
    {
        PlayerPrefs.SetInt("ChosenCharacter", 0);
        continueToGame();
    }
    public void ChoseCharacter2()
    {
        PlayerPrefs.SetInt("ChosenCharacter", 1);
        continueToGame();
    }
    public void ChoseCharacter3()
    {
        PlayerPrefs.SetInt("ChosenCharacter", 2);
        continueToGame();
    }
    public void continueToGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
