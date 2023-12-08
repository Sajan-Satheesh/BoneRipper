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
    [SerializeField] TMP_Text levelName;
    [SerializeField, Range(0, 1)] float levelNameAnimationSpeed;

    private float actualHealth;
    private int actualBonesCount;


    private int hudLevelCount = 1;
    private float hudLevelTarget = -1f;
    private int hudBonesCount;

    private void Awake()
    {
        resetLevelName();
        levelName.gameObject.SetActive(true);
    }
    private void Start()
    {
        PlayerService.instance.onGameOver += reactOnGameOver;
        WorldService.instance.onNewLevel += reactOnNewLevel;
    }

  
    private void Update()
    {
        if (actualHealth != healthMeter.fillAmount) playHealthReduction();
        if (hudBonesCount != actualBonesCount) playBoneCollection();
        if (levelName.gameObject.activeSelf) playLevelCountDisplay();
    }

    #region animations
    #region level name Anim
    private void playLevelCountDisplay()
    {
        levelNameAnimation();
        if (levelName.color.a <= 0)
        {
            resetLevelName();
        }
    }

    private void levelNameAnimation()
    {
        hudLevelTarget += levelNameAnimationSpeed * Time.deltaTime;
        Color nameColor = levelName.color;
        nameColor.a = 1f - Mathf.Abs(hudLevelTarget);
        levelName.color = nameColor;
    }

    private void resetLevelName()
    {
        levelName.gameObject.SetActive(false);
        Color nameColor = levelName.color;
        nameColor.a = 0f;
        levelName.color = nameColor;
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
        levelName.text = "Level " + hudLevelCount;
        levelName.gameObject.SetActive(true);
        if(gameOverScreen.activeSelf)
        {
            deActivateGameOverScreen();
        }
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
}
