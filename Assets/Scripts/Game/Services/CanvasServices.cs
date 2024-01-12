using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CanvasServices : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Image healthMeter;
    [SerializeField] TMP_Text T_levelName;
    [SerializeField] TMP_Text T_boneCount;
    private int boneCount = 0;
    private string highScoreKey = "HighScore";
    [SerializeField, Range(0, 1)] float levelNameAnimationSpeed;
    
    private float actualHealth;
    private int actualBonesCount;

    private int hudLevelCount = 1;
    private float hudLevelTarget = -1f;
    private int hudBonesCount;

    private void Awake()
    {
        resetLevelName();
        T_levelName.gameObject.SetActive(true);
    }
    private void Start()
    {
        PlayerService.instance.events.Subscribe_OnGameOver(reactOnGameOver);
        WorldService.instance.events.Subscribe_OnNewLevel(reactOnNewLevel);
        EnemyService.instance.events.Subscribe_OnEnemyDestroyed(reactOnEnemyDestroyed);
    }

   

    private void Update()
    {
        if (actualHealth != healthMeter.fillAmount) playHealthReduction();
        if (hudBonesCount != actualBonesCount) playBoneCollection();
        if (T_levelName.gameObject.activeSelf) playLevelCountDisplay();
    }

    #region animations
    #region level name Anim
    private void playLevelCountDisplay()
    {
        levelNameAnimation();
        if (T_levelName.color.a <= 0)
        {
            resetLevelName();
        }
    }

    private void levelNameAnimation()
    {
        hudLevelTarget += levelNameAnimationSpeed * Time.deltaTime;
        Color nameColor = T_levelName.color;
        nameColor.a = 1f - Mathf.Abs(hudLevelTarget);
        T_levelName.color = nameColor;
    }

    private void resetLevelName()
    {
        T_levelName.gameObject.SetActive(false);
        Color nameColor = T_levelName.color;
        nameColor.a = 0f;
        T_levelName.color = nameColor;
        hudLevelTarget = -1f;
    }
    #endregion

    private void playHealthReduction()
    {
        Mathf.Lerp(healthMeter.fillAmount, actualHealth, 0.1f);
    }

    private void playBoneCollection()
    {
        hudBonesCount = actualBonesCount;
    }
    #endregion

    #region re-Actions
    private void reactOnEnemyDestroyed()
    {
        boneCount += 5;
        T_boneCount.text = boneCount.ToString();
        checkHighScore();
    }

    private void checkHighScore()
    {
        if (!PlayerPrefs.HasKey(highScoreKey))
        {
            PlayerPrefs.SetInt(highScoreKey, boneCount);
        }
        else
        {
            if (PlayerPrefs.GetInt(highScoreKey) < boneCount) PlayerPrefs.SetInt(highScoreKey, boneCount);
        }
    }

    private void reactOnGameOver()
    {
        activateGameOverScreen();
    }

    private void reactOnPlayerHarmed(float health)
    {
        actualHealth = health / 100;
    }
    private void reactOnBoneCollected()
    {
        actualBonesCount += 1;
    }
    private void reactOnNewLevel()
    {
        hudLevelCount = requestLevelNum();
        T_levelName.text = "Level " + hudLevelCount;
        T_levelName.gameObject.SetActive(true);
        if(gameOverScreen.activeSelf)
        {
            deActivateGameOverScreen();
            boneCount= 0;
        }
        T_boneCount.text = boneCount.ToString();
    }

    #endregion

    private int requestLevelNum()
    {
        return WorldService.instance.getLevelNum();
    }

    private void activateGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
    private void deActivateGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }
    private void OnDisable()
    {
        PlayerService.instance.events.UnSubscribe_OnGameOver(reactOnGameOver);
        WorldService.instance.events.UnSubscribe_OnNewLevel(reactOnNewLevel);
        EnemyService.instance.events.UnSubscribe_OnEnemyDestroyed(reactOnEnemyDestroyed);
    }
}
