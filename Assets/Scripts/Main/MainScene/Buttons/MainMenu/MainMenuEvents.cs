using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject backButton;
    private Stack<GameObject> tracker = new Stack<GameObject>();
    private void Awake()
    {
        backButton.SetActive(false);
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
