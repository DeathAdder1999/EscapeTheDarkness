using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speed;
    public bool movement;

    private static int waveNum;
    public static int WaveNum { get { return waveNum; } }
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        movement = false;
        StartCoroutine(Wait());
        waveNum = 0;

        if(Inventory.construction > 0) {
            speed = 9.0f;
        } else {
            speed = 7.0f;
        }
    }

    private void FixedUpdate() {
        if (GameFlowManager.gamePaused) rb.velocity = Vector3.zero;
        else if (movement)
            rb.velocity = new Vector3(0.0f, 0.0f, speed);
        else
            rb.velocity = Vector3.zero;
    }

    public void Stop() {
        movement = false;
    }

    public void Continue() {
        movement = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("EnvironmentDestroyer")) {
            other.gameObject.GetComponent<EnvironmentDestroyer>().DestroyEnvironment();
        } else if (other.gameObject.CompareTag("SpawnTrigger")) {
            GameFlowManager.spawn = true;
            NextWave();
        }
    }

    private void NextWave() {
        if (speed > 30) return;

        waveNum++;
        speed += 0.5f;
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(0.05f);
        movement = true;
    }

}
