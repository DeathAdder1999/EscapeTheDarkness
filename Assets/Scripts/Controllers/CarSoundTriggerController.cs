using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundTriggerController : MonoBehaviour {
    [SerializeField] private Rigidbody cameraRb;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.velocity = cameraRb.velocity;
    }
}
