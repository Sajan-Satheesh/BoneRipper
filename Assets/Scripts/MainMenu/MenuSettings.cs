using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private Slider SL_bgMusic;
    [SerializeField] private AudioSource A_bgMusic;
    // Start is called before the first frame update

    public void Start()
    {
        SL_bgMusic.onValueChanged.AddListener(updateBgMusic);
    }
    // Update is called once per frame

    private void updateBgMusic(float sliderValue)
    {

        A_bgMusic.volume = SL_bgMusic.value / SL_bgMusic.maxValue;
    }
}
