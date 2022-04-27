using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDestroyer : MonoBehaviour {
   [SerializeField] private GameObject environment;

    public void DestroyEnvironment() {
        Destroy(environment);
    }
}
