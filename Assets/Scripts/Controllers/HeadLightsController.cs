using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLightsController : MonoBehaviour {
    [SerializeField] private Light[] headLights;
    [SerializeField] private GameObject[] planes;
    [SerializeField] private CarController car;

    public CarController Car { get { return car; } }

    private Rigidbody rb;
    private CarController carController;
    private bool turnedOff;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        carController = car.GetComponent<CarController>();
        TurnOn();
        GameFlowManager.environmentTurnedOff = false;
        turnedOff = false;
    }

    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Car")) {
            if (other.gameObject == car.gameObject) return;

            carController.velocity = -10.0f;
        }
    }
    

    private void Update() {
        if (turnedOff) return;
        else if (GameFlowManager.environmentTurnedOff)
            TurnOff();
        
    }


    //TO BE DELETED

    private void  TurnOff() {
        headLights[0].enabled = false;
        headLights[1].enabled = false;
        foreach(GameObject plane in planes) {
            plane.SetActive(false);
        }
        turnedOff = true;
    }

    private void TurnOn() {
        headLights[0].enabled = true;
        headLights[1].enabled = true;
        foreach (GameObject plane in planes) {
            plane.SetActive(true);
        }
        turnedOff = false;
    }

    
}
