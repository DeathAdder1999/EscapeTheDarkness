using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public static bool playerIsDead;
    public Text scoreText;
    private static int score;
    public static int Score { get { return score; } }

    private void Start() {
        playerIsDead = false;
    }

    private void Update() {
        if (playerIsDead) GameFlowManager.SetHighscore(score);
    }

    public void SetScore(int value) {
        if (playerIsDead || GameFlowManager.gamePaused) return;

        scoreText.text = "Score: " + value;
        score = value;
    }

 
}
