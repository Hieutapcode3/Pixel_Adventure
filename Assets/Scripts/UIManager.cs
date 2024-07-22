using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject OptionPanel;
    public void PauseGame()
    {
        OptionPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        OptionPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void Option()
    {
        // Add option functionality here
    }

    public void Exit()
    {
        SceneManager.LoadScene("Start Screen");
        Time.timeScale = 1f;
    }
}
