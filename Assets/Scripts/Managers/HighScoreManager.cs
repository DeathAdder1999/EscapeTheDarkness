using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {
    [SerializeField] private Text darkness;
    [SerializeField] private Text blinkingLights;

    private void Awake() {
        darkness.text = "HIGHSCORE: " + GameFlowManager.HighScoreDarkness;
        blinkingLights.text = "HIGHSCORE: " + GameFlowManager.HighScoreBlinkingLights;
    }
}
