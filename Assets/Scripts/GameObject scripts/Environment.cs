using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Environment : MonoBehaviour {
    public static bool isInGame;
    public Transform spawn;
    public LightController[] streetLights;
    public LightController[] decorativeLights;
    [SerializeField] private Light[] otherLights;
    public Transform[] carSpawns;
    public static Transform carSpawnRight;
    public static Transform carSpawnMid;
    public static Transform carSpawnLeft;
    public static Transform spawnPoint;
    public bool turnOff; //DELETED
    public bool turnedOff;
    private AudioSource turningOffSound;
    private static float originalVolume1 = 0.5f;

    [SerializeField] private GameObject camera;
    private AudioSource sound;
    private float originalVolume;
    private bool endSoundPlayed;
    
    private void Awake() {
        if(camera != null) {
            sound = camera.GetComponent<AudioSource>();
            if (sound != null) originalVolume = sound.volume;
            else originalVolume = 0;
        }

        turningOffSound = GetComponent<AudioSource>();
        endSoundPlayed = false;

        spawnPoint = spawn;
        if(Inventory.construction > 0) {
            carSpawns = new Transform[3];
            for(int i = 0; i < 3; i++) {
                carSpawns[i] = null;
            }
        }

        if(carSpawns.Length != 0) {
            carSpawnRight = carSpawns[0];
            carSpawnMid = carSpawns[1];
            carSpawnLeft = carSpawns[2];
        }
        turnOff = false;
        turnedOff = false;

        if (SceneManager.GetActiveScene().name == "Game") isInGame = true;
        else isInGame = false;

      
    }

    private void Update() {
        if (GameFlowManager.environmentTurnedOff && !endSoundPlayed) {
            endSoundPlayed = true;
            turningOffSound.Play();
        }

        if(turnOff) {
            TurnOffLights();
            turnOff = false;
        }

        if (!GameFlowManager.SoundEffectsOn) {

            if (turningOffSound != null) turningOffSound.volume = 0.0f;

            if (camera == null) return;

            if (sound != null) sound.volume = 0;
        } else {
            if (sound != null) sound.volume = originalVolume;
            if(turningOffSound != null) turningOffSound.volume = originalVolume1;
        }
    }

    public static Transform GetCarSpawn() {

        int randomIndex = Random.Range(0, 4);

        if (randomIndex == 0) return carSpawnRight;
        else if (randomIndex == 1 || randomIndex == 2) return carSpawnMid;
        else return carSpawnLeft;
    }
    
    public void TurnOffLights() {
        if (decorativeLights.Length > 0) {
            turningOffSound.Play();
            foreach (LightController light in decorativeLights) {
                light.on = false;
            }
          
        }

        if (otherLights.Length > 0) {
            foreach (Light light in otherLights) {
                light.enabled = false;
            }
        }

        StartCoroutine(TriggerAnimation());
    }

    private IEnumerator TriggerAnimation() {

        for(int i = 0; i < streetLights.Length - 1; i += 2) {

            if (GameFlowManager.gameSessionFinished) yield break; //If animation was skipped

            if (!streetLights[i].notVisible && !streetLights[i + 1].notVisible) {
                streetLights[i].on = false;
                streetLights[i + 1].on = false;
                turningOffSound.Play();
            }
            yield return new WaitForSeconds(0.45f);
        }


        turnedOff = true;
    }

    public void TurnOffAll() {
        turningOffSound.Play();
        foreach(LightController light in streetLights) {
            light.TurnOff();
        }
    }

}
