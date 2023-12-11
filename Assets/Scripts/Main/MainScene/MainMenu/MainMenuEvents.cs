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
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject backButton;
    [SerializeField] private TMP_Text HighScore;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerPosInMainMenu;
    private Stack<GameObject> tracker = new Stack<GameObject>();
    private string highScoreKey = "HighScore";

    public void Start()
    {
        gameObject.SetActive(true);    
        stats.SetActive(false);
        settings.SetActive(false);
        shop.SetActive(false);
        backButton.SetActive(false);
        player.transform.position = playerPosInMainMenu.transform.position;
        player.transform.localRotation = playerPosInMainMenu.transform.localRotation;
        GameService.instance.setPlayerShopSlot(player.GetComponent<PlayerShopSlots>());
    }
    private void OnEnable()
    {
        backButton.SetActive(false);
        updateHighScore();
    }

    private void Update()
    {
        if (player.transform.position != playerPosInMainMenu.position)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, playerPosInMainMenu.position, 0.1f);
        }
        if(player.transform.localRotation != playerPosInMainMenu.localRotation)
        {
            player.transform.rotation = Quaternion.Lerp(player.transform.localRotation, playerPosInMainMenu.localRotation, 0.1f);
        }
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
            player.SetActive(true);
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
        player.SetActive(false);
        trackButtons(req);
    }

    public void playGame()
    {
        SceneManager.LoadScene(1);
        
    }
}
