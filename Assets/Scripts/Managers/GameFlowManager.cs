using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour {
#region VARIABLES
    public static bool gamePaused;
    public static bool gameSessionFinished;

    //Player looks
    [SerializeField] private SkinnedMeshRenderer[] hairStyles;
    [SerializeField] private SkinnedMeshRenderer[] topStyles;
    [SerializeField] private SkinnedMeshRenderer[] pantsStyles;
    [SerializeField] private SkinnedMeshRenderer[] shoeStyles;
    [SerializeField] private GameObject playerPrefab;
    private static GameObject player;
    private static PlayerController playerController;

    //SOUNDS
    private static AudioSource music;
    private static float musicVolume;
    public static float MusicVolume { get { return musicVolume; } }
    private static bool soundEffectsOn;
    public static bool SoundEffectsOn { get { return soundEffectsOn; } }
    private static bool musicOn;
    public static bool MusicOn { get { return musicOn; } }

    public static bool created;
    public static bool spawn;
    public GameObject[] environmentsToSpawn;
    private Transform spawnPoint;
    private Scene currentScene;
    private static int leftNum; //number of times left lights turned off in a row
    private static int rightNum; //number of times right lights turned off in a row
    private static int noneNum; //number of times none of the lights turned off in a row
    private static GameObject currentEnvironment;
    private static GameObject previousEnvironment;


    public static bool lightsTurnedOff;
    public static bool playerRespawned;

    //Persistent data
    private static PlayerAppearance appearance;
    private static int highScoreDarkness;
    private static int highScoreBlinkingLights;
    public static int currency;
    public static int HighScoreDarkness { get { return highScoreDarkness; } }
    public static int HighScoreBlinkingLights {  get { return highScoreBlinkingLights; } }

    public static bool playerIsDead;
    public static bool environmentTurnedOff;

    public static string gameMode; //Can be only "darkness" or "blinking lights"
#endregion

#region INITIALIZATION
    private void Awake() { //Called everytime new scene is loaded
        if (created) {
            DestroyImmediate(gameObject);
        } else {
            QualitySettings.vSyncCount = 0; //Turn off vsync
            Application.targetFrameRate = 45; //Does not work on mobile

            DontDestroyOnLoad(gameObject);
            created = true;
            spawn = false;

            //Initialize Game Variables
            LoadGameData();
            environmentTurnedOff = false;

            currentScene = SceneManager.GetActiveScene();
            gamePaused = false;
            playerIsDead = false;
            playerRespawned = false;
            gameSessionFinished = false;

            music = null;

        }
    }

    private void Update() { 
        Scene activeScene = SceneManager.GetActiveScene();
        if (currentScene != activeScene && activeScene.name == "Game") { //Scene has changed to game
            currentScene = activeScene;
            SetUpGame();
        }


        if (spawn) {
            SpawnEnvironment();
            spawn = false;
        }
    }

    #endregion

#region GAME I/O
    public static bool delete = true;

    private static void SaveData(PlayerData data) {
        PlayerAppearance pa = data.Appearance;
        if (!PlayerPrefs.HasKey("SavedBefore")) {
            PlayerPrefs.SetInt("SavedBefore", 1); 
        }
        PlayerPrefs.SetInt("HairID", pa.HairID);
        PlayerPrefs.SetInt("TopID", pa.TopID);
        PlayerPrefs.SetInt("PantsID", pa.PantsID);

        PlayerPrefs.SetInt("HighScoreDarkness", data.HighScoreDarkness);
        PlayerPrefs.SetInt("HighScoreBlinkingLights", data.HighScoreBlinkinghLights);
        PlayerPrefs.SetInt("Currency", data.Currency);

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
       
        Debug.Log("SecurityKey: " + data.SecurityKey);

        PlayerPrefs.SetString("SecurityKey", data.SecurityKey);

        int musicEnabled, soundEnabled;
        musicEnabled = soundEnabled = 0;

        if (musicOn) musicEnabled = 1;
        if (soundEffectsOn) soundEnabled = 1;

        PlayerPrefs.SetInt("MusicOn", musicEnabled);
        PlayerPrefs.SetInt("SoundEffectsOn", soundEnabled);
    }

    private static void LoadGameData() {
        if (!PlayerPrefs.HasKey("SavedBefore")) {
            GameFlowManager.appearance = new PlayerAppearance(0, 0, 0, 0); //Default appearance
            musicVolume = 0.5f;
            soundEffectsOn = true;
            musicOn = true;
            GameFlowManager.currency = 0; //default currency
            return;
        }

        int hairID = PlayerPrefs.GetInt("HairID");
        int topID = PlayerPrefs.GetInt("TopID");
        int pantsID = PlayerPrefs.GetInt("PantsID");
        int shoesID = PlayerPrefs.GetInt("ShoesID");

        int highScoreDarkness = PlayerPrefs.GetInt("HighScoreDarkness");
        int highScoreBlinkingLights = PlayerPrefs.GetInt("HighScoreBlinkingLights");
        int currency = PlayerPrefs.GetInt("Currency");

        PlayerAppearance appearance = new PlayerAppearance(hairID, topID, pantsID, shoesID);
        GameFlowManager.appearance = appearance;

        //Verify data
        PlayerData dataLoaded = new PlayerData(currency, highScoreBlinkingLights, highScoreDarkness, appearance);
        string securityKey = PlayerPrefs.GetString("SecurityKey");


        if (securityKey != null && securityKey != dataLoaded.SecurityKey) {
            GameFlowManager.highScoreBlinkingLights = 0;
            GameFlowManager.highScoreDarkness = 0;
            GameFlowManager.currency = 0;
        } else {
            GameFlowManager.highScoreBlinkingLights = highScoreBlinkingLights;
            GameFlowManager.highScoreDarkness = highScoreDarkness;
            GameFlowManager.currency = currency;
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        int m = PlayerPrefs.GetInt("MusicOn");
        int e = PlayerPrefs.GetInt("SoundEffectsOn");

        if (m == 1) musicOn = true;
        else musicOn = false;

        if (e == 1) soundEffectsOn = true;
        else soundEffectsOn = false;
    }

    private void OnApplicationQuit() {
        SaveData(new PlayerData(currency, highScoreBlinkingLights , 0, appearance)); // 0 is highScoreDarkness has to be replaced later on
    }

    #endregion

#region GAMEPLAY

    private void SetUpGame() {
        if (appearance != null) SetAppearance(appearance);
        player = Instantiate(playerPrefab) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        music = GameObject.FindWithTag("InGameMusic").GetComponent<AudioSource>();
        music.volume = musicVolume;

        if (!musicOn) TurnOffMusic();
        else TurnOnMusic();

        if (!soundEffectsOn) TurnOffEffects();
        else TurnOnEffects();

        GameObject prefabToSpawn;

        if (Inventory.construction > 0) prefabToSpawn = environmentsToSpawn[2]; //EnvironmentConstruction
        else prefabToSpawn = environmentsToSpawn[0];

        GameObject environment = Instantiate(prefabToSpawn, new Vector3(0, 0, 0), new Quaternion(0.0f, 0.7f, 0.0f, 0.7f)) as GameObject;
        currentEnvironment = environment;
        spawnPoint = Environment.spawnPoint;

        

    }

    // Not Optimized
        public void SpawnEnvironment() {
            GameObject prefabToSpawn;

            spawnPoint = Environment.spawnPoint;

            float randomNumber = Random.Range(0.0f, 1.0f);

            if (Inventory.construction > 0) {
                if (randomNumber < 0.7f)
                    prefabToSpawn = environmentsToSpawn[2]; //EnvrionmentConstruction
                else
                    prefabToSpawn = environmentsToSpawn[3]; //EnvironmentCrossRoadConstruction;
                prefabToSpawn = environmentsToSpawn[2];
            } else {
                if (randomNumber < 0.7f)
                    prefabToSpawn = environmentsToSpawn[0]; //Envrionment1
                else
                    prefabToSpawn = environmentsToSpawn[1]; //EnvironmentCrossRoad;

            }


            GameObject newEnvironment = Instantiate(prefabToSpawn, new Vector3(0.0f, spawnPoint.position.y, spawnPoint.position.z), new Quaternion(0.0f, 0.7f, 0.0f, 0.7f)) as GameObject;
            previousEnvironment = currentEnvironment;
            currentEnvironment = newEnvironment;
        }
        

  
    public static void ActivateObstacle(GameObject obstacleTrigger) {
        if (Inventory.fixTheLight > 0) return; //If Fix the light is bought from the store
        

        Lights lights = obstacleTrigger.GetComponent<Lights>();
        float activationChance = Random.Range(0.0f, 1.0f);

        if (activationChance < 0.2f) {
            noneNum++;
            if(noneNum <= 2) return;
        }

        noneNum = 0;

        if(leftNum >= 2) {
            lights.light2.GetComponent<LightController>().blink = true;
            leftNum = 0;
            rightNum++;
            return;
        }else if(rightNum >= 2) {
            lights.light1.GetComponent<LightController>().blink = true;
            rightNum = 0;
            leftNum++;
            return;
        }

        int light1Chance =  Random.Range(0, 2);

        if (light1Chance == 1 && lights.light1 != null) {
            lights.light1.GetComponent<LightController>().blink = true;
            rightNum = 0;
            leftNum++;
        } else {
            lights.light2.GetComponent<LightController>().blink = true;
            leftNum = 0;
            rightNum++;
        }
    }

    public static IEnumerator TurnOffEnvironment() { //Turns off the lights of the environments
        Environment cur = currentEnvironment.GetComponent<Environment>();
        cur.TurnOffLights();

        while (!cur.turnedOff) yield return new WaitForSeconds(0.01f);

        if (previousEnvironment != null) {
            Environment previous = previousEnvironment.GetComponent<Environment>();
            previous.TurnOffLights();
            while (!previous.turnedOff) yield return new WaitForSeconds(0.01f);
        }


        environmentTurnedOff = true;
        gameSessionFinished = true;
    }

    public static void TurnOffAll() {
        Environment cur = currentEnvironment.GetComponent<Environment>();
        cur.TurnOffAll();

        if (previousEnvironment != null) {
            Environment previous = previousEnvironment.GetComponent<Environment>();
            previous.TurnOffAll();
        }

        environmentTurnedOff = true;
        gameSessionFinished = true;
    }
    #endregion

#region PLAYERAPPEARANCE

    public void SetAppearance(PlayerAppearance playerAppearance) {
        appearance = playerAppearance;

        for(int i = 0; i < hairStyles.Length; i++) {
            if (i != appearance.HairID) hairStyles[i].enabled = false;
            else hairStyles[i].enabled = true;
        }

        for (int i = 0; i < topStyles.Length; i++) {
            if (i != appearance.TopID) topStyles[i].enabled = false;
            else topStyles[i].enabled = true;
        }

        for (int i = 0; i < pantsStyles.Length; i++) {
            if (i != appearance.PantsID) pantsStyles[i].enabled = false;
            else pantsStyles[i].enabled = true;
        }

        for (int i = 0; i < shoeStyles.Length; i++) {
            if (i != appearance.ShoesID) shoeStyles[i].enabled = false;
            else shoeStyles[i].enabled = true;
        }

    }

    public IEnumerator PlayerBlink() {
        Collider playerCollider = player.GetComponent<Collider>();
        playerCollider.enabled = false;
    
        
        for(int i = 0; i < 20; i++) {
            playerController.Disappear();

            yield return new WaitForSeconds(0.1f);

            playerController.Appear();

            yield return new WaitForSeconds(0.1f);
        }

        playerCollider.enabled = true;
    }


    public static PlayerAppearance GetAppearance() {
        if (appearance == null) return null;

        return new PlayerAppearance(appearance.HairID, appearance.TopID, appearance.PantsID, appearance.ShoesID);
    }


    #endregion

#region HIGHSCORE
    public static void SetHighscore(int score) {
        if(gameMode == "darkness" && score > highScoreDarkness) {
            highScoreDarkness = score;
        }else if(gameMode == "blinking lights" && score > highScoreBlinkingLights) {
            highScoreBlinkingLights = score;
        }

        return;
    }
    #endregion

#region SOUNDS

    public static void SetMusicVolume(float volume) {
        musicVolume = volume;

        if (music == null) return; //Not in Game Scene

        music.volume = musicVolume;
    }

    public static void TurnOnMusic() {
        musicOn = true;
        if(music != null) music.enabled = true;
    }

    public static void TurnOnEffects() {
        soundEffectsOn = true;
    }

    public static void TurnOffMusic() {
        musicOn = false;
        if(music != null) music.enabled = false;
    }

    public static void TurnOffEffects() {
        soundEffectsOn = false;
    }

    #endregion
}
