using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameObject GameOverUI;
    public GameObject Spawner;
    public void GameOver()
    {
        // checks to see if the game has ended, if the game is over
        // a GameOver Panel is displayed
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GameOver");
            Time.timeScale = 0f;
            GameOverUI.SetActive(true);
            Spawner.SetActive(false);
        }
       
    }

    //method that when called reloads the current scene 
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //method that when called returns to the main menu scene 
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //method that ends the game when the game is built
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
