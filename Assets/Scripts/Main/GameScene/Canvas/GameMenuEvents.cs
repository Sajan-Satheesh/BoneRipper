using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuEvents : MonoBehaviour
{
    public void restart()
    {
        WorldService.instance.reloadCurrentLevel();
    }

    public void openRequired(GameObject req)
    {
        gameObject.SetActive(false);
        req.SetActive(true);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0); 
    }

}
