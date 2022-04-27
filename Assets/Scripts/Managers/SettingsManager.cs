using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Image musicButton;
    [SerializeField] private Image effectsButton;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Sprite[] musicIcons; // 1 - on, 0 - off
    [SerializeField] private Sprite[] effectIcons; // 1 - on, 0 - off

    private float musicVolume;

    private void Awake() {
        Debug.Log("Awake");
    
       SetUpIcons();

       musicVolumeSlider.value = GameFlowManager.MusicVolume;

       musicVolume = musicVolumeSlider.value;
    }

    private void Update() {
        if(musicVolumeSlider.value != musicVolume) {
            musicVolume = musicVolumeSlider.value;
            GameFlowManager.SetMusicVolume(musicVolume);
            Debug.Log("Music volume set to: " + GameFlowManager.MusicVolume);
        }
        
    }

    public void Close() {
        if(pausePanel != null) pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void MusicIconClicked() {
        if (GameFlowManager.MusicOn) {
            GameFlowManager.TurnOffMusic();
            musicButton.sprite = musicIcons[0];
        } else {
            GameFlowManager.TurnOnMusic();
            musicButton.sprite = musicIcons[1];
        }
    }

    public void EffectsIconClicked() {
        if (GameFlowManager.SoundEffectsOn) {
            GameFlowManager.TurnOffEffects();
            effectsButton.sprite = effectIcons[0];
        } else {
            GameFlowManager.TurnOnEffects();
            effectsButton.sprite = effectIcons[1];
        }
    }

    private void SetUpIcons() {

        //Set up music icon
        if (GameFlowManager.MusicOn) musicButton.sprite = musicIcons[1];
        else musicButton.sprite = musicIcons[0];

        //Set up effects icon
        if (GameFlowManager.SoundEffectsOn) effectsButton.sprite = effectIcons[1];
        else effectsButton.sprite = effectIcons[0];
        
    }

    

}
