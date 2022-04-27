using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WarningStand : MonoBehaviour {
    public GameObject lightBulb1;
    public GameObject lightBulb2;

    private Behaviour halo1;
    private Behaviour halo2;

    private void Awake() {
        halo1 = lightBulb1.GetComponent<Behaviour>();
        halo2 = lightBulb2.GetComponent<Behaviour>();
    }

    private void Start() {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink() {
        Behaviour first;
        Behaviour second;

        int randomNum = Random.Range(0, 2);

        if(randomNum == 0) {
            first = halo1;
            second = halo2;
        } else {
            first = halo2;
            second = halo1;
        }

        while (true) {
            first.enabled = false;
            yield return new WaitForSeconds(0.5f);
            first.enabled = true;
            second.enabled = false;
            yield return new WaitForSeconds(0.5f);
            second.enabled = true;
        }
    }
}
