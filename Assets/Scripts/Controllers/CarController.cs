using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour {
    [SerializeField] private HeadLightsController headLightsController;
    private Rigidbody rb;
    public float velocity;
    private bool collisionWithPlayer;
    private bool carCollision;
    private AudioSource[] audioSources;
    private float startVelocity;
    private float maxVelocity;
    private float minVelocity;
    private float originalVolume;
    private AudioSource soundToPlay;
    public bool onRoad;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        collisionWithPlayer = false;
        carCollision = false;
        maxVelocity = -30.0f; //Default
        minVelocity = -15.0f; //Default
        audioSources = GetComponents<AudioSource>();
        originalVolume = 0.2f;

        onRoad = false;
    }

    private void Update() {

        if (GameFlowManager.gamePaused) {
            if(soundToPlay != null)
                soundToPlay.Pause();
        } else {
            if (soundToPlay != null) {
                if (!soundToPlay.isPlaying)
                    soundToPlay.Play();
            }
        }

        //Sound 
        if (!GameFlowManager.SoundEffectsOn) {
            foreach(AudioSource audio in audioSources) {
                audio.volume = 0;
            }
        } else {
            foreach (AudioSource audio in audioSources) {
                audio.volume = originalVolume;
            }
        }
    }

    private void Start() {
        if(Inventory.fixTheLight > 0) {
            maxVelocity = -40.0f;
            minVelocity = -25.0f;
        }
        startVelocity = velocity = Random.Range(maxVelocity, minVelocity);
    }

    private void FixedUpdate() {
        if (GameFlowManager.gamePaused) {
            rb.velocity = Vector3.zero;
            Debug.Log("Game is actually paused!");
        } else {
            if (startVelocity == 0.0f) {
                startVelocity = velocity = Random.Range(maxVelocity, minVelocity);
            }
            rb.position = new Vector3(rb.position.x, 0.1f, rb.position.z);
            rb.velocity = new Vector3(0.0f, 0.0f, velocity);
        }
    }

    public void FullStop() {
        velocity = 0.0f;
       // StartCoroutine(TryToMove());
    }

    private void ContinueMovement() {
        velocity = startVelocity;
    }

    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("CarDestroyer")) {
            onRoad = false;
        }
    }
    


    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) FullStop();
    }

    private void OnCollisionExit(Collision collision) {
        ContinueMovement(); 
    }

    private IEnumerator TryToMove() {

        yield return new WaitForSeconds(0.3f);
        if (!collisionWithPlayer && !carCollision) ContinueMovement();
    }

    private IEnumerator StartSound() {


        if (SceneManager.GetActiveScene().name == "Main Menu") {
            yield return null;
        }

       yield return new WaitForSeconds(0.5f);

        float randomNum = Random.Range(0.0f, 1.0f);

        if (randomNum <= 0.5f) soundToPlay = audioSources[0];
        else soundToPlay = audioSources[1];

        soundToPlay.Play();
    }

    public void ResetVelocity() {
        startVelocity = velocity = Random.Range(maxVelocity, minVelocity);
        StartCoroutine(StartSound());
    }
}
