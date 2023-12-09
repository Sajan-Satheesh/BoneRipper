using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject stats;
    [SerializeField] private GameObject backButton;
    [SerializeField] private TMP_Text HighScore;
    private Stack<GameObject> tracker = new Stack<GameObject>();
    private string highScoreKey = "HighScore";

    private void Awake()
    {
        gameObject.SetActive(true);    
        stats.SetActive(false);
        settings.SetActive(false);
        backButton.SetActive(false);
    }
    private void OnEnable()
    {
        backButton.SetActive(false);
        updateHighScore();
    }

    private void updateHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey)) HighScore.text = PlayerPrefs.GetInt(highScoreKey).ToString();
        else HighScore.text = 0.ToString();
    }
    private void trackButtons(GameObject obj)
    {
        tracker.Push(obj);
        if(tracker.Count > 0)
        {
            backButton.SetActive(true);
        }
    }

    public void onBackClick()
    {
        tracker.Pop().SetActive(false);
        if(tracker.Count == 0 )
        {
            backButton.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    public void onResetHighScore()
    {
        PlayerPrefs.SetInt(highScoreKey,0);
        updateHighScore();
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    public void openRequired(GameObject req)
    {
        gameObject.SetActive(false);
        req.SetActive(true);
        trackButtons(req);
    }

    public void playGame()
    {
        SceneManager.LoadScene(1);
        
    }
}
