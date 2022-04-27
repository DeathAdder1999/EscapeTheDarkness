using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    public bool on;
    public bool blink;
    public Light light;
    public bool notVisible;

    private void Awake() {
        on = true;
        blink = false;
        notVisible = false;
    }

    private void Update() {
        if (light == null) return;

        light.enabled = on;
        if (blink) {
            StartCoroutine(Blink());
            blink = false;
        }
    }

    public IEnumerator Blink() {
        for(int i = 0; i < 5; i++) {
            if (!GameFlowManager.gamePaused) 
                on = false;
            

            float randomTimeOn = Random.Range(0.0f, 0.5f);

            yield return new WaitForSeconds(randomTimeOn);

            if (!GameFlowManager.gamePaused) 
                on = true;
             else
                i--;

            float randomTimeOff = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(randomTimeOff);

        }

        TurnOff();
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("LightSoundController")) {
            Debug.Log("True");
            notVisible = true;
        }
    }

    public void TurnOff() {
        on = false;
        StopAllCoroutines();
    }

}
