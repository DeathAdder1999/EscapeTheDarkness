using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour {

    [SerializeField] private Sprite[] backgroundImages; //0 - darkness, 1 - blinking lights
    [SerializeField] private Image canvasImage;
    [SerializeField] private Text modeDescription;

    private void Awake() {
        if(GameFlowManager.gameMode == "darkness") {
            canvasImage.sprite = backgroundImages[0];
        }else if(GameFlowManager.gameMode == "blinking lights") {
            canvasImage.sprite = backgroundImages[1];
            modeDescription.text = "Avoid cars and darkness. Be careful when running under a blinking light, you never know what waits for you in the darkness...";
        }
    }

    private void Start() {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame() {

        yield return new WaitForSeconds(2.0f);

        AsyncOperation async = SceneManager.LoadSceneAsync("Game");

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }

    }

 
}
