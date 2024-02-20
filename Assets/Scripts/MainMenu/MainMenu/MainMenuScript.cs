using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject stats;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject backButton;
    [SerializeField] private TMP_Text highScore;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private Transform playerPosInMainMenu;
    private Stack<GameObject> tracker = new Stack<GameObject>();
    public MainMenuEvents mainMenuEvents;
    private readonly string highScoreKey = "HighScore";

    private void Awake()
    {
        mainMenuEvents = new MainMenuEvents(this, playerView);
    }
    public void Start()
    {
        gameObject.SetActive(true);
        GameService.instance.menuPlayer = playerView.gameObject;
        stats.SetActive(false);
        settings.SetActive(false);
        shop.SetActive(false);
        backButton.SetActive(false);
        playerView.transform.position = playerPosInMainMenu.transform.position;
        playerView.transform.localRotation = playerPosInMainMenu.transform.localRotation;
        GameService.instance.setPlayerShopSlot(playerView.GetComponent<PlayerShopSlots>());
    }
    private void OnEnable()
    {
        backButton.SetActive(false);
        updateHighScore();
    }

    private void Update()
    {
        if (playerView.transform.position != playerPosInMainMenu.position)
        {
            playerView.transform.position = Vector3.Lerp(playerView.transform.position, playerPosInMainMenu.position, 0.1f);
        }
        if(playerView.transform.localRotation != playerPosInMainMenu.localRotation)
        {
            playerView.transform.rotation = Quaternion.Lerp(playerView.transform.localRotation, playerPosInMainMenu.localRotation, 0.1f);
        }
    }
    private void updateHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey)) highScore.text = PlayerPrefs.GetInt(highScoreKey).ToString();
        else highScore.text = 0.ToString();
    }

    public void onBackClick()
    {
        mainMenuEvents.onBackClick(backButton.gameObject);
    }

    public void onResetHighScore()
    {
        mainMenuEvents.onResetHighScore(highScore);
    }

    public void QuitGame()
    {
        mainMenuEvents.QuitGame();
    }

    public void openRequired(GameObject req)
    {
        mainMenuEvents.openRequired(req, backButton);
    }

    public void playGame()
    {
        mainMenuEvents.playGame();  
    }
}
