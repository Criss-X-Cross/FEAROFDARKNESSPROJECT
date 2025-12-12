using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Ubah "GameScene" menjadi nama scene game kamu
        //SceneManager.LoadScene("MainMenuScene");
        SceneManager.LoadSceneAsync("GameScene"); //loading screen if its feel Hang when "transitioning"
    }

    public void ExitGame()
    {
        Debug.Log("Game exited.");
        Application.Quit();
    }
}
