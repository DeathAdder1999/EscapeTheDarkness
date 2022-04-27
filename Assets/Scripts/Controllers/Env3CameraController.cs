using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env3CameraController : MonoBehaviour {
    private AudioSource[] sounds;
    private float originalVolumeBackground = 1;
    private float originalVolumeCarHorn1 = 0.5f;
    private float originalVolumeCarHorn2 = 0.05f;
    private float originalVolumePoliceSiren = 0.05f;
    private bool soundEnabled;

    private void Awake() {
        sounds = GetComponents<AudioSource>();
        soundEnabled = GameFlowManager.SoundEffectsOn;
        if (!soundEnabled) {
            DisableSounds();
        }
    }

    private void Start() {
        StartNoises();
    }

    private void Update() {
        if (GameFlowManager.SoundEffectsOn && !soundEnabled)
            EnableSounds();
        else if (!GameFlowManager.SoundEffectsOn && soundEnabled)
            DisableSounds();
    }

    private void StartNoises() {
        AudioSource carHorn1 = sounds[1];
        AudioSource carHorn2 = sounds[2];
        AudioSource policeSiren = sounds[3];

        StartCoroutine(StartNoise(carHorn1, 9.0f));
        StartCoroutine(StartNoise(carHorn2, 9.0f));
        StartCoroutine(StartNoise(policeSiren, 9.0f));
    }

    private IEnumerator StartNoise(AudioSource noise, float maxWaitTime) {
        while (true) {
            float waitTime = Random.Range(3.0f, maxWaitTime);

            yield return new WaitForSeconds(waitTime);

            if(!noise.isPlaying)
                noise.Play();
        }
    }

    private void DisableSounds() {
        foreach (AudioSource sound in sounds)
            sound.volume = 0.0f;

        soundEnabled = false;
    }

    private void EnableSounds() {
        sounds[0].volume = originalVolumeBackground;
        sounds[1].volume = originalVolumeCarHorn1;
        sounds[2].volume = originalVolumeCarHorn2;
        sounds[3].volume = originalVolumePoliceSiren;
        soundEnabled = true;
    }
}
