using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static bool created = false;

    private void Awake() {
        if (!created) {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        } else {
            Destroy(this.gameObject);
        }
    }

    public void LoadLevel(string name) {
        SceneManager.LoadScene(name);
    }

  
}
