using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroyer : MonoBehaviour {
    [SerializeField] private Rigidbody camera;

    private Rigidbody rb;

	void Awake () {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        rb.velocity = camera.velocity;
	}
}
