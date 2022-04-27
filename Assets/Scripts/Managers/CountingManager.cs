using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingManager : MonoBehaviour {
    [SerializeField] private GameObject countingPanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text score;
    [SerializeField] private Text currency;
    [SerializeField] private Text timesTwo;
    [SerializeField] private GameObject skipButton;

    private bool currencyDone;
    private bool scoreDone;
    private static bool countingPanelActive;

    private static bool skip;

    private void Awake() {

         GameFlowManager.gameSessionFinished = false;
         countingPanel.SetActive(false);
         pauseButton.SetActive(true);
         scoreText.enabled = true;
         timesTwo.enabled = false;
         skip = false;
         countingPanelActive = false;

         currencyDone = false;
         scoreDone = false;
  
    }

    private void Update() {

        if (scoreDone && currencyDone) skipButton.SetActive(false);

        if (GameFlowManager.gameSessionFinished && !countingPanelActive) {
            countingPanelActive = true;
            StartCoroutine(EnableCountingPanel());
            Count();
        }
    }

    private IEnumerator EnableCountingPanel() {
        yield return new WaitForSeconds(0.3f);
        pauseButton.SetActive(false);
        scoreText.enabled = false;
        countingPanel.SetActive(true);

        if(Inventory.doubleMoney > 0) {
            timesTwo.enabled = true;
            Debug.Log("doubleMoney before: " + Inventory.doubleMoney);
            Inventory.doubleMoney--;
            Debug.Log("doubleMoney after: " + Inventory.doubleMoney);
        }

        if(Inventory.construction > 0) {        
            Inventory.construction--;
        }

    }

    private void Count() {
        int scoreAchieved = ScoreManager.Score;
        int currencyEarned = scoreAchieved / 4; 

        GameFlowManager.currency += currencyEarned;

        StartCoroutine(CountScore(scoreAchieved));
        StartCoroutine(CountCurrency(currencyEarned));
        
    }

    private IEnumerator CountScore(int scoreAchieved) {
        int currentScore = scoreAchieved;
        float rate = 0.02f;

        while (currentScore != -1) {
            if (skip) {
                score.text = "0";
                break;
            }
            score.text = currentScore + "";
            currentScore--;

            yield return new WaitForSeconds(rate);
        }

        scoreDone = true;
    }

    private IEnumerator CountCurrency(int currencyEarned) {
        int currentAmount = 0;
        float rate = 0.2f;

        if(Inventory.doubleMoney > 0) {
            rate = 0.1f;
            currencyEarned *= 2;
        }

        while(currentAmount != currencyEarned + 1) {
            if (skip) {
              currency.text = currencyEarned + "";
              break;
            }

            currency.text = currentAmount + "";
            currentAmount++;

            yield return new WaitForSeconds(rate);
        }

        currencyDone = true;
    }

    public static void SkipCounting() {
        skip = true;
    }

}
