using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Text timerTxt;


    private void Awake() {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        timerTxt.enabled = false;
    }

    public void PauseGame() {
        Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
        StopAllCoroutines();
        timerTxt.enabled = false;
        GameFlowManager.gamePaused = true;
    }

    public void ContinueGame() {
        StartCoroutine(Timer());

        pausePanel.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void Settings() {
        settingsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    private IEnumerator Timer() {
        timerTxt.enabled = true;
        timerTxt.text = "3";

        yield return new WaitForSeconds(1.0f);

        timerTxt.text = "2";

        yield return new WaitForSeconds(1.0f);

        timerTxt.text = "1";

        yield return new WaitForSeconds(1.0f);

        timerTxt.enabled = false;
        GameFlowManager.gamePaused = false;
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart() {
        GameFlowManager.gameSessionFinished = false;
        GameFlowManager.environmentTurnedOff = false;
        GameFlowManager.playerIsDead = false;
        SceneManager.LoadScene("Game");
    }

    private void OnDestroy() {
        Time.timeScale = 1.0f;
        GameFlowManager.gamePaused = false;
    }



}
