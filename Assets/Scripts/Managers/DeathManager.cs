using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour {
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject secondChanceButton;
    [SerializeField] private GameObject watchVideoButton;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject pauseButton;
    private GameFlowManager gameManager;
    private bool deathPanelActive;
    public static bool inputReceived;
    private bool secondChanceUsed;
    private bool videoWatched;
    private bool deathAnimationSkipped;
    public static int videoStatus; //Used to keep track of ads

    private void Awake() {
        gameManager = GameObject.FindWithTag("GameFlowManager").GetComponent<GameFlowManager>();
        secondChanceButton.SetActive(false);
        watchVideoButton.SetActive(false);
        deathPanel.SetActive(false);
        deathPanelActive = false;
        inputReceived = false;
        secondChanceUsed = false;
        videoWatched = false;
        skipButton.SetActive(false);
        videoStatus = -1;
        deathAnimationSkipped = false;
    }

    private void Update() {
        if (GameFlowManager.playerIsDead && deathPanelActive) return;
        else if (GameFlowManager.playerIsDead) {
            
            if(videoWatched && (secondChanceUsed || Inventory.secondChance == 0) ) {
                Die();
                return;
            }

            if(Inventory.secondChance > 0 && !secondChanceUsed) {
               secondChanceButton.SetActive(true);
            }

            if (!videoWatched) {
                watchVideoButton.SetActive(true);
            }

            DisableUI();
            deathPanel.SetActive(true);
            deathPanelActive = true;
        }
    }

    private void DisableUI() {
        skipButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void WatchVideo() {
        StartCoroutine(Video());
    }

    private IEnumerator Video() {
        AdsManager.ShowRewardedAdd(1);

        while (videoStatus != -2 && videoStatus != 1) yield return null;

        videoWatched = true;
       
        DontDie(videoStatus);
    }

    public void DontDie(int mode) { //! watch video, 0 - use second chance

        if (mode != 0 && mode != 1) return;

        if (mode == 0) {
            Inventory.secondChance--;
            secondChanceUsed = true;
        }

        GameFlowManager.playerRespawned = true;
        inputReceived = true;
        deathPanel.SetActive(false);
        deathPanelActive = false;
        GameFlowManager.playerIsDead = false;
        Resume();
        StartCoroutine(gameManager.PlayerBlink());
        cameraController.Continue();
        secondChanceButton.SetActive(false);
        watchVideoButton.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void Die() {
        inputReceived = true;
        deathPanel.SetActive(false);
        skipButton.SetActive(true);
        cameraController.Stop();
        Resume();

        if (Inventory.construction > 0) Inventory.construction--;
        if (Inventory.fixTheLight > 0) Inventory.fixTheLight--;
    }

    private void Resume() {
        Time.timeScale = 1.0f;
        GameFlowManager.gamePaused = false;
    }

    public void Skip() {

        if (deathAnimationSkipped) {
            CountingManager.SkipCounting();
            skipButton.SetActive(false);
        } else {
            deathAnimationSkipped = true;
            GameFlowManager.gameSessionFinished = true;

            if (PlayerController.deathStatus == 1)
                GameFlowManager.TurnOffAll();
            else if (PlayerController.deathStatus == 0) {
                Debug.Log("sss");
            }
        }

       
        

        

    }
}
