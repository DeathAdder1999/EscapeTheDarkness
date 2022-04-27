using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody cameraRb; //In order to sync the velocity of camera with the player
    private int movementDirection; // -1 - left, 0 - straight , 1 - right

    private ScoreManager scoreManager;

    [SerializeField] SkinnedMeshRenderer[] appearance; //Looks
    private Collider playerCollider; //Player collider
    private AudioSource[] audioSources;
    private float originalVolume;

    [SerializeField] private CarSpawnManager carSpawnManager;
    private Animator playerAC;
    private int lineNum; //Mid: 0, Left: -1 , Right: 1 
    private Rigidbody rb; //Player RigidBody
    private CameraController camController;
    private float horizontalVelocity;
    private bool isMoving;
    private float originalSpeed;
    private float speedIncreaseRatio; // (new camera speed / original camera speed) ALWAYS MORE THAN ONE

    private GameObject killerStreetLight; //May be null if killed by car
    private GameObject killerCar; //May be null if killed by street light

    private int startPoint; //Used for calculating distance travelled

    //Appearance
    private int hairID;
    private int topID;
    private int pantsID;
    private int shoesID;

    //Death
    public static int deathStatus; //-1 undefined, 0 - light, 1 - car

#region INITIALIZATION

    private void Awake() {
        //Set up all of the variables
        rb = GetComponent<Rigidbody>();
        playerAC = GetComponent<Animator>();
        cameraRb = GameObject.FindWithTag("MainCamera").GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
        GameFlowManager.playerIsDead = false;

        if (cameraRb == null) Debug.Log("Camera not found !!!");

        audioSources = GetComponents<AudioSource>();
        originalVolume = audioSources[0].volume;

        camController = cameraRb.GetComponent<CameraController>();
        lineNum = 0;
        horizontalVelocity = 0.0f;
        isMoving = false;
        originalSpeed = camController.speed;
        killerStreetLight = null;
        deathStatus = -1;

        //Initialize appearance ID's

        for(int i = 1; i < 8; i++) {
            if (appearance[i].enabled) hairID = i;
        }

        for (int i = 8; i < 11; i++) {
            if (appearance[i].enabled) topID = i;
        }

        for (int i = 11; i < 13; i++) {
            if (appearance[i].enabled) pantsID = i;
        }

        for(int i = 13; i < 15; i++) {
            if (appearance[i].enabled) shoesID = i;
        }

        appearance[15].enabled = false; //Liam_Naked
    }

    private void Start() {
        playerAC.SetBool("isRunning", true);
        startPoint = (int) rb.position.z;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector3(horizontalVelocity, 0.0f, cameraRb.velocity.z); 
    }

    private void Update() {
        int distanceTravelled = Mathf.Abs(startPoint - (int)rb.position.z);
        scoreManager.SetScore(distanceTravelled);

        if (GameFlowManager.gamePaused) {
            playerAC.speed = 0.0f;
            return;
        } else {
            playerAC.speed = 1.0f;
        }   
        
        if (GameFlowManager.playerIsDead) {
            rb.velocity = Vector3.zero;
            if (!isMoving)  return; 
        }

        if (!GameFlowManager.SoundEffectsOn) {
            foreach (AudioSource audio in audioSources) {
                audio.volume = 0;
            }
        } else {
            foreach (AudioSource audio in audioSources) {
                audio.volume = originalVolume;
            }
        }

        speedIncreaseRatio = (camController.speed / originalSpeed);


        if (isMoving) return; //Check if player is already changing lines wait

        if (MobileInputManager.SwipeLeft) {
            if (lineNum == -1) return; //If he is at max left part return
            lineNum--;

            StartCoroutine(Move(-8.0f / speedIncreaseRatio, -1));
        }else if (MobileInputManager.SwipeRight) {
            if (lineNum == 1) return;
            lineNum++;

            StartCoroutine(Move(8.0f / speedIncreaseRatio, 1));
        }

    }

    #endregion

#region MOVEMENT
    private IEnumerator Move(float horizontalSpeed, int direction) {
        float rotation = 0.0f;

        if(direction == -1) {
            movementDirection = -1;
            rotation = -0.3f;
        }else if(direction == 1) {
            movementDirection = 1;
            rotation = 0.3f;
        } else {
            Debug.Log("Unexpected input");
            yield return null;
        }

        isMoving = true;
        gameObject.transform.rotation = new Quaternion(0.0f, rotation, 0.0f, 1.0f);
        horizontalVelocity = horizontalSpeed * speedIncreaseRatio;

        yield return new WaitForSeconds(0.5f);

        movementDirection = 0; 
        rb.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        horizontalVelocity = 0.0f;
        isMoving = false;
        FixPosition();

    }


    private void FixPosition() {
        if(lineNum == 0) {
            rb.transform.position = new Vector3(2.0f ,rb.transform.position.y, rb.transform.position.z);
        }else if(lineNum == -1) {
            rb.transform.position = new Vector3(-2.0f, rb.transform.position.y, rb.transform.position.z);
        } else {
            rb.transform.position = new Vector3(6.0f, rb.transform.position.y, rb.transform.position.z);
        }
    }

#endregion

#region COLLISION HANDLING
    private void OnCollisionEnter(Collision collision) {

        if(collision.collider.gameObject.CompareTag("Obstacle")) {
            StartCoroutine(Die());
        }else if (collision.collider.gameObject.CompareTag("Car")) {
            killerCar = collision.gameObject;
            StartCoroutine(Die());
        }
    }

    private void OnTriggerStay(Collider other) {


        if (other.gameObject.CompareTag("ObstacleLight")) {
            if (!other.gameObject.GetComponent<LightController>().on) {
                killerStreetLight = other.gameObject;
                StartCoroutine(Die());
           }
        }
    }

    private void OnTriggerEnter(Collider other) {
       if (other.gameObject.CompareTag("ObstacleLight")) {
            if (!other.gameObject.GetComponent<LightController>().on) {
                killerStreetLight = other.gameObject;
                StartCoroutine(Die());
            }
        }else if (other.gameObject.CompareTag("ObstacleTrigger")) {
            GameFlowManager.ActivateObstacle(other.gameObject);
        }
    }

    #endregion

#region DEATH
    private IEnumerator Die() {
        if (GameFlowManager.playerIsDead) {
            yield break;
        }

        GameFlowManager.playerIsDead = true;

        //Pause game and Activate Death Panel
        Time.timeScale = 0.0f;
        GameFlowManager.gamePaused = true;

        while (!DeathManager.inputReceived) {
            yield return null;
        }

        DeathManager.inputReceived = false;

        if (GameFlowManager.playerRespawned) {
            GameFlowManager.playerRespawned = false; //In order for respawn to behave like a trigger
            yield break; //Stop Death Animation if Video is Watched
        }

        //THIS PART IS RESPONSIBLE FOR BUG WITH POSTION WHEN HIT BY A CAR
      /*  if (isMoving) {
            if (movementDirection == -1) lineNum++;
            if (movementDirection == 1) lineNum--;
            FixPosition();
        }
        */

        ScoreManager.playerIsDead = true; //Stop scoring system
        playerAC.SetTrigger("isDead");  //Change Animation
        playerAC.SetBool("isRunning", false);
        camController.Stop(); //Stop the camera
        rb.constraints = RigidbodyConstraints.FreezeAll; //Freeze position

        if (killerStreetLight != null) { //If killed by light , start the respectful death animation
            deathStatus = 0;
            StartCoroutine(DeathAnimationLight());
            playerCollider.enabled = false; //Disable collider
        } else if (killerCar != null) {
            deathStatus = 1;
            StartCoroutine(GameFlowManager.TurnOffEnvironment());
        } else {
            GameFlowManager.TurnOffAll();
        }
    }

    private IEnumerator DeathAnimationLight() {
        LightController controller = killerStreetLight.GetComponent<LightController>();
        AudioSource scream = audioSources[0];

        controller.TurnOff();

        Disappear();

        yield return new WaitForSeconds(0.3f);

 
        scream.Play();

        yield return new WaitForSeconds(2.0f);

        controller.on = true;

        GameFlowManager.gameSessionFinished = true;
    }

    public void Disappear() {
        foreach(SkinnedMeshRenderer renderer in appearance) {
            renderer.enabled = false;
        }
    }

    public void Appear() {
        appearance[0].enabled = true; //Liam_Base
        appearance[hairID].enabled = true;
        appearance[topID].enabled = true;
        appearance[pantsID].enabled = true;
        appearance[shoesID].enabled = true;
    }
    #endregion


}
