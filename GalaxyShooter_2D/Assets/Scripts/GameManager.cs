using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    // Update is called once per frame
    void Update()
    {
        if (_isGameOver is true && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void RestartGame()
    {
        Debug.Log("Restart game");
        _isGameOver = false;
        SceneManager.LoadScene(1); // 1-Game scene
    }

    // public methods
    public void GameOver()
    {
        _isGameOver = true;
    }
}
