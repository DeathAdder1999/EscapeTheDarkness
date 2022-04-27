using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModesManager : MonoBehaviour {

    [SerializeField] private Text highScoreDarkness;
    [SerializeField] private Text highScoreBlinkingLights;

    private void Awake() {
        highScoreDarkness.text = "highscore: " + GameFlowManager.HighScoreDarkness.ToString();
        highScoreBlinkingLights.text = "highscore: "  + GameFlowManager.HighScoreBlinkingLights.ToString();
    }

    public void LoadScene(string scene) {
        if (scene == "darkness") GameFlowManager.gameMode = "darkness";
        else if (scene == "blinking lights") GameFlowManager.gameMode = "blinking lights";

        SceneManager.LoadScene("Loading Scene");
    }
}
