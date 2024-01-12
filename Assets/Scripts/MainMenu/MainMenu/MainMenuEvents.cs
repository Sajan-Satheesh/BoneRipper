using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents
{
    GameObject mainMenu = null;
    GameObject player = null;
    private readonly string highScoreKey = "HighScore";
    private Stack<GameObject> tracker = new Stack<GameObject>();

    public MainMenuEvents(MainMenuScript _mainMenu, PlayerView _player) { mainMenu = _mainMenu.gameObject; player = _player.gameObject; }
    private void updateHighScore(TMP_Text highScore)
    {
        if (PlayerPrefs.HasKey(highScoreKey)) highScore.text = PlayerPrefs.GetInt(highScoreKey).ToString();
        else highScore.text = 0.ToString();
    }
    private void trackButtons(GameObject obj, GameObject backButton)
    {
        tracker.Push(obj);
        if (tracker.Count > 0)
        {
            backButton.SetActive(true);
        }
    }

    public void onBackClick(GameObject backButton)
    {
        tracker.Pop().SetActive(false);
        if (tracker.Count == 0)
        {
            backButton.SetActive(false);
            mainMenu.SetActive(true);
            player.SetActive(true);
        }
    }

    public void onResetHighScore(TMP_Text highScore)
    {
        PlayerPrefs.SetInt(highScoreKey, 0);
        updateHighScore(highScore);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    public void openRequired(GameObject req, GameObject backButton)
    {
        mainMenu.SetActive(false);
        req.SetActive(true);
        player.SetActive(false);
        trackButtons(req,backButton);
    }

    public void playGame()
    {
        SceneManager.LoadScene(1);
    }
}
