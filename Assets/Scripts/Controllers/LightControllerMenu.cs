using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControllerMenu : MonoBehaviour {
    public Light light;
    private bool on;
    private Coroutine blinking;
    private float originalIntensity;
    private AudioSource electricitySound;
    private float originalVolume;

    private void Awake() {
        electricitySound = GetComponent<AudioSource>();
        originalVolume = electricitySound.volume;
        on = true;
        light.enabled = on;
        originalIntensity = light.intensity;
    }

    private void Update() {
        light.enabled = on;

        if (!GameFlowManager.SoundEffectsOn) electricitySound.volume = 0;
        else electricitySound.volume = originalVolume;
    }

    private void Start() {
        StartCoroutine(ControlBlink());
    }


    private IEnumerator ControlBlink() {
        while (true) {
            blinking = StartCoroutine(Blink());
            yield return new WaitForSeconds(30.0f);
            StopCoroutine(blinking);
            yield return new WaitForSeconds(10.0f);
        }
    }

    private IEnumerator Blink() {
        while (true) {
            float randomIntensity = Random.Range(0.0f, 8.0f);
            float randomTime = Random.Range(0.1f, 0.5f);

            light.intensity = randomIntensity;
            electricitySound.Play();
            yield return new WaitForSeconds(randomTime);
            light.intensity = originalIntensity;
            electricitySound.Stop();
            randomTime = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(randomTime);
        }
        
    }


}
